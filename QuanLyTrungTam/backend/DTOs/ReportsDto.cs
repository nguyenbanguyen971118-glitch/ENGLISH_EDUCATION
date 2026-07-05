using System;
using System.Collections.Generic;

namespace backend.DTOs
{
    // DTOs for Admin Overview
    public class AdminOverviewReportDto
    {
        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalClasses { get; set; }
        public int TotalCourses { get; set; }
        
        // Attendance rates
        public AttendanceRateDto AttendanceRate { get; set; } = new();
        
        // Score distribution
        public ScoreDistributionDto ScoreDistribution { get; set; } = new();
        
        // Sĩ số từng lớp học
        public List<ClassSizeDto> ClassSizes { get; set; } = new();
    }

    public class AttendanceRateDto
    {
        public int Present { get; set; }
        public int Absent { get; set; }
        public int Late { get; set; }
        public int Excused { get; set; }
        public int Total { get; set; }
        public double PresentPercent => Total > 0 ? Math.Round((double)(Present + Late) / Total * 100, 2) : 0;
    }

    public class ScoreDistributionDto
    {
        public int Under5 { get; set; } // Yếu / Kém
        public int From5To7 { get; set; } // Trung bình
        public int From7To85 { get; set; } // Khá
        public int Above85 { get; set; } // Giỏi / Xuất sắc
        public int Total { get; set; }
        public double AverageScore { get; set; }
    }

    public class ClassSizeDto
    {
        public string ClassName { get; set; } = string.Empty;
        public int StudentCount { get; set; }
        public int Capacity { get; set; }
    }

    // DTOs for Teacher Class Overview
    public class TeacherClassReportDto
    {
        public Guid ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public int TotalStudents { get; set; }
        
        public AttendanceRateDto AttendanceRate { get; set; } = new();
        public ScoreDistributionDto ScoreDistribution { get; set; } = new();
        public double HomeworkCompletionRate { get; set; } // Tỷ lệ đã nộp bài tập (%)
        
        public List<StudentReportItemDto> Students { get; set; } = new();
    }

    public class StudentReportItemDto
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public double AttendanceRate { get; set; } // % đi học đầy đủ (present + late)
        public double AverageScore { get; set; }
        public int CompletedHomeworkCount { get; set; }
        public int TotalHomeworkCount { get; set; }
    }

    // DTOs for Student Overview
    public class StudentReportDto
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        
        public AttendanceRateDto AttendanceRate { get; set; } = new();
        public double HomeworkCompletionRate { get; set; } // % bài tập đã hoàn thành
        
        // Danh sách điểm số theo chuỗi thời gian bài tập
        public List<StudentScoreTrendDto> ScoreTrend { get; set; } = new();
    }

    public class StudentScoreTrendDto
    {
        public string AssignmentName { get; set; } = string.Empty;
        public DateTime? SubmitTime { get; set; }
        public double? Score { get; set; }
        public double MaxScore { get; set; }
    }
}
