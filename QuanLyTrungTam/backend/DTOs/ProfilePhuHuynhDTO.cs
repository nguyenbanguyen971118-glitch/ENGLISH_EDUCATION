namespace backend.DTOs;

// DTO trả dữ liệu hồ sơ phụ huynh cho frontend ở màn hình xem/chỉnh sửa profile.
public class ProfilePhuHuynhDTO
{
    public string HoTen { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string AnhDaiDien { get; set; } = string.Empty;
    public string SoDienThoai { get; set; } = string.Empty;
    public string DiaChiLienHe { get; set; } = string.Empty;
    public string NgheNghiep { get; set; } = string.Empty;
}
