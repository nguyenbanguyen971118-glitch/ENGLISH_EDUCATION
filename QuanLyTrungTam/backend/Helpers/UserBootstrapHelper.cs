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

        if (mappingCount < RequiredSeedUsers.Length)
        {
            return false;
        }

        var teacherUser = await _context.Nguoidungs.FirstOrDefaultAsync(x =>
            x.Email == "giaovien@qltt.local" && (x.DaXoa == null || x.DaXoa == false));
        var studentUser = await _context.Nguoidungs.FirstOrDefaultAsync(x =>
            x.Email == "hocsinh@qltt.local" && (x.DaXoa == null || x.DaXoa == false));
        var parentUser = await _context.Nguoidungs.FirstOrDefaultAsync(x =>
            x.Email == "phuhuynh@qltt.local" && (x.DaXoa == null || x.DaXoa == false));

        var teacherProfileExists = teacherUser != null && await _context.Giangviens.AnyAsync(x =>
            x.MaNguoiDung == teacherUser.MaNguoiDung && (x.DaXoa == null || x.DaXoa == false));
        var studentProfileExists = studentUser != null && await _context.Hocsinhs.AnyAsync(x =>
            x.MaNguoiDung == studentUser.MaNguoiDung && (x.DaXoa == null || x.DaXoa == false));
        var parentProfileExists = parentUser != null && await _context.Phuhuynhs.AnyAsync(x =>
            x.MaNguoiDung == parentUser.MaNguoiDung && (x.DaXoa == null || x.DaXoa == false));

        return teacherProfileExists && studentProfileExists && parentProfileExists;
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

            if (seed.TenVaiTro == "Giao_Vien")
            {
                var teacherProfile = await _context.Giangviens
                    .FirstOrDefaultAsync(x => x.MaNguoiDung == user.MaNguoiDung);

                if (teacherProfile == null)
                {
                    _context.Giangviens.Add(new Giangvien
                    {
                        MaGiangVien = Guid.NewGuid(),
                        MaNguoiDung = user.MaNguoiDung,
                        SoDienThoai = null,
                        QueQuan = null,
                        TrinhDoChuyenMon = string.Empty,
                        HocVi = null,
                        KinhNghiemGiangDay = null,
                        TrangThai = true,
                        DaXoa = false,
                        ThoiGianTao = DateTime.UtcNow
                    });
                }
                else if (teacherProfile.DaXoa == true)
                {
                    teacherProfile.DaXoa = false;
                    teacherProfile.TrangThai = true;
                    teacherProfile.ThoiGianSua = DateTime.UtcNow;
                }
            }
        }

        await _context.SaveChangesAsync();

        // Create GiangVien profile for teacher seed user if it doesn't exist
        var teacherUser = await _context.Nguoidungs.FirstOrDefaultAsync(x =>
            x.Email == "giaovien@qltt.local" && (x.DaXoa == null || x.DaXoa == false));

        if (teacherUser != null)
        {
            var existingTeacherProfile = await _context.Giangviens.FirstOrDefaultAsync(x =>
                x.MaNguoiDung == teacherUser.MaNguoiDung && (x.DaXoa == null || x.DaXoa == false));

            if (existingTeacherProfile == null)
            {
                var giangvien = new Giangvien
                {
                    MaGiangVien = Guid.NewGuid(),
                    MaNguoiDung = teacherUser.MaNguoiDung,
                    SoDienThoai = null,
                    QueQuan = null,
                    TrinhDoChuyenMon = "Chưa cập nhật",
                    HocVi = null,
                    KinhNghiemGiangDay = null,
                    TrangThai = true,
                    DaXoa = false,
                    ThoiGianTao = DateTime.UtcNow
                };

                _context.Giangviens.Add(giangvien);
                await _context.SaveChangesAsync();
            }
        }

        var studentUser = await _context.Nguoidungs.FirstOrDefaultAsync(x =>
            x.Email == "hocsinh@qltt.local" && (x.DaXoa == null || x.DaXoa == false));

        if (studentUser != null)
        {
            var existingStudentProfile = await _context.Hocsinhs.FirstOrDefaultAsync(x =>
                x.MaNguoiDung == studentUser.MaNguoiDung && (x.DaXoa == null || x.DaXoa == false));

            if (existingStudentProfile == null)
            {
                var hocsinh = new Hocsinh
                {
                    MaHocSinh = Guid.NewGuid(),
                    MaNguoiDung = studentUser.MaNguoiDung,
                    TruongDangTheoHoc = "Chưa cập nhật",
                    TrangThai = true,
                    DaXoa = false,
                    ThoiGianTao = DateTime.UtcNow
                };

                _context.Hocsinhs.Add(hocsinh);
                await _context.SaveChangesAsync();
            }
        }

        var parentUser = await _context.Nguoidungs.FirstOrDefaultAsync(x =>
            x.Email == "phuhuynh@qltt.local" && (x.DaXoa == null || x.DaXoa == false));

        if (parentUser != null)
        {
            var existingParentProfile = await _context.Phuhuynhs.FirstOrDefaultAsync(x =>
                x.MaNguoiDung == parentUser.MaNguoiDung && (x.DaXoa == null || x.DaXoa == false));

            if (existingParentProfile == null)
            {
                var phuhuynh = new Phuhuynh
                {
                    MaPhuHuynh = Guid.NewGuid(),
                    MaNguoiDung = parentUser.MaNguoiDung,
                    SoDienThoai = "0000000000",
                    TrangThai = true,
                    DaXoa = false,
                    ThoiGianTao = DateTime.UtcNow
                };

                _context.Phuhuynhs.Add(phuhuynh);
                await _context.SaveChangesAsync();
            }
        }
    }
}
