using backend.Data;
using backend.DTOs;
using backend.Helpers;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Authorize(Roles = "Admin,Giao_Vien")]
[Route("api/classes/{classId:guid}/students")]
public class ClassStudentsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ClassStudentsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetStudents(Guid classId)
    {
        if (!await CanManageClassAsync(classId))
        {
            return Forbid();
        }

        var students = await _db.Hocsinhlophocs
            .Where(x => x.MaLopHoc == classId && x.DaXoa != true)
            .Include(x => x.MaHocSinhNavigation)
            .ThenInclude(x => x.MaNguoiDungNavigation)
            .OrderBy(x => x.MaHocSinhNavigation.MaNguoiDungNavigation.HoTen)
            .Select(x => ToDto(x))
            .ToListAsync();

        return Ok(ApiResponseDto<List<ClassStudentDto>>.Ok(students));
    }

    [HttpPost]
    public async Task<IActionResult> AddStudent(Guid classId, [FromBody] AddClassStudentRequestDto request)
    {
        if (!await CanManageClassAsync(classId))
        {
            return Forbid();
        }

        var classEntity = await _db.Lophocs.FirstOrDefaultAsync(x => x.MaLopHoc == classId && x.DaXoa != true);
        if (classEntity == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Khong tim thay lop hoc.", "CLASS_NOT_FOUND"));
        }

        var studentExists = await _db.Hocsinhs.AnyAsync(x => x.MaHocSinh == request.StudentId && x.DaXoa != true);
        if (!studentExists)
        {
            return NotFound(ApiResponseDto<object>.Fail("Khong tim thay hoc sinh.", "STUDENT_NOT_FOUND"));
        }

        var existing = await _db.Hocsinhlophocs.FirstOrDefaultAsync(x => x.MaLopHoc == classId && x.MaHocSinh == request.StudentId);
        if (existing != null && existing.DaXoa != true && existing.TrangThai != false)
        {
            return BadRequest(ApiResponseDto<object>.Fail("Hoc sinh da co trong lop.", "CLASS_STUDENT_EXISTS"));
        }

        var currentCount = await CountActiveStudentsAsync(classId);
        if (classEntity.SiSoToiDa.HasValue && currentCount >= classEntity.SiSoToiDa.Value)
        {
            return BadRequest(ApiResponseDto<object>.Fail("Lop hoc da vuot suc chua.", "CLASS_CAPACITY_FULL"));
        }

        if (existing == null)
        {
            _db.Hocsinhlophocs.Add(new Hocsinhlophoc
            {
                MaLopHoc = classId,
                MaHocSinh = request.StudentId,
                NgayThamGia = DateOnly.FromDateTime(DateTime.Today),
                TrangThai = true,
                DaXoa = false,
                ThoiGianTao = DateTime.UtcNow
            });
        }
        else
        {
            existing.DaXoa = false;
            existing.TrangThai = true;
            existing.NgayThamGia = DateOnly.FromDateTime(DateTime.Today);
            existing.NgayRoiLop = null;
            existing.ThoiGianSua = DateTime.UtcNow;
        }

        classEntity.SiSoHienTai = currentCount + 1;
        classEntity.ThoiGianSua = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        var saved = await _db.Hocsinhlophocs
            .Include(x => x.MaHocSinhNavigation)
            .ThenInclude(x => x.MaNguoiDungNavigation)
            .FirstAsync(x => x.MaLopHoc == classId && x.MaHocSinh == request.StudentId);

        return Ok(ApiResponseDto<ClassStudentDto>.Ok(ToDto(saved), "Da them hoc sinh vao lop."));
    }

    [HttpDelete("{studentId:guid}")]
    public async Task<IActionResult> RemoveStudent(Guid classId, Guid studentId)
    {
        if (!await CanManageClassAsync(classId))
        {
            return Forbid();
        }

        var enrollment = await _db.Hocsinhlophocs.FirstOrDefaultAsync(x => x.MaLopHoc == classId && x.MaHocSinh == studentId && x.DaXoa != true);
        if (enrollment == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Hoc sinh khong co trong lop.", "CLASS_STUDENT_NOT_FOUND"));
        }

        enrollment.DaXoa = true;
        enrollment.TrangThai = false;
        enrollment.NgayRoiLop = DateOnly.FromDateTime(DateTime.Today);
        enrollment.ThoiGianSua = DateTime.UtcNow;

        var classEntity = await _db.Lophocs.FirstOrDefaultAsync(x => x.MaLopHoc == classId);
        if (classEntity != null)
        {
            classEntity.SiSoHienTai = Math.Max(0, await CountActiveStudentsAsync(classId) - 1);
            classEntity.ThoiGianSua = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();
        return Ok(ApiResponseDto<object>.Ok(new { classId, studentId }, "Da xoa mem hoc sinh khoi lop."));
    }

    private async Task<bool> CanManageClassAsync(Guid classId)
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

    private async Task<int> CountActiveStudentsAsync(Guid classId)
    {
        return await _db.Hocsinhlophocs.CountAsync(x => x.MaLopHoc == classId && x.DaXoa != true && x.TrangThai != false);
    }

    private static ClassStudentDto ToDto(Hocsinhlophoc enrollment)
    {
        var user = enrollment.MaHocSinhNavigation.MaNguoiDungNavigation;
        return new ClassStudentDto
        {
            StudentId = enrollment.MaHocSinh,
            UserId = user.MaNguoiDung,
            FullName = user.HoTen,
            Email = user.Email,
            JoinedAt = enrollment.NgayThamGia,
            Active = enrollment.TrangThai != false && enrollment.DaXoa != true
        };
    }
}
