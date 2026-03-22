using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginData)
        {
            // 1. Tìm người dùng theo schema mới
            var user = await _context.Nguoidungs
                .FirstOrDefaultAsync(u => u.Email == loginData.Email && u.MatKhauHash == loginData.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng!" });
            }

            // 2. Lấy vai trò đầu tiên của user
            var role = await _context.Nguoidungvaitros
                .Where(x => x.MaNguoiDung == user.MaNguoiDung && (x.DaXoa == null || x.DaXoa == false))
                .Select(x => x.MaVaiTroNavigation)
                .FirstOrDefaultAsync();

            string roleName = role?.TenVaiTro ?? "User";

            // 3. Tìm ProfileID tương ứng để Frontend dùng gọi API dữ liệu
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

            if (role == null)
            {
                return Unauthorized(new { message = "Tài khoản chưa được gán vai trò." });
            }

            var permissionCodes = await _context.Vaitroquyens
                .Where(x => x.MaVaiTro == role.MaVaiTro && (x.DaXoa == null || x.DaXoa == false))
                .Select(x => x.MaQuyenNavigation.TenQuyen)
                .ToListAsync();

            // 4. Trả về Object đầy đủ cho React
            return Ok(new
            {
                id = user.MaNguoiDung,
                fullName = user.HoTen,
                role = roleName, 
                profileId = profileId, // Cực kỳ quan trọng cho Dashboard
                token = "dummy_token_" + Guid.NewGuid().ToString(), // Sau này thay bằng JWT
                permissionCodes
            });
        }
    }

    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}