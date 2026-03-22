using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class PermissionBootstrapService
{
    private readonly AppDbContext _context;

    public PermissionBootstrapService(AppDbContext context)
    {
        _context = context;
    }

    public async Task EnsureInitializedAsync()
    {
        var functionSeeds = new[]
        {
            new { Ten = "QuanLyNguoiDung", MoTa = "Quản lý người dùng" },
            new { Ten = "QuanLyLopHoc", MoTa = "Quản lý lớp học" },
            new { Ten = "QuanLyBaoCao", MoTa = "Quản lý báo cáo" }
        };

        foreach (var fn in functionSeeds)
        {
            var exists = await _context.Chucnangs.AnyAsync(x => x.TenChucNang == fn.Ten);
            if (!exists)
            {
                _context.Chucnangs.Add(new Chucnang
                {
                    TenChucNang = fn.Ten,
                    MoTa = fn.MoTa,
                    TrangThai = true,
                    DaXoa = false
                });
            }
        }

        await _context.SaveChangesAsync();

        var allFunctions = await _context.Chucnangs.ToListAsync();
        var userFn = allFunctions.First(x => x.TenChucNang == "QuanLyNguoiDung");
        var classFn = allFunctions.First(x => x.TenChucNang == "QuanLyLopHoc");
        var reportFn = allFunctions.First(x => x.TenChucNang == "QuanLyBaoCao");

        var permissionSeeds = new[]
        {
            new { MaChucNang = userFn.MaChucNang, Ten = "PAGE_ADMIN_USERS_VIEW", MoTa = "Xem trang quản lý người dùng" },
            new { MaChucNang = userFn.MaChucNang, Ten = "USERS_CREATE", MoTa = "Tạo người dùng" },
            new { MaChucNang = userFn.MaChucNang, Ten = "USERS_EDIT", MoTa = "Sửa người dùng" },
            new { MaChucNang = userFn.MaChucNang, Ten = "USERS_DELETE", MoTa = "Xóa người dùng" },
            new { MaChucNang = classFn.MaChucNang, Ten = "PAGE_ADMIN_CLASSES_VIEW", MoTa = "Xem trang quản lý lớp học" },
            new { MaChucNang = classFn.MaChucNang, Ten = "PAGE_TEACHER_SCHEDULE_VIEW", MoTa = "Xem lịch dạy giáo viên" },
            new { MaChucNang = classFn.MaChucNang, Ten = "PAGE_STUDENT_SCHEDULE_VIEW", MoTa = "Xem lịch học học sinh" },
            new { MaChucNang = classFn.MaChucNang, Ten = "PAGE_PARENT_SCHEDULE_VIEW", MoTa = "Xem lịch học phụ huynh" },
            new { MaChucNang = reportFn.MaChucNang, Ten = "PAGE_ADMIN_REPORTS_VIEW", MoTa = "Xem báo cáo" }
        };

        foreach (var p in permissionSeeds)
        {
            var exists = await _context.Quyens.AnyAsync(x => x.TenQuyen == p.Ten);
            if (!exists)
            {
                _context.Quyens.Add(new Quyen
                {
                    MaChucNang = p.MaChucNang,
                    TenQuyen = p.Ten,
                    MoTa = p.MoTa,
                    TrangThai = true,
                    DaXoa = false
                });
            }
        }

        await _context.SaveChangesAsync();

        var allPermissions = await _context.Quyens.ToListAsync();
        var allRoles = await _context.Vaitros.ToListAsync();

        foreach (var role in allRoles)
        {
            var allowCodes = role.TenVaiTro switch
            {
                "Admin" => allPermissions.Select(x => x.TenQuyen).ToHashSet(),
                "Giao_Vien" => new HashSet<string>
                {
                    "PAGE_TEACHER_SCHEDULE_VIEW",
                    "PAGE_ADMIN_REPORTS_VIEW"
                },
                "Hoc_Sinh" => new HashSet<string>
                {
                    "PAGE_STUDENT_SCHEDULE_VIEW"
                },
                "Phu_Huynh" => new HashSet<string>
                {
                    "PAGE_PARENT_SCHEDULE_VIEW"
                },
                _ => new HashSet<string>()
            };

            var permissionIds = allPermissions
                .Where(x => allowCodes.Contains(x.TenQuyen))
                .Select(x => x.MaQuyen)
                .ToList();

            foreach (var permissionId in permissionIds)
            {
                var exists = await _context.Vaitroquyens.AnyAsync(x =>
                    x.MaVaiTro == role.MaVaiTro && x.MaQuyen == permissionId);

                if (!exists)
                {
                    _context.Vaitroquyens.Add(new Vaitroquyen
                    {
                        MaVaiTro = role.MaVaiTro,
                        MaQuyen = permissionId,
                        TrangThai = true,
                        DaXoa = false
                    });
                }
            }
        }

        await _context.SaveChangesAsync();
    }
}
