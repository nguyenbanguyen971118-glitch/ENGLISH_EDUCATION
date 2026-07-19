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
            new { Ten = "QuanLyNguoiDung",    MoTa = "Quản lý người dùng" },
            new { Ten = "QuanLyLopHoc",        MoTa = "Quản lý lớp học" },
            new { Ten = "QuanLyBaoCao",        MoTa = "Quản lý báo cáo" },
            new { Ten = "QuanLyPhanQuyen",     MoTa = "Quản lý phân quyền" },
            new { Ten = "QuanLyThongBao",      MoTa = "Quản lý thông báo" },
            new { Ten = "QuanLyTinNhan",       MoTa = "Quản lý tin nhắn" },
            new { Ten = "QuanLyDiemDanh",      MoTa = "Quản lý điểm danh" },
            new { Ten = "QuanLyLichHoc",       MoTa = "Quản lý lịch học / lịch dạy" },
            new { Ten = "QuanLyKhoaHoc",       MoTa = "Quản lý khóa học" },
            new { Ten = "QuanLyPhuHuynh",      MoTa = "Quản lý phụ huynh - học sinh" },
            new { Ten = "QuanLyBaiTap",        MoTa = "Quản lý bài tập" },
            new { Ten = "QuanLyNoiDungHoc",    MoTa = "Quản lý nội dung học" }
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
        var fnMap = allFunctions.ToDictionary(x => x.TenChucNang, x => x.MaChucNang);

        int Fn(string name) => fnMap[name];

        var permissionSeeds = new[]
        {
            // ---- Quản lý người dùng ----
            new { MaChucNang = Fn("QuanLyNguoiDung"),  Ten = "PAGE_ADMIN_USERS_VIEW",          MoTa = "Xem trang quản lý người dùng" },
            new { MaChucNang = Fn("QuanLyNguoiDung"),  Ten = "USERS_CREATE",                   MoTa = "Tạo người dùng" },
            new { MaChucNang = Fn("QuanLyNguoiDung"),  Ten = "USERS_EDIT",                     MoTa = "Sửa người dùng" },
            new { MaChucNang = Fn("QuanLyNguoiDung"),  Ten = "USERS_DELETE",                   MoTa = "Xóa người dùng" },

            // ---- Quản lý lớp học ----
            new { MaChucNang = Fn("QuanLyLopHoc"),     Ten = "PAGE_ADMIN_CLASSES_VIEW",        MoTa = "Xem trang quản lý lớp học" },
            new { MaChucNang = Fn("QuanLyLopHoc"),     Ten = "CLASSES_STUDENTS_VIEW",          MoTa = "Xem danh sách học sinh trong lớp" },

            // ---- Quản lý báo cáo ----
            new { MaChucNang = Fn("QuanLyBaoCao"),     Ten = "PAGE_ADMIN_REPORTS_VIEW",        MoTa = "Xem báo cáo Admin" },
            new { MaChucNang = Fn("QuanLyBaoCao"),     Ten = "PAGE_TEACHER_REPORTS_VIEW",      MoTa = "Xem báo cáo giáo viên" },

            // ---- Quản lý phân quyền ----
            new { MaChucNang = Fn("QuanLyPhanQuyen"),  Ten = "PAGE_ADMIN_PERMISSIONS_VIEW",   MoTa = "Xem trang phân quyền" },
            new { MaChucNang = Fn("QuanLyPhanQuyen"),  Ten = "PERMISSIONS_ROLE_CREATE",        MoTa = "Tạo vai trò mới" },
            new { MaChucNang = Fn("QuanLyPhanQuyen"),  Ten = "PERMISSIONS_ROLE_EDIT",          MoTa = "Sửa quyền của vai trò" },
            new { MaChucNang = Fn("QuanLyPhanQuyen"),  Ten = "PERMISSIONS_ROLE_DELETE",        MoTa = "Xóa vai trò" },

            // ---- Quản lý thông báo ----
            new { MaChucNang = Fn("QuanLyThongBao"),   Ten = "NOTIFICATIONS_VIEW_ALL",         MoTa = "Xem toàn bộ thông báo (Admin)" },
            new { MaChucNang = Fn("QuanLyThongBao"),   Ten = "NOTIFICATIONS_CREATE",           MoTa = "Tạo thông báo" },
            new { MaChucNang = Fn("QuanLyThongBao"),   Ten = "NOTIFICATIONS_EDIT",             MoTa = "Sửa thông báo" },
            new { MaChucNang = Fn("QuanLyThongBao"),   Ten = "NOTIFICATIONS_DELETE",           MoTa = "Xóa thông báo" },

            // ---- Quản lý tin nhắn ----
            new { MaChucNang = Fn("QuanLyTinNhan"),    Ten = "MESSAGES_ACCESS",                MoTa = "Truy cập tính năng nhắn tin" },

            // ---- Quản lý điểm danh ----
            new { MaChucNang = Fn("QuanLyDiemDanh"),   Ten = "ATTENDANCE_VIEW",                MoTa = "Xem điểm danh (Admin/Giáo viên)" },
            new { MaChucNang = Fn("QuanLyDiemDanh"),   Ten = "ATTENDANCE_EDIT",                MoTa = "Cập nhật điểm danh (Giáo viên)" },
            new { MaChucNang = Fn("QuanLyDiemDanh"),   Ten = "ATTENDANCE_VIEW_PARENT",         MoTa = "Xem điểm danh con (Phụ huynh)" },
            new { MaChucNang = Fn("QuanLyDiemDanh"),   Ten = "ATTENDANCE_VIEW_STUDENT",        MoTa = "Xem điểm danh của mình (Học sinh)" },
            new { MaChucNang = Fn("QuanLyDiemDanh"),   Ten = "ATTENDANCE_ADMIN_VIEW",          MoTa = "Xem điểm danh tổng hợp (Admin)" },

            // ---- Quản lý lịch học ----
            new { MaChucNang = Fn("QuanLyLichHoc"),    Ten = "PAGE_TEACHER_SCHEDULE_VIEW",    MoTa = "Xem lịch dạy giáo viên" },
            new { MaChucNang = Fn("QuanLyLichHoc"),    Ten = "PAGE_STUDENT_SCHEDULE_VIEW",    MoTa = "Xem lịch học học sinh" },
            new { MaChucNang = Fn("QuanLyLichHoc"),    Ten = "PAGE_PARENT_SCHEDULE_VIEW",     MoTa = "Xem lịch học phụ huynh" },
            new { MaChucNang = Fn("QuanLyLichHoc"),    Ten = "SCHEDULE_CHANGE_REQUEST_ADMIN", MoTa = "Duyệt/từ chối yêu cầu đổi lịch (Admin)" },

            // ---- Quản lý khóa học ----
            new { MaChucNang = Fn("QuanLyKhoaHoc"),    Ten = "COURSES_CREATE",                 MoTa = "Tạo khóa học" },
            new { MaChucNang = Fn("QuanLyKhoaHoc"),    Ten = "COURSES_EDIT",                   MoTa = "Sửa khóa học" },
            new { MaChucNang = Fn("QuanLyKhoaHoc"),    Ten = "COURSES_DELETE",                 MoTa = "Xóa khóa học" },

            // ---- Quản lý phụ huynh ----
            new { MaChucNang = Fn("QuanLyPhuHuynh"),   Ten = "PARENT_CHILDREN_EDIT",          MoTa = "Gán/xóa học sinh cho phụ huynh" },

            // ---- Quản lý bài tập ----
            new { MaChucNang = Fn("QuanLyBaiTap"),     Ten = "ASSIGNMENTS_ADMIN_VIEW",         MoTa = "Xem bài tập (Admin)" },
            new { MaChucNang = Fn("QuanLyBaiTap"),     Ten = "ASSIGNMENTS_STUDENT_ACCESS",     MoTa = "Truy cập bài tập (Học sinh)" },

            // ---- Quản lý nội dung học ----
            new { MaChucNang = Fn("QuanLyNoiDungHoc"), Ten = "STUDY_CONTENT_ADMIN",            MoTa = "Quản lý nội dung (Admin)" },
            new { MaChucNang = Fn("QuanLyNoiDungHoc"), Ten = "STUDY_CONTENT_TEACHER",          MoTa = "Quản lý nội dung (Giáo viên)" },
            new { MaChucNang = Fn("QuanLyNoiDungHoc"), Ten = "STUDY_CONTENT_STUDENT",          MoTa = "Xem nội dung học (Học sinh)" }
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
                // Admin có toàn bộ quyền trong hệ thống.
                "Admin" => allPermissions.Select(x => x.TenQuyen).ToHashSet(),

                "Giao_Vien" => new HashSet<string>
                {
                    "PAGE_TEACHER_SCHEDULE_VIEW",
                    "PAGE_TEACHER_REPORTS_VIEW",
                    "ATTENDANCE_VIEW",
                    "ATTENDANCE_EDIT",
                    "STUDY_CONTENT_TEACHER",
                    "MESSAGES_ACCESS",
                    "CLASSES_STUDENTS_VIEW"
                },

                "Hoc_Sinh" => new HashSet<string>
                {
                    "PAGE_STUDENT_SCHEDULE_VIEW",
                    "ATTENDANCE_VIEW_STUDENT",
                    "ASSIGNMENTS_STUDENT_ACCESS",
                    "STUDY_CONTENT_STUDENT"
                },

                "Phu_Huynh" => new HashSet<string>
                {
                    "PAGE_PARENT_SCHEDULE_VIEW",
                    "ATTENDANCE_VIEW_PARENT",
                    "MESSAGES_ACCESS"
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
