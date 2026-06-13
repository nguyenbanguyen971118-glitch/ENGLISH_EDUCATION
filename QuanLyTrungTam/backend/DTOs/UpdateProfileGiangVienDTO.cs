using System;

namespace backend.DTOs
{
    // DTO nhận dữ liệu cập nhật hồ sơ giảng viên từ frontend.
    public class UpdateProfileGiangVienDTO
    {
        public string HoTen { get; set; } = null!;
        public string? SoDienThoai { get; set; }
        public string? QueQuan { get; set; }
        public string TrinhDoChuyenMon { get; set; } = null!;  // Bắt buộc
        public string? HocVi { get; set; }
        public string? KinhNghiemGiangDay { get; set; }
        public string? AnhDaiDien { get; set; }
    }
}
