using backend.DTOs;

namespace backend.Services
{
    public interface IAdminAttendanceService
    {
        Task<IEnumerable<AttendanceSummaryDto>> GetAllAttendanceSummariesAsync(
            string? classCode = null,
            string? status = null,
            DateOnly? fromDate = null,
            DateOnly? toDate = null
        );
    }
}