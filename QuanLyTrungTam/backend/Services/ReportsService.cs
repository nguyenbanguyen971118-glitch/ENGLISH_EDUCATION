using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    /// <summary>
    /// Dịch vụ tổng hợp dữ liệu báo cáo thống kê chuyên cần, điểm số 
    /// và hỗ trợ xuất dữ liệu ra file CSV cho các đối tượng người dùng: Admin, Giáo viên, Học sinh.
    /// </summary>
    public class ReportsService : IReportsService
    {
        private readonly AppDbContext _context;

        public ReportsService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lấy dữ liệu thống kê tổng quan toàn trung tâm dành cho tài khoản Admin.
        /// Bao gồm: Tổng số lượng học sinh/giáo viên/lớp học/khóa học, tỷ lệ điểm danh chung, 
        /// phổ điểm trung bình toàn hệ thống và biểu đồ quy mô sĩ số của từng lớp.
        /// </summary>
        /// <returns>Đối tượng chứa dữ liệu thống kê tổng quan AdminOverviewReportDto</returns>
        public async Task<AdminOverviewReportDto> GetAdminOverviewAsync()
        {
            // Lấy tổng số học sinh, giáo viên, lớp học và khóa học đang hoạt động trong hệ thống
            var totalStudents = await _context.Hocsinhs.CountAsync(x => x.DaXoa != true);
            var totalTeachers = await _context.Giangviens.CountAsync(x => x.DaXoa != true);
            var totalClasses = await _context.Lophocs.CountAsync(x => x.DaXoa != true);
            var totalCourses = await _context.Khoahocs.CountAsync(x => x.DaXoa != true);

            // Truy vấn toàn bộ lịch sử điểm danh đang hoạt động để phân tích tỷ lệ đi học chung
            var attendanceRecords = await _context.Diemdanhs
                .Where(x => x.DaXoa != true)
                .Include(x => x.MaTrangThaiNavigation)
                .ToListAsync();

            var attendanceRate = new AttendanceRateDto();
            foreach (var record in attendanceRecords)
            {
                var code = record.MaTrangThaiNavigation?.MaCode;
                attendanceRate.Total++;
                if (code == "DD_PRESENT") attendanceRate.Present++;
                else if (code == "DD_ABSENT") attendanceRate.Absent++;
                else if (code == "DD_LATE") attendanceRate.Late++;
                else if (code == "DD_EXCUSED") attendanceRate.Excused++;
            }

            // Truy vấn toàn bộ điểm số bài tập của học sinh để phân tích phân phối phổ điểm
            var scores = await _context.Nopbais
                .Where(x => x.DaXoa != true && x.DiemSo != null)
                .Select(x => (double)x.DiemSo!.Value)
                .ToListAsync();

            var scoreDist = new ScoreDistributionDto();
            if (scores.Any())
            {
                scoreDist.Total = scores.Count;
                scoreDist.AverageScore = Math.Round(scores.Average(), 2);
                scoreDist.Under5 = scores.Count(s => s < 5.0);
                scoreDist.From5To7 = scores.Count(s => s >= 5.0 && s < 7.0);
                scoreDist.From7To85 = scores.Count(s => s >= 7.0 && s < 8.5);
                scoreDist.Above85 = scores.Count(s => s >= 8.5);
            }

            // Lấy thông tin quy mô sĩ số (số học viên thực tế vs sức chứa tối đa) của các lớp
            var classes = await _context.Lophocs
                .Where(x => x.DaXoa != true)
                .Select(x => new ClassSizeDto
                {
                    ClassName = x.TenLop,
                    StudentCount = _context.Hocsinhlophocs.Count(h => h.MaLopHoc == x.MaLopHoc && h.DaXoa != true && h.TrangThai != false),
                    Capacity = x.SiSoToiDa ?? 0
                })
                .ToListAsync();

            return new AdminOverviewReportDto
            {
                TotalStudents = totalStudents,
                TotalTeachers = totalTeachers,
                TotalClasses = totalClasses,
                TotalCourses = totalCourses,
                AttendanceRate = attendanceRate,
                ScoreDistribution = scoreDist,
                ClassSizes = classes
            };
        }

        /// <summary>
        /// Tạo nội dung báo cáo tổng quan toàn hệ thống dưới dạng chuỗi CSV tương thích với MS Excel.
        /// Chứa thông tin về danh sách các lớp, sĩ số, sức chứa, tỷ lệ chuyên cần và điểm trung bình của lớp.
        /// </summary>
        /// <returns>Chuỗi ký tự định dạng CSV kèm UTF-8 BOM để hiển thị đúng tiếng Việt</returns>
        public async Task<string> ExportAdminReportCsvAsync()
        {
            var sb = new StringBuilder();
            sb.Append("\uFEFF"); // Ghi ký tự UTF-8 BOM ở đầu file để Excel mở trực tiếp không bị lỗi font tiếng Việt
            sb.AppendLine("DANH SÁCH LỚP HỌC VÀ THỐNG KÊ CHI TIẾT");
            sb.AppendLine("Tên Lớp,Sĩ Số Hiện Tại,Sĩ Số Tối Đa,Tỷ Lệ Chuyên Cần (%),Điểm Trung Bình");

            // Lấy thông tin từng lớp học để tính toán thống kê chi tiết cho báo cáo
            var classes = await _context.Lophocs.Where(x => x.DaXoa != true).ToListAsync();
            foreach (var c in classes)
            {
                // Tính tỷ lệ chuyên cần của lớp học (số buổi đi học đúng giờ/muộn chia cho tổng số bản ghi điểm danh)
                var classAtt = await _context.Diemdanhs
                    .Where(x => x.DaXoa != true && x.MaBuoiHocNavigation.MaLopHoc == c.MaLopHoc)
                    .Include(x => x.MaTrangThaiNavigation)
                    .ToListAsync();
                
                int attTotal = classAtt.Count;
                int attPresent = classAtt.Count(x => x.MaTrangThaiNavigation?.MaCode == "DD_PRESENT" || x.MaTrangThaiNavigation?.MaCode == "DD_LATE");
                double attRate = attTotal > 0 ? Math.Round((double)attPresent / attTotal * 100, 2) : 0;

                // Tính điểm trung bình cộng của toàn bộ học sinh trong lớp học đó
                var classScores = await _context.Nopbais
                    .Where(x => x.DaXoa != true && x.MaSuKienNavigation.MaLopHoc == c.MaLopHoc && x.DiemSo != null)
                    .Select(x => (double)x.DiemSo!.Value)
                    .ToListAsync();
                
                double avgScore = classScores.Any() ? Math.Round(classScores.Average(), 2) : 0;
                int currentStudents = await _context.Hocsinhlophocs.CountAsync(h => h.MaLopHoc == c.MaLopHoc && h.DaXoa != true && h.TrangThai != false);

                // Thêm dòng thông tin lớp học vào CSV (xử lý escape dấu nháy kép cho cột Tên Lớp)
                sb.AppendLine($"\"{c.TenLop.Replace("\"", "\"\"")}\",{currentStudents},{c.SiSoToiDa ?? 0},{attRate},{avgScore}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Lấy báo cáo chi tiết của một lớp học cụ thể dưới vai trò của Giáo viên phụ trách.
        /// Kiểm tra quyền truy cập của giáo viên, lấy danh sách học sinh, phân tích tỷ lệ chuyên cần của lớp, 
        /// phổ điểm số và danh sách tiến độ (tỷ lệ chuyên cần, điểm TB, số bài nộp) của từng học sinh trong lớp.
        /// </summary>
        /// <param name="teacherProfileId">Mã hồ sơ giảng viên của người dùng đang đăng nhập</param>
        /// <param name="classId">Mã lớp học cần xem báo cáo thống kê</param>
        /// <returns>Đối tượng chứa dữ liệu thống kê lớp học TeacherClassReportDto</returns>
        public async Task<TeacherClassReportDto> GetTeacherClassOverviewAsync(Guid teacherProfileId, Guid classId)
        {
            // BƯỚC 1: Kiểm tra tính hợp lệ bảo mật xem giáo viên hiện tại có thực sự phụ trách lớp học này không
            var isAssigned = await _context.Giangvienlophocs
                .AnyAsync(x => x.MaGiangVien == teacherProfileId && x.MaLopHoc == classId && x.DaXoa != true);
            if (!isAssigned)
            {
                throw new UnauthorizedAccessException("Giáo viên không được phân công dạy lớp này.");
            }

            var classEntity = await _context.Lophocs.FirstOrDefaultAsync(x => x.MaLopHoc == classId && x.DaXoa != true);
            if (classEntity == null)
            {
                throw new KeyNotFoundException("Không tìm thấy lớp học.");
            }

            // BƯỚC 2: Truy vấn danh sách học viên hiện đang tham gia lớp học
            var enrollments = await _context.Hocsinhlophocs
                .Where(x => x.MaLopHoc == classId && x.DaXoa != true && x.TrangThai != false)
                .Include(x => x.MaHocSinhNavigation)
                .ThenInclude(x => x.MaNguoiDungNavigation)
                .ToListAsync();

            // BƯỚC 3: Lấy toàn bộ lịch sử điểm danh của lớp học đó
            var attendanceRecords = await _context.Diemdanhs
                .Where(x => x.DaXoa != true && x.MaBuoiHocNavigation.MaLopHoc == classId)
                .Include(x => x.MaTrangThaiNavigation)
                .ToListAsync();

            // BƯỚC 4: Lấy danh sách bài tập về nhà và bài kiểm tra thuộc lớp học
            var events = await _context.Sukienlophocs
                .Where(x => x.MaLopHoc == classId && x.MaBaiTap != null && x.DaXoa != true)
                .ToListAsync();
            var eventIds = events.Select(e => e.MaSuKien).ToList();

            // BƯỚC 5: Lấy danh sách kết quả bài nộp của học sinh đối với các bài tập của lớp
            var submissions = await _context.Nopbais
                .Where(x => eventIds.Contains(x.MaSuKien) && x.DaXoa != true)
                .ToListAsync();

            var report = new TeacherClassReportDto
            {
                ClassId = classId,
                ClassName = classEntity.TenLop,
                TotalStudents = enrollments.Count
            };

            // Phân tích tỷ lệ điểm danh chung cho biểu đồ tròn chuyên cần của lớp
            foreach (var record in attendanceRecords)
            {
                var code = record.MaTrangThaiNavigation?.MaCode;
                report.AttendanceRate.Total++;
                if (code == "DD_PRESENT") report.AttendanceRate.Present++;
                else if (code == "DD_ABSENT") report.AttendanceRate.Absent++;
                else if (code == "DD_LATE") report.AttendanceRate.Late++;
                else if (code == "DD_EXCUSED") report.AttendanceRate.Excused++;
            }

            // Phân tích phổ điểm chung của các bài tập trong lớp
            var classScores = submissions.Where(s => s.DiemSo != null).Select(s => (double)s.DiemSo!.Value).ToList();
            if (classScores.Any())
            {
                report.ScoreDistribution.Total = classScores.Count;
                report.ScoreDistribution.AverageScore = Math.Round(classScores.Average(), 2);
                report.ScoreDistribution.Under5 = classScores.Count(s => s < 5.0);
                report.ScoreDistribution.From5To7 = classScores.Count(s => s >= 5.0 && s < 7.0);
                report.ScoreDistribution.From7To85 = classScores.Count(s => s >= 7.0 && s < 8.5);
                report.ScoreDistribution.Above85 = classScores.Count(s => s >= 8.5);
            }

            // Tính tỷ lệ hoàn thành bài tập về nhà chung của cả lớp
            int totalPossibleSubmissions = enrollments.Count * events.Count;
            int completedSubmissions = submissions.Count(s => s.ThoiGianNop != null);
            report.HomeworkCompletionRate = totalPossibleSubmissions > 0
                ? Math.Round((double)completedSubmissions / totalPossibleSubmissions * 100, 2)
                : 0;

            // BƯỚC 6: Xây dựng thống kê chi tiết cho từng học sinh để hiển thị dạng bảng trên FE
            foreach (var enrollment in enrollments)
            {
                var studentId = enrollment.MaHocSinh;
                var studentName = enrollment.MaHocSinhNavigation?.MaNguoiDungNavigation?.HoTen ?? "Học sinh";

                // Tính toán tỷ lệ đi học riêng biệt của học sinh này
                var studentAttendance = attendanceRecords.Where(a => a.MaHocSinh == studentId).ToList();
                int attTotal = studentAttendance.Count;
                int attPresent = studentAttendance.Count(a => a.MaTrangThaiNavigation?.MaCode == "DD_PRESENT" || a.MaTrangThaiNavigation?.MaCode == "DD_LATE");
                double attRate = attTotal > 0 ? Math.Round((double)attPresent / attTotal * 100, 2) : 0;

                // Tính điểm trung bình và đếm số lượng bài tập học sinh này đã làm/được giao
                var studentSubs = submissions.Where(s => s.MaHocSinh == studentId).ToList();
                var studentScores = studentSubs.Where(s => s.DiemSo != null).Select(s => (double)s.DiemSo!.Value).ToList();
                double avgScore = studentScores.Any() ? Math.Round(studentScores.Average(), 2) : 0;
                int completedHw = studentSubs.Count(s => s.ThoiGianNop != null);

                report.Students.Add(new StudentReportItemDto
                {
                    StudentId = studentId,
                    StudentName = studentName,
                    AttendanceRate = attRate,
                    AverageScore = avgScore,
                    CompletedHomeworkCount = completedHw,
                    TotalHomeworkCount = events.Count
                });
            }

            return report;
        }

        /// <summary>
        /// Xuất file CSV báo cáo thống kê tình hình học tập và chuyên cần của tất cả học sinh trong một lớp.
        /// Thường được giáo viên sử dụng để quản lý tiến độ hoặc nhập điểm xuất ra Excel.
        /// </summary>
        /// <param name="teacherProfileId">Mã hồ sơ giảng viên của người dùng đăng nhập</param>
        /// <param name="classId">Mã lớp học cần xuất dữ liệu</param>
        /// <returns>Nội dung file CSV dạng chuỗi ký tự UTF-8 BOM</returns>
        public async Task<string> ExportTeacherClassReportCsvAsync(Guid teacherProfileId, Guid classId)
        {
            // Kiểm tra bảo mật và lấy dữ liệu lớp học thông qua service có sẵn
            var isAssigned = await _context.Giangvienlophocs
                .AnyAsync(x => x.MaGiangVien == teacherProfileId && x.MaLopHoc == classId && x.DaXoa != true);
            if (!isAssigned)
            {
                throw new UnauthorizedAccessException("Giáo viên không được phân công dạy lớp này.");
            }

            var classEntity = await _context.Lophocs.FirstOrDefaultAsync(x => x.MaLopHoc == classId && x.DaXoa != true);
            var overview = await GetTeacherClassOverviewAsync(teacherProfileId, classId);

            var sb = new StringBuilder();
            sb.Append("\uFEFF"); // UTF-8 BOM
            sb.AppendLine($"BÁO CÁO CHI TIẾT LỚP HỌC: {classEntity?.TenLop}");
            sb.AppendLine($"Sĩ số: {overview.TotalStudents} học sinh");
            sb.AppendLine($"Tỷ lệ hoàn thành bài tập chung: {overview.HomeworkCompletionRate}%");
            sb.AppendLine();
            sb.AppendLine("Họ Tên Học Sinh,Tỷ Lệ Chuyên Cần (%),Điểm Trung Bình,Bài Tập Hoàn Thành,Tổng Số Bài Tập");

            // Tạo từng dòng dữ liệu cho mỗi học sinh trong lớp học
            foreach (var student in overview.Students)
            {
                sb.AppendLine($"\"{student.StudentName.Replace("\"", "\"\"")}\",{student.AttendanceRate},{student.AverageScore},{student.CompletedHomeworkCount},{student.TotalHomeworkCount}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Lấy báo cáo thống kê học tập cá nhân dành riêng cho tài khoản Học sinh đang đăng nhập.
        /// Tổng hợp: Tỷ lệ chuyên cần cá nhân, tỷ lệ hoàn thành bài tập về nhà và biểu đồ đường (Line) xu hướng điểm số qua các bài tập đã nộp.
        /// </summary>
        /// <param name="studentProfileId">Mã hồ sơ học sinh của tài khoản đang đăng nhập</param>
        /// <returns>Đối tượng chứa dữ liệu thống kê cá nhân StudentReportDto</returns>
        public async Task<StudentReportDto> GetStudentOverviewAsync(Guid studentProfileId)
        {
            var student = await _context.Hocsinhs
                .Include(x => x.MaNguoiDungNavigation)
                .FirstOrDefaultAsync(x => x.MaHocSinh == studentProfileId && x.DaXoa != true);
            if (student == null)
            {
                throw new KeyNotFoundException("Không tìm thấy học sinh.");
            }

            // Lấy toàn bộ lịch sử điểm danh của riêng học sinh này
            var attendanceRecords = await _context.Diemdanhs
                .Where(x => x.MaHocSinh == studentProfileId && x.DaXoa != true)
                .Include(x => x.MaTrangThaiNavigation)
                .ToListAsync();

            // Lấy toàn bộ bài làm học sinh đã nộp
            var submissions = await _context.Nopbais
                .Where(x => x.MaHocSinh == studentProfileId && x.DaXoa != true)
                .Include(x => x.MaSuKienNavigation)
                .ToListAsync();

            // Lấy danh sách các lớp học sinh đang học để tính tổng số bài tập được giao
            var studentClassIds = await _context.Hocsinhlophocs
                .Where(x => x.MaHocSinh == studentProfileId && x.DaXoa != true && x.TrangThai != false)
                .Select(x => x.MaLopHoc)
                .ToListAsync();
            var totalAssignments = await _context.Sukienlophocs
                .CountAsync(x => studentClassIds.Contains(x.MaLopHoc) && x.MaBaiTap != null && x.DaXoa != true);

            var report = new StudentReportDto
            {
                StudentId = studentProfileId,
                StudentName = student.MaNguoiDungNavigation?.HoTen ?? "Học sinh"
            };

            // Tổng hợp chuyên cần cá nhân
            foreach (var record in attendanceRecords)
            {
                var code = record.MaTrangThaiNavigation?.MaCode;
                report.AttendanceRate.Total++;
                if (code == "DD_PRESENT") report.AttendanceRate.Present++;
                else if (code == "DD_ABSENT") report.AttendanceRate.Absent++;
                else if (code == "DD_LATE") report.AttendanceRate.Late++;
                else if (code == "DD_EXCUSED") report.AttendanceRate.Excused++;
            }

            // Tính tỷ lệ hoàn thành bài tập về nhà của bản thân
            int completedHwCount = submissions.Count(s => s.ThoiGianNop != null);
            report.HomeworkCompletionRate = totalAssignments > 0
                ? Math.Round((double)completedHwCount / totalAssignments * 100, 2)
                : 0;

            // Sắp xếp các bài tập đã làm theo thời gian để vẽ biểu đồ đường xu hướng điểm số (Line Chart)
            var sortedSubmissions = submissions
                .Where(s => s.DiemSo != null)
                .OrderBy(s => s.ThoiGianNop ?? s.ThoiGianTao)
                .ToList();

            foreach (var sub in sortedSubmissions)
            {
                var eventEntity = sub.MaSuKienNavigation;
                string assignmentName = "Bài tập";
                if (eventEntity?.MaBaiTap != null)
                {
                    var bt = await _context.Baitaps.FirstOrDefaultAsync(b => b.MaBaiTap == eventEntity.MaBaiTap.Value);
                    if (bt != null)
                    {
                        assignmentName = bt.TenBaiTap;
                    }
                }

                report.ScoreTrend.Add(new StudentScoreTrendDto
                {
                    AssignmentName = assignmentName,
                    SubmitTime = sub.ThoiGianNop,
                    Score = sub.DiemSo != null ? (double)sub.DiemSo.Value : null,
                    MaxScore = 10.0
                });
            }

            return report;
        }

        /// <summary>
        /// Xuất báo cáo kết quả học tập và lịch sử bài tập của cá nhân học sinh.
        /// Thường được dùng như học bạ điện tử cá nhân để gửi cho phụ huynh hoặc lưu trữ cá nhân.
        /// </summary>
        /// <param name="studentProfileId">Mã hồ sơ học sinh đang đăng nhập</param>
        /// <returns>Nội dung học bạ CSV dạng chuỗi ký tự UTF-8 BOM</returns>
        public async Task<string> ExportStudentReportCsvAsync(Guid studentProfileId)
        {
            var student = await _context.Hocsinhs
                .Include(x => x.MaNguoiDungNavigation)
                .FirstOrDefaultAsync(x => x.MaHocSinh == studentProfileId && x.DaXoa != true);
            var overview = await GetStudentOverviewAsync(studentProfileId);

            var sb = new StringBuilder();
            sb.Append("\uFEFF"); // UTF-8 BOM
            sb.AppendLine($"PHIẾU BÁO CÁO HỌC TẬP CÁ NHÂN");
            sb.AppendLine($"Học sinh: {student?.MaNguoiDungNavigation?.HoTen}");
            sb.AppendLine($"Tỷ lệ đi học đúng giờ: {overview.AttendanceRate.PresentPercent}%");
            sb.AppendLine($"Tỷ lệ hoàn thành bài tập: {overview.HomeworkCompletionRate}%");
            sb.AppendLine();
            sb.AppendLine("Tên Bài Tập,Thời Gian Nộp,Điểm Số,Thang Điểm");

            // Tạo các dòng thông tin cho mỗi bài tập học sinh đã làm và có điểm số
            foreach (var trend in overview.ScoreTrend)
            {
                string submitDateStr = trend.SubmitTime.HasValue ? trend.SubmitTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "Chưa nộp";
                string scoreStr = trend.Score.HasValue ? trend.Score.Value.ToString("F1") : "Chưa chấm";
                sb.AppendLine($"\"{trend.AssignmentName.Replace("\"", "\"\"")}\",{submitDateStr},{scoreStr},{trend.MaxScore}");
            }

            return sb.ToString();
        }
    }
}
