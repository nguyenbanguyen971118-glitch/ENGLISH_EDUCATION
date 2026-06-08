// Services/AdminAttendanceService.cs
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;

namespace backend.Services
{
    public class AdminAttendanceService : IAdminAttendanceService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AdminAttendanceService> _logger;

        public AdminAttendanceService(AppDbContext context, ILogger<AdminAttendanceService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<AttendanceSummaryDto>> GetAllAttendanceSummariesAsync(
            string? classCode = null,
            string? status = null,
            DateOnly? fromDate = null,
            DateOnly? toDate = null)
        {
            // ── Bước 1: Đếm trước bằng 2 query độc lập → tránh N+1 ──────────

            // 1a. Số học sinh đã có bản ghi điểm danh theo MaBuoiHoc
            //     Chú ý: DaXoa là tinyint(1) → bool trong EF Core
            var presentCountDict = await _context.Diemdanhs
                .Where(d => d.DaXoa == false)
                .GroupBy(d => d.MaBuoiHoc)
                .Select(g => new { MaBuoiHoc = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.MaBuoiHoc, x => x.Count);

            // 1b. Tổng học sinh theo MaLopHoc
            var totalCountDict = await _context.Hocsinhlophocs
                .Where(h => h.DaXoa == false)
                .GroupBy(h => h.MaLopHoc)
                .Select(g => new { MaLopHoc = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.MaLopHoc, x => x.Count);

            // ── Bước 2: Query chính ──────────────────────────────────────────
            var query =
                from bh in _context.Buoihocs
                where bh.DaXoa == false

                // Join LopHoc
                join lh in _context.Lophocs
                    on bh.MaLopHoc equals lh.MaLopHoc
                where lh.DaXoa == false

                // Join TietHoc – tiết bắt đầu (MaTiet là int)
                join tietBD in _context.Tiethocs
                    on bh.MaTietBatDau equals tietBD.MaTiet

                // Join TietHoc – tiết kết thúc
                join tietKT in _context.Tiethocs
                    on bh.MaTietKetThuc equals tietKT.MaTiet

                // Left join ChiTietKhoaHoc_LopHoc
                join ctklh in _context.ChitietkhoahocLophocs
                    on lh.MaLopHoc equals ctklh.MaLopHoc into ctklhGroup
                from ctklh in ctklhGroup
                    .Where(x => x.DaXoa == false)
                    .DefaultIfEmpty()

                    // Left join KhoaHoc
                join kh in _context.Khoahocs
                    on ctklh.MaKhoaHoc equals kh.MaKhoaHoc into khGroup
                from kh in khGroup.DefaultIfEmpty()

                    // Left join GiangVienLopHoc
                join gvlh in _context.Giangvienlophocs
                    on lh.MaLopHoc equals gvlh.MaLopHoc into gvlhGroup
                from gvlh in gvlhGroup
                    .Where(x => x.DaXoa == false)
                    .DefaultIfEmpty()

                    // Left join GiangVien
                join gv in _context.Giangviens
                    on gvlh.MaGiangVien equals gv.MaGiangVien into gvGroup
                from gv in gvGroup.DefaultIfEmpty()

                    // Left join NguoiDung (lấy tên GV)
                join nd in _context.Nguoidungs
                    on gv.MaNguoiDung equals nd.MaNguoiDung into ndGroup
                from nd in ndGroup.DefaultIfEmpty()

                    // Bộ lọc tùy chọn
                where (classCode == null || lh.TenLop.Contains(classCode))
                   && (fromDate == null || bh.NgayHoc >= fromDate)
                   && (toDate == null || bh.NgayHoc <= toDate)

                orderby bh.NgayHoc descending, tietBD.GioBatDau

                // Chỉ chiếu các cột cần thiết
                select new
                {
                    bh.MaBuoiHoc,
                    lh.MaLopHoc,
                    lh.TenLop,
                    TenKhoaHoc = kh != null ? kh.TenKhoaHoc : "Chưa xác định",
                    TeacherName = nd != null ? nd.HoTen : "Chưa phân công",
                    bh.NgayHoc,                 // kiểu DateOnly
                    tietBD.GioBatDau,           // kiểu TimeSpan (time)
                    tietKT.GioKetThuc           // kiểu TimeSpan (time)
                };

            var rawList = await query.ToListAsync();

            // ── Bước 3: Map DTO + gắn count từ Dictionary (in-memory O(1)) ──
            var result = rawList
                .Select(x =>
                {
                    var present = presentCountDict.GetValueOrDefault(x.MaBuoiHoc, 0);
                    var total = totalCountDict.GetValueOrDefault(x.MaLopHoc, 0);

                    return new AttendanceSummaryDto
                    {
                        Id = x.MaBuoiHoc.ToString(),
                        ClassCode = x.TenLop,
                        CourseName = x.TenKhoaHoc,
                        Teacher = x.TeacherName,
                        // DateOnly.ToString() cần format trực tiếp
                        Date = x.NgayHoc.ToString("dd/MM/yyyy"),
                        // TimeSpan format: @hh\:mm
                        Time = $"{x.GioBatDau:hh\\:mm} - {x.GioKetThuc:hh\\:mm}",
                        Present = present,
                        Total = total,
                        Status = present > 0 ? "Đã điểm danh" : "Chưa điểm danh"
                    };
                })
                // Lọc status sau khi tính toán (vì phụ thuộc vào present)
                .Where(x => status == null || x.Status == status)
                .ToList();

            return result;
        }
    }
}