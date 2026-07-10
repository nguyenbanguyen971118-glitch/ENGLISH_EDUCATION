using System;

namespace backend.Attributes;

/// <summary>
/// Attribute metadata để middleware kiểm tra quyền truy cập dựa vào MaChucNang và MaQuyen.
/// Sử dụng: [AuthorizeByPermission(MaChucNang = 1, PermissionIds = new[] { 2 })]
/// Có thể truyền MaChucNang = 0 để chỉ kiểm tra theo PermissionIds.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeByPermissionAttribute : Attribute
{
    /// <summary>
    /// Mã chức năng (trang) cần kiểm tra
    /// </summary>
    public int MaChucNang { get; set; }

    /// <summary>
    /// Danh sách MaQuyen cần kiểm tra.
    /// Nếu user có đủ toàn bộ mã quyền trong danh sách này thì được phép.
    /// </summary>
    public int[] PermissionIds { get; set; } = Array.Empty<int>();

    /// <summary>
    /// Mã quyền dạng chuỗi (ví dụ: USERS_CREATE, PAGE_PARENT_SCHEDULE_VIEW)
    /// </summary>
    public string PermissionCode { get; set; } = string.Empty;

    public AuthorizeByPermissionAttribute()
    {
    }

    public AuthorizeByPermissionAttribute(string permissionCode)
    {
        PermissionCode = permissionCode;
    }
}
