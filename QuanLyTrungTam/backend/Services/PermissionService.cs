using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services;

public class PermissionService : IPermissionService
{
    private static readonly HashSet<string> SystemRoles = new(new[]
    {
        "Admin", "Giao_Vien", "Hoc_Sinh", "Phu_Huynh"
    }, StringComparer.Ordinal);

    private readonly IPermissionRepository _permissionRepository;

    public PermissionService(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<object> GetMatrixAsync()
    {
        var permissions = await _permissionRepository.GetRawPermissionsAsync();
        var roles = await _permissionRepository.GetActiveRolesAsync();
        var mappings = await _permissionRepository.GetActiveMappingsAsync();

        var groupedPages = permissions
            .GroupBy(x => new { x.MaChucNang, x.TenChucNang, x.MaTrang, x.TenTrang })
            .Select(g => new
            {
                maTrang = g.Key.MaTrang,
                maChucNang = g.Key.MaChucNang,
                tenChucNang = g.Key.TenChucNang,
                tenTrang = g.Key.TenTrang,
                quyens = g.Select(p => new
                {
                    p.MaQuyen,
                    p.PermissionCode,
                    p.ThuTu
                }).OrderBy(x => x.ThuTu).ToList()
            })
            .OrderBy(x => x.maTrang)
            .ToList();

        return new
        {
            roles,
            pages = groupedPages,
            mappings
        };
    }

    public async Task<object?> GetRolePermissionsAsync(int roleId)
    {
        var role = await _permissionRepository.GetRoleByIdAsync(roleId);
        if (role == null)
        {
            return null;
        }

        var permissionCodes = await _permissionRepository.GetPermissionCodesByRoleIdAsync(roleId);

        return new
        {
            roleId,
            roleName = role.TenVaiTro,
            permissionCodes
        };
    }

    public async Task<bool> CheckPermissionAsync(int roleId, string permissionCode)
    {
        return await _permissionRepository.HasPermissionAsync(roleId, permissionCode);
    }

    public async Task<(bool Success, string Message, object? Data)> CreateRoleAsync(string tenVaiTro, List<string> permissionCodes)
    {
        if (string.IsNullOrWhiteSpace(tenVaiTro))
        {
            return (false, "Tên nhóm quyền không được để trống.", null);
        }

        var normalized = tenVaiTro.Trim();
        var existed = await _permissionRepository.RoleNameExistsAsync(normalized);
        if (existed)
        {
            return (false, "Tên nhóm quyền đã tồn tại.", null);
        }

        var roleId = await _permissionRepository.CreateRoleAsync(normalized);
        var permissionIds = await _permissionRepository.GetPermissionIdsByCodesAsync(permissionCodes ?? new List<string>());
        await _permissionRepository.ReplaceRolePermissionsAsync(roleId, permissionIds);

        return (true, "Tạo vai trò thành công.", new { MaVaiTro = roleId, TenVaiTro = normalized });
    }

    public async Task<(bool Success, string Message)> UpdateRolePermissionsAsync(int roleId, List<string> permissionCodes)
    {
        var role = await _permissionRepository.GetRoleByIdAsync(roleId);
        if (role == null)
        {
            return (false, "Không tìm thấy nhóm quyền.");
        }

        var permissionIds = await _permissionRepository.GetPermissionIdsByCodesAsync(permissionCodes ?? new List<string>());
        await _permissionRepository.ReplaceRolePermissionsAsync(roleId, permissionIds);

        return (true, "Cập nhật quyền thành công.");
    }

    public async Task<(bool Success, string Message)> DeleteRoleAsync(int roleId)
    {
        var role = await _permissionRepository.GetRoleByIdAsync(roleId);
        if (role == null || (role.DaXoa != null && role.DaXoa == true))
        {
            return (false, "Không tìm thấy nhóm quyền.");
        }

        if (SystemRoles.Contains(role.TenVaiTro ?? string.Empty))
        {
            return (false, "Không thể xoá vai trò hệ thống.");
        }

        var hasAssignedUsers = await _permissionRepository.HasAssignedUsersAsync(roleId);
        if (hasAssignedUsers)
        {
            return (false, "Vai trò đang được gán cho người dùng, không thể xoá.");
        }

        await _permissionRepository.SoftDeleteRoleAsync(roleId);
        return (true, "Xoá vai trò thành công.");
    }
}
