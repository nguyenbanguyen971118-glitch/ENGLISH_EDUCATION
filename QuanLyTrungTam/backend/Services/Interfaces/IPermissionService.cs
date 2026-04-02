namespace backend.Services.Interfaces;

public interface IPermissionService
{
    Task<object> GetMatrixAsync();
    Task<object?> GetRolePermissionsAsync(int roleId);
    Task<bool> CheckPermissionAsync(int roleId, string permissionCode);
    Task<(bool Success, string Message, object? Data)> CreateRoleAsync(string tenVaiTro, List<string> permissionCodes);
    Task<(bool Success, string Message)> UpdateRolePermissionsAsync(int roleId, List<string> permissionCodes);
    Task<(bool Success, string Message)> DeleteRoleAsync(int roleId);
}
