using backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

/// <summary>
/// Controller quản lý ma trận phân quyền, nhóm quyền và các thao tác cập nhật quyền.
/// Toàn bộ nghiệp vụ chính được đẩy xuống tầng service để giữ đúng cấu trúc 3 lớp.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PermissionsController : ControllerBase
{
    private readonly IPermissionService _permissionService;

    public PermissionsController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [HttpGet("matrix")]
    public async Task<IActionResult> GetMatrix()
    {
        // Trả toàn bộ ma trận quyền để FE hiển thị màn hình phân quyền.
        var matrix = await _permissionService.GetMatrixAsync();
        return Ok(matrix);
    }

    [HttpGet("roles/{roleId:int}")]
    public async Task<IActionResult> GetRolePermissions(int roleId)
    {
        // Lấy danh sách quyền hiện tại của một vai trò cụ thể.
        var result = await _permissionService.GetRolePermissionsAsync(roleId);
        if (result == null)
        {
            return NotFound(new { message = "Không tìm thấy nhóm quyền." });
        }

        return Ok(result);
    }

    [HttpGet("check")]
    public async Task<IActionResult> CheckPermission([FromQuery] int roleId, [FromQuery] string permissionCode)
    {
        if (string.IsNullOrWhiteSpace(permissionCode))
        {
            return BadRequest(new { message = "permissionCode là bắt buộc." });
        }

        // Dùng để kiểm tra nhanh một vai trò có quyền cụ thể hay không.
        var hasPermission = await _permissionService.CheckPermissionAsync(roleId, permissionCode);

        return Ok(new { roleId, permissionCode, hasPermission });
    }

    [HttpPost("roles")]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
    {
        // Tạo vai trò mới kèm danh sách quyền ban đầu.
        var result = await _permissionService.CreateRoleAsync(request.TenVaiTro, request.PermissionCodes ?? new List<string>());
        if (!result.Success)
        {
            return BadRequest(new { message = result.Message });
        }

        return Ok(result.Data);
    }

    [HttpPut("roles/{roleId:int}")]
    public async Task<IActionResult> UpdateRolePermissions(int roleId, [FromBody] UpdateRolePermissionsRequest request)
    {
        // Ghi đè toàn bộ danh sách quyền của vai trò.
        var result = await _permissionService.UpdateRolePermissionsAsync(roleId, request.PermissionCodes ?? new List<string>());
        if (!result.Success)
        {
            return NotFound(new { message = result.Message });
        }

        return Ok(new { message = result.Message });
    }

    [HttpDelete("roles/{roleId:int}")]
    public async Task<IActionResult> DeleteRole(int roleId)
    {
        // Xóa mềm vai trò nếu không còn người dùng nào được gán.
        var result = await _permissionService.DeleteRoleAsync(roleId);
        if (!result.Success)
        {
            if (result.Message == "Không tìm thấy nhóm quyền.")
            {
                return NotFound(new { message = result.Message });
            }

            return BadRequest(new { message = result.Message });
        }

        return Ok(new { message = result.Message });
    }
}

public class CreateRoleRequest
{
    // Tên vai trò hiển thị trên hệ thống, ví dụ: Admin, Giao_Vien, Hoc_Sinh.
    public string TenVaiTro { get; set; } = string.Empty;

    // Danh sách mã quyền được gán cho vai trò ngay khi tạo mới.
    public List<string> PermissionCodes { get; set; } = new();
}

public class UpdateRolePermissionsRequest
{
    // Danh sách mã quyền mới của vai trò sau khi cập nhật.
    public List<string> PermissionCodes { get; set; } = new();
}
