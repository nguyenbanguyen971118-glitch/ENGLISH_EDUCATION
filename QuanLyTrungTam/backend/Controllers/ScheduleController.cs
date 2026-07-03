using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Helpers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using backend.Services.Interfaces;

namespace backend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IScheduleChangeRequestService _scheduleChangeRequestService;
        private readonly Microsoft.Extensions.Logging.ILogger<ScheduleController> _logger;

        public ScheduleController(AppDbContext db, IScheduleChangeRequestService scheduleChangeRequestService, Microsoft.Extensions.Logging.ILogger<ScheduleController> logger)
        {
            _db = db;
            _scheduleChangeRequestService = scheduleChangeRequestService;
            _logger = logger;
        }

        private static DateOnly GetWeekStart(DateOnly date)
        {
            var diff = ((int)date.DayOfWeek + 6) % 7;
            return date.AddDays(-diff);
        }

        private static int GetDayIndex(DateOnly weekStart, DateOnly date)
        {
            return date.DayNumber - weekStart.DayNumber;
        }

        private async Task<string> ResolveTeacherNameAsync(Guid maLopHoc)
        {
            var teacher = await _db.Giangvienlophocs
                .Where(g => g.MaLopHoc == maLopHoc && (g.DaXoa == null || g.DaXoa == false) && (g.TrangThai == null || g.TrangThai == true))
                .Select(g => g.MaGiangVienNavigation.MaNguoiDungNavigation.HoTen)
                .FirstOrDefaultAsync();

            return teacher ?? "Chưa phân công";
        }

        private async Task<Guid?> ResolveTeacherIdAsync(Guid maLopHoc)
        {
            return await _db.Giangvienlophocs
                .Where(g => g.MaLopHoc == maLopHoc && (g.DaXoa == null || g.DaXoa == false) && (g.TrangThai == null || g.TrangThai == true))
                .Select(g => (Guid?)g.MaGiangVien)
                .FirstOrDefaultAsync();
        }

        private async Task<string> ResolveRoomNameAsync(Guid? maPhongHoc)
        {
            if (maPhongHoc == null)
            {
                return "Chưa gán phòng";
            }

            var room = await _db.Phonghocs
                .Where(p => p.MaPhongHoc == maPhongHoc.Value)
                .Select(p => p.TenPhong)
                .FirstOrDefaultAsync();

            return room ?? "Chưa gán phòng";
        }

        private async Task EnsureTietHocSeedAsync()
        {
            var existing = await _db.Tiethocs.Select(t => t.MaTiet).ToListAsync();
            var slots = new (int Id, string Name, string Start, string End)[]
            {
                (1, "Tiet 1", "07:00:00", "07:50:00"),
                (2, "Tiet 2", "07:55:00", "08:45:00"),
                (3, "Tiet 3", "08:50:00", "09:40:00"),
                (4, "Tiet 4", "09:50:00", "10:40:00"),
                (5, "Tiet 5", "10:45:00", "11:35:00"),
                (6, "Tiet 6", "12:30:00", "13:20:00"),
                (7, "Tiet 7", "13:25:00", "14:15:00"),
                (8, "Tiet 8", "14:20:00", "15:10:00"),
                (9, "Tiet 9", "15:20:00", "16:10:00"),
                (10, "Tiet 10", "16:15:00", "17:05:00"),
                (11, "Tiet 11", "17:30:00", "18:20:00"),
                (12, "Tiet 12", "18:25:00", "19:15:00"),
                (13, "Tiet 13", "19:20:00", "20:10:00"),
                (14, "Tiet 14", "20:15:00", "21:05:00")
            };

            foreach (var slot in slots.Where(s => !existing.Contains(s.Id)))
            {
                _db.Tiethocs.Add(new Tiethoc
                {
                    MaTiet = slot.Id,
                    TenTiet = slot.Name,
                    GioBatDau = TimeOnly.Parse(slot.Start),
                    GioKetThuc = TimeOnly.Parse(slot.End),
                    TrangThai = true,
                    DaXoa = false,
                    ThoiGianTao = DateTime.UtcNow
                });
            }

            await _db.SaveChangesAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBuoiHocDto dto)
        {
            if (dto.MaTietBatDau > dto.MaTietKetThuc)
                return BadRequest(new { error = "MaTietBatDau must be <= MaTietKetThuc" });

            await EnsureTietHocSeedAsync();

            var date = DateOnly.FromDateTime(dto.NgayHoc.Date);

            var overlapping = await _db.Buoihocs
                .Where(b => b.NgayHoc == date && b.MaTietBatDau <= dto.MaTietKetThuc && b.MaTietKetThuc >= dto.MaTietBatDau)
                .ToListAsync();

            var errors = new List<string>();

            // Class conflict
            if (overlapping.Any(b => b.MaLopHoc == dto.MaLopHoc))
                errors.Add("Trùng Lớp học: lớp này đã có buổi học trùng giờ.");

            // Room conflict
            if (dto.MaPhongHoc != null && overlapping.Any(b => b.MaPhongHoc != null && b.MaPhongHoc == dto.MaPhongHoc))
                errors.Add("Trùng Phòng học: phòng này đã được sử dụng ở khung giờ đó.");

            // Teacher conflict: get teachers assigned to the class being scheduled
            var teachersForClass = await _db.Giangvienlophocs
                .Where(g => g.MaLopHoc == dto.MaLopHoc && (g.DaXoa == null || g.DaXoa == false) && (g.TrangThai == null || g.TrangThai == true))
                .Select(g => g.MaGiangVien)
                .Distinct()
                .ToListAsync();

            if (teachersForClass.Any())
            {
                var busyTeachers = await _db.Giangvienlophocs
                    .Where(g => overlapping.Select(b => b.MaLopHoc).Contains(g.MaLopHoc))
                    .Select(g => g.MaGiangVien)
                    .Distinct()
                    .ToListAsync();

                if (teachersForClass.Intersect(busyTeachers).Any())
                    errors.Add("Trùng Giảng viên: một hoặc nhiều giảng viên đang bận trong khung giờ đó.");
            }

            if (errors.Any())
                return BadRequest(new { errors });

            var entity = new Buoihoc
            {
                MaBuoiHoc = Guid.NewGuid(),
                MaLopHoc = dto.MaLopHoc,
                MaPhongHoc = dto.MaPhongHoc,
                NgayHoc = date,
                MaTietBatDau = dto.MaTietBatDau,
                MaTietKetThuc = dto.MaTietKetThuc,
                TieuDe = dto.TieuDe,
                NoiDung = dto.NoiDung,
                ThoiGianTao = DateTime.UtcNow,
                TrangThai = true,
                DaXoa = false
            };

            _db.Buoihocs.Add(entity);
            await _db.SaveChangesAsync();

            return Ok(new { id = entity.MaBuoiHoc });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBuoiHocDto dto)
        {
            var existing = await _db.Buoihocs.FindAsync(id);
            if (existing == null) return NotFound();

            if (dto.MaTietBatDau > dto.MaTietKetThuc)
                return BadRequest(new { error = "MaTietBatDau must be <= MaTietKetThuc" });

            var date = DateOnly.FromDateTime(dto.NgayHoc.Date);

            var overlapping = await _db.Buoihocs
                .Where(b => b.MaBuoiHoc != id && b.NgayHoc == date && b.MaTietBatDau <= dto.MaTietKetThuc && b.MaTietKetThuc >= dto.MaTietBatDau)
                .ToListAsync();

            var errors = new List<string>();

            if (overlapping.Any(b => b.MaLopHoc == dto.MaLopHoc))
                errors.Add("Trùng Lớp học: lớp này đã có buổi học trùng giờ.");

            if (dto.MaPhongHoc != null && overlapping.Any(b => b.MaPhongHoc != null && b.MaPhongHoc == dto.MaPhongHoc))
                errors.Add("Trùng Phòng học: phòng này đã được sử dụng ở khung giờ đó.");

            var teachersForClass = await _db.Giangvienlophocs
                .Where(g => g.MaLopHoc == dto.MaLopHoc && (g.DaXoa == null || g.DaXoa == false) && (g.TrangThai == null || g.TrangThai == true))
                .Select(g => g.MaGiangVien)
                .Distinct()
                .ToListAsync();

            if (teachersForClass.Any())
            {
                var busyTeachers = await _db.Giangvienlophocs
                    .Where(g => overlapping.Select(b => b.MaLopHoc).Contains(g.MaLopHoc))
                    .Select(g => g.MaGiangVien)
                    .Distinct()
                    .ToListAsync();

                if (teachersForClass.Intersect(busyTeachers).Any())
                    errors.Add("Trùng Giảng viên: một hoặc nhiều giảng viên đang bận trong khung giờ đó.");
            }

            if (errors.Any())
                return BadRequest(new { errors });

            existing.MaLopHoc = dto.MaLopHoc;
            existing.MaPhongHoc = dto.MaPhongHoc;
            existing.NgayHoc = date;
            existing.MaTietBatDau = dto.MaTietBatDau;
            existing.MaTietKetThuc = dto.MaTietKetThuc;
            existing.TieuDe = dto.TieuDe;
            existing.NoiDung = dto.NoiDung;
            existing.ThoiGianSua = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Cập nhật buổi học thành công.",
                id = existing.MaBuoiHoc
            });
        }

        [HttpGet("sample-payload")]
        public IActionResult GetSamplePayload()
        {
            var classRow = _db.Lophocs
                .Where(x => x.DaXoa == null || x.DaXoa == false)
                .OrderBy(x => x.TenLop)
                .FirstOrDefault();

            var roomRow = _db.Phonghocs
                .Where(x => x.DaXoa == null || x.DaXoa == false)
                .OrderBy(x => x.TenPhong)
                .FirstOrDefault();

            var sessionRow = _db.Buoihocs
                .Where(x => x.DaXoa == null || x.DaXoa == false)
                .OrderByDescending(x => x.ThoiGianTao)
                .FirstOrDefault();

            return Ok(new
            {
                create = new
                {
                    MaLopHoc = classRow?.MaLopHoc,
                    MaPhongHoc = roomRow?.MaPhongHoc,
                    NgayHoc = sessionRow?.NgayHoc.ToDateTime(TimeOnly.MinValue) ?? new DateTime(2026, 5, 8),
                    MaTietBatDau = 1,
                    MaTietKetThuc = 2,
                    TieuDe = classRow != null ? $"{classRow.TenLop} - Buổi học" : "Buổi học",
                    NoiDung = "Nội dung buổi học theo lớp thực tế trong hệ thống"
                },
                update = new
                {
                    MaLopHoc = sessionRow?.MaLopHoc ?? classRow?.MaLopHoc,
                    MaPhongHoc = sessionRow?.MaPhongHoc,
                    NgayHoc = sessionRow?.NgayHoc.ToDateTime(TimeOnly.MinValue) ?? new DateTime(2026, 5, 8),
                    MaTietBatDau = sessionRow?.MaTietBatDau ?? 3,
                    MaTietKetThuc = sessionRow?.MaTietKetThuc ?? 4,
                    TieuDe = sessionRow?.TieuDe ?? "Buổi học cập nhật",
                    NoiDung = sessionRow?.NoiDung ?? "Nội dung cập nhật theo buổi học thực tế"
                },
                notes = new[]
                {
                    "Dữ liệu mẫu được lấy từ các bản ghi thực tế trong hệ thống nếu có.",
                    "MaPhongHoc có thể để null nếu chưa chốt phòng.",
                    "MaTietBatDau phải nhỏ hơn hoặc bằng MaTietKetThuc."
                }
            });
        }

        [HttpGet("admin-board")]
        public async Task<IActionResult> GetAdminBoard([FromQuery] DateTime? weekStart)
        {
            var monday = GetWeekStart(DateOnly.FromDateTime((weekStart ?? DateTime.Today).Date));
            var weekEnd = monday.AddDays(6);

            var scheduleRows = await _db.Buoihocs
                .Where(b => (b.DaXoa == null || b.DaXoa == false) && b.NgayHoc >= monday && b.NgayHoc <= weekEnd)
                .Include(b => b.MaLopHocNavigation)
                .Include(b => b.MaPhongHocNavigation)
                .Include(b => b.MaLopHocNavigation.Giangvienlophocs)
                .ThenInclude(gl => gl.MaGiangVienNavigation)
                .ThenInclude(gv => gv.MaNguoiDungNavigation)
                .ToListAsync();

            var schedules = new List<AdminScheduleItemDto>();
            foreach (var row in scheduleRows)
            {
                var teacherName = row.MaLopHocNavigation?.Giangvienlophocs
                    .FirstOrDefault(g => (g.DaXoa == null || g.DaXoa == false) && (g.TrangThai == null || g.TrangThai == true))
                    ?.MaGiangVienNavigation?.MaNguoiDungNavigation?.HoTen ?? "Chưa phân công";

                var teacherId = row.MaLopHocNavigation?.Giangvienlophocs
                    .FirstOrDefault(g => (g.DaXoa == null || g.DaXoa == false) && (g.TrangThai == null || g.TrangThai == true))
                    ?.MaGiangVien;

                schedules.Add(new AdminScheduleItemDto
                {
                    Id = row.MaBuoiHoc,
                    MaBuoiHoc = row.MaBuoiHoc,
                    MaLopHoc = row.MaLopHoc,
                    ClassCode = row.MaLopHocNavigation?.TenLop ?? row.MaLopHoc.ToString(),
                    ClassName = row.MaLopHocNavigation?.TenLop ?? string.Empty,
                    MaGiangVien = teacherId,
                    Teacher = teacherName,
                    MaPhongHoc = row.MaPhongHoc,
                    Room = row.MaPhongHocNavigation?.TenPhong ?? "Chưa gán phòng",
                    NgayHoc = row.NgayHoc,
                    DayIdx = GetDayIndex(monday, row.NgayHoc),
                    SlotId = row.MaTietBatDau,
                    SlotEndId = row.MaTietKetThuc,
                    Subject = row.TieuDe ?? row.MaLopHocNavigation?.TenLop ?? "Buổi học",
                    IsConflict = false,
                    ConflictReason = null
                });
            }

            var requestRows = await _db.Yeucaulichdays
                .Where(y => (y.DaXoa == null || y.DaXoa == false) && (y.TrangThaiDuyet == null || y.TrangThaiDuyet == 0))
                .Include(y => y.MaLopHocNavigation)
                .Include(y => y.MaGiangVienNavigation).ThenInclude(gv => gv.MaNguoiDungNavigation)
                .Include(y => y.MaBuoiHocNavigation)
                .ToListAsync();

            var rescheduleRequests = requestRows.Select(row => new AdminRescheduleRequestDto
            {
                Id = row.MaYeuCau,
                MaYeuCau = row.MaYeuCau,
                MaBuoiHoc = row.MaBuoiHoc,
                MaLopHoc = row.MaLopHoc,
                ClassCode = row.MaLopHocNavigation?.TenLop ?? row.MaLopHoc.ToString(),
                Teacher = row.MaGiangVienNavigation?.MaNguoiDungNavigation?.HoTen ?? "Chưa rõ",
                OldDate = row.MaBuoiHocNavigation != null
                    ? $"{row.MaBuoiHocNavigation.NgayHoc:dd/MM} - Ca {row.MaBuoiHocNavigation.MaTietBatDau}"
                    : "Chưa có lịch gốc",
                NewDate = $"{row.NgayHocDeXuat:dd/MM} - Ca {row.MaTietBatDauDeXuat}",
                Reason = row.LyDo ?? string.Empty,
                Status = row.TrangThaiDuyet
            }).ToList();

            var classes = await _db.Lophocs
                .Where(x => x.DaXoa == null || x.DaXoa == false)
                .OrderBy(x => x.TenLop)
                .Select(x => new ScheduleLookupItemDto { Id = x.MaLopHoc, Name = x.TenLop })
                .ToListAsync();

            var teachers = await _db.Giangviens
                .Where(x => x.DaXoa == null || x.DaXoa == false)
                .OrderBy(x => x.MaNguoiDungNavigation.HoTen)
                .Select(x => new ScheduleLookupItemDto { Id = x.MaGiangVien, Name = x.MaNguoiDungNavigation.HoTen })
                .ToListAsync();

            var rooms = await _db.Phonghocs
                .Where(x => x.DaXoa == null || x.DaXoa == false)
                .OrderBy(x => x.TenPhong)
                .Select(x => new ScheduleLookupItemDto { Id = x.MaPhongHoc, Name = x.TenPhong })
                .ToListAsync();

            return Ok(new AdminScheduleBoardDto
            {
                WeekStart = monday.ToDateTime(TimeOnly.MinValue),
                Schedules = schedules,
                RescheduleRequests = rescheduleRequests,
                Classes = classes,
                Teachers = teachers,
                Rooms = rooms
            });
        }

        [HttpGet("teacher-board")]
        public async Task<IActionResult> GetTeacherBoard([FromQuery] DateTime? weekStart, [FromQuery] string? teacherId = null)
        {
            try
            {
                // Lấy teacherId từ JWT claim (profileId) hoặc từ query parameter (cho dev)
                var currentTeacherId = string.IsNullOrEmpty(teacherId)
                    ? (User.FindFirst("profileId")?.Value ?? User.FindFirst("sub")?.Value)
                    : teacherId;
                
                if (string.IsNullOrEmpty(currentTeacherId) || !Guid.TryParse(currentTeacherId, out var parsedTeacherId))
                {
                    return Unauthorized(new { message = "Không thể xác định giáo viên hiện tại." });
                }

                var monday = GetWeekStart(DateOnly.FromDateTime((weekStart ?? DateTime.Today).Date));
                var weekEnd = monday.AddDays(6);

                // Lấy lịch của giáo viên hiện tại
                var scheduleRows = await _db.Buoihocs
                    .Where(b => (b.DaXoa == null || b.DaXoa == false) && b.NgayHoc >= monday && b.NgayHoc <= weekEnd)
                    .Include(b => b.MaLopHocNavigation)
                    .ThenInclude(l => l.Giangvienlophocs)
                    .Include(b => b.MaPhongHocNavigation)
                    .Where(b => b.MaLopHocNavigation.Giangvienlophocs.Any(g => g.MaGiangVien == parsedTeacherId))
                    .ToListAsync();

                var schedules = scheduleRows.Select(row => new AdminScheduleItemDto
                {
                    Id = row.MaBuoiHoc,
                    MaBuoiHoc = row.MaBuoiHoc,
                    MaLopHoc = row.MaLopHoc,
                    ClassCode = row.MaLopHocNavigation?.TenLop ?? row.MaLopHoc.ToString(),
                    ClassName = row.MaLopHocNavigation?.TenLop ?? string.Empty,
                    MaGiangVien = parsedTeacherId,
                    Teacher = User.FindFirst("name")?.Value ?? "Giáo viên",
                    MaPhongHoc = row.MaPhongHoc,
                    Room = row.MaPhongHocNavigation?.TenPhong ?? "Chưa gán phòng",
                    NgayHoc = row.NgayHoc,
                    DayIdx = GetDayIndex(monday, row.NgayHoc),
                    SlotId = row.MaTietBatDau,
                    SlotEndId = row.MaTietKetThuc,
                    Subject = row.TieuDe ?? row.MaLopHocNavigation?.TenLop ?? "Buổi học",
                    IsConflict = false,
                    ConflictReason = null
                }).ToList();

                // Lấy yêu cầu đổi lịch của giáo viên (sử dụng projection để tránh mapping navigation lỗi)
                var rawRequests = await _db.Yeucaulichdays
                    .Where(y => (y.DaXoa == null || y.DaXoa == false) && y.MaGiangVien == parsedTeacherId)
                    .Select(y => new {
                        y.MaYeuCau,
                        y.MaBuoiHoc,
                        y.MaLopHoc,
                        y.NgayHocDeXuat,
                        y.MaTietBatDauDeXuat,
                        y.MaTietKetThucDeXuat,
                        y.LyDo,
                        y.TrangThaiDuyet
                    })
                    .ToListAsync();

                // Lấy tên lớp và thông tin buổi gốc riêng để ghép
                var classIds = rawRequests.Select(r => r.MaLopHoc).Distinct().ToList();
                var buoiIds = rawRequests.Where(r => r.MaBuoiHoc != null).Select(r => r.MaBuoiHoc!.Value).Distinct().ToList();

                var classMap = await _db.Lophocs
                    .Where(l => classIds.Contains(l.MaLopHoc))
                    .ToDictionaryAsync(l => l.MaLopHoc, l => l.TenLop);

                var buoiMap = await _db.Buoihocs
                    .Where(b => buoiIds.Contains(b.MaBuoiHoc))
                    .ToDictionaryAsync(b => b.MaBuoiHoc, b => b);

                var rescheduleRequests = rawRequests.Select(row => new AdminRescheduleRequestDto
                {
                    Id = row.MaYeuCau,
                    MaYeuCau = row.MaYeuCau,
                    MaBuoiHoc = row.MaBuoiHoc,
                    MaLopHoc = row.MaLopHoc,
                    ClassCode = classMap.ContainsKey(row.MaLopHoc) ? classMap[row.MaLopHoc] : string.Empty,
                    Teacher = User.FindFirst("name")?.Value ?? "Giáo viên",
                    OldDate = (row.MaBuoiHoc != null && buoiMap.ContainsKey(row.MaBuoiHoc.Value))
                        ? $"{buoiMap[row.MaBuoiHoc.Value].NgayHoc:dd/MM} - Ca {buoiMap[row.MaBuoiHoc.Value].MaTietBatDau}"
                        : "Không xác định",
                    NewDate = $"{row.NgayHocDeXuat:dd/MM} - Ca {row.MaTietBatDauDeXuat}",
                    Reason = row.LyDo ?? string.Empty,
                    Status = row.TrangThaiDuyet
                }).ToList();

                // Lấy danh sách phòng học
                var rooms = await _db.Phonghocs
                    .Where(x => x.DaXoa == null || x.DaXoa == false)
                    .OrderBy(x => x.TenPhong)
                    .Select(x => new ScheduleLookupItemDto { Id = x.MaPhongHoc, Name = x.TenPhong })
                    .ToListAsync();

                return Ok(new AdminScheduleBoardDto
                {
                    WeekStart = monday.ToDateTime(TimeOnly.MinValue),
                    Schedules = schedules,
                    RescheduleRequests = rescheduleRequests,
                    Classes = new List<ScheduleLookupItemDto>(),
                    Teachers = new List<ScheduleLookupItemDto>(),
                    Rooms = rooms
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetTeacherBoard failed");
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message, trace = ex.ToString() });
            }
        }

        [HttpPut("reschedule-requests/{id:guid}/approve")]
        public async Task<IActionResult> ApproveRescheduleRequest(Guid id)
        {
            var request = await _db.Yeucaulichdays.Include(x => x.MaBuoiHocNavigation).FirstOrDefaultAsync(x => x.MaYeuCau == id);
            if (request == null) return NotFound(new { message = "Không tìm thấy yêu cầu đổi lịch." });

            if (request.MaBuoiHocNavigation != null)
            {
                var targetDate = request.NgayHocDeXuat;
                var targetStart = request.MaTietBatDauDeXuat;
                var targetEnd = request.MaTietKetThucDeXuat;

                var overlapping = await _db.Buoihocs
                    .Where(b => b.MaBuoiHoc != request.MaBuoiHoc && b.NgayHoc == targetDate && b.MaTietBatDau <= targetEnd && b.MaTietKetThuc >= targetStart)
                    .ToListAsync();

                var hasRoomConflict = request.MaBuoiHocNavigation.MaPhongHoc != null && overlapping.Any(b => b.MaPhongHoc == request.MaBuoiHocNavigation.MaPhongHoc);
                var teachersForClass = await _db.Giangvienlophocs
                    .Where(g => g.MaLopHoc == request.MaLopHoc)
                    .Select(g => g.MaGiangVien)
                    .Distinct()
                    .ToListAsync();
                var busyTeachers = await _db.Giangvienlophocs
                    .Where(g => overlapping.Select(b => b.MaLopHoc).Contains(g.MaLopHoc))
                    .Select(g => g.MaGiangVien)
                    .Distinct()
                    .ToListAsync();

                if (hasRoomConflict || teachersForClass.Intersect(busyTeachers).Any())
                {
                    return BadRequest(new { message = "Khung giờ mới bị trùng, không thể duyệt đổi lịch." });
                }

                request.MaBuoiHocNavigation.NgayHoc = targetDate;
                request.MaBuoiHocNavigation.MaTietBatDau = targetStart;
                request.MaBuoiHocNavigation.MaTietKetThuc = targetEnd;
                request.MaBuoiHocNavigation.ThoiGianSua = DateTime.UtcNow;
            }

            request.TrangThaiDuyet = 1;
            request.ThoiGianSua = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Đã duyệt đổi lịch thành công.", id = request.MaYeuCau });
        }

        [HttpPut("reschedule-requests/{id:guid}/reject")]
        public async Task<IActionResult> RejectRescheduleRequest(Guid id)
        {
            var request = await _db.Yeucaulichdays.FirstOrDefaultAsync(x => x.MaYeuCau == id);
            if (request == null) return NotFound(new { message = "Không tìm thấy yêu cầu đổi lịch." });

            request.TrangThaiDuyet = 2;
            request.ThoiGianSua = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Đã từ chối yêu cầu đổi lịch.", id = request.MaYeuCau });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _db.Buoihocs.FindAsync(id);
            if (existing == null) return NotFound(new { message = "Không tìm thấy buổi học." });

            existing.DaXoa = true;
            existing.ThoiGianSua = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Đã xóa buổi học.", id });
        }

        [HttpGet("rooms")]
        public async Task<IActionResult> GetRooms()
        {
            var rooms = await _db.Phonghocs
                .Where(x => x.DaXoa == null || x.DaXoa == false)
                .OrderBy(x => x.TenPhong)
                .Select(x => new
                {
                    id = x.MaPhongHoc,
                    tenPhong = x.TenPhong,
                    sucChua = x.SucChua,
                    active = x.TrangThai
                })
                .ToListAsync();

            return Ok(rooms);
        }

        [HttpPost("rooms/create")]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.TenPhong))
            {
                return BadRequest(ApiResponseDto<object>.Fail("Tên phòng học không được để trống.", "ROOM_NAME_REQUIRED"));
            }

            var roomName = dto.TenPhong.Trim();
            if (roomName.Length > 50)
            {
                return BadRequest(ApiResponseDto<object>.Fail("Tên phòng học không được vượt quá 50 ký tự.", "ROOM_NAME_TOO_LONG"));
            }

            if (await _db.Phonghocs.AnyAsync(p => (p.DaXoa == null || p.DaXoa == false) && p.TenPhong == roomName))
            {
                return BadRequest(ApiResponseDto<object>.Fail("Tên phòng học đã tồn tại.", "ROOM_NAME_EXISTS"));
            }

            var room = new Phonghoc
            {
                MaPhongHoc = Guid.NewGuid(),
                TenPhong = roomName,
                TrangThai = true,
                DaXoa = false,
                ThoiGianTao = DateTime.UtcNow
            };

            _db.Phonghocs.Add(room);
            await _db.SaveChangesAsync();

            return Ok(ApiResponseDto<object>.Ok(new { roomId = room.MaPhongHoc }, $"Đã tạo phòng học {room.TenPhong} thành công."));
        }

        [HttpGet("rooms/{id:guid}")]
        public async Task<IActionResult> GetRoom(Guid id)
        {
            var room = await _db.Phonghocs.FindAsync(id);
            if (room == null || (room.DaXoa != null && room.DaXoa == true))
                return NotFound(ApiResponseDto<object>.Fail("Không tìm thấy phòng học."));

            return Ok(new
            {
                id = room.MaPhongHoc,
                tenPhong = room.TenPhong,
                sucChua = room.SucChua,
                active = room.TrangThai
            });
        }

        [HttpPut("rooms/{id:guid}")]
        public async Task<IActionResult> UpdateRoom(Guid id, [FromBody] UpdateRoomDto dto)
        {
            var room = await _db.Phonghocs.FindAsync(id);
            if (room == null || (room.DaXoa != null && room.DaXoa == true))
                return NotFound(ApiResponseDto<object>.Fail("Không tìm thấy phòng học."));

            if (!string.IsNullOrWhiteSpace(dto.TenPhong))
            {
                var newName = dto.TenPhong.Trim();
                if (newName.Length > 50)
                    return BadRequest(ApiResponseDto<object>.Fail("Tên phòng học không được vượt quá 50 ký tự.", "ROOM_NAME_TOO_LONG"));

                var exists = await _db.Phonghocs.AnyAsync(p => p.MaPhongHoc != id && (p.DaXoa == null || p.DaXoa == false) && p.TenPhong == newName);
                if (exists)
                    return BadRequest(ApiResponseDto<object>.Fail("Tên phòng học đã tồn tại.", "ROOM_NAME_EXISTS"));

                room.TenPhong = newName;
            }

            // Note: capacity (SucChua) is no longer editable via API.

            if (dto.TrangThai.HasValue)
            {
                room.TrangThai = dto.TrangThai.Value;
            }

            room.ThoiGianSua = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return Ok(ApiResponseDto<object>.Ok(new { roomId = room.MaPhongHoc }, $"Đã cập nhật phòng học {room.TenPhong} thành công."));
        }

        [HttpDelete("rooms/{id:guid}")]
        public async Task<IActionResult> DeleteRoom(Guid id)
        {
            var room = await _db.Phonghocs.FindAsync(id);
            if (room == null || (room.DaXoa != null && room.DaXoa == true))
                return NotFound(ApiResponseDto<object>.Fail("Không tìm thấy phòng học."));

            room.DaXoa = true;
            room.ThoiGianSua = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return Ok(ApiResponseDto<object>.Ok(new { roomId = id }, "Đã xóa phòng học."));
        }

        [HttpGet("available-rooms")]
        public async Task<IActionResult> GetAvailableRooms([FromQuery] DateTime ngayHoc, [FromQuery] int maTietBatDau, [FromQuery] int maTietKetThuc)
        {
            var date = DateOnly.FromDateTime(ngayHoc.Date);

            var overlapping = await _db.Buoihocs
                .Where(b => b.NgayHoc == date && b.MaTietBatDau <= maTietKetThuc && b.MaTietKetThuc >= maTietBatDau)
                .ToListAsync();

            var busyRoomIds = overlapping.Where(b => b.MaPhongHoc != null).Select(b => b.MaPhongHoc!.Value).Distinct().ToList();

            var rooms = await _db.Phonghocs
                .Where(p => !(busyRoomIds.Contains(p.MaPhongHoc)))
                .Select(p => new AvailableRoomDto { MaPhongHoc = p.MaPhongHoc, TenPhong = p.TenPhong })
                .ToListAsync();

            return Ok(rooms);
        }

        [HttpGet("available-teachers")]
        public async Task<IActionResult> GetAvailableTeachers([FromQuery] DateTime ngayHoc, [FromQuery] int maTietBatDau, [FromQuery] int maTietKetThuc)
        {
            var date = DateOnly.FromDateTime(ngayHoc.Date);

            var overlapping = await _db.Buoihocs
                .Where(b => b.NgayHoc == date && b.MaTietBatDau <= maTietKetThuc && b.MaTietKetThuc >= maTietBatDau)
                .ToListAsync();

            var busyTeacherIds = await _db.Giangvienlophocs
                .Where(g => overlapping.Select(b => b.MaLopHoc).Contains(g.MaLopHoc))
                .Select(g => g.MaGiangVien)
                .Distinct()
                .ToListAsync();

            var teachers = await _db.Giangviens
                .Where(t => !(busyTeacherIds.Contains(t.MaGiangVien)))
                .Select(t => new AvailableTeacherDto { MaGiangVien = t.MaGiangVien, MaNguoiDung = t.MaNguoiDung })
                .ToListAsync();

            return Ok(teachers);
        }

        [HttpGet("student-classes")]
        public async Task<IActionResult> GetStudentClasses([FromQuery] string? studentId = null)
        {
            // Lấy studentId từ JWT claim (profileId) hoặc từ query parameter (cho dev)
            var currentStudentId = string.IsNullOrEmpty(studentId)
                ? (User.FindFirst("profileId")?.Value ?? User.FindFirst("sub")?.Value)
                : studentId;
                
            if (string.IsNullOrEmpty(currentStudentId) || !Guid.TryParse(currentStudentId, out var parsedStudentId))
            {
                return Unauthorized(new { message = "Không thể xác định học sinh hiện tại." });
            }

            // Lấy danh sách lớp của học sinh
            var studentClasses = await _db.Hocsinhlophocs
                .Where(h => h.MaHocSinh == parsedStudentId && 
                           (h.DaXoa == null || h.DaXoa == false) && 
                           (h.TrangThai == null || h.TrangThai == true))
                .Include(h => h.MaLopHocNavigation)
                .ThenInclude(l => l.Giangvienlophocs)
                .ThenInclude(gl => gl.MaGiangVienNavigation)
                .ThenInclude(gv => gv.MaNguoiDungNavigation)
                .ToListAsync();

            var classes = new List<StudentClassItemDto>();
            
            foreach (var studentClass in studentClasses)
            {
                var classInfo = studentClass.MaLopHocNavigation;
                
                // Lấy tên giáo viên chính của lớp
                var teacherName = classInfo?.Giangvienlophocs
                    .FirstOrDefault(g => (g.DaXoa == null || g.DaXoa == false) && (g.TrangThai == null || g.TrangThai == true))
                    ?.MaGiangVienNavigation?.MaNguoiDungNavigation?.HoTen ?? "Chưa phân công";

                // Đếm số học sinh trong lớp
                var studentCount = await _db.Hocsinhlophocs
                    .CountAsync(h => h.MaLopHoc == classInfo.MaLopHoc && 
                                    (h.DaXoa == null || h.DaXoa == false) && 
                                    (h.TrangThai == null || h.TrangThai == true));

                classes.Add(new StudentClassItemDto
                {
                    Id = classInfo.MaLopHoc,
                    ClassCode = classInfo.TenLop,
                    ClassName = classInfo.TenLop,
                    Teacher = teacherName,
                    StudentCount = studentCount,
                    MaxStudents = classInfo.SiSoToiDa ?? 0,
                    StartDate = classInfo.NgayBatDau,
                    EndDate = classInfo.NgayKetThuc,
                    Status = classInfo.TrangThai == true ? "Hoạt động" : "Không hoạt động"
                });
            }

            return Ok(new StudentClassesDto
            {
                Classes = classes.OrderBy(c => c.ClassCode).ToList()
            });
        }

        [HttpGet("student-class-detail/{classId:guid}")]
        public async Task<IActionResult> GetStudentClassDetail(Guid classId, [FromQuery] string? studentId = null)
        {
            // Lấy studentId từ JWT claim hoặc từ query parameter
            var currentStudentId = string.IsNullOrEmpty(studentId)
                ? (User.FindFirst("profileId")?.Value ?? User.FindFirst("sub")?.Value)
                : studentId;
                
            if (string.IsNullOrEmpty(currentStudentId) || !Guid.TryParse(currentStudentId, out var parsedStudentId))
            {
                return Unauthorized(new { message = "Không thể xác định học sinh hiện tại." });
            }

            // Kiểm tra học sinh có thuộc lớp này không
            var studentInClass = await _db.Hocsinhlophocs
                .FirstOrDefaultAsync(h => h.MaHocSinh == parsedStudentId && 
                                         h.MaLopHoc == classId &&
                                         (h.DaXoa == null || h.DaXoa == false) && 
                                         (h.TrangThai == null || h.TrangThai == true));

            if (studentInClass == null)
            {
                return Forbid();
            }

            // Lấy thông tin lớp
            var classInfo = await _db.Lophocs
                .Where(l => l.MaLopHoc == classId && (l.DaXoa == null || l.DaXoa == false))
                .Include(l => l.Giangvienlophocs)
                .ThenInclude(gl => gl.MaGiangVienNavigation)
                .ThenInclude(gv => gv.MaNguoiDungNavigation)
                .FirstOrDefaultAsync();

            if (classInfo == null)
            {
                return NotFound(new { message = "Không tìm thấy lớp học." });
            }

            // Lấy danh sách học sinh trong lớp
            var studentNames = await _db.Hocsinhlophocs
                .Where(h => h.MaLopHoc == classId && 
                           (h.DaXoa == null || h.DaXoa == false) && 
                           (h.TrangThai == null || h.TrangThai == true))
                .Include(h => h.MaHocSinhNavigation)
                .ThenInclude(hs => hs.MaNguoiDungNavigation)
                .Select(h => h.MaHocSinhNavigation.MaNguoiDungNavigation.HoTen)
                .Distinct()
                .ToListAsync();

            // Lấy lịch học sắp tới của lớp (7 ngày tới)
            var today = DateOnly.FromDateTime(DateTime.Today);
            var nextWeek = today.AddDays(7);

            var upcomingSchedules = await _db.Buoihocs
                .Where(b => b.MaLopHoc == classId &&
                           (b.DaXoa == null || b.DaXoa == false) &&
                           b.NgayHoc >= today &&
                           b.NgayHoc <= nextWeek)
                .Include(b => b.MaPhongHocNavigation)
                .OrderBy(b => b.NgayHoc)
                .ThenBy(b => b.MaTietBatDau)
                .ToListAsync();

            var teacherName = classInfo.Giangvienlophocs
                .FirstOrDefault(g => (g.DaXoa == null || g.DaXoa == false) && (g.TrangThai == null || g.TrangThai == true))
                ?.MaGiangVienNavigation?.MaNguoiDungNavigation?.HoTen ?? "Chưa phân công";

            var teacherId = classInfo.Giangvienlophocs
                .FirstOrDefault(g => (g.DaXoa == null || g.DaXoa == false) && (g.TrangThai == null || g.TrangThai == true))
                ?.MaGiangVien;

            var studentCount = await _db.Hocsinhlophocs
                .CountAsync(h => h.MaLopHoc == classId && 
                                (h.DaXoa == null || h.DaXoa == false) && 
                                (h.TrangThai == null || h.TrangThai == true));

            var scheduleList = upcomingSchedules.Select(schedule => new StudentScheduleItemDto
            {
                Id = schedule.MaBuoiHoc,
                ClassCode = classInfo.TenLop,
                ClassName = classInfo.TenLop,
                Teacher = teacherName,
                Room = schedule.MaPhongHocNavigation?.TenPhong ?? "Chưa gán phòng",
                NgayHoc = schedule.NgayHoc,
                DayIdx = 0, // Không cần trong detail view
                SlotId = schedule.MaTietBatDau,
                SlotEndId = schedule.MaTietKetThuc,
                Subject = schedule.TieuDe ?? classInfo.TenLop
            }).ToList();

            return Ok(new StudentClassDetailDto
            {
                Id = classInfo.MaLopHoc,
                ClassCode = classInfo.TenLop,
                ClassName = classInfo.TenLop,
                Teacher = teacherName,
                TeacherId = teacherId,
                StudentCount = studentCount,
                MaxStudents = classInfo.SiSoToiDa ?? 0,
                StartDate = classInfo.NgayBatDau,
                EndDate = classInfo.NgayKetThuc,
                Status = classInfo.TrangThai == true ? "Hoạt động" : "Không hoạt động",
                StudentNames = studentNames,
                UpcomingSchedules = scheduleList
            });
        }

        [HttpGet("student-board")]
        public async Task<IActionResult> GetStudentBoard([FromQuery] DateTime? weekStart, [FromQuery] string? studentId = null)
        {
            // Lấy studentId từ JWT claim (profileId) hoặc từ query parameter (cho dev)
            var currentStudentId = string.IsNullOrEmpty(studentId)
                ? (User.FindFirst("profileId")?.Value ?? User.FindFirst("sub")?.Value)
                : studentId;
                
            if (string.IsNullOrEmpty(currentStudentId) || !Guid.TryParse(currentStudentId, out var parsedStudentId))
            {
                return Unauthorized(new { message = "Không thể xác định học sinh hiện tại." });
            }

            var monday = GetWeekStart(DateOnly.FromDateTime((weekStart ?? DateTime.Today).Date));
            var weekEnd = monday.AddDays(6);

            // Lấy danh sách lớp của học sinh
            var studentClasses = await _db.Hocsinhlophocs
                .Where(h => h.MaHocSinh == parsedStudentId && (h.DaXoa == null || h.DaXoa == false) && (h.TrangThai == null || h.TrangThai == true))
                .Select(h => h.MaLopHoc)
                .Distinct()
                .ToListAsync();

            // Lấy lịch của các lớp mà học sinh thuộc về
            var scheduleRows = await _db.Buoihocs
                .Where(b => (b.DaXoa == null || b.DaXoa == false) && 
                           b.NgayHoc >= monday && 
                           b.NgayHoc <= weekEnd && 
                           studentClasses.Contains(b.MaLopHoc))
                .Include(b => b.MaLopHocNavigation)
                .Include(b => b.MaPhongHocNavigation)
                .Include(b => b.MaLopHocNavigation.Giangvienlophocs)
                .ThenInclude(gl => gl.MaGiangVienNavigation)
                .ThenInclude(gv => gv.MaNguoiDungNavigation)
                .OrderBy(b => b.NgayHoc)
                .ThenBy(b => b.MaTietBatDau)
                .ToListAsync();

            var schedules = new List<StudentScheduleItemDto>();
            foreach (var row in scheduleRows)
            {
                var teacherName = row.MaLopHocNavigation?.Giangvienlophocs
                    .FirstOrDefault(g => (g.DaXoa == null || g.DaXoa == false) && (g.TrangThai == null || g.TrangThai == true))
                    ?.MaGiangVienNavigation?.MaNguoiDungNavigation?.HoTen ?? "Chưa phân công";

                schedules.Add(new StudentScheduleItemDto
                {
                    Id = row.MaBuoiHoc,
                    ClassCode = row.MaLopHocNavigation?.TenLop ?? row.MaLopHoc.ToString(),
                    ClassName = row.MaLopHocNavigation?.TenLop ?? string.Empty,
                    Teacher = teacherName,
                    Room = row.MaPhongHocNavigation?.TenPhong ?? "Chưa gán phòng",
                    NgayHoc = row.NgayHoc,
                    DayIdx = GetDayIndex(monday, row.NgayHoc),
                    SlotId = row.MaTietBatDau,
                    SlotEndId = row.MaTietKetThuc,
                    Subject = row.TieuDe ?? row.MaLopHocNavigation?.TenLop ?? "Buổi học"
                });
            }

            return Ok(new StudentScheduleBoardDto
            {
                WeekStart = monday.ToDateTime(TimeOnly.MinValue),
                Schedules = schedules
            });
        }

        [HttpGet("parent-board")]
        public async Task<IActionResult> GetParentBoard([FromQuery] DateTime? weekStart, [FromQuery] string? parentId = null)
        {
            // Lấy parentId từ JWT claim (profileId) hoặc từ query parameter (cho dev)
            var currentParentId = string.IsNullOrEmpty(parentId)
                ? (User.FindFirst("profileId")?.Value ?? User.FindFirst("sub")?.Value)
                : parentId;
                
            if (string.IsNullOrEmpty(currentParentId) || !Guid.TryParse(currentParentId, out var parsedParentId))
            {
                return Unauthorized(new { message = "Không thể xác định phụ huynh hiện tại." });
            }

            var monday = GetWeekStart(DateOnly.FromDateTime((weekStart ?? DateTime.Today).Date));
            var weekEnd = monday.AddDays(6);

            // Lấy danh sách học sinh của phụ huynh
            var parentStudents = await _db.Phuhuynhhocsinhs
                .Where(p => p.MaPhuHuynh == parsedParentId && (p.DaXoa == null || p.DaXoa == false) && (p.TrangThai == null || p.TrangThai == true))
                .Select(p => p.MaHocSinh)
                .Distinct()
                .ToListAsync();

            if (!parentStudents.Any())
            {
                return Ok(new ParentScheduleBoardDto
                {
                    WeekStart = monday.ToDateTime(TimeOnly.MinValue),
                    Schedules = new List<ParentScheduleItemDto>()
                });
            }

            // Lấy danh sách lớp của các học sinh
            var studentClasses = await _db.Hocsinhlophocs
                .Where(h => parentStudents.Contains(h.MaHocSinh) && 
                           (h.DaXoa == null || h.DaXoa == false) && 
                           (h.TrangThai == null || h.TrangThai == true))
                .Include(h => h.MaHocSinhNavigation)
                .ThenInclude(hs => hs.MaNguoiDungNavigation)
                .ToListAsync();

            var studentNamesById = await _db.Hocsinhs
                .Where(h => parentStudents.Contains(h.MaHocSinh) &&
                           (h.DaXoa == null || h.DaXoa == false) &&
                           (h.TrangThai == null || h.TrangThai == true))
                .Select(h => new
                {
                    h.MaHocSinh,
                    HoTen = h.MaNguoiDungNavigation.HoTen,
                    TenDangNhap = h.MaNguoiDungNavigation.TenDangNhap
                })
                .ToDictionaryAsync(
                    x => x.MaHocSinh,
                    x => !string.IsNullOrWhiteSpace(x.HoTen) ? x.HoTen : (!string.IsNullOrWhiteSpace(x.TenDangNhap) ? x.TenDangNhap : x.MaHocSinh.ToString())
                );

            var classIds = studentClasses.Select(h => h.MaLopHoc).Distinct().ToList();

            // Lấy lịch của các lớp
            var scheduleRows = await _db.Buoihocs
                .Where(b => (b.DaXoa == null || b.DaXoa == false) && 
                           b.NgayHoc >= monday && 
                           b.NgayHoc <= weekEnd && 
                           classIds.Contains(b.MaLopHoc))
                .Include(b => b.MaLopHocNavigation)
                .Include(b => b.MaPhongHocNavigation)
                .Include(b => b.MaLopHocNavigation.Giangvienlophocs)
                .ThenInclude(gl => gl.MaGiangVienNavigation)
                .ThenInclude(gv => gv.MaNguoiDungNavigation)
                .OrderBy(b => b.NgayHoc)
                .ThenBy(b => b.MaTietBatDau)
                .ToListAsync();

            var schedules = new List<ParentScheduleItemDto>();
            foreach (var schedule in scheduleRows)
            {
                // Tìm học sinh thuộc lớp này
                var studentsInClass = studentClasses
                    .Where(h => h.MaLopHoc == schedule.MaLopHoc)
                    .ToList();

                var teacherName = schedule.MaLopHocNavigation?.Giangvienlophocs
                    .FirstOrDefault(g => (g.DaXoa == null || g.DaXoa == false) && (g.TrangThai == null || g.TrangThai == true))
                    ?.MaGiangVienNavigation?.MaNguoiDungNavigation?.HoTen ?? "Chưa phân công";

                // Tạo một record cho mỗi học sinh trong lớp
                foreach (var studentClass in studentsInClass)
                {
                    var studentName = studentNamesById.TryGetValue(studentClass.MaHocSinh, out var mappedName)
                        ? mappedName
                        : studentClass.MaHocSinhNavigation?.MaNguoiDungNavigation?.HoTen ?? studentClass.MaHocSinh.ToString();

                    schedules.Add(new ParentScheduleItemDto
                    {
                        Id = schedule.MaBuoiHoc,
                        StudentId = studentClass.MaHocSinh.ToString(),
                        StudentName = studentName,
                        ClassCode = schedule.MaLopHocNavigation?.TenLop ?? schedule.MaLopHoc.ToString(),
                        ClassName = schedule.MaLopHocNavigation?.TenLop ?? string.Empty,
                        Teacher = teacherName,
                        Room = schedule.MaPhongHocNavigation?.TenPhong ?? "Chưa gán phòng",
                        NgayHoc = schedule.NgayHoc,
                        DayIdx = GetDayIndex(monday, schedule.NgayHoc),
                        SlotId = schedule.MaTietBatDau,
                        SlotEndId = schedule.MaTietKetThuc,
                        Subject = schedule.TieuDe ?? schedule.MaLopHocNavigation?.TenLop ?? "Buổi học"
                    });
                }
            }

            return Ok(new ParentScheduleBoardDto
            {
                WeekStart = monday.ToDateTime(TimeOnly.MinValue),
                Schedules = schedules
            });
        }

        // ============= Schedule Change Request Endpoints =============

        /// <summary>
        /// Giáo viên tạo yêu cầu đổi lịch dạy
        /// </summary>
        [HttpPost("change-request")]
        public async Task<IActionResult> CreateScheduleChangeRequest([FromBody] CreateScheduleChangeRequestDto dto)
        {
            var maGiangVien = User.GetProfileId() ?? User.GetUserId();
            if (!maGiangVien.HasValue)
            {
                return Unauthorized(new { error = "Không thể lấy thông tin giáo viên từ token" });
            }

            var result = await _scheduleChangeRequestService.CreateScheduleChangeRequestAsync(maGiangVien.Value, dto);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        /// <summary>
        /// Lấy danh sách yêu cầu đổi lịch chờ duyệt (chỉ admin)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("change-requests/pending")]
        public async Task<IActionResult> GetPendingScheduleChangeRequests()
        {
            var requests = await _scheduleChangeRequestService.GetPendingRequestsAsync();
            return Ok(requests);
        }

        /// <summary>
        /// Lấy danh sách yêu cầu đổi lịch của giáo viên đăng nhập
        /// </summary>
        [HttpGet("change-requests/my-requests")]
        public async Task<IActionResult> GetMyScheduleChangeRequests()
        {
            var maGiangVien = User.GetProfileId() ?? User.GetUserId();
            if (!maGiangVien.HasValue)
            {
                return Unauthorized(new { error = "Không thể lấy thông tin giáo viên từ token" });
            }

            var requests = await _scheduleChangeRequestService.GetRequestsByTeacherAsync(maGiangVien.Value);
            return Ok(requests);
        }

        /// <summary>
        /// Lấy chi tiết một yêu cầu đổi lịch
        /// </summary>
        [HttpGet("change-requests/{maYeuCau}")]
        public async Task<IActionResult> GetScheduleChangeRequestDetail(Guid maYeuCau)
        {
            var request = await _scheduleChangeRequestService.GetRequestByIdAsync(maYeuCau);
            
            if (request == null)
            {
                return NotFound(new { error = "Yêu cầu không tồn tại" });
            }

            return Ok(request);
        }

        /// <summary>
        /// Admin duyệt yêu cầu đổi lịch
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost("change-request/approve")]
        public async Task<IActionResult> ApproveScheduleChangeRequest([FromBody] ApproveScheduleChangeRequestDto dto)
        {
            if (dto.TrangThaiDuyet != 1 && dto.TrangThaiDuyet != 2)
            {
                return BadRequest(new { error = "Trạng thái không hợp lệ (1: Duyệt, 2: Từ chối)" });
            }

            ScheduleChangeRequestResponseDto result;

            if (dto.TrangThaiDuyet == 1)
            {
                result = await _scheduleChangeRequestService.ApproveRequestAsync(dto.MaYeuCau, dto.GhiChuAdmin);
            }
            else
            {
                result = await _scheduleChangeRequestService.RejectRequestAsync(dto.MaYeuCau, dto.GhiChuAdmin);
            }

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        /// <summary>
        /// Admin từ chối yêu cầu đổi lịch
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost("change-request/reject")]
        public async Task<IActionResult> RejectScheduleChangeRequest([FromBody] ApproveScheduleChangeRequestDto dto)
        {
            var result = await _scheduleChangeRequestService.RejectRequestAsync(dto.MaYeuCau, dto.GhiChuAdmin);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
