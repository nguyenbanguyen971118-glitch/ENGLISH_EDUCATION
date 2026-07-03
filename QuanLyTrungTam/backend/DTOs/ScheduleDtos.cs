using System;
using System.Collections.Generic;

namespace backend.DTOs
{
    public class CreateBuoiHocDto
    {
        /// <summary>
        /// Mã lớp học cần sắp xếp buổi học.
        /// </summary>
        public Guid MaLopHoc { get; set; }

        /// <summary>
        /// Mã phòng học dự kiến sử dụng. Có thể để trống nếu chưa chốt phòng.
        /// </summary>
        public Guid? MaPhongHoc { get; set; }

        /// <summary>
        /// Ngày học của buổi học. Backend chỉ lấy phần ngày.
        /// </summary>
        public DateTime NgayHoc { get; set; }

        /// <summary>
        /// Tiết bắt đầu của buổi học.
        /// </summary>
        public int MaTietBatDau { get; set; }

        /// <summary>
        /// Tiết kết thúc của buổi học.
        /// </summary>
        public int MaTietKetThuc { get; set; }

        /// <summary>
        /// Tiêu đề hiển thị của buổi học.
        /// </summary>
        public string? TieuDe { get; set; }

        /// <summary>
        /// Nội dung hoặc ghi chú của buổi học.
        /// </summary>
        public string? NoiDung { get; set; }
    }

    public class UpdateBuoiHocDto : CreateBuoiHocDto
    {
    }

    public class AvailableRoomDto
    {
        /// <summary>
        /// Mã phòng học.
        /// </summary>
        public Guid MaPhongHoc { get; set; }

        /// <summary>
        /// Tên phòng học.
        /// </summary>
        public string TenPhong { get; set; } = null!;
    }

    public class CreateRoomDto
    {
        /// <summary>
        /// Tên phòng học mới.
        /// </summary>
        public string TenPhong { get; set; } = null!;
    }

    public class UpdateRoomDto
    {
        /// <summary>
        /// Tên phòng học (tùy chọn để cập nhật).
        /// </summary>
        public string? TenPhong { get; set; }
        /// <summary>
        /// Trạng thái hoạt động (tùy chọn).
        /// </summary>
        public bool? TrangThai { get; set; }
    }

    public class AvailableTeacherDto
    {
        /// <summary>
        /// Mã giảng viên.
        /// </summary>
        public Guid MaGiangVien { get; set; }

        /// <summary>
        /// Mã người dùng của giảng viên.
        /// </summary>
        public Guid MaNguoiDung { get; set; }
    }

    // ============= Schedule Change Request DTOs =============

    /// <summary>
    /// DTO để giáo viên tạo yêu cầu đổi lịch dạy
    /// </summary>
    public class CreateScheduleChangeRequestDto
    {
        /// <summary>
        /// Mã buổi học cần đổi
        /// </summary>
        public Guid MaBuoiHoc { get; set; }

        /// <summary>
        /// 1: Đổi thời gian, 2: Đổi phòng
        /// </summary>
        public sbyte LoaiYeuCau { get; set; }

        /// <summary>
        /// Ngày học mới (nếu đổi thời gian)
        /// </summary>
        public DateOnly? NgayHocDeXuat { get; set; }

        /// <summary>
        /// Tiết bắt đầu mới (nếu đổi thời gian)
        /// </summary>
        public int? MaTietBatDauDeXuat { get; set; }

        /// <summary>
        /// Tiết kết thúc mới (nếu đổi thời gian)
        /// </summary>
        public int? MaTietKetThucDeXuat { get; set; }

        /// <summary>
        /// Mã phòng học mới (nếu đổi phòng)
        /// </summary>
        public Guid? MaPhongHocDeXuat { get; set; }

        /// <summary>
        /// Lý do đề xuất đổi
        /// </summary>
        public string? LyDo { get; set; }
    }

    /// <summary>
    /// DTO chi tiết yêu cầu đổi lịch (hiển thị trong danh sách admin duyệt)
    /// </summary>
    public class ScheduleChangeRequestDetailDto
    {
        public Guid MaYeuCau { get; set; }

        public Guid MaGiangVien { get; set; }

        public string? TenGiangVien { get; set; }

        public Guid MaLopHoc { get; set; }

        public string? TenLop { get; set; }

        public Guid MaBuoiHoc { get; set; }

        public sbyte LoaiYeuCau { get; set; }

        /// <summary>
        /// Thông tin buổi học hiện tại
        /// </summary>
        public DateTime NgayHocHienTai { get; set; }

        public int MaTietBatDauHienTai { get; set; }

        public int MaTietKetThucHienTai { get; set; }

        public Guid? MaPhongHocHienTai { get; set; }

        public string? TenPhongHocHienTai { get; set; }

        /// <summary>
        /// Thông tin đề xuất mới
        /// </summary>
        public DateTime? NgayHocDeXuat { get; set; }

        public int? MaTietBatDauDeXuat { get; set; }

        public int? MaTietKetThucDeXuat { get; set; }

        public Guid? MaPhongHocDeXuat { get; set; }

        public string? TenPhongHocDeXuat { get; set; }

        public string? LyDo { get; set; }

        /// <summary>
        /// 0: Chờ xử lý, 1: Đã duyệt, 2: Từ chối
        /// </summary>
        public sbyte? TrangThaiDuyet { get; set; }

        public string? GhiChuAdmin { get; set; }

        public DateTime? ThoiGianTao { get; set; }

        public DateTime? ThoiGianSua { get; set; }
    }

    /// <summary>
    /// DTO Response sau khi tạo yêu cầu đổi lịch thành công
    /// </summary>
    public class ScheduleChangeRequestResponseDto
    {
        public Guid MaYeuCau { get; set; }

        public string Message { get; set; } = null!;

        public bool Success { get; set; }
    }

    /// <summary>
    /// DTO duyệt/từ chối yêu cầu đổi lịch (cho Admin)
    /// </summary>
    public class ApproveScheduleChangeRequestDto
    {
        public Guid MaYeuCau { get; set; }

        /// <summary>
        /// 1: Duyệt, 2: Từ chối
        /// </summary>
        public sbyte TrangThaiDuyet { get; set; }

        /// <summary>
        /// Ghi chú từ Admin
        /// </summary>
        public string? GhiChuAdmin { get; set; }
    }
}
