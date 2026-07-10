using System;
using System.Text;
using System.Threading.Tasks;
using backend.Attributes;
using backend.DTOs;
using backend.Helpers;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    /// <summary>
    /// API Controller cung cấp các dịch vụ xuất báo cáo thống kê chuyên cần, điểm số
    /// và hỗ trợ xuất dữ liệu ra file Excel (.csv) cho các phân hệ Admin, Giáo viên và Học sinh.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsService _reportsService;

        public ReportsController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        /// <summary>
        /// Lấy thống kê tổng quan toàn trung tâm (sĩ số, giảng viên, khóa học, chuyên cần và phân bổ điểm số) dành cho Admin.
        /// Yêu cầu quyền: PAGE_ADMIN_REPORTS_VIEW
        /// </summary>
        [HttpGet("admin/overview")]
        [AuthorizeByPermission("PAGE_ADMIN_REPORTS_VIEW")]
        public async Task<IActionResult> GetAdminOverview()
        {
            try
            {
                var report = await _reportsService.GetAdminOverviewAsync();
                return Ok(ApiResponseDto<AdminOverviewReportDto>.Ok(report, "Lấy thống kê tổng quan admin thành công."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDto<object>.Fail($"Lỗi: {ex.Message}", "REPORT_ADMIN_OVERVIEW_ERROR"));
            }
        }

        /// <summary>
        /// Xuất file Excel (.csv UTF-8 BOM) thống kê tổng quan danh sách lớp học cho Admin.
        /// Yêu cầu quyền: PAGE_ADMIN_REPORTS_VIEW
        /// </summary>
        [HttpGet("admin/export")]
        [AuthorizeByPermission("PAGE_ADMIN_REPORTS_VIEW")]
        public async Task<IActionResult> ExportAdminReport()
        {
            try
            {
                var csv = await _reportsService.ExportAdminReportCsvAsync();
                var fileBytes = Encoding.UTF8.GetBytes(csv);
                return File(fileBytes, "text/csv; charset=utf-8", "BaoCaoTongQuanAdmin.csv");
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<object>.Fail($"Lỗi xuất Excel: {ex.Message}", "REPORT_ADMIN_EXPORT_ERROR"));
            }
        }

        /// <summary>
        /// Lấy thống kê tổng quan của tất cả các lớp giảng dạy dành cho Giáo viên.
        /// Yêu cầu quyền: PAGE_TEACHER_REPORTS_VIEW
        /// </summary>
        [HttpGet("teacher/overview")]
        [AuthorizeByPermission("PAGE_TEACHER_REPORTS_VIEW")]
        public async Task<IActionResult> GetTeacherOverview()
        {
            try
            {
                var teacherProfileId = User.GetProfileId();
                if (!teacherProfileId.HasValue)
                {
                    return BadRequest(ApiResponseDto<object>.Fail("Không tìm thấy mã giáo viên trong phiên đăng nhập.", "TEACHER_PROFILE_NOT_FOUND"));
                }

                var report = await _reportsService.GetTeacherOverviewAsync(teacherProfileId.Value);
                return Ok(ApiResponseDto<TeacherOverviewReportDto>.Ok(report, "Lấy thống kê tổng quan giảng dạy thành công."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDto<object>.Fail($"Lỗi: {ex.Message}", "REPORT_TEACHER_OVERVIEW_ERROR"));
            }
        }

        /// <summary>
        /// Xuất file Excel (.csv UTF-8 BOM) thống kê tổng quan các lớp phụ trách cho Giáo viên.
        /// Yêu cầu quyền: PAGE_TEACHER_REPORTS_VIEW
        /// </summary>
        [HttpGet("teacher/export")]
        [AuthorizeByPermission("PAGE_TEACHER_REPORTS_VIEW")]
        public async Task<IActionResult> ExportTeacherOverview()
        {
            try
            {
                var teacherProfileId = User.GetProfileId();
                if (!teacherProfileId.HasValue)
                {
                    return BadRequest(ApiResponseDto<object>.Fail("Không tìm thấy mã giáo viên trong phiên đăng nhập.", "TEACHER_PROFILE_NOT_FOUND"));
                }

                var csv = await _reportsService.ExportTeacherOverviewCsvAsync(teacherProfileId.Value);
                var fileBytes = Encoding.UTF8.GetBytes(csv);
                return File(fileBytes, "text/csv; charset=utf-8", "BaoCaoTongQuanGiangDay.csv");
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<object>.Fail($"Lỗi xuất Excel: {ex.Message}", "REPORT_TEACHER_EXPORT_ERROR"));
            }
        }

        /// <summary>
        /// Lấy thống kê chuyên cần, phổ điểm bài tập và tiến độ nộp bài chi tiết của từng học sinh trong một lớp học.
        /// Yêu cầu quyền: PAGE_TEACHER_REPORTS_VIEW
        /// </summary>
        /// <param name="classId">Mã định danh duy nhất của lớp học cần xem báo cáo</param>
        [HttpGet("teacher/classes/{classId:guid}/overview")]
        [AuthorizeByPermission("PAGE_TEACHER_REPORTS_VIEW")]
        public async Task<IActionResult> GetTeacherClassOverview(Guid classId)
        {
            try
            {
                var teacherProfileId = User.GetProfileId();
                if (!teacherProfileId.HasValue)
                {
                    return BadRequest(ApiResponseDto<object>.Fail("Không tìm thấy mã giáo viên trong phiên đăng nhập.", "TEACHER_PROFILE_NOT_FOUND"));
                }

                var report = await _reportsService.GetTeacherClassOverviewAsync(teacherProfileId.Value, classId);
                return Ok(ApiResponseDto<TeacherClassReportDto>.Ok(report, "Lấy thống kê lớp học thành công."));
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDto<object>.Fail($"Lỗi: {ex.Message}", "REPORT_TEACHER_OVERVIEW_ERROR"));
            }
        }

        /// <summary>
        /// Xuất báo cáo điểm thi và chuyên cần chi tiết của học sinh trong lớp phụ trách dưới dạng file Excel (.csv).
        /// Yêu cầu quyền: PAGE_TEACHER_REPORTS_VIEW
        /// </summary>
        /// <param name="classId">Mã định danh duy nhất của lớp học cần xuất báo cáo</param>
        [HttpGet("teacher/classes/{classId:guid}/export")]
        [AuthorizeByPermission("PAGE_TEACHER_REPORTS_VIEW")]
        public async Task<IActionResult> ExportTeacherClassReport(Guid classId)
        {
            try
            {
                var teacherProfileId = User.GetProfileId();
                if (!teacherProfileId.HasValue)
                {
                    return BadRequest(ApiResponseDto<object>.Fail("Không tìm thấy mã giáo viên trong phiên đăng nhập.", "TEACHER_PROFILE_NOT_FOUND"));
                }

                var csv = await _reportsService.ExportTeacherClassReportCsvAsync(teacherProfileId.Value, classId);
                var fileBytes = Encoding.UTF8.GetBytes(csv);
                return File(fileBytes, "text/csv; charset=utf-8", $"BaoCaoLopHoc_{classId}.csv");
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<object>.Fail($"Lỗi xuất Excel: {ex.Message}", "REPORT_TEACHER_EXPORT_ERROR"));
            }
        }

        /// <summary>
        /// Lấy thống kê kết quả học tập cá nhân (điểm trung bình, chuyên cần, xu hướng điểm bài tập) của học sinh đang đăng nhập.
        /// Yêu cầu quyền: PAGE_STUDENT_SCHEDULE_VIEW
        /// </summary>
        [HttpGet("student/overview")]
        [AuthorizeByPermission("PAGE_STUDENT_SCHEDULE_VIEW")]
        public async Task<IActionResult> GetStudentOverview()
        {
            try
            {
                var studentProfileId = User.GetProfileId();
                if (!studentProfileId.HasValue)
                {
                    return BadRequest(ApiResponseDto<object>.Fail("Không tìm thấy mã học sinh trong phiên đăng nhập.", "STUDENT_PROFILE_NOT_FOUND"));
                }

                var report = await _reportsService.GetStudentOverviewAsync(studentProfileId.Value);
                return Ok(ApiResponseDto<StudentReportDto>.Ok(report, "Lấy thống kê học tập thành công."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDto<object>.Fail($"Lỗi: {ex.Message}", "REPORT_STUDENT_OVERVIEW_ERROR"));
            }
        }

        /// <summary>
        /// Xuất học bạ điện tử cá nhân (lịch sử nộp bài, điểm số) của học sinh đang đăng nhập dưới dạng file Excel (.csv).
        /// Yêu cầu quyền: PAGE_STUDENT_SCHEDULE_VIEW
        /// </summary>
        [HttpGet("student/export")]
        [AuthorizeByPermission("PAGE_STUDENT_SCHEDULE_VIEW")]
        public async Task<IActionResult> ExportStudentReport()
        {
            try
            {
                var studentProfileId = User.GetProfileId();
                if (!studentProfileId.HasValue)
                {
                    return BadRequest(ApiResponseDto<object>.Fail("Không tìm thấy mã học sinh trong phiên đăng nhập.", "STUDENT_PROFILE_NOT_FOUND"));
                }

                var csv = await _reportsService.ExportStudentReportCsvAsync(studentProfileId.Value);
                var fileBytes = Encoding.UTF8.GetBytes(csv);
                return File(fileBytes, "text/csv; charset=utf-8", "BaoCaoHocTapCaNhan.csv");
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto<object>.Fail($"Lỗi xuất Excel: {ex.Message}", "REPORT_STUDENT_EXPORT_ERROR"));
            }
        }
    }
}
