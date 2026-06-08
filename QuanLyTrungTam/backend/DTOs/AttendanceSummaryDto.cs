namespace backend.DTOs
{
    public class AttendanceSummaryDto
    {
        public string Id { get; set; }           // MaBuoiHoc
        public string ClassCode { get; set; } // TenLop
        public string CourseName { get; set; }// TenKhoaHoc
        public string Teacher { get; set; }   // HoTen GiangVien
        public string Date { get; set; }      // NgayHoc (dd/MM/yyyy)
        public string Time { get; set; }      // GioBatDau - GioKetThuc
        public int Present { get; set; }      // Số HS đã điểm danh
        public int Total { get; set; }        // Tổng HS trong lớp
        public string Status { get; set; }    // "Đã điểm danh" / "Chưa điểm danh"
    }
}
