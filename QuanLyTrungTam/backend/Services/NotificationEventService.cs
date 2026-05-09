using backend.DTOs;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using backend.Hubs;

namespace backend.Services;

/// <summary>
/// Service để xử lý các sự kiện tự động sinh ra thông báo
/// Ví dụ: tạo bài tập mới, thay đổi lịch học, tạo bài kiểm tra, v.v.
/// </summary>
public class NotificationEventService
{
    private readonly INotificationService _notificationService;
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationEventService(
        INotificationService notificationService,
        IHubContext<NotificationHub> hubContext)
    {
        _notificationService = notificationService;
        _hubContext = hubContext;
    }

    /// <summary>
    /// Gửi thông báo cho tất cả sinh viên khi có bài tập mới
    /// </summary>
    public async Task NotifyNewAssignmentAsync(Guid creatorId, string assignmentTitle, string description)
    {
        var dto = new CreateNotificationDto
        {
            Title = $"Bài tập mới: {assignmentTitle}",
            Content = description,
            DoiTuong = "Hoc_Sinh"
        };

        var result = await _notificationService.CreateAsync(creatorId, dto);
        
        if (result.Success)
        {
            // Gửi thông báo real-time cho tất cả sinh viên
            await _hubContext.Clients.Group(NotificationHub.GetStudentGroup())
                .SendAsync("NewNotification", result.Data);
        }
    }

    /// <summary>
    /// Gửi thông báo cho tất cả giáo viên khi có sự kiện quan trọng
    /// </summary>
    public async Task NotifyTeachersAsync(Guid creatorId, string title, string content)
    {
        var dto = new CreateNotificationDto
        {
            Title = title,
            Content = content,
            DoiTuong = "Giao_Vien"
        };

        var result = await _notificationService.CreateAsync(creatorId, dto);
        
        if (result.Success)
        {
            await _hubContext.Clients.Group(NotificationHub.GetTeacherGroup())
                .SendAsync("NewNotification", result.Data);
        }
    }

    /// <summary>
    /// Gửi thông báo cho tất cả phụ huynh
    /// </summary>
    public async Task NotifyParentsAsync(Guid creatorId, string title, string content)
    {
        var dto = new CreateNotificationDto
        {
            Title = title,
            Content = content,
            DoiTuong = "Phu_Huynh"
        };

        var result = await _notificationService.CreateAsync(creatorId, dto);
        
        if (result.Success)
        {
            await _hubContext.Clients.Group(NotificationHub.GetParentGroup())
                .SendAsync("NewNotification", result.Data);
        }
    }

    /// <summary>
    /// Gửi thông báo cho tất cả người dùng (Admin sẽ dùng)
    /// </summary>
    public async Task NotifyAllAsync(Guid creatorId, string title, string content)
    {
        var dto = new CreateNotificationDto
        {
            Title = title,
            Content = content,
            DoiTuong = "Tat_Ca"
        };

        var result = await _notificationService.CreateAsync(creatorId, dto);
        
        if (result.Success)
        {
            // Gửi đến tất cả người dùng
            await _hubContext.Clients.All
                .SendAsync("NewNotification", result.Data);
        }
    }

    /// <summary>
    /// Gửi thông báo cho một người dùng cụ thể
    /// </summary>
    public async Task NotifyUserAsync(Guid creatorId, Guid targetUserId, string title, string content)
    {
        var dto = new CreateNotificationDto
        {
            Title = title,
            Content = content,
            DoiTuong = "Tat_Ca"
        };

        var result = await _notificationService.CreateAsync(creatorId, dto);
        
        if (result.Success)
        {
            // Gửi đến user cụ thể
            await _hubContext.Clients.Group(NotificationHub.GetUserGroup(targetUserId))
                .SendAsync("NewNotification", result.Data);
        }
    }

    /// <summary>
    /// Gửi thông báo khi có thay đổi lịch học
    /// </summary>
    public async Task NotifyScheduleChangeAsync(Guid creatorId, string className, string details)
    {
        var dto = new CreateNotificationDto
        {
            Title = $"Thay đổi lịch học - {className}",
            Content = details,
            DoiTuong = "Tat_Ca"
        };

        var result = await _notificationService.CreateAsync(creatorId, dto);
        
        if (result.Success)
        {
            await _hubContext.Clients.All
                .SendAsync("ScheduleChanged", result.Data);
        }
    }

    /// <summary>
    /// Gửi thông báo khi một sinh viên nộp bài
    /// </summary>
    public async Task NotifyAssignmentSubmittedAsync(Guid creatorId, string studentName, string assignmentTitle)
    {
        var dto = new CreateNotificationDto
        {
            Title = $"Nộp bài mới: {assignmentTitle}",
            Content = $"{studentName} đã nộp bài cho {assignmentTitle}",
            DoiTuong = "Giao_Vien"
        };

        var result = await _notificationService.CreateAsync(creatorId, dto);
        
        if (result.Success)
        {
            await _hubContext.Clients.Group(NotificationHub.GetTeacherGroup())
                .SendAsync("NewNotification", result.Data);
        }
    }

    /// <summary>
    /// Gửi thông báo về điểm số cho sinh viên
    /// </summary>
    public async Task NotifyGradeAsync(Guid creatorId, Guid studentId, string assignmentTitle, string grade)
    {
        var dto = new CreateNotificationDto
        {
            Title = $"Điểm số: {assignmentTitle}",
            Content = $"Bạn đã nhận được điểm số cho {assignmentTitle}: {grade}",
            DoiTuong = "Hoc_Sinh"
        };

        var result = await _notificationService.CreateAsync(creatorId, dto);
        
        if (result.Success)
        {
            // Gửi đến sinh viên cụ thể
            await _hubContext.Clients.Group(NotificationHub.GetUserGroup(studentId))
                .SendAsync("NewNotification", result.Data);
        }
    }
}
