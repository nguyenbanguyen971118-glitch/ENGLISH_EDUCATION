namespace backend.DTOs;

public sealed class UserListItemDto
{
    public Guid Id { get; set; }
    public Guid? ProfileId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public sbyte AccountType { get; set; }
    public bool Active { get; set; }
    public List<RoleItemDto> Roles { get; set; } = new();
}

public sealed class RoleItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public sealed class CreateUserRequestDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public sbyte AccountType { get; set; }
    public List<int> RoleIds { get; set; } = new();
}

public sealed class UpdateUserRequestDto
{
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public sbyte AccountType { get; set; }
    public bool Active { get; set; } = true;
}

public sealed class UpdateUserRolesRequestDto
{
    public List<int> RoleIds { get; set; } = new();
}

public sealed class CourseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? BasePrice { get; set; }
    public bool Active { get; set; }
}

public sealed class UpsertCourseRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? BasePrice { get; set; }
    public bool Active { get; set; } = true;
}

public sealed class ClassDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int CurrentSize { get; set; }
    public int? Capacity { get; set; }
    public bool Active { get; set; }
    public List<CourseDto> Courses { get; set; } = new();
    public List<TeacherItemDto> Teachers { get; set; } = new();
}

public sealed class TeacherItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public sealed class TeacherOverviewDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public List<TeacherClassOverviewDto> Classes { get; set; } = new();
}

public sealed class TeacherClassOverviewDto
{
    public Guid ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public List<CourseDto> Courses { get; set; } = new();
}

public sealed partial class UpsertClassRequestDto
{
    public string Name { get; set; } = string.Empty;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int? Capacity { get; set; }
    public bool Active { get; set; } = true;
    public List<Guid> CourseIds { get; set; } = new();
    public List<Guid> TeacherIds { get; set; } = new();
}

public sealed class ScheduleConfigDto
{
    public List<string> Days { get; set; } = new();
    public List<int> Periods { get; set; } = new();
    public Guid? RoomId { get; set; }
}

public sealed partial class UpsertClassRequestDto
{
    // Total number of periods (BR01/BR02). Required if not defined on course.
    public int? TotalPeriods { get; set; }

    // Schedule configuration: multiple rows each containing days and periods.
    public List<ScheduleConfigDto> ScheduleConfigs { get; set; } = new();
}

public sealed class ClassStudentDto
{
    public Guid StudentId { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public DateOnly? JoinedAt { get; set; }
    public bool Active { get; set; }
}

public sealed class AddClassStudentRequestDto
{
    public Guid StudentId { get; set; }
}

public sealed class AttendanceItemRequestDto
{
    public Guid StudentId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Note { get; set; }
}

public sealed class SaveAttendanceRequestDto
{
    public List<AttendanceItemRequestDto> Items { get; set; } = new();
}

public sealed class AttendanceItemDto
{
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusLabel { get; set; } = string.Empty;
    public string? Note { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public sealed class ParentChildAttendanceDto
{
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public Guid ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public Guid SessionId { get; set; }
    public DateOnly SessionDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string StatusLabel { get; set; } = string.Empty;
    public string? Note { get; set; }
}

public sealed class AddClassTeacherRequestDto
{
    public Guid TeacherId { get; set; }
}

public sealed class ClassTeacherDto
{
    public Guid TeacherId { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public DateOnly? JoinedAt { get; set; }
    public bool Active { get; set; }
}

public sealed class AddParentChildRequestDto
{
    public Guid StudentId { get; set; }
}

public sealed class ParentChildDto
{
    public Guid StudentId { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public bool Active { get; set; }
}

public sealed class ClassSessionDto
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public Guid? RoomId { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public int StartPeriod { get; set; }
    public int EndPeriod { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public bool Active { get; set; }
}

public sealed class UpsertClassSessionRequestDto
{
    public DateTime Date { get; set; }
    public Guid? RoomId { get; set; }
    public int StartPeriod { get; set; }
    public int EndPeriod { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
}
