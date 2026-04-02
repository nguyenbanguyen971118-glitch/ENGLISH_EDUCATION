using backend.Models;
using backend.DTOs;

namespace backend.Repositories.Interfaces;

public interface IPermissionRepository
{
    Task<List<PermissionFlatDto>> GetRawPermissionsAsync();
    Task<List<RoleLiteDto>> GetActiveRolesAsync();
    Task<List<RolePermissionMapDto>> GetActiveMappingsAsync();
    Task<Vaitro?> GetRoleByIdAsync(int roleId);
    Task<bool> RoleNameExistsAsync(string roleName);
    Task<List<int>> GetPermissionIdsByCodesAsync(List<string> permissionCodes);
    Task<List<string>> GetPermissionCodesByRoleIdAsync(int roleId);
    Task<bool> HasPermissionAsync(int roleId, string permissionCode);
    Task<int> CreateRoleAsync(string roleName);
    Task ReplaceRolePermissionsAsync(int roleId, List<int> permissionIds);
    Task<bool> HasAssignedUsersAsync(int roleId);
    Task SoftDeleteRoleAsync(int roleId);
}
