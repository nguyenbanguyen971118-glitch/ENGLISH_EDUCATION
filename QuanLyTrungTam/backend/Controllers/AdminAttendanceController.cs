// Controllers/AdminAttendanceController.cs
using backend.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.DTOs;
using backend.Services;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/admin/attendance")]
    [AuthorizeByPermission("ATTENDANCE_ADMIN_VIEW")]
    public class AdminAttendanceController : ControllerBase
    {
        private readonly IAdminAttendanceService _attendanceService;
        private readonly ILogger<AdminAttendanceController> _logger;

        public AdminAttendanceController(
            IAdminAttendanceService attendanceService,
            ILogger<AdminAttendanceController> logger)
        {
            _attendanceService = attendanceService;
            _logger = logger;
        }

        /// <summary>
        /// GET /api/admin/attendance
        /// Query params: classCode, status, fromDate (yyyy-MM-dd), toDate (yyyy-MM-dd)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAttendanceSummaries(
            [FromQuery] string? classCode = null,
            [FromQuery] string? status = null,
            [FromQuery] DateOnly? fromDate = null,
            [FromQuery] DateOnly? toDate = null)
        {
            try
            {
                var data = await _attendanceService.GetAllAttendanceSummariesAsync(
                    classCode, status, fromDate, toDate);

                return Ok(ApiResponseDto<IEnumerable<AttendanceSummaryDto>>.Ok(
                    data: data,
                    message: "Lấy danh sách điểm danh thành công.",
                    code: "SUCCESS",
                    messageKey: "ATTENDANCE.GET_ALL_SUCCESS"
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[AdminAttendance] Lỗi khi lấy danh sách điểm danh.");

                return StatusCode(500, ApiResponseDto<object>.Fail(
                    message: "Đã xảy ra lỗi phía server. Vui lòng thử lại sau.",
                    errorCode: "ATTENDANCE.GET_ALL_FAILED",
                    messageKey: "ATTENDANCE.GET_ALL_FAILED"
                ));
            }
        }
    }
}