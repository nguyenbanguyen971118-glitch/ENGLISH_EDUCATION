namespace backend.DTOs;

// DTO trả dữ liệu hồ sơ học sinh cho frontend ở màn hình xem/chỉnh sửa profile.
public class ProfileHocSinhDTO
{
    public string HoTen { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string AnhDaiDien { get; set; } = string.Empty;
    public string NgaySinh { get; set; } = string.Empty;
    public string QueQuan { get; set; } = string.Empty;
    public string TruongDangTheoHoc { get; set; } = string.Empty;
    public string SoDienThoaiNguoiThan { get; set; } = string.Empty;
}
