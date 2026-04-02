using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Helpers;

public class UserBootstrapHelper
{
    private readonly AppDbContext _context;
    private static readonly string[] RequiredRoles =
    {
        "Admin",
        "Giao_Vien",
        "Hoc_Sinh",
        "Phu_Huynh"
    };

    private static readonly string[] RequiredSeedUsers =
    {
        "admin@qltt.local",
        "giaovien@qltt.local",
        "hocsinh@qltt.local",
        "phuhuynh@qltt.local"
    };

    public UserBootstrapHelper(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsInitializedAsync()
    {
        var existingRoles = await _context.Vaitros
            .Where(x => RequiredRoles.Contains(x.TenVaiTro) && (x.DaXoa == null || x.DaXoa == false))
            .Select(x => x.TenVaiTro)
            .ToListAsync();

        if (existingRoles.Count != RequiredRoles.Length)
        {
            return false;
        }

        var existingSeedUsers = await _context.Nguoidungs
            .Where(x => RequiredSeedUsers.Contains(x.Email) && (x.DaXoa == null || x.DaXoa == false))
            .Select(x => x.MaNguoiDung)
            .ToListAsync();

        if (existingSeedUsers.Count != RequiredSeedUsers.Length)
        {
            return false;
        }

        var mappingCount = await _context.Nguoidungvaitros.CountAsync(x =>
            existingSeedUsers.Contains(x.MaNguoiDung) &&
            (x.DaXoa == null || x.DaXoa == false));

        return mappingCount >= RequiredSeedUsers.Length;
    }

    public async Task EnsureInitializedAsync()
    {
        var roleSeeds = new[]
        {
            new { TenVaiTro = "Admin", LoaiTaiKhoan = (sbyte)1 },
            new { TenVaiTro = "Giao_Vien", LoaiTaiKhoan = (sbyte)2 },
            new { TenVaiTro = "Hoc_Sinh", LoaiTaiKhoan = (sbyte)3 },
            new { TenVaiTro = "Phu_Huynh", LoaiTaiKhoan = (sbyte)4 }
        };

        foreach (var roleSeed in roleSeeds)
        {
            var roleExists = await _context.Vaitros.AnyAsync(x => x.TenVaiTro == roleSeed.TenVaiTro);
            if (!roleExists)
            {
                _context.Vaitros.Add(new Vaitro
                {
                    TenVaiTro = roleSeed.TenVaiTro,
                    TrangThai = true,
                    DaXoa = false
                });
            }
        }

        await _context.SaveChangesAsync();

        var roleMap = await _context.Vaitros
            .Where(x => roleSeeds.Select(r => r.TenVaiTro).Contains(x.TenVaiTro))
            .ToDictionaryAsync(x => x.TenVaiTro, x => x.MaVaiTro);

        var userSeeds = new[]
        {
            new
            {
                TenDangNhap = "admin",
                MatKhauCu = "admin123",
                MatKhau = "QlttAdmin@2026!A9",
                HoTen = "Tai khoan Admin",
                Email = "admin@qltt.local",
                TenVaiTro = "Admin",
                LoaiTaiKhoan = (sbyte)1
            },
            new
            {
                TenDangNhap = "giaovien",
                MatKhauCu = "giaovien123",
                MatKhau = "QlttTeacher@2026!G7",
                HoTen = "Tai khoan Giao Vien",
                Email = "giaovien@qltt.local",
                TenVaiTro = "Giao_Vien",
                LoaiTaiKhoan = (sbyte)2
            },
            new
            {
                TenDangNhap = "hocsinh",
                MatKhauCu = "hocsinh123",
                MatKhau = "QlttStudent@2026!S5",
                HoTen = "Tai khoan Hoc Sinh",
                Email = "hocsinh@qltt.local",
                TenVaiTro = "Hoc_Sinh",
                LoaiTaiKhoan = (sbyte)3
            },
            new
            {
                TenDangNhap = "phuhuynh",
                MatKhauCu = "phuhuynh123",
                MatKhau = "QlttParent@2026!P6",
                HoTen = "Tai khoan Phu Huynh",
                Email = "phuhuynh@qltt.local",
                TenVaiTro = "Phu_Huynh",
                LoaiTaiKhoan = (sbyte)4
            }
        };

        foreach (var seed in userSeeds)
        {
            var user = await _context.Nguoidungs.FirstOrDefaultAsync(x =>
                x.TenDangNhap == seed.TenDangNhap || x.Email == seed.Email);

            if (user == null)
            {
                user = new Nguoidung
                {
                    MaNguoiDung = Guid.NewGuid(),
                    TenDangNhap = seed.TenDangNhap,
                    MatKhauHash = seed.MatKhau,
                    HoTen = seed.HoTen,
                    Email = seed.Email,
                    LoaiTaiKhoan = seed.LoaiTaiKhoan,
                    TrangThai = true,
                    DaXoa = false,
                    DaXacMinhEmail = true,
                    ThoiGianTao = DateTime.UtcNow
                };

                _context.Nguoidungs.Add(user);
                await _context.SaveChangesAsync();
            }
            else if (user.MatKhauHash == seed.MatKhauCu)
            {
                // Upgrade weak default passwords to stronger seeded values.
                user.MatKhauHash = seed.MatKhau;
                user.ThoiGianSua = DateTime.UtcNow;
                _context.Nguoidungs.Update(user);
                await _context.SaveChangesAsync();
            }

            if (!roleMap.TryGetValue(seed.TenVaiTro, out var roleId))
            {
                continue;
            }

            var hasMapping = await _context.Nguoidungvaitros.AnyAsync(x =>
                x.MaNguoiDung == user.MaNguoiDung && x.MaVaiTro == roleId);

            if (!hasMapping)
            {
                _context.Nguoidungvaitros.Add(new Nguoidungvaitro
                {
                    MaNguoiDung = user.MaNguoiDung,
                    MaVaiTro = roleId,
                    TrangThai = true,
                    DaXoa = false,
                    ThoiGianTao = DateTime.UtcNow
                });
            }
        }

        await _context.SaveChangesAsync();
    }
}