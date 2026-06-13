namespace backend.DTOs;

// DTO trả dữ liệu hồ sơ giảng viên cho frontend ở màn hình xem/chỉnh sửa profile.
public class ProfileGiangVienDTO
{
    public string HoTen { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string AnhDaiDien { get; set; } = string.Empty;
    public string SoDienThoai { get; set; } = string.Empty;
    public string QueQuan { get; set; } = string.Empty;
    public string TrinhDoChuyenMon { get; set; } = string.Empty;
    public string HocVi { get; set; } = string.Empty;
    public string KinhNghiemGiangDay { get; set; } = string.Empty;
}
