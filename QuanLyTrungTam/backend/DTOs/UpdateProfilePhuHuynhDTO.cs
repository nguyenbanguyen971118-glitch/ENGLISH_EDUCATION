namespace backend.DTOs;

// DTO nhận dữ liệu cập nhật hồ sơ phụ huynh từ frontend.
public class UpdateProfilePhuHuynhDTO
{
    public string HoTen { get; set; } = string.Empty;
    public string? SoDienThoai { get; set; }
    public string? DiaChiLienHe { get; set; }
    public string? NgheNghiep { get; set; }
    public string? AnhDaiDien { get; set; }
}
