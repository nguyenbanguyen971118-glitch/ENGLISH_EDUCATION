using backend.Attributes;
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
[Route("api/classes")]
public class ClassesController : ControllerBase
{
    private readonly AppDbContext _db;

    public ClassesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var classes = await _db.Lophocs
            .Where(c => c.DaXoa != true)
            .Include(c => c.ChitietkhoahocLophocs.Where(x => x.DaXoa != true))
            .ThenInclude(x => x.MaKhoaHocNavigation)
            .Include(c => c.Giangvienlophocs.Where(x => x.DaXoa != true))
            .ThenInclude(x => x.MaGiangVienNavigation)
            .ThenInclude(x => x.MaNguoiDungNavigation)
            .OrderBy(c => c.TenLop)
            .Select(c => ToDto(c))
            .ToListAsync();

        return Ok(ApiResponseDto<List<ClassDto>>.Ok(classes));
    }

    [HttpGet("assigned-to-me")]
    [Authorize(Roles = "Giao_Vien")]
    public async Task<IActionResult> GetAssignedToMe()
    {
        var teacherId = User.GetProfileId();
        if (!teacherId.HasValue)
        {
            return NotFound(ApiResponseDto<object>.Fail("Khong tim thay ho so giao vien.", "TEACHER_PROFILE_NOT_FOUND"));
        }

        var classes = await _db.Lophocs
            .Where(c =>
                c.DaXoa != true &&
                c.Giangvienlophocs.Any(g =>
                    g.MaGiangVien == teacherId.Value &&
                    g.DaXoa != true &&
                    g.TrangThai != false))
            .Include(c => c.ChitietkhoahocLophocs.Where(x => x.DaXoa != true))
            .ThenInclude(x => x.MaKhoaHocNavigation)
            .Include(c => c.Giangvienlophocs.Where(x => x.DaXoa != true))
            .ThenInclude(x => x.MaGiangVienNavigation)
            .ThenInclude(x => x.MaNguoiDungNavigation)
            .Include(c => c.Hocsinhlophocs.Where(x => x.DaXoa != true && x.TrangThai != false))
            .OrderBy(c => c.TenLop)
            .Select(c => ToDto(c))
            .ToListAsync();

        return Ok(ApiResponseDto<List<ClassDto>>.Ok(classes));
    }

    [HttpGet("teachers-overview")]
    [AuthorizeByPermission("PAGE_ADMIN_CLASSES_VIEW")]
    public async Task<IActionResult> GetTeachersOverview()
    {
        var teachers = await _db.Giangviens
            .Where(t => t.DaXoa != true)
            .Include(t => t.MaNguoiDungNavigation)
            .Include(t => t.Giangvienlophocs.Where(x => x.DaXoa != true))
            .ThenInclude(x => x.MaLopHocNavigation)
            .ThenInclude(x => x.ChitietkhoahocLophocs.Where(x => x.DaXoa != true))
            .ThenInclude(x => x.MaKhoaHocNavigation)
            .OrderBy(t => t.MaNguoiDungNavigation.HoTen)
            .Select(t => ToOverviewDto(t))
            .ToListAsync();

        return Ok(ApiResponseDto<List<TeacherOverviewDto>>.Ok(teachers));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var classEntity = await LoadClassAsync(id);
        return classEntity == null
            ? NotFound(ApiResponseDto<object>.Fail("Khong tim thay lop hoc.", "CLASS_NOT_FOUND"))
            : Ok(ApiResponseDto<ClassDto>.Ok(ToDto(classEntity)));
    }

    [HttpPost]
    [AuthorizeByPermission("PAGE_ADMIN_CLASSES_VIEW")]
    public async Task<IActionResult> Create([FromBody] UpsertClassRequestDto request)
    {
        var validation = await ValidateRequestAsync(request);
        if (validation != null)
        {
            return BadRequest(validation);
        }

        // Start a transaction so that class + schedules are saved atomically
        await using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            var classEntity = new Lophoc
            {
                MaLopHoc = Guid.NewGuid(),
                TenLop = request.Name.Trim(),
                NgayBatDau = request.StartDate,
                NgayKetThuc = request.EndDate,
                SiSoHienTai = 0,
                SiSoToiDa = request.Capacity,
                TrangThai = request.Active,
                DaXoa = false,
                ThoiGianTao = DateTime.UtcNow
            };

            _db.Lophocs.Add(classEntity);
            await _db.SaveChangesAsync();

            await ReplaceCourseLinksAsync(classEntity.MaLopHoc, request.CourseIds);
            await ReplaceTeacherLinksAsync(classEntity.MaLopHoc, request.TeacherIds);
            await _db.SaveChangesAsync();

            // Generate schedules if configuration provided and TotalPeriods > 0
            var scheduleGenerator = HttpContext.RequestServices.GetRequiredService<backend.Services.ScheduleGeneratorService>();
            if ((request.ScheduleConfigs != null && request.ScheduleConfigs.Count > 0) && (request.TotalPeriods.HasValue && request.TotalPeriods.Value > 0))
            {
                var (Success, Error, EndDate) = await scheduleGenerator.GenerateAndPersistAsync(classEntity.MaLopHoc, request);
                if (!Success)
                {
                    await tx.RollbackAsync();
                    return BadRequest(ApiResponseDto<object>.Fail(Error ?? "Khong the tao lich.", "SCHEDULE_GENERATION_FAILED"));
                }

                if (EndDate.HasValue)
                {
                    classEntity.NgayKetThuc = EndDate.Value;
                    _db.Lophocs.Update(classEntity);
                    await _db.SaveChangesAsync();
                }
            }

            await tx.CommitAsync();

            var created = await LoadClassAsync(classEntity.MaLopHoc);
            return CreatedAtAction(nameof(GetById), new { id = classEntity.MaLopHoc }, ApiResponseDto<ClassDto>.Ok(ToDto(created!)));
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return BadRequest(ApiResponseDto<object>.Fail(ex.Message ?? "Loi khi tao lop.", "CREATE_CLASS_FAILED"));
        }
    }

    [HttpPut("{id:guid}")]
    [AuthorizeByPermission("PAGE_ADMIN_CLASSES_VIEW")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpsertClassRequestDto request)
    {
        var classEntity = await _db.Lophocs.FirstOrDefaultAsync(c => c.MaLopHoc == id && c.DaXoa != true);
        if (classEntity == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Khong tim thay lop hoc.", "CLASS_NOT_FOUND"));
        }

        var validation = await ValidateRequestAsync(request, id);
        if (validation != null)
        {
            return BadRequest(validation);
        }

        var activeStudentCount = await CountActiveStudentsAsync(id);
        if (request.Capacity.HasValue && request.Capacity.Value < activeStudentCount)
        {
            return BadRequest(ApiResponseDto<object>.Fail("Suc chua moi nho hon so hoc sinh hien tai.", "CLASS_CAPACITY_TOO_SMALL"));
        }

        classEntity.TenLop = request.Name.Trim();
        classEntity.NgayBatDau = request.StartDate;
        classEntity.NgayKetThuc = request.EndDate;
        classEntity.SiSoToiDa = request.Capacity;
        classEntity.SiSoHienTai = activeStudentCount;
        classEntity.TrangThai = request.Active;
        classEntity.ThoiGianSua = DateTime.UtcNow;

        await ReplaceCourseLinksAsync(id, request.CourseIds);
        await ReplaceTeacherLinksAsync(id, request.TeacherIds);
        await _db.SaveChangesAsync();

        var updated = await LoadClassAsync(id);
        return Ok(ApiResponseDto<ClassDto>.Ok(ToDto(updated!)));
    }

    [HttpDelete("{id:guid}")]
    [AuthorizeByPermission("PAGE_ADMIN_CLASSES_VIEW")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var classEntity = await _db.Lophocs.FirstOrDefaultAsync(c => c.MaLopHoc == id && c.DaXoa != true);
        if (classEntity == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Khong tim thay lop hoc.", "CLASS_NOT_FOUND"));
        }

        classEntity.DaXoa = true;
        classEntity.TrangThai = false;
        classEntity.ThoiGianSua = DateTime.UtcNow;

        foreach (var link in await _db.ChitietkhoahocLophocs.Where(x => x.MaLopHoc == id && x.DaXoa != true).ToListAsync())
        {
            link.DaXoa = true;
            link.TrangThai = false;
            link.ThoiGianSua = DateTime.UtcNow;
        }

        foreach (var link in await _db.Giangvienlophocs.Where(x => x.MaLopHoc == id && x.DaXoa != true).ToListAsync())
        {
            link.DaXoa = true;
            link.TrangThai = false;
            link.ThoiGianSua = DateTime.UtcNow;
        }

        // Xóa tất cả lịch học của lớp này
        foreach (var schedule in await _db.Buoihocs.Where(x => x.MaLopHoc == id && x.DaXoa != true).ToListAsync())
        {
            schedule.DaXoa = true;
            schedule.TrangThai = false;
            schedule.ThoiGianSua = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();
        return Ok(ApiResponseDto<object>.Ok(new { id }, "Da xoa mem lop hoc."));
    }

    private async Task<ApiResponseDto<object>?> ValidateRequestAsync(UpsertClassRequestDto request, Guid? currentId = null)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return ApiResponseDto<object>.Fail("Ten lop hoc la bat buoc.", "CLASS_NAME_REQUIRED");
        }

        if (request.Capacity.HasValue && request.Capacity.Value <= 0)
        {
            return ApiResponseDto<object>.Fail("Suc chua lop phai lon hon 0.", "CLASS_INVALID_CAPACITY");
        }

        var nameExists = await _db.Lophocs.AnyAsync(c =>
            c.DaXoa != true &&
            c.TenLop == request.Name.Trim() &&
            (currentId == null || c.MaLopHoc != currentId.Value));
        if (nameExists)
        {
            return ApiResponseDto<object>.Fail("Ten lop hoc da ton tai.", "CLASS_NAME_EXISTS");
        }

        var courseIds = request.CourseIds?.Distinct().ToList() ?? new List<Guid>();
        if (courseIds.Count > 0)
        {
            var courseCount = await _db.Khoahocs.CountAsync(c => courseIds.Contains(c.MaKhoaHoc) && c.DaXoa != true);
            if (courseCount != courseIds.Count)
            {
                return ApiResponseDto<object>.Fail("Mot hoac nhieu khoa hoc khong ton tai.", "COURSE_NOT_FOUND");
            }
        }

        var teacherIds = request.TeacherIds?.Distinct().ToList() ?? new List<Guid>();
        if (teacherIds.Count > 0)
        {
            var teacherCount = await _db.Giangviens.CountAsync(t => teacherIds.Contains(t.MaGiangVien) && t.DaXoa != true);
            if (teacherCount != teacherIds.Count)
            {
                return ApiResponseDto<object>.Fail("Mot hoac nhieu giao vien khong ton tai.", "TEACHER_NOT_FOUND");
            }
        }

        // No mandatory requirement for TotalPeriods here — Admin may input freely.

        return null;
    }

    private async Task ReplaceCourseLinksAsync(Guid classId, List<Guid> courseIds)
    {
        courseIds = courseIds?.Distinct().ToList() ?? new List<Guid>();
        var existing = await _db.ChitietkhoahocLophocs.Where(x => x.MaLopHoc == classId).ToListAsync();

        foreach (var link in existing)
        {
            var shouldActive = courseIds.Contains(link.MaKhoaHoc);
            link.DaXoa = !shouldActive;
            link.TrangThai = shouldActive;
            link.ThoiGianSua = DateTime.UtcNow;
        }

        var existingIds = existing.Select(x => x.MaKhoaHoc).ToHashSet();
        foreach (var courseId in courseIds.Where(id => !existingIds.Contains(id)))
        {
            _db.ChitietkhoahocLophocs.Add(new ChitietkhoahocLophoc
            {
                MaLopHoc = classId,
                MaKhoaHoc = courseId,
                TrangThai = true,
                DaXoa = false,
                ThoiGianTao = DateTime.UtcNow
            });
        }
    }

    private async Task ReplaceTeacherLinksAsync(Guid classId, List<Guid> teacherIds)
    {
        teacherIds = teacherIds?.Distinct().ToList() ?? new List<Guid>();
        var existing = await _db.Giangvienlophocs.Where(x => x.MaLopHoc == classId).ToListAsync();

        foreach (var link in existing)
        {
            var shouldActive = teacherIds.Contains(link.MaGiangVien);
            link.DaXoa = !shouldActive;
            link.TrangThai = shouldActive;
            link.ThoiGianSua = DateTime.UtcNow;
        }

        var existingIds = existing.Select(x => x.MaGiangVien).ToHashSet();
        foreach (var teacherId in teacherIds.Where(id => !existingIds.Contains(id)))
        {
            _db.Giangvienlophocs.Add(new Giangvienlophoc
            {
                MaLopHoc = classId,
                MaGiangVien = teacherId,
                NgayThamGia = DateOnly.FromDateTime(DateTime.Today),
                TrangThai = true,
                DaXoa = false,
                ThoiGianTao = DateTime.UtcNow
            });
        }
    }

    private async Task<Lophoc?> LoadClassAsync(Guid id)
    {
        return await _db.Lophocs
            .Where(c => c.MaLopHoc == id && c.DaXoa != true)
            .Include(c => c.ChitietkhoahocLophocs.Where(x => x.DaXoa != true))
            .ThenInclude(x => x.MaKhoaHocNavigation)
            .Include(c => c.Giangvienlophocs.Where(x => x.DaXoa != true))
            .ThenInclude(x => x.MaGiangVienNavigation)
            .ThenInclude(x => x.MaNguoiDungNavigation)
            .FirstOrDefaultAsync();
    }

    private async Task<int> CountActiveStudentsAsync(Guid classId)
    {
        return await _db.Hocsinhlophocs.CountAsync(x => x.MaLopHoc == classId && x.DaXoa != true && x.TrangThai != false);
    }

    private static ClassDto ToDto(Lophoc classEntity)
    {
        return new ClassDto
        {
            Id = classEntity.MaLopHoc,
            Name = classEntity.TenLop,
            StartDate = classEntity.NgayBatDau,
            EndDate = classEntity.NgayKetThuc,
            CurrentSize = classEntity.SiSoHienTai ?? classEntity.Hocsinhlophocs.Count(x => x.DaXoa != true),
            Capacity = classEntity.SiSoToiDa,
            Active = classEntity.TrangThai != false,
            Courses = classEntity.ChitietkhoahocLophocs
                .Where(x => x.DaXoa != true)
                .Select(x => new CourseDto
                {
                    Id = x.MaKhoaHoc,
                    Name = x.MaKhoaHocNavigation?.TenKhoaHoc ?? x.MaKhoaHoc.ToString(),
                    Description = x.MaKhoaHocNavigation?.MoTa,
                    BasePrice = x.MaKhoaHocNavigation?.GiaCoBan,
                    Active = x.MaKhoaHocNavigation?.TrangThai != false
                })
                .ToList(),
            Teachers = classEntity.Giangvienlophocs
                .Where(x => x.DaXoa != true)
                .Select(x => new TeacherItemDto
                {
                    Id = x.MaGiangVien,
                    Name = x.MaGiangVienNavigation?.MaNguoiDungNavigation?.HoTen ?? x.MaGiangVien.ToString()
                })
                .ToList()
        };
    }

    private static TeacherOverviewDto ToOverviewDto(Giangvien teacher)
    {
        return new TeacherOverviewDto
        {
            Id = teacher.MaGiangVien,
            Name = teacher.MaNguoiDungNavigation?.HoTen ?? teacher.MaGiangVien.ToString(),
            Email = teacher.MaNguoiDungNavigation?.Email,
            Classes = teacher.Giangvienlophocs
                .Where(x => x.DaXoa != true && x.MaLopHocNavigation != null && x.MaLopHocNavigation.DaXoa != true)
                .Select(x => new TeacherClassOverviewDto
                {
                    ClassId = x.MaLopHoc,
                    ClassName = x.MaLopHocNavigation!.TenLop,
                    Courses = x.MaLopHocNavigation!.ChitietkhoahocLophocs
                        .Where(c => c.DaXoa != true)
                        .Select(c => new CourseDto
                        {
                            Id = c.MaKhoaHoc,
                            Name = c.MaKhoaHocNavigation?.TenKhoaHoc ?? c.MaKhoaHoc.ToString(),
                            Description = c.MaKhoaHocNavigation?.MoTa,
                            BasePrice = c.MaKhoaHocNavigation?.GiaCoBan,
                            Active = c.MaKhoaHocNavigation?.TrangThai != false
                        })
                        .ToList()
                })
                .ToList()
        };
    }
}
