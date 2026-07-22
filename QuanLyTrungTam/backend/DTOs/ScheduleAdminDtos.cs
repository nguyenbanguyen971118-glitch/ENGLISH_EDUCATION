using System;
using System.Collections.Generic;

namespace backend.DTOs;

public sealed class ScheduleLookupItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public sealed class AdminScheduleItemDto
{
    public Guid Id { get; set; }
    public Guid MaBuoiHoc { get; set; }
    public Guid MaLopHoc { get; set; }
    public string ClassCode { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public Guid? MaGiangVien { get; set; }
    public string Teacher { get; set; } = string.Empty;
    public Guid? MaPhongHoc { get; set; }
    public string Room { get; set; } = string.Empty;
    public DateOnly NgayHoc { get; set; }
    public int DayIdx { get; set; }
    public int SlotId { get; set; }
    public int SlotEndId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public bool IsConflict { get; set; }
    public string? ConflictReason { get; set; }
}

public sealed class AdminRescheduleRequestDto
{
    public Guid Id { get; set; }
    public Guid MaYeuCau { get; set; }
    public Guid? MaBuoiHoc { get; set; }
    public Guid MaLopHoc { get; set; }
    public string ClassCode { get; set; } = string.Empty;
    public string Teacher { get; set; } = string.Empty;
    public string OldDate { get; set; } = string.Empty;
    public string NewDate { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public sbyte? Status { get; set; }
    /// <summary>1: Đổi thời gian, 2: Đổi phòng</summary>
    public sbyte? LoaiYeuCau { get; set; }
    public string? TenPhongHocHienTai { get; set; }
    public string? TenPhongHocDeXuat { get; set; }
}

public sealed class AdminScheduleBoardDto
{
    public DateTime WeekStart { get; set; }
    public List<AdminScheduleItemDto> Schedules { get; set; } = new();
    public List<AdminRescheduleRequestDto> RescheduleRequests { get; set; } = new();
    public List<ScheduleLookupItemDto> Classes { get; set; } = new();
    public List<ScheduleLookupItemDto> Teachers { get; set; } = new();
    public List<ScheduleLookupItemDto> Rooms { get; set; } = new();
}

public sealed class StudentScheduleItemDto
{
    public Guid Id { get; set; }
    public string ClassCode { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string Teacher { get; set; } = string.Empty;
    public string Room { get; set; } = string.Empty;
    public DateOnly NgayHoc { get; set; }
    public int DayIdx { get; set; }
    public int SlotId { get; set; }
    public int SlotEndId { get; set; }
    public string Subject { get; set; } = string.Empty;
}

public sealed class StudentScheduleBoardDto
{
    public DateTime WeekStart { get; set; }
    public List<StudentScheduleItemDto> Schedules { get; set; } = new();
}

public sealed class ParentScheduleItemDto
{
    public Guid Id { get; set; }
    public string StudentId { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string ClassCode { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string Teacher { get; set; } = string.Empty;
    public string Room { get; set; } = string.Empty;
    public DateOnly NgayHoc { get; set; }
    public int DayIdx { get; set; }
    public int SlotId { get; set; }
    public int SlotEndId { get; set; }
    public string Subject { get; set; } = string.Empty;
}

public sealed class ParentScheduleBoardDto
{
    public DateTime WeekStart { get; set; }
    public List<ParentScheduleItemDto> Schedules { get; set; } = new();
}

public sealed class StudentClassItemDto
{
    public Guid Id { get; set; }
    public string ClassCode { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string Teacher { get; set; } = string.Empty;
    public int StudentCount { get; set; }
    public int MaxStudents { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
}

public sealed class StudentClassesDto
{
    public List<StudentClassItemDto> Classes { get; set; } = new();
}

public sealed class StudentClassDetailDto
{
    public Guid Id { get; set; }
    public string ClassCode { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string Teacher { get; set; } = string.Empty;
    public Guid? TeacherId { get; set; }
    public int StudentCount { get; set; }
    public int MaxStudents { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<string> StudentNames { get; set; } = new();
    public List<StudentScheduleItemDto> UpcomingSchedules { get; set; } = new();
}
