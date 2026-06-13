namespace backend.DTOs;

// DTO nhận dữ liệu cập nhật hồ sơ học sinh từ frontend.
public class UpdateProfileHocSinhDTO
{
    public string HoTen { get; set; } = string.Empty;
    public string? NgaySinh { get; set; }
    public string? QueQuan { get; set; }
    public string? TruongDangTheoHoc { get; set; }
    public string? SoDienThoaiNguoiThan { get; set; }
    public string? AnhDaiDien { get; set; }
}
