using backend.Data;
using backend.DTOs;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly AppDbContext _context;

    public UserService(IUserRepository userRepository, AppDbContext context)
    {
        _userRepository = userRepository;
        _context = context;
    }

    public async Task<object> AuthenticateAsync(LoginDto loginData)
    {
        // 1. Tìm người dùng theo email
        var user = await _userRepository.GetByEmailAsync(loginData.Email);

        if (user == null)
        {
            return new { success = false, message = "Email hoặc mật khẩu không đúng!" };
        }

        // 2. Verify password (current implementation stores plain text, should use hashing)
        if (user.MatKhauHash != loginData.Password)
        {
            return new { success = false, message = "Email hoặc mật khẩu không đúng!" };
        }

        // 3. Lấy vai trò đầu tiên của user
        var role = await _context.Nguoidungvaitros
            .Where(x => x.MaNguoiDung == user.MaNguoiDung && (x.DaXoa == null || x.DaXoa == false))
            .Select(x => x.MaVaiTroNavigation)
            .FirstOrDefaultAsync();

        if (role == null)
        {
            return new { success = false, message = "Tài khoản chưa được gán vai trò." };
        }

        string roleName = role.TenVaiTro ?? "User";

        // 4. Tìm ProfileID tương ứng
        Guid? profileId = null;
        if (roleName == "Hoc_Sinh")
        {
            var hs = await _context.Hocsinhs.FirstOrDefaultAsync(h => h.MaNguoiDung == user.MaNguoiDung);
            profileId = hs?.MaHocSinh;
        }
        else if (roleName == "Giao_Vien")
        {
            var gv = await _context.Giangviens.FirstOrDefaultAsync(g => g.MaNguoiDung == user.MaNguoiDung);
            profileId = gv?.MaGiangVien;
        }

        // 5. Lấy danh sách permissions
        var permissionCodes = await _context.Vaitroquyens
            .Where(x => x.MaVaiTro == role.MaVaiTro && (x.DaXoa == null || x.DaXoa == false))
            .Select(x => x.MaQuyenNavigation.TenQuyen)
            .ToListAsync();

        // 6. Trả về dữ liệu đầy đủ
        return new
        {
            success = true,
            data = new
            {
                id = user.MaNguoiDung,
                fullName = user.HoTen,
                role = roleName,
                profileId = profileId,
                token = "dummy_token_" + Guid.NewGuid().ToString(),
                permissionCodes = permissionCodes
            }
        };
    }

    public async Task<object> GetUserByIdAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        
        if (user == null)
        {
            return new { success = false, message = "Người dùng không tồn tại." };
        }

        var role = await _context.Nguoidungvaitros
            .Where(x => x.MaNguoiDung == user.MaNguoiDung && (x.DaXoa == null || x.DaXoa == false))
            .Select(x => x.MaVaiTroNavigation)
            .FirstOrDefaultAsync();

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
