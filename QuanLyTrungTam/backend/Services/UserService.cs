using backend.DTOs;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services;

/// <summary>
/// Xử lý toàn bộ nghiệp vụ xác thực người dùng: đăng nhập, làm mới token,
/// đăng xuất và đồng bộ lại quyền của người dùng.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordService _passwordService;
    private readonly IRefreshSessionStore _refreshSessionStore;
    private readonly IConfiguration _configuration;

    public UserService(
        IUserRepository userRepository,
        IJwtTokenService jwtTokenService,
        IPasswordService passwordService,
        IRefreshSessionStore refreshSessionStore,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _passwordService = passwordService;
        _refreshSessionStore = refreshSessionStore;
        _configuration = configuration;
    }

    public async Task<ApiResponseDto<AuthResultDto>> AuthenticateAsync(string email, string password)
    {
        // Bước 1: Tìm người dùng theo email.
        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null)
        {
            return ApiResponseDto<AuthResultDto>.Fail("Email hoặc mật khẩu không đúng!", "AUTH_INVALID_CREDENTIALS");
        }

        if (user.TrangThai == false)
        {
            return ApiResponseDto<AuthResultDto>.Fail("Tài khoản của bạn đã bị khóa hoặc vô hiệu hóa.", "AUTH_USER_DISABLED");
        }

        // Bước 2: Kiểm tra mật khẩu.
        // Hỗ trợ giai đoạn chuyển đổi từ plain text sang bcrypt để tránh làm gãy tài khoản cũ.
        if (!_passwordService.VerifyPassword(password, user.MatKhauHash))
        {
            return ApiResponseDto<AuthResultDto>.Fail("Email hoặc mật khẩu không đúng!", "AUTH_INVALID_CREDENTIALS");
        }

        var isLegacyPlainText = !string.IsNullOrWhiteSpace(user.MatKhauHash)
            && !(user.MatKhauHash.StartsWith("$2a$") || user.MatKhauHash.StartsWith("$2b$") || user.MatKhauHash.StartsWith("$2y$"));

        if (isLegacyPlainText)
        {
            // Tự nâng cấp mật khẩu cũ sang hash mạnh ngay sau khi đăng nhập thành công.
            user.MatKhauHash = _passwordService.HashPassword(password);
        }

        user.LanCuoiDangNhap = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        // Bước 3: Lấy vai trò chính của người dùng qua repository.
        var role = await _userRepository.GetUserRoleAsync(user.MaNguoiDung);

        if (role == null)
        {
            return ApiResponseDto<AuthResultDto>.Fail("Tài khoản chưa được gán vai trò.", "AUTH_ROLE_NOT_ASSIGNED");
        }

        string roleName = role.TenVaiTro ?? "User";

        // Bước 4: Xác định ProfileId để FE điều hướng đúng màn hình theo vai trò.
        var profileId = await _userRepository.GetUserProfileIdAsync(user.MaNguoiDung, roleName);

        // Bước 5: Gom danh sách quyền của user theo vai trò.
        var permissionCodes = (await _userRepository.GetUserPermissionsAsync(role.MaVaiTro)).ToList();

        // Bước 6: Phát hành access token + refresh token và tạo session để quản lý refresh.
        var accessToken = _jwtTokenService.GenerateAccessToken(user.MaNguoiDung, user.Email ?? string.Empty, roleName, user.HoTen ?? string.Empty, profileId);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        var refreshDays = int.TryParse(_configuration["JwtSettings:RefreshTokenDays"], out var parsedDays) ? parsedDays : 7;
        var refreshExpiry = DateTime.UtcNow.AddDays(refreshDays);
        var sessionId = _refreshSessionStore.CreateOrReplace(user.MaNguoiDung, refreshToken, refreshExpiry);

        // Bước 7: Trả về response chuẩn hóa để FE dùng chung format xử lý.
        var result = new AuthResultDto
        {
            Id = user.MaNguoiDung,
            FullName = user.HoTen ?? string.Empty,
            Role = roleName,
            ProfileId = profileId,
            Token = accessToken,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            SessionId = sessionId,
            PermissionCodes = permissionCodes
        };

        return ApiResponseDto<AuthResultDto>.Ok(result, "Đăng nhập thành công");
    }

    public async Task<ApiResponseDto<RefreshTokenResultDto>> RefreshAsync(string sessionId, string refreshToken)
    {
        // Bước 1: Kiểm tra dữ liệu đầu vào.
        if (string.IsNullOrWhiteSpace(sessionId) || string.IsNullOrWhiteSpace(refreshToken))
        {
            return ApiResponseDto<RefreshTokenResultDto>.Fail("Thiếu thông tin refresh token.", "AUTH_REFRESH_INVALID_INPUT");
        }

        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();
        var refreshDays = int.TryParse(_configuration["JwtSettings:RefreshTokenDays"], out var parsedDays) ? parsedDays : 7;
        var newRefreshExpiry = DateTime.UtcNow.AddDays(refreshDays);

        // Bước 2: Xác thực session hiện tại và rotate refresh token để giảm rủi ro bị replay.
        var rotated = _refreshSessionStore.TryValidateAndRotate(sessionId, refreshToken, newRefreshToken, newRefreshExpiry, out var userId);
        if (!rotated)
        {
            return ApiResponseDto<RefreshTokenResultDto>.Fail("Refresh token không hợp lệ hoặc đã hết hạn.", "AUTH_REFRESH_INVALID");
        }

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null || user.TrangThai == false)
        {
            return ApiResponseDto<RefreshTokenResultDto>.Fail("Người dùng không tồn tại hoặc đã bị vô hiệu hóa.", "AUTH_USER_NOT_FOUND");
        }

        var role = await _userRepository.GetUserRoleAsync(user.MaNguoiDung);
        var roleName = role?.TenVaiTro ?? "User";
        var profileId = await _userRepository.GetUserProfileIdAsync(user.MaNguoiDung, roleName);

        // Bước 3: Cấp access token mới sau khi rotate thành công.
        var newAccessToken = _jwtTokenService.GenerateAccessToken(user.MaNguoiDung, user.Email ?? string.Empty, roleName, user.HoTen ?? string.Empty, profileId);

        var payload = new RefreshTokenResultDto
        {
            Token = newAccessToken,
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            SessionId = sessionId
        };

        return ApiResponseDto<RefreshTokenResultDto>.Ok(payload, "Làm mới token thành công");
    }

    /// <summary>
    /// Cập nhật lại danh sách quyền của người dùng sau khi admin thay đổi phân quyền.
    /// </summary>
    public async Task<ApiResponseDto<object>> RefreshPermissionsAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return ApiResponseDto<object>.Fail("Người dùng không tồn tại.", "AUTH_USER_NOT_FOUND");
        }

        var role = await _userRepository.GetUserRoleAsync(userId);
        if (role == null)
        {
            return ApiResponseDto<object>.Fail("Tài khoản chưa được gán vai trò.", "AUTH_ROLE_NOT_ASSIGNED");
        }

        var permissionCodes = (await _userRepository.GetUserPermissionsAsync(role.MaVaiTro)).ToList();

        return ApiResponseDto<object>.Ok(new
        {
            userId,
            role = role.TenVaiTro,
            permissionCodes
        }, "Cập nhật quyền thành công");
    }

    /// <summary>
    /// Vô hiệu hóa phiên refresh token của người dùng để kết thúc đăng nhập.
    /// </summary>
    public async Task<ApiResponseDto<object>> LogoutAsync(string sessionId)
    {
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            return ApiResponseDto<object>.Fail("Thiếu sessionId.", "AUTH_LOGOUT_INVALID_INPUT");
        }

        // Xóa session khỏi store để refresh token không còn dùng được nữa.
        _refreshSessionStore.Revoke(sessionId);

        return ApiResponseDto<object>.Ok(null, "Đã đăng xuất thành công");
    }

    public async Task<object> GetUserByIdAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        
        if (user == null)
        {
            return new { success = false, message = "Người dùng không tồn tại." };
        }

        var role = await _userRepository.GetUserRoleAsync(user.MaNguoiDung);

        return new
        {
            success = true,
            data = new
            {
                id = user.MaNguoiDung,
                fullName = user.HoTen,
                email = user.Email,
                role = role?.TenVaiTro ?? "User"
            }
        };
    }
}
