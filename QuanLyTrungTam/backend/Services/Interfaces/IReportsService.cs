using System;
using System.Threading.Tasks;
using backend.DTOs;

namespace backend.Services.Interfaces
{
    /// <summary>
    /// Giao diện định nghĩa các nghiệp vụ báo cáo, thống kê và xuất dữ liệu báo cáo ra file CSV (Excel-compatible).
    /// </summary>
    public interface IReportsService
    {
        /// <summary>
        /// Lấy số liệu thống kê tổng quan toàn trung tâm (Học viên, Giáo viên, Lớp học, Khóa học, Chuyên cần chung, Phổ điểm) cho Admin.
        /// </summary>
        Task<AdminOverviewReportDto> GetAdminOverviewAsync();

        /// <summary>
        /// Tạo báo cáo danh sách lớp học và các chỉ số chuyên cần/học lực dưới dạng chuỗi định dạng CSV cho Admin.
        /// </summary>
        Task<string> ExportAdminReportCsvAsync();
        
        /// <summary>
        /// Lấy số liệu chuyên cần, phổ điểm và bảng tiến độ học tập chi tiết của từng học sinh trong một lớp dành cho Giáo viên.
        /// </summary>
        Task<TeacherClassReportDto> GetTeacherClassOverviewAsync(Guid teacherProfileId, Guid classId);

        /// <summary>
        /// Tạo báo cáo kết quả chi tiết của từng học viên trong lớp dưới dạng chuỗi định dạng CSV cho Giáo viên.
        /// </summary>
        Task<string> ExportTeacherClassReportCsvAsync(Guid teacherProfileId, Guid classId);
        
        /// <summary>
        /// Lấy báo cáo chuyên cần cá nhân, tiến độ hoàn thành bài tập và biểu đồ xu hướng điểm số cho Học sinh.
        /// </summary>
        Task<StudentReportDto> GetStudentOverviewAsync(Guid studentProfileId);

        /// <summary>
        /// Tạo học bạ điện tử cá nhân lưu trữ lịch sử nộp bài tập và chuyên cần dạng CSV cho Học sinh.
        /// </summary>
        Task<string> ExportStudentReportCsvAsync(Guid studentProfileId);
    }
}
