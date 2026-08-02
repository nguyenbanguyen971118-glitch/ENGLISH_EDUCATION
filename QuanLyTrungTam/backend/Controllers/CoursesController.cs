using backend.Attributes;
using backend.Data;
using backend.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Authorize]
[Route("api/courses")]
public class CoursesController : ControllerBase
{
    private readonly AppDbContext _db;

    public CoursesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var courses = await _db.Khoahocs
            .Where(c => c.DaXoa != true)
            .OrderBy(c => c.TenKhoaHoc)
            .Select(c => ToDto(c))
            .ToListAsync();

        return Ok(ApiResponseDto<List<CourseDto>>.Ok(courses));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var course = await _db.Khoahocs.FirstOrDefaultAsync(c => c.MaKhoaHoc == id && c.DaXoa != true);
        return course == null
            ? NotFound(ApiResponseDto<object>.Fail("Khong tim thay khoa hoc.", "COURSE_NOT_FOUND"))
            : Ok(ApiResponseDto<CourseDto>.Ok(ToDto(course)));
    }

    [HttpPost]
    [AuthorizeByPermission("COURSES_CREATE")]
    public async Task<IActionResult> Create([FromBody] UpsertCourseRequestDto request)
    {
        var validation = await ValidateRequestAsync(request);
        if (validation != null)
        {
            return BadRequest(validation);
        }

        var course = new Khoahoc
        {
            MaKhoaHoc = Guid.NewGuid(),
            TenKhoaHoc = request.Name.Trim(),
            MoTa = request.Description?.Trim(),
            GiaCoBan = request.BasePrice,
            TrangThai = request.Active,
            DaXoa = false,
            ThoiGianTao = DateTime.UtcNow
        };

        _db.Khoahocs.Add(course);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = course.MaKhoaHoc }, ApiResponseDto<CourseDto>.Ok(ToDto(course)));
    }

    [HttpPut("{id:guid}")]
    [AuthorizeByPermission("COURSES_EDIT")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpsertCourseRequestDto request)
    {
        var course = await _db.Khoahocs.FirstOrDefaultAsync(c => c.MaKhoaHoc == id && c.DaXoa != true);
        if (course == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Khong tim thay khoa hoc.", "COURSE_NOT_FOUND"));
        }

        var validation = await ValidateRequestAsync(request, id);
        if (validation != null)
        {
            return BadRequest(validation);
        }

        course.TenKhoaHoc = request.Name.Trim();
        course.MoTa = request.Description?.Trim();
        course.GiaCoBan = request.BasePrice;
        course.TrangThai = request.Active;
        course.ThoiGianSua = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(ApiResponseDto<CourseDto>.Ok(ToDto(course)));
    }

    [HttpDelete("{id:guid}")]
    [AuthorizeByPermission("COURSES_DELETE")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var course = await _db.Khoahocs.FirstOrDefaultAsync(c => c.MaKhoaHoc == id && c.DaXoa != true);
        if (course == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Khong tim thay khoa hoc.", "COURSE_NOT_FOUND"));
        }

        course.DaXoa = true;
        course.TrangThai = false;
        course.ThoiGianSua = DateTime.UtcNow;

        var classLinks = await _db.ChitietkhoahocLophocs.Where(x => x.MaKhoaHoc == id && x.DaXoa != true).ToListAsync();
        foreach (var link in classLinks)
        {
            link.DaXoa = true;
            link.TrangThai = false;
            link.ThoiGianSua = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();
        return Ok(ApiResponseDto<object>.Ok(new { id }, "Da xoa mem khoa hoc."));
    }

    private async Task<ApiResponseDto<object>?> ValidateRequestAsync(UpsertCourseRequestDto request, Guid? currentId = null)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return ApiResponseDto<object>.Fail("Ten khoa hoc la bat buoc.", "COURSE_NAME_REQUIRED");
        }

        var normalizedName = request.Name.Trim();
        var exists = await _db.Khoahocs.AnyAsync(c =>
            c.DaXoa != true &&
            c.TenKhoaHoc == normalizedName &&
            (currentId == null || c.MaKhoaHoc != currentId.Value));

        return exists ? ApiResponseDto<object>.Fail("Ten khoa hoc da ton tai.", "COURSE_NAME_EXISTS") : null;
    }

    private static CourseDto ToDto(Khoahoc course)
    {
        return new CourseDto
        {
            Id = course.MaKhoaHoc,
            Name = course.TenKhoaHoc,
            Description = course.MoTa,
            BasePrice = course.GiaCoBan,
            Active = course.TrangThai != false
        };
    }
}
