using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PermissionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PermissionsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("matrix")]
    public async Task<IActionResult> GetMatrix()
    {
        var permissions = await _context.Quyens
            .Include(x => x.MaChucNangNavigation)
            .OrderBy(x => x.MaChucNang)
            .ThenBy(x => x.MaQuyen)
            .Select(x => new
            {
                MaChucNang = x.MaQuyen,
                MaChucNangCode = x.TenQuyen,
                TenChucNang = x.MaChucNangNavigation.TenChucNang,
                MaTrang = "func_" + x.MaChucNang,
                TenTrang = x.MaChucNangNavigation.TenChucNang,
                ThuTu = x.MaQuyen
            })
            .ToListAsync();

        var roles = await _context.Vaitros
            .Select(x => new { x.MaVaiTro, x.TenVaiTro })
            .ToListAsync();

        var mappings = await _context.Vaitroquyens
            .Where(x => x.DaXoa == null || x.DaXoa == false)
            .Select(x => new { x.MaVaiTro, MaChucNang = x.MaQuyen })
            .ToListAsync();

        var groupedPages = permissions
            .GroupBy(x => new { x.MaTrang, x.TenTrang })
            .Select(g => new
            {
                maTrang = g.Key.MaTrang,
                tenTrang = g.Key.TenTrang,
                chucNangs = g.Select(p => new
                {
                    p.MaChucNang,
                    p.MaChucNangCode,
                    p.TenChucNang,
                    p.ThuTu
                }).OrderBy(x => x.ThuTu)
            })
            .OrderBy(x => x.maTrang)
            .ToList();

        return Ok(new
        {
            roles,
            pages = groupedPages,
            mappings
        });
    }

    [HttpGet("role/{roleId:int}")]
    public async Task<IActionResult> GetRolePermissions(int roleId)
    {
        var role = await _context.Vaitros.FirstOrDefaultAsync(x => x.MaVaiTro == roleId);
        if (role == null)
        {
            return NotFound(new { message = "Không tìm thấy nhóm quyền." });
        }

        var permissionCodes = await _context.Vaitroquyens
            .Where(x => x.MaVaiTro == roleId)
            .Select(x => x.MaQuyenNavigation.TenQuyen)
            .ToListAsync();

        return Ok(new
        {
            roleId,
            roleName = role.TenVaiTro,
            permissionCodes
        });
    }

    [HttpGet("check")]
    public async Task<IActionResult> CheckPermission([FromQuery] int roleId, [FromQuery] string permissionCode)
    {
        if (string.IsNullOrWhiteSpace(permissionCode))
        {
            return BadRequest(new { message = "permissionCode là bắt buộc." });
        }

        var hasPermission = await _context.Vaitroquyens.AnyAsync(x =>
            x.MaVaiTro == roleId && x.MaQuyenNavigation.TenQuyen == permissionCode);

        return Ok(new { roleId, permissionCode, hasPermission });
    }

    [HttpPost("roles")]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.TenVaiTro))
        {
            return BadRequest(new { message = "Tên nhóm quyền không được để trống." });
        }

        var normalized = request.TenVaiTro.Trim();
        var existed = await _context.Vaitros.AnyAsync(x => x.TenVaiTro == normalized);
        if (existed)
        {
            return BadRequest(new { message = "Tên nhóm quyền đã tồn tại." });
        }

        var role = new Vaitro { TenVaiTro = normalized };
        _context.Vaitros.Add(role);
        await _context.SaveChangesAsync();

        if (request.PermissionCodes?.Count > 0)
        {
            var permissions = await _context.Quyens
                .Where(x => request.PermissionCodes.Contains(x.TenQuyen))
                .Select(x => x.MaQuyen)
                .ToListAsync();

            foreach (var permissionId in permissions)
            {
                _context.Vaitroquyens.Add(new Vaitroquyen
                {
                    MaVaiTro = role.MaVaiTro,
                    MaQuyen = permissionId,
                    TrangThai = true,
                    DaXoa = false
                });
            }

            await _context.SaveChangesAsync();
        }

        return Ok(new
        {
            role.MaVaiTro,
            role.TenVaiTro
        });
    }

    [HttpPut("role/{roleId:int}")]
    public async Task<IActionResult> UpdateRolePermissions(int roleId, [FromBody] UpdateRolePermissionsRequest request)
    {
        var role = await _context.Vaitros.FirstOrDefaultAsync(x => x.MaVaiTro == roleId);
        if (role == null)
        {
            return NotFound(new { message = "Không tìm thấy nhóm quyền." });
        }

        var permissions = await _context.Quyens
            .Where(x => request.PermissionCodes.Contains(x.TenQuyen))
            .Select(x => x.MaQuyen)
            .ToListAsync();

        var oldMappings = _context.Vaitroquyens.Where(x => x.MaVaiTro == roleId);
        _context.Vaitroquyens.RemoveRange(oldMappings);

        foreach (var permissionId in permissions)
        {
            _context.Vaitroquyens.Add(new Vaitroquyen
            {
                MaVaiTro = roleId,
                MaQuyen = permissionId,
                TrangThai = true,
                DaXoa = false
            });
        }

        await _context.SaveChangesAsync();

        return Ok(new { message = "Cập nhật quyền thành công." });
    }
}

public class CreateRoleRequest
{
    public string TenVaiTro { get; set; } = string.Empty;

    public List<string> PermissionCodes { get; set; } = new();
}

public class UpdateRolePermissionsRequest
{
    public List<string> PermissionCodes { get; set; } = new();
}
