using System;
using System.Collections.Generic;

namespace backend.DTOs;

public class TeacherSpecializationDto
{
    public Guid MaGiangVien { get; set; }
    public string TenGiangVien { get; set; } = string.Empty;
    public List<Guid> CourseIds { get; set; } = new();
    public List<string> CourseNames { get; set; } = new();
}

public class AssignCoursesRequestDto
{
    public Guid MaGiangVien { get; set; }
    public List<Guid> CourseIds { get; set; } = new();
}

public class ClassDashboardDto
{
    public Guid ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public int TotalStudents { get; set; }
    public int TotalTests { get; set; }
    public List<TestScoreDistributionDto> Tests { get; set; } = new();
    public List<StudentAttendanceSummaryDto> Attendance { get; set; } = new();
    public List<ScoreIntervalBinDto> AverageScoreDistribution { get; set; } = new();
}

public class TestScoreDistributionDto
{
    public Guid TestId { get; set; }
    public string TestName { get; set; } = string.Empty;
    public List<ScoreCountDto> ScoreDistribution { get; set; } = new();
}

public class ScoreCountDto
{
    public int Score { get; set; } // Rounded integer score from 0 to 10
    public int Count { get; set; }
}

public class StudentAttendanceSummaryDto
{
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int TotalSessions { get; set; }
    public int PresentSessions { get; set; }
    public int AbsentSessions { get; set; }
    public int LateSessions { get; set; }
    public int ExcusedSessions { get; set; }
    public double AttendanceRate { get; set; } // (Present + Late) / Total * 100
    public double AbsentRate { get; set; } // Absent / Total * 100
}

public class ScoreIntervalBinDto
{
    public string Interval { get; set; } = string.Empty; // "0-1", "1-2", ..., "9-10"
    public int Count { get; set; }
}

public class StudentCourseDashboardDto
{
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public Guid ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public double AttendanceRate { get; set; }
    public List<StudentTestResultDto> Tests { get; set; } = new();
}

public class StudentTestResultDto
{
    public Guid TestId { get; set; }
    public string TestName { get; set; } = string.Empty;
    public double? Score { get; set; }
    public double ClassAverage { get; set; }
    public string Rank { get; set; } = string.Empty; // e.g. "3/15"
}

public class ParentChildDashboardDto
{
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public List<ChildCourseSummaryDto> Courses { get; set; } = new();
}

public class ChildCourseSummaryDto
{
    public Guid ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public double? FinalAverageScore { get; set; } // Final average grade for this course
}
