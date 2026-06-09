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
public class ParentChildrenController : ControllerBase
{
    private readonly AppDbContext _db;

    public ParentChildrenController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("api/parents/{parentUserId:guid}/children")]
    public async Task<IActionResult> GetChildren(Guid parentUserId)
    {
        if (!User.IsInRole("Admin") && User.GetUserId() != parentUserId)
        {
            return Forbid();
        }

        var parent = await _db.Phuhuynhs.FirstOrDefaultAsync(x => x.MaNguoiDung == parentUserId && x.DaXoa != true);
        if (parent == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Khong tim thay phu huynh.", "PARENT_NOT_FOUND"));
        }

        var rows = await _db.Phuhuynhhocsinhs
            .Where(x => x.MaPhuHuynh == parent.MaPhuHuynh && x.DaXoa != true)
            .Include(x => x.MaHocSinhNavigation)
            .ThenInclude(x => x.MaNguoiDungNavigation)
            .OrderBy(x => x.MaHocSinhNavigation.MaNguoiDungNavigation.HoTen)
            .ToListAsync();

        return Ok(ApiResponseDto<List<ParentChildDto>>.Ok(rows.Select(ToDto).ToList()));
    }

    [HttpPost("api/parents/{parentUserId:guid}/children")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddChild(Guid parentUserId, [FromBody] AddParentChildRequestDto request)
    {
        var parent = await _db.Phuhuynhs.FirstOrDefaultAsync(x => x.MaNguoiDung == parentUserId && x.DaXoa != true);
        if (parent == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Khong tim thay phu huynh.", "PARENT_NOT_FOUND"));
        }

        if (!await _db.Hocsinhs.AnyAsync(x => x.MaHocSinh == request.StudentId && x.DaXoa != true))
        {
            return NotFound(ApiResponseDto<object>.Fail("Khong tim thay hoc sinh.", "STUDENT_NOT_FOUND"));
        }

        var existing = await _db.Phuhuynhhocsinhs.FirstOrDefaultAsync(x => x.MaPhuHuynh == parent.MaPhuHuynh && x.MaHocSinh == request.StudentId);
        if (existing != null)
        {
            existing.DaXoa = false;
            existing.TrangThai = true;
            existing.ThoiGianSua = DateTime.UtcNow;
        }
        else
        {
            _db.Phuhuynhhocsinhs.Add(new Phuhuynhhocsinh
            {
                MaPhuHuynh = parent.MaPhuHuynh,
                MaHocSinh = request.StudentId,
                TrangThai = true,
                DaXoa = false,
                ThoiGianTao = DateTime.UtcNow
            });
        }

        await _db.SaveChangesAsync();
        return Ok(ApiResponseDto<object>.Ok(new { parentUserId, studentId = request.StudentId }, "Da gan phu huynh voi hoc sinh."));
    }

    [HttpDelete("api/parents/{parentUserId:guid}/children/{studentId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveChild(Guid parentUserId, Guid studentId)
    {
        var parent = await _db.Phuhuynhs.FirstOrDefaultAsync(x => x.MaNguoiDung == parentUserId && x.DaXoa != true);
        if (parent == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Khong tim thay phu huynh.", "PARENT_NOT_FOUND"));
        }

        var row = await _db.Phuhuynhhocsinhs.FirstOrDefaultAsync(x => x.MaPhuHuynh == parent.MaPhuHuynh && x.MaHocSinh == studentId && x.DaXoa != true);
        if (row == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Khong tim thay lien ket phu huynh-hoc sinh.", "PARENT_CHILD_NOT_FOUND"));
        }

        row.DaXoa = true;
        row.TrangThai = false;
        row.ThoiGianSua = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return Ok(ApiResponseDto<object>.Ok(new { parentUserId, studentId }, "Da go hoc sinh khoi phu huynh."));
    }

    private static ParentChildDto ToDto(Phuhuynhhocsinh row)
    {
        return new ParentChildDto
        {
            StudentId = row.MaHocSinh,
            UserId = row.MaHocSinhNavigation.MaNguoiDung,
            FullName = row.MaHocSinhNavigation.MaNguoiDungNavigation.HoTen,
            Email = row.MaHocSinhNavigation.MaNguoiDungNavigation.Email,
            Active = row.TrangThai != false && row.DaXoa != true
        };
    }
}
