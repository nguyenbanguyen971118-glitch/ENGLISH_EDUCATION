using backend.Data;
using backend.DTOs;
using backend.Helpers;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

// Controller phục vụ toàn bộ luồng hồ sơ giảng viên:
// xem profile, cập nhật thông tin, upload avatar, xóa avatar.
[Route("api/[controller]")]
[ApiController]
[Authorize] // Profile cá nhân - chỉ cần đăng nhập, bảo vệ bằng userId từ JWT
public class GiangVienController : ControllerBase
{
    private const long MaxAvatarFileSize = 5 * 1024 * 1024;
    private static readonly string[] AllowedAvatarExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];

    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public GiangVienController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    // Lấy hồ sơ của chính giảng viên đang đăng nhập.
    [HttpGet("my-profile")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized(ApiResponseDto<object>.Fail("Bạn chưa đăng nhập.", "AUTH_INVALID_TOKEN"));
        }

        var user = await _context.Nguoidungs
            .AsNoTracking()
            .Include(n => n.Giangvien)
            .FirstOrDefaultAsync(n => n.MaNguoiDung == userId.Value && n.DaXoa != true);

        if (user == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Không tìm thấy thông tin giảng viên.", "TEACHER_PROFILE_NOT_FOUND"));
        }

        var profile = BuildProfileDto(user, user.Giangvien != null && user.Giangvien.DaXoa != true ? user.Giangvien : null);
        return Ok(ApiResponseDto<ProfileGiangVienDTO>.Ok(profile, "Lấy thông tin cá nhân thành công."));
    }

    // Cập nhật thông tin hồ sơ giảng viên; nếu chưa có bản ghi giangvien thì tạo mới.
    [HttpPut("update-profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileGiangVienDTO dto)
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized(ApiResponseDto<object>.Fail("Bạn chưa đăng nhập.", "AUTH_INVALID_TOKEN"));
        }

        if (string.IsNullOrWhiteSpace(dto.HoTen))
        {
            return BadRequest(ApiResponseDto<object>.Fail("Họ và tên là bắt buộc.", "TEACHER_PROFILE_NAME_REQUIRED"));
        }

        var user = await _context.Nguoidungs
            .Include(n => n.Giangvien)
            .FirstOrDefaultAsync(n => n.MaNguoiDung == userId.Value && n.DaXoa != true);

        if (user == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Không tìm thấy người dùng.", "USER_NOT_FOUND"));
        }

        var now = DateTime.UtcNow;
        user.HoTen = dto.HoTen.Trim();
        user.AnhDaiDien = NormalizeOptional(dto.AnhDaiDien);
        user.NguoiSua = userId.Value;
        user.ThoiGianSua = now;

        if (user.Giangvien == null)
        {
            user.Giangvien = new Giangvien
            {
                MaGiangVien = Guid.NewGuid(),
                MaNguoiDung = user.MaNguoiDung,
                SoDienThoai = NormalizeOptional(dto.SoDienThoai),
                QueQuan = NormalizeOptional(dto.QueQuan),
                TrinhDoChuyenMon = NormalizeRequired(dto.TrinhDoChuyenMon),
                HocVi = NormalizeOptional(dto.HocVi),
                KinhNghiemGiangDay = NormalizeOptional(dto.KinhNghiemGiangDay),
                TrangThai = true,
                DaXoa = false,
                NguoiTao = userId.Value,
                ThoiGianTao = now
            };

            _context.Giangviens.Add(user.Giangvien);
        }
        else
        {
            user.Giangvien.SoDienThoai = NormalizeOptional(dto.SoDienThoai);
            user.Giangvien.QueQuan = NormalizeOptional(dto.QueQuan);
            user.Giangvien.TrinhDoChuyenMon = NormalizeRequired(dto.TrinhDoChuyenMon);
            user.Giangvien.HocVi = NormalizeOptional(dto.HocVi);
            user.Giangvien.KinhNghiemGiangDay = NormalizeOptional(dto.KinhNghiemGiangDay);
            user.Giangvien.TrangThai = true;
            user.Giangvien.DaXoa = false;
            user.Giangvien.NguoiSua = userId.Value;
            user.Giangvien.ThoiGianSua = now;
        }

        await _context.SaveChangesAsync();

        var updatedProfile = BuildProfileDto(user, user.Giangvien);
        return Ok(ApiResponseDto<ProfileGiangVienDTO>.Ok(updatedProfile, "Cập nhật thông tin thành công."));
    }

    // Upload ảnh đại diện và lưu đường dẫn vào NguoiDung để sidebar/profile dùng chung.
    [HttpPost("upload-avatar")]
    public async Task<IActionResult> UploadAvatar([FromForm] IFormFile file)
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized(ApiResponseDto<object>.Fail("Bạn chưa đăng nhập.", "AUTH_INVALID_TOKEN"));
        }

        if (file == null || file.Length == 0)
        {
            return BadRequest(ApiResponseDto<object>.Fail("Chưa chọn file ảnh.", "AVATAR_FILE_REQUIRED"));
        }

        if (file.Length > MaxAvatarFileSize)
        {
            return BadRequest(ApiResponseDto<object>.Fail("Dung lượng ảnh không vượt quá 5MB.", "AVATAR_FILE_TOO_LARGE"));
        }

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedAvatarExtensions.Contains(fileExtension))
        {
            return BadRequest(ApiResponseDto<object>.Fail("Chỉ chấp nhận file ảnh jpg, jpeg, png, gif hoặc webp.", "AVATAR_INVALID_EXTENSION"));
        }

        var user = await _context.Nguoidungs
            .FirstOrDefaultAsync(n => n.MaNguoiDung == userId.Value && n.DaXoa != true);

        if (user == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Không tìm thấy người dùng.", "USER_NOT_FOUND"));
        }

        var webRootPath = EnsureWebRootPath();
        var avatarsFolderPath = Path.Combine(webRootPath, "uploads", "avatars");
        Directory.CreateDirectory(avatarsFolderPath);

        var now = DateTime.UtcNow;
        await CleanupCurrentAvatarAsync(user, userId.Value, now, webRootPath);

        var fileName = $"avatar_{userId.Value:N}_{Guid.NewGuid():N}{fileExtension}";
        var physicalPath = Path.Combine(avatarsFolderPath, fileName);
        var relativePath = $"/uploads/avatars/{fileName}";

        await using (var stream = new FileStream(physicalPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        user.AnhDaiDien = relativePath;
        user.NguoiSua = userId.Value;
        user.ThoiGianSua = now;

        _context.Tainguyenluutrus.Add(new Tainguyenluutru
        {
            MaTaiNguyen = Guid.NewGuid(),
            MaNguoiDung = userId.Value,
            TenTaiNguyen = fileName,
            Link = relativePath,
            NguoiTao = userId.Value,
            ThoiGianTao = now,
            TrangThai = true,
            DaXoa = false
        });

        await _context.SaveChangesAsync();

        return Ok(ApiResponseDto<object>.Ok(
            new { avatarUrl = relativePath },
            "Upload ảnh đại diện thành công."
        ));
    }

    // Xóa avatar hiện tại cả ở storage lẫn dữ liệu liên kết trong hệ thống.
    [HttpDelete("delete-avatar")]
    public async Task<IActionResult> DeleteAvatar()
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized(ApiResponseDto<object>.Fail("Bạn chưa đăng nhập.", "AUTH_INVALID_TOKEN"));
        }

        var user = await _context.Nguoidungs
            .FirstOrDefaultAsync(n => n.MaNguoiDung == userId.Value && n.DaXoa != true);

        if (user == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Không tìm thấy người dùng.", "USER_NOT_FOUND"));
        }

        var now = DateTime.UtcNow;
        var webRootPath = EnsureWebRootPath();

        await CleanupCurrentAvatarAsync(user, userId.Value, now, webRootPath);

        user.AnhDaiDien = null;
        user.NguoiSua = userId.Value;
        user.ThoiGianSua = now;

        await _context.SaveChangesAsync();

        return Ok(ApiResponseDto<object>.Ok(
            new { avatarUrl = string.Empty },
            "Xóa ảnh đại diện thành công."
        ));
    }

    // Dọn file avatar cũ và đánh dấu mềm các resource cũ để tránh dữ liệu rác.
    private async Task CleanupCurrentAvatarAsync(Nguoidung user, Guid userId, DateTime now, string webRootPath)
    {
        if (string.IsNullOrWhiteSpace(user.AnhDaiDien))
        {
            return;
        }

        var currentAvatarPath = user.AnhDaiDien.Trim();

        if (IsStoredUploadPath(currentAvatarPath))
        {
            var relativePhysicalPath = currentAvatarPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var physicalPath = Path.Combine(webRootPath, relativePhysicalPath);

            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }
        }

        var previousResources = await _context.Tainguyenluutrus
            .Where(x => x.MaNguoiDung == userId && x.Link == currentAvatarPath && x.DaXoa != true)
            .ToListAsync();

        foreach (var resource in previousResources)
        {
            resource.DaXoa = true;
            resource.TrangThai = false;
            resource.NguoiSua = userId;
            resource.ThoiGianSua = now;
        }
    }

    private string EnsureWebRootPath()
    {
        var webRootPath = _environment.WebRootPath;
        if (!string.IsNullOrWhiteSpace(webRootPath))
        {
            return webRootPath;
        }

        webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
        if (!Directory.Exists(webRootPath))
        {
            Directory.CreateDirectory(webRootPath);
        }

        return webRootPath;
    }

    private static bool IsStoredUploadPath(string path)
    {
        return path.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase)
            || path.StartsWith("uploads/", StringComparison.OrdinalIgnoreCase);
    }

    // Gom dữ liệu từ bảng NguoiDung và Giangvien thành payload trả về frontend.
    private static ProfileGiangVienDTO BuildProfileDto(Nguoidung user, Giangvien? teacherProfile)
    {
        return new ProfileGiangVienDTO
        {
            HoTen = user.HoTen ?? string.Empty,
            Email = user.Email ?? string.Empty,
            AnhDaiDien = user.AnhDaiDien ?? string.Empty,
            SoDienThoai = teacherProfile?.SoDienThoai ?? string.Empty,
            QueQuan = teacherProfile?.QueQuan ?? string.Empty,
            TrinhDoChuyenMon = teacherProfile?.TrinhDoChuyenMon ?? string.Empty,
            HocVi = teacherProfile?.HocVi ?? string.Empty,
            KinhNghiemGiangDay = teacherProfile?.KinhNghiemGiangDay ?? string.Empty
        };
    }

    private static string NormalizeRequired(string? value)
    {
        return value?.Trim() ?? string.Empty;
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
