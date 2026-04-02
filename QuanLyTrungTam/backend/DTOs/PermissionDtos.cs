namespace backend.DTOs;

/// <summary>
/// DTO phẳng dùng để trả dữ liệu quyền theo chức năng cho màn hình ma trận phân quyền.
/// </summary>
public class PermissionFlatDto
{
    // Mã chức năng (page/module) trong hệ thống.
    public int MaChucNang { get; set; }

    // Mã quyền gốc trong bảng Quyen.
    public int MaQuyen { get; set; }

    // Mã quyền dùng ở FE/API, ví dụ: Xem, Tạo, Sửa, Xóa.
    public string PermissionCode { get; set; } = string.Empty;

    // Tên hiển thị của chức năng.
    public string TenChucNang { get; set; } = string.Empty;

    // Mã trang hoặc mã kỹ thuật của màn hình.
    public string MaTrang { get; set; } = string.Empty;

    // Tên trang hiển thị trên giao diện.
    public string TenTrang { get; set; } = string.Empty;

    // Thứ tự hiển thị trong ma trận.
    public int ThuTu { get; set; }
}

/// <summary>
/// DTO rút gọn để hiển thị danh sách vai trò trong màn hình phân quyền.
/// </summary>
public class RoleLiteDto
{
    // Mã vai trò trong database.
    public int MaVaiTro { get; set; }

    // Tên vai trò hiển thị, ví dụ: Admin, Giao_Vien.
    public string TenVaiTro { get; set; } = string.Empty;
}

/// <summary>
/// DTO ánh xạ vai trò với quyền để dựng ma trận phân quyền.
/// </summary>
public class RolePermissionMapDto
{
    // Mã vai trò.
    public int MaVaiTro { get; set; }

    // Mã quyền được gán cho vai trò.
    public int MaQuyen { get; set; }
}