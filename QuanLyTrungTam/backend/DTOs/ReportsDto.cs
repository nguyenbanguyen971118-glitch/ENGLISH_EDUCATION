using System;
using System.Collections.Generic;

namespace backend.DTOs
{
    /// <summary>
    /// Lớp DTO cơ sở chứa các số liệu thống kê tổng quan (Sĩ số học sinh, số lớp học,
    /// chuyên cần, phân phối điểm số và kích thước lớp). Dùng làm lớp cha cho Admin và Giáo viên.
    /// </summary>
    public class BaseOverviewReportDto
    {
        public int TotalStudents { get; set; }
        public int TotalClasses { get; set; }
        
        // Tỷ lệ điểm danh chuyên cần
        public AttendanceRateDto AttendanceRate { get; set; } = new();
        
        // Phân phối phổ điểm
        public ScoreDistributionDto ScoreDistribution { get; set; } = new();
        
        // Sĩ số từng lớp học
        public List<ClassSizeDto> ClassSizes { get; set; } = new();
    }

    /// <summary>
    /// DTO chứa dữ liệu thống kê tổng quan toàn trung tâm dành cho Admin.
    /// Kế thừa các chỉ số cơ bản từ BaseOverviewReportDto và bổ sung tổng số giáo viên và khóa học.
    /// </summary>
    public class AdminOverviewReportDto : BaseOverviewReportDto
    {
        public int TotalTeachers { get; set; }
        public int TotalCourses { get; set; }
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

    // DTOs cho thống kê chi tiết theo từng lớp phụ trách của Giáo viên
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

    // DTOs cho thống kê cá nhân của Học sinh
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

    /// <summary>
    /// DTO chứa dữ liệu thống kê tổng quan các lớp giảng dạy dành cho Giáo viên.
    /// Kế thừa toàn bộ các thuộc tính tổng hợp từ BaseOverviewReportDto.
    /// </summary>
    public class TeacherOverviewReportDto : BaseOverviewReportDto
    {
        // Kế thừa toàn bộ từ BaseOverviewReportDto
    }
}
