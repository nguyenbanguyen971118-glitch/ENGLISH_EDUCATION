using backend.Data;
using backend.DTOs;
using backend.Helpers;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Authorize]
public class AttendanceController : ControllerBase
{
    private static readonly Dictionary<string, (string Code, string Label)> StatusMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["present"] = ("DD_PRESENT", "Co mat"),
        ["co_mat"] = ("DD_PRESENT", "Co mat"),
        ["absent"] = ("DD_ABSENT", "Vang"),
        ["vang"] = ("DD_ABSENT", "Vang"),
        ["late"] = ("DD_LATE", "Di muon"),
        ["di_muon"] = ("DD_LATE", "Di muon"),
        ["excused"] = ("DD_EXCUSED", "Co phep"),
        ["co_phep"] = ("DD_EXCUSED", "Co phep")
    };

    private readonly AppDbContext _db;

    public AttendanceController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("api/classes/{classId:guid}/sessions/{sessionId:guid}/attendance")]
    [Authorize(Roles = "Admin,Giao_Vien")]
    public async Task<IActionResult> GetClassSessionAttendance(Guid classId, Guid sessionId)
    {
        if (!await SessionBelongsToClassAsync(classId, sessionId))
        {
            return NotFound(ApiResponseDto<object>.Fail("Khong tim thay buoi hoc trong lop.", "SESSION_NOT_FOUND"));
        }

        if (!await CanAccessClassAsync(classId))
        {
            return Forbid();
        }

        var statusLookup = await GetAttendanceStatusLookupAsync();
        var students = await _db.Hocsinhlophocs
            .Where(x => x.MaLopHoc == classId && x.DaXoa != true && x.TrangThai != false)
            .Include(x => x.MaHocSinhNavigation)
            .ThenInclude(x => x.MaNguoiDungNavigation)
            .OrderBy(x => x.MaHocSinhNavigation.MaNguoiDungNavigation.HoTen)
            .ToListAsync();

        var attendance = await _db.Diemdanhs
            .Where(x => x.MaBuoiHoc == sessionId && x.DaXoa != true)
            .ToDictionaryAsync(x => x.MaHocSinh, x => x);

        var result = students.Select(enrollment =>
        {
            attendance.TryGetValue(enrollment.MaHocSinh, out var row);
            return new AttendanceItemDto
            {
                StudentId = enrollment.MaHocSinh,
                StudentName = enrollment.MaHocSinhNavigation.MaNguoiDungNavigation.HoTen,
                Status = row?.MaTrangThai != null && statusLookup.TryGetValue(row.MaTrangThai.Value, out var status) ? status.Status : string.Empty,
                StatusLabel = row?.MaTrangThai != null && statusLookup.TryGetValue(row.MaTrangThai.Value, out status) ? status.Label : "Chua diem danh",
                Note = row?.GhiChu,
                UpdatedAt = row?.ThoiGianSua ?? row?.ThoiGianTao
            };
        }).ToList();

        return Ok(ApiResponseDto<List<AttendanceItemDto>>.Ok(result));
    }

    [HttpPost("api/classes/{classId:guid}/sessions/{sessionId:guid}/attendance")]
    [Authorize(Roles = "Giao_Vien")]
    public async Task<IActionResult> SaveClassSessionAttendance(Guid classId, Guid sessionId, [FromBody] SaveAttendanceRequestDto request)
    {
        if (!await SessionBelongsToClassAsync(classId, sessionId))
        {
            return NotFound(ApiResponseDto<object>.Fail("Khong tim thay buoi hoc trong lop.", "SESSION_NOT_FOUND"));
        }

        if (!await CanAccessClassAsync(classId))
        {
            return Forbid();
        }

        if (request.Items == null || request.Items.Count == 0)
        {
            return BadRequest(ApiResponseDto<object>.Fail("Danh sach diem danh khong duoc de trong.", "ATTENDANCE_EMPTY"));
        }

        var duplicateStudent = request.Items.GroupBy(x => x.StudentId).FirstOrDefault(g => g.Count() > 1);
        if (duplicateStudent != null)
        {
            return BadRequest(ApiResponseDto<object>.Fail("Khong duoc gui trung hoc sinh trong cung request.", "ATTENDANCE_DUPLICATE_STUDENT"));
        }

        var enrolledStudentIds = await _db.Hocsinhlophocs
            .Where(x => x.MaLopHoc == classId && x.DaXoa != true && x.TrangThai != false)
            .Select(x => x.MaHocSinh)
            .ToListAsync();

        var invalidStudent = request.Items.FirstOrDefault(x => !enrolledStudentIds.Contains(x.StudentId));
        if (invalidStudent != null)
        {
            return BadRequest(ApiResponseDto<object>.Fail("Hoc sinh khong thuoc lop nay.", "ATTENDANCE_STUDENT_NOT_IN_CLASS"));
        }

        var statusIdsByStatus = await EnsureAttendanceStatusIdsAsync();
        var existingRows = await _db.Diemdanhs
            .Where(x => x.MaBuoiHoc == sessionId && request.Items.Select(i => i.StudentId).Contains(x.MaHocSinh))
            .ToDictionaryAsync(x => x.MaHocSinh, x => x);

        foreach (var item in request.Items)
        {
            if (!TryNormalizeStatus(item.Status, out var normalizedStatus))
            {
                return BadRequest(ApiResponseDto<object>.Fail($"Trang thai diem danh khong hop le: {item.Status}", "ATTENDANCE_INVALID_STATUS"));
            }

            var statusId = statusIdsByStatus[normalizedStatus];
            if (existingRows.TryGetValue(item.StudentId, out var row))
            {
                row.MaTrangThai = statusId;
                row.GhiChu = item.Note?.Trim();
                row.DaXoa = false;
                row.TrangThai = true;
                row.ThoiGianSua = DateTime.UtcNow;
            }
            else
            {
                _db.Diemdanhs.Add(new Diemdanh
                {
                    MaBuoiHoc = sessionId,
                    MaHocSinh = item.StudentId,
                    MaTrangThai = statusId,
                    GhiChu = item.Note?.Trim(),
                    TrangThai = true,
                    DaXoa = false,
                    ThoiGianTao = DateTime.UtcNow
                });
            }
        }

        await _db.SaveChangesAsync();
        return Ok(ApiResponseDto<object>.Ok(new { classId, sessionId, count = request.Items.Count }, "Da luu diem danh."));
    }

    [HttpGet("api/parents/{parentUserId:guid}/children/attendance")]
    [Authorize(Roles = "Phu_Huynh")]
    public async Task<IActionResult> GetParentChildrenAttendance(Guid parentUserId)
    {
        var currentUserId = User.GetUserId();
        if (!currentUserId.HasValue || currentUserId.Value != parentUserId)
        {
            return Forbid();
        }

        var parentProfile = await _db.Phuhuynhs.FirstOrDefaultAsync(x => x.MaNguoiDung == parentUserId && x.DaXoa != true);
        if (parentProfile == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Khong tim thay ho so phu huynh.", "PARENT_NOT_FOUND"));
        }

        var childIds = await _db.Phuhuynhhocsinhs
            .Where(x => x.MaPhuHuynh == parentProfile.MaPhuHuynh && x.DaXoa != true && x.TrangThai != false)
            .Select(x => x.MaHocSinh)
            .ToListAsync();

        var statusLookup = await GetAttendanceStatusLookupAsync();

        var rows = await _db.Diemdanhs
            .Where(x => childIds.Contains(x.MaHocSinh) && x.DaXoa != true)
            .Include(x => x.MaHocSinhNavigation)
            .ThenInclude(x => x.MaNguoiDungNavigation)
            .Include(x => x.MaBuoiHocNavigation)
            .ThenInclude(x => x.MaLopHocNavigation)
            .OrderByDescending(x => x.MaBuoiHocNavigation.NgayHoc)
            .ThenBy(x => x.MaHocSinhNavigation.MaNguoiDungNavigation.HoTen)
            .ToListAsync();

        var result = rows.Select(row =>
        {
            var status = row.MaTrangThai != null && statusLookup.TryGetValue(row.MaTrangThai.Value, out var found)
                ? found
                : (Status: "unknown", Label: "Khong ro");

            return new ParentChildAttendanceDto
            {
                StudentId = row.MaHocSinh,
                StudentName = row.MaHocSinhNavigation.MaNguoiDungNavigation.HoTen,
                ClassId = row.MaBuoiHocNavigation.MaLopHoc,
                ClassName = row.MaBuoiHocNavigation.MaLopHocNavigation.TenLop,
                SessionId = row.MaBuoiHoc,
                SessionDate = row.MaBuoiHocNavigation.NgayHoc,
                Status = status.Status,
                StatusLabel = status.Label,
                Note = row.GhiChu
            };
        }).ToList();

        return Ok(ApiResponseDto<List<ParentChildAttendanceDto>>.Ok(result));
    }

    [HttpGet("api/students/me/attendance")]
    [Authorize(Roles = "Hoc_Sinh")]
    public async Task<IActionResult> GetMyAttendance()
    {
        var studentId = User.GetProfileId();
        if (!studentId.HasValue)
        {
            return NotFound(ApiResponseDto<object>.Fail("Khong tim thay ho so hoc sinh.", "STUDENT_PROFILE_NOT_FOUND"));
        }

        var exists = await _db.Hocsinhs.AnyAsync(x => x.MaHocSinh == studentId.Value && x.DaXoa != true);
        if (!exists)
        {
            return NotFound(ApiResponseDto<object>.Fail("Khong tim thay hoc sinh.", "STUDENT_NOT_FOUND"));
        }

        var statusLookup = await GetAttendanceStatusLookupAsync();
        var rows = await _db.Diemdanhs
            .Where(x => x.MaHocSinh == studentId.Value && x.DaXoa != true)
            .Include(x => x.MaHocSinhNavigation)
            .ThenInclude(x => x.MaNguoiDungNavigation)
            .Include(x => x.MaBuoiHocNavigation)
            .ThenInclude(x => x.MaLopHocNavigation)
            .OrderByDescending(x => x.MaBuoiHocNavigation.NgayHoc)
            .ToListAsync();

        var result = rows.Select(row =>
        {
            var status = row.MaTrangThai != null && statusLookup.TryGetValue(row.MaTrangThai.Value, out var found)
                ? found
                : (Status: "unknown", Label: "Khong ro");

            return new ParentChildAttendanceDto
            {
                StudentId = row.MaHocSinh,
                StudentName = row.MaHocSinhNavigation.MaNguoiDungNavigation.HoTen,
                ClassId = row.MaBuoiHocNavigation.MaLopHoc,
                ClassName = row.MaBuoiHocNavigation.MaLopHocNavigation.TenLop,
                SessionId = row.MaBuoiHoc,
                SessionDate = row.MaBuoiHocNavigation.NgayHoc,
                Status = status.Status,
                StatusLabel = status.Label,
                Note = row.GhiChu
            };
        }).ToList();

        return Ok(ApiResponseDto<List<ParentChildAttendanceDto>>.Ok(result));
    }

    private async Task<bool> SessionBelongsToClassAsync(Guid classId, Guid sessionId)
    {
        return await _db.Buoihocs.AnyAsync(x => x.MaBuoiHoc == sessionId && x.MaLopHoc == classId && x.DaXoa != true);
    }

    private async Task<bool> CanAccessClassAsync(Guid classId)
    {
        if (User.IsInRole("Admin"))
        {
            return true;
        }

        var teacherId = User.GetProfileId();
        if (!teacherId.HasValue)
        {
            return false;
        }

        return await _db.Giangvienlophocs.AnyAsync(x =>
            x.MaLopHoc == classId &&
            x.MaGiangVien == teacherId.Value &&
            x.DaXoa != true &&
            x.TrangThai != false);
    }

    private static bool TryNormalizeStatus(string status, out string normalizedStatus)
    {
        normalizedStatus = string.Empty;
        if (!StatusMap.TryGetValue(status?.Trim() ?? string.Empty, out var mapped))
        {
            return false;
        }

        normalizedStatus = mapped.Code;
        return true;
    }

    private async Task<Dictionary<string, int>> EnsureAttendanceStatusIdsAsync()
    {
        var group = await _db.Nhomdanhmucs.FirstOrDefaultAsync(x => x.MaCode == "DIEM_DANH_STATUS");
        if (group == null)
        {
            group = new Nhomdanhmuc
            {
                MaCode = "DIEM_DANH_STATUS",
                TenNhom = "Trang thai diem danh",
                TrangThai = true,
                DaXoa = false,
                ThoiGianTao = DateTime.UtcNow
            };
            _db.Nhomdanhmucs.Add(group);
            await _db.SaveChangesAsync();
        }

        var required = StatusMap.Values.DistinctBy(x => x.Code).ToList();
        var existing = await _db.Chitietdanhmucs
            .Where(x => x.MaNhom == group.MaNhom && required.Select(r => r.Code).Contains(x.MaCode ?? string.Empty))
            .ToListAsync();

        foreach (var status in required.Where(r => existing.All(x => x.MaCode != r.Code)))
        {
            _db.Chitietdanhmucs.Add(new Chitietdanhmuc
            {
                MaNhom = group.MaNhom,
                MaCode = status.Code,
                TenChiTiet = status.Label,
                TrangThai = true,
                DaXoa = false,
                ThoiGianTao = DateTime.UtcNow
            });
        }

        await _db.SaveChangesAsync();

        return await _db.Chitietdanhmucs
            .Where(x => x.MaNhom == group.MaNhom && required.Select(r => r.Code).Contains(x.MaCode ?? string.Empty))
            .ToDictionaryAsync(x => x.MaCode!, x => x.MaChiTiet);
    }

    private async Task<Dictionary<int, (string Status, string Label)>> GetAttendanceStatusLookupAsync()
    {
        var codes = StatusMap.Values.Select(x => x.Code).Distinct().ToHashSet();
        return await _db.Chitietdanhmucs
            .Where(x => x.MaCode != null && codes.Contains(x.MaCode))
            .ToDictionaryAsync(
                x => x.MaChiTiet,
                x => (Status: CodeToPublicStatus(x.MaCode!), Label: x.TenChiTiet));
    }

    private static string CodeToPublicStatus(string code)
    {
        return code switch
        {
            "DD_PRESENT" => "present",
            "DD_ABSENT" => "absent",
            "DD_LATE" => "late",
            "DD_EXCUSED" => "excused",
            _ => "unknown"
        };
    }
}
