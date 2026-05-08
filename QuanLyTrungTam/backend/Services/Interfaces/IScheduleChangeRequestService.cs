using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DTOs;

namespace backend.Services.Interfaces
{
    public interface IScheduleChangeRequestService
    {
        Task<ScheduleChangeRequestResponseDto> CreateScheduleChangeRequestAsync(Guid maGiangVien, CreateScheduleChangeRequestDto dto);
        Task<List<ScheduleChangeRequestDetailDto>> GetPendingRequestsAsync();
        Task<List<ScheduleChangeRequestDetailDto>> GetRequestsByTeacherAsync(Guid maGiangVien);
        Task<ScheduleChangeRequestDetailDto?> GetRequestByIdAsync(Guid maYeuCau);
        Task<ScheduleChangeRequestResponseDto> ApproveRequestAsync(Guid maYeuCau, string? ghiChuAdmin = null);
        Task<ScheduleChangeRequestResponseDto> RejectRequestAsync(Guid maYeuCau, string? ghiChuAdmin = null);
    }
}
