using backend.Attributes;
using backend.Data;
using backend.DTOs;
using backend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
[AuthorizeByPermission("STUDY_CONTENT_STUDENT")]
public class StudentStudyContentController : ControllerBase
{
    private static readonly CultureInfo VietnameseCulture = CultureInfo.GetCultureInfo("vi-VN");
    private static readonly FileExtensionContentTypeProvider ContentTypeProvider = new();

    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public StudentStudyContentController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [HttpGet("current-content")]
    public async Task<IActionResult> GetCurrentContent()
    {
        var auth = await ResolveCurrentStudentAsync();
        if (auth.Error != null)
        {
            return auth.Error;
        }

        var currentContext = await ResolveCurrentStudyContextAsync(auth.StudentId!.Value);
        if (currentContext.Error != null)
        {
            return currentContext.Error;
        }

        var overview = await BuildOverviewAsync(
            currentContext.CourseId!.Value,
            currentContext.ClassId,
            currentContext.ClassName,
            currentContext.CourseName!,
            currentContext.ClassId.HasValue ? [currentContext.ClassId.Value] : null);

        return Ok(ApiResponseDto<StudyContentOverviewDto>.Ok(overview, "Lấy nội dung học tập hiện tại thành công."));
    }

    [HttpGet("courses")]
    public async Task<IActionResult> GetCourses()
    {
        var auth = await ResolveCurrentStudentAsync();
        if (auth.Error != null)
        {
            return auth.Error;
        }

        var accessibleStudentClasses = QueryAccessibleStudentClasses(auth.StudentId!.Value);
        var enrolledCourses = await accessibleStudentClasses
            .Join(
                _context.ChitietkhoahocLophocs.AsNoTracking().Where(x => x.DaXoa != true && x.TrangThai != false),
                studentClass => studentClass.MaLopHoc,
                courseClass => courseClass.MaLopHoc,
                (studentClass, courseClass) => new
                {
                    courseClass.MaKhoaHoc,
                    courseClass.MaLopHoc,
                    TenKhoaHoc = courseClass.MaKhoaHocNavigation.TenKhoaHoc,
                    KhoaHocDaXoa = courseClass.MaKhoaHocNavigation.DaXoa,
                    KhoaHocTrangThai = courseClass.MaKhoaHocNavigation.TrangThai
                })
            .Where(x => x.KhoaHocDaXoa != true && x.KhoaHocTrangThai != false)
            .ToListAsync();

        var courseIds = enrolledCourses
            .Select(x => x.MaKhoaHoc)
            .Distinct()
            .ToList();

        var enrolledClassKeys = enrolledCourses
            .Select(x => $"{x.MaKhoaHoc:N}:{x.MaLopHoc:N}")
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var chapterCounts = courseIds.Count == 0
            ? new Dictionary<Guid, int>()
            : (await _context.Chuonghocs
                .AsNoTracking()
                .Where(x =>
                    x.MaLopHoc.HasValue &&
                    courseIds.Contains(x.MaKhoaHoc) &&
                    x.DaXoa != true &&
                    x.TrangThai != false)
                .Select(x => new
                {
                    x.MaKhoaHoc,
                    x.MaLopHoc
                })
                .ToListAsync())
                .Where(x => x.MaLopHoc.HasValue && enrolledClassKeys.Contains($"{x.MaKhoaHoc:N}:{x.MaLopHoc.Value:N}"))
                .GroupBy(x => x.MaKhoaHoc)
                .ToDictionary(x => x.Key, x => x.Count());

        var courses = enrolledCourses
            .GroupBy(x => new { x.MaKhoaHoc, x.TenKhoaHoc })
            .Select(x => new StudyContentCourseOptionDto
            {
                MaKhoaHoc = x.Key.MaKhoaHoc,
                TenKhoaHoc = x.Key.TenKhoaHoc,
                SoLopHoc = x.Select(item => item.MaLopHoc).Distinct().Count(),
                SoChuongHoc = chapterCounts.TryGetValue(x.Key.MaKhoaHoc, out var count) ? count : 0
            })
            .OrderBy(x => x.TenKhoaHoc)
            .ToList();

        return Ok(ApiResponseDto<List<StudyContentCourseOptionDto>>.Ok(courses, "Lấy danh sách khóa học của học sinh thành công."));
    }

    [HttpGet("courses/{courseId:guid}/classes")]
    public async Task<IActionResult> GetClassesByCourse(Guid courseId)
    {
        var auth = await ResolveCurrentStudentAsync();
        if (auth.Error != null)
        {
            return auth.Error;
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var classRows = await QueryAccessibleStudentClasses(auth.StudentId!.Value)
            .Join(
                _context.ChitietkhoahocLophocs.AsNoTracking().Where(x =>
                    x.MaKhoaHoc == courseId &&
                    x.DaXoa != true &&
                    x.TrangThai != false),
                studentClass => studentClass.MaLopHoc,
                courseClass => courseClass.MaLopHoc,
                (studentClass, courseClass) => new
                {
                    courseClass.MaLopHoc,
                    courseClass.MaKhoaHoc,
                    TenLop = courseClass.MaLopHocNavigation.TenLop,
                    NgayBatDau = courseClass.MaLopHocNavigation.NgayBatDau.HasValue
                        ? courseClass.MaLopHocNavigation.NgayBatDau.Value.ToString("yyyy-MM-dd")
                        : null,
                    NgayKetThuc = courseClass.MaLopHocNavigation.NgayKetThuc.HasValue
                        ? courseClass.MaLopHocNavigation.NgayKetThuc.Value.ToString("yyyy-MM-dd")
                        : null,
                    LaLopHienTai = !studentClass.NgayRoiLop.HasValue || studentClass.NgayRoiLop.Value >= today
                })
            .ToListAsync();

        var classes = classRows
            .GroupBy(x => new { x.MaLopHoc, x.MaKhoaHoc, x.TenLop, x.NgayBatDau, x.NgayKetThuc })
            .Select(x => new StudyContentClassOptionDto
            {
                MaLopHoc = x.Key.MaLopHoc,
                MaKhoaHoc = x.Key.MaKhoaHoc,
                TenLop = x.Key.TenLop,
                NgayBatDau = x.Key.NgayBatDau,
                NgayKetThuc = x.Key.NgayKetThuc,
                LaLopHienTai = x.Any(item => item.LaLopHienTai)
            })
            .OrderByDescending(x => x.LaLopHienTai)
            .ThenByDescending(x => x.NgayKetThuc ?? string.Empty)
            .ThenByDescending(x => x.NgayBatDau ?? string.Empty)
            .ThenBy(x => x.TenLop)
            .ToList();

        if (classes.Count == 0)
        {
            var courseExists = await _context.Khoahocs
                .AsNoTracking()
                .AnyAsync(x => x.MaKhoaHoc == courseId && x.DaXoa != true && x.TrangThai != false);

            if (!courseExists)
            {
                return NotFound(ApiResponseDto<object>.Fail("Không tìm thấy khóa học.", "STUDY_CONTENT_COURSE_NOT_FOUND"));
            }

            return ForbiddenStudyContentResponse();
        }

        return Ok(ApiResponseDto<List<StudyContentClassOptionDto>>.Ok(classes, "Lấy danh sách lớp học của học sinh thành công."));
    }

    [HttpGet("contents")]
    public async Task<IActionResult> GetContents([FromQuery] Guid courseId, [FromQuery] Guid? classId)
    {
        var auth = await ResolveCurrentStudentAsync();
        if (auth.Error != null)
        {
            return auth.Error;
        }

        string? className = null;
        List<Guid>? visibleClassIds = null;
        if (classId.HasValue)
        {
            var classAccess = await EnsureStudentHasClassInCourseAsync(auth.StudentId!.Value, courseId, classId.Value);
            if (classAccess.Error != null)
            {
                return classAccess.Error;
            }

            className = classAccess.ClassName;
            visibleClassIds = [classId.Value];
        }
        else
        {
            visibleClassIds = await GetStudentClassIdsByCourseAsync(auth.StudentId!.Value, courseId);
            if (visibleClassIds.Count == 0)
            {
                var courseExists = await _context.Khoahocs
                    .AsNoTracking()
                    .AnyAsync(x => x.MaKhoaHoc == courseId && x.DaXoa != true && x.TrangThai != false);

                if (!courseExists)
                {
                    return NotFound(ApiResponseDto<object>.Fail("Không tìm thấy khóa học.", "STUDY_CONTENT_COURSE_NOT_FOUND"));
                }

                return ForbiddenStudyContentResponse();
            }
        }

        var course = await _context.Khoahocs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.MaKhoaHoc == courseId && x.DaXoa != true && x.TrangThai != false);

        if (course == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Không tìm thấy khóa học.", "STUDY_CONTENT_COURSE_NOT_FOUND"));
        }

        var overview = await BuildOverviewAsync(courseId, classId, className, course.TenKhoaHoc, visibleClassIds);
        return Ok(ApiResponseDto<StudyContentOverviewDto>.Ok(overview, "Lấy nội dung học tập thành công."));
    }

    [HttpGet("documents/{documentId:guid}/download")]
    public async Task<IActionResult> DownloadDocument(Guid documentId, [FromQuery] Guid? classId)
    {
        var auth = await ResolveCurrentStudentAsync();
        if (auth.Error != null)
        {
            return auth.Error;
        }

        var document = await _context.Tailieus
            .AsNoTracking()
            .Where(x => x.MaTaiLieu == documentId && x.DaXoa != true && x.TrangThai != false)
            .Select(x => new
            {
                x.MaTaiLieu,
                x.TenTaiLieu,
                x.LinkTaiLieu,
                MaKhoaHoc = x.MaChuongHocNavigation.MaKhoaHoc,
                MaLopHoc = x.MaChuongHocNavigation.MaLopHoc
            })
            .FirstOrDefaultAsync();

        if (document == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Không tìm thấy tài liệu.", "STUDY_CONTENT_DOCUMENT_NOT_FOUND"));
        }

        if (classId.HasValue)
        {
            if (document.MaLopHoc != classId.Value)
            {
                return NotFound(ApiResponseDto<object>.Fail("Tài liệu không thuộc lớp đã chọn.", "STUDY_CONTENT_DOCUMENT_CLASS_MISMATCH"));
            }

            var classAccess = await EnsureStudentHasClassInCourseAsync(auth.StudentId!.Value, document.MaKhoaHoc, classId.Value);
            if (classAccess.Error != null)
            {
                return classAccess.Error;
            }
        }
        else
        {
            if (!document.MaLopHoc.HasValue)
            {
                return ForbiddenStudyContentResponse();
            }

            var classAccess = await EnsureStudentHasClassInCourseAsync(auth.StudentId!.Value, document.MaKhoaHoc, document.MaLopHoc.Value);
            if (classAccess.Error != null)
            {
                return classAccess.Error;
            }
        }

        var link = document.LinkTaiLieu?.Trim();
        if (string.IsNullOrWhiteSpace(link))
        {
            return NotFound(ApiResponseDto<object>.Fail("Tài liệu chưa có đường dẫn tải xuống.", "STUDY_CONTENT_DOCUMENT_LINK_MISSING"));
        }

        if (IsExternalUrl(link))
        {
            return Redirect(link);
        }

        if (!IsStoredStudyMaterialPath(link))
        {
            return BadRequest(ApiResponseDto<object>.Fail("Tài liệu không hỗ trợ tải xuống qua hệ thống.", "STUDY_CONTENT_DOCUMENT_LINK_INVALID"));
        }

        var webRootPath = EnsureWebRootPath();
        var relativePhysicalPath = link.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var physicalPath = Path.Combine(webRootPath, relativePhysicalPath);

        if (!System.IO.File.Exists(physicalPath))
        {
            return NotFound(ApiResponseDto<object>.Fail("Không tìm thấy file tài liệu trên máy chủ.", "STUDY_CONTENT_DOCUMENT_FILE_NOT_FOUND"));
        }

        if (!ContentTypeProvider.TryGetContentType(document.TenTaiLieu, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        return PhysicalFile(physicalPath, contentType, document.TenTaiLieu);
    }

    private async Task<(Guid? UserId, Guid? StudentId, IActionResult? Error)> ResolveCurrentStudentAsync()
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return (null, null, Unauthorized(ApiResponseDto<object>.Fail("Bạn chưa đăng nhập.", "AUTH_INVALID_TOKEN")));
        }

        var student = await _context.Hocsinhs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.MaNguoiDung == userId.Value && x.DaXoa != true && x.TrangThai != false);

        if (student == null)
        {
            return (userId, null, NotFound(ApiResponseDto<object>.Fail("Không tìm thấy hồ sơ học sinh.", "STUDENT_PROFILE_NOT_FOUND")));
        }

        return (userId, student.MaHocSinh, null);
    }

    private async Task<(Guid? CourseId, string? CourseName, Guid? ClassId, string? ClassName, IActionResult? Error)> ResolveCurrentStudyContextAsync(Guid studentId)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        // If legacy data still contains more than one active assignment, prefer the most recent one.
        var currentContext = await _context.Hocsinhlophocs
            .AsNoTracking()
            .Where(x =>
                x.MaHocSinh == studentId &&
                x.DaXoa != true &&
                x.TrangThai != false &&
                (!x.NgayThamGia.HasValue || x.NgayThamGia.Value <= today) &&
                (!x.NgayRoiLop.HasValue || x.NgayRoiLop.Value >= today))
            .Join(
                _context.ChitietkhoahocLophocs.AsNoTracking().Where(x => x.DaXoa != true && x.TrangThai != false),
                studentClass => studentClass.MaLopHoc,
                courseClass => courseClass.MaLopHoc,
                (studentClass, courseClass) => new
                {
                    courseClass.MaKhoaHoc,
                    TenKhoaHoc = courseClass.MaKhoaHocNavigation.TenKhoaHoc,
                    courseClass.MaLopHoc,
                    TenLop = courseClass.MaLopHocNavigation.TenLop,
                    studentClass.NgayThamGia,
                    studentClass.ThoiGianTao,
                    NgayBatDauLop = courseClass.MaLopHocNavigation.NgayBatDau
                })
            .OrderByDescending(x => x.NgayThamGia ?? DateOnly.MinValue)
            .ThenByDescending(x => x.NgayBatDauLop ?? DateOnly.MinValue)
            .ThenByDescending(x => x.ThoiGianTao ?? DateTime.MinValue)
            .FirstOrDefaultAsync();

        if (currentContext == null)
        {
            return (null, null, null, null,
                NotFound(ApiResponseDto<object>.Fail("Học sinh chưa được xếp lớp học hiện tại.", "STUDY_CONTENT_CURRENT_ENROLLMENT_NOT_FOUND")));
        }

        return (currentContext.MaKhoaHoc, currentContext.TenKhoaHoc, currentContext.MaLopHoc, currentContext.TenLop, null);
    }

    private async Task<List<Guid>> GetStudentClassIdsByCourseAsync(Guid studentId, Guid courseId)
    {
        return await QueryAccessibleStudentClasses(studentId)
            .Join(
                _context.ChitietkhoahocLophocs.AsNoTracking().Where(x =>
                    x.MaKhoaHoc == courseId &&
                    x.DaXoa != true &&
                    x.TrangThai != false),
                studentClass => studentClass.MaLopHoc,
                courseClass => courseClass.MaLopHoc,
                (_, courseClass) => courseClass.MaLopHoc)
            .Distinct()
            .ToListAsync();
    }

    private async Task<(string? ClassName, IActionResult? Error)> EnsureStudentHasClassInCourseAsync(Guid studentId, Guid courseId, Guid classId)
    {
        var classInfo = await QueryAccessibleStudentClasses(studentId)
            .Where(x =>
                x.MaLopHoc == classId &&
                x.MaHocSinh == studentId)
            .Join(
                _context.ChitietkhoahocLophocs.AsNoTracking().Where(x =>
                    x.MaKhoaHoc == courseId &&
                    x.MaLopHoc == classId &&
                    x.DaXoa != true &&
                    x.TrangThai != false),
                studentClass => studentClass.MaLopHoc,
                courseClass => courseClass.MaLopHoc,
                (_, courseClass) => new
                {
                    courseClass.MaLopHocNavigation.TenLop
                })
            .FirstOrDefaultAsync();

        if (classInfo == null)
        {
            var classBelongsToCourse = await _context.ChitietkhoahocLophocs
                .AsNoTracking()
                .AnyAsync(x =>
                    x.MaKhoaHoc == courseId &&
                    x.MaLopHoc == classId &&
                    x.DaXoa != true &&
                    x.TrangThai != false);

            if (!classBelongsToCourse)
            {
                return (null, NotFound(ApiResponseDto<object>.Fail("Lớp học không thuộc khóa học đã chọn.", "STUDY_CONTENT_CLASS_NOT_FOUND")));
            }

            return (null, ForbiddenStudyContentResponse());
        }

        return (classInfo.TenLop, null);
    }

    private IQueryable<backend.Models.Hocsinhlophoc> QueryAccessibleStudentClasses(Guid studentId)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        return _context.Hocsinhlophocs
            .AsNoTracking()
            .Where(x =>
                x.MaHocSinh == studentId &&
                x.DaXoa != true &&
                x.TrangThai != false &&
                (!x.NgayThamGia.HasValue || x.NgayThamGia.Value <= today));
    }

    private async Task<StudyContentOverviewDto> BuildOverviewAsync(
        Guid courseId,
        Guid? classId,
        string? className,
        string courseName,
        IReadOnlyCollection<Guid>? visibleClassIds = null)
    {
        var chaptersQuery = _context.Chuonghocs
            .AsNoTracking()
            .Where(x => x.MaKhoaHoc == courseId && x.DaXoa != true && x.TrangThai != false);

        if (classId.HasValue)
        {
            chaptersQuery = chaptersQuery.Where(x => x.MaLopHoc == classId.Value);
        }
        else if (visibleClassIds != null && visibleClassIds.Count > 0)
        {
            chaptersQuery = chaptersQuery.Where(x => x.MaLopHoc.HasValue && visibleClassIds.Contains(x.MaLopHoc.Value));
        }

        var chapters = await chaptersQuery
            .OrderBy(x => x.MaLopHocNavigation != null ? x.MaLopHocNavigation.TenLop : string.Empty)
            .ThenBy(x => x.ThuTu ?? int.MaxValue)
            .ThenBy(x => x.TenChuong)
            .Select(x => new
            {
                x.MaChuong,
                x.MaKhoaHoc,
                x.MaLopHoc,
                TenLop = x.MaLopHocNavigation != null ? x.MaLopHocNavigation.TenLop : null,
                x.TenChuong,
                x.MoTa,
                x.ThuTu
            })
            .ToListAsync();

        var chapterIds = chapters.Select(x => x.MaChuong).ToList();
        var documents = chapterIds.Count == 0
            ? []
            : await _context.Tailieus
                .AsNoTracking()
                .Where(x => chapterIds.Contains(x.MaChuongHoc) && x.DaXoa != true && x.TrangThai != false)
                .OrderByDescending(x => x.ThoiGianTao ?? DateTime.MinValue)
                .Select(x => new
                {
                    x.MaTaiLieu,
                    x.MaChuongHoc,
                    x.TenTaiLieu,
                    x.LinkTaiLieu,
                    x.MoTa,
                    x.ThoiGianTao
                })
                .ToListAsync();

        var documentLookup = documents
            .GroupBy(x => x.MaChuongHoc)
            .ToDictionary(
                x => x.Key,
                x => x.Select(BuildDocumentDto).ToList());

        return new StudyContentOverviewDto
        {
            MaKhoaHoc = courseId,
            TenKhoaHoc = courseName,
            MaLopHoc = classId,
            TenLop = className,
            NoiDungHocTap = chapters.Select(chapter => new StudyContentChapterDto
            {
                MaChuong = chapter.MaChuong,
                MaKhoaHoc = chapter.MaKhoaHoc,
                MaLopHoc = chapter.MaLopHoc,
                TenLop = chapter.TenLop,
                TenChuong = chapter.TenChuong,
                MoTa = chapter.MoTa,
                ThuTu = chapter.ThuTu,
                TaiLieu = documentLookup.TryGetValue(chapter.MaChuong, out var chapterDocuments)
                    ? chapterDocuments
                    : []
            }).ToList()
        };
    }

    private string EnsureWebRootPath()
    {
        var webRootPath = _environment.WebRootPath;
        if (!string.IsNullOrWhiteSpace(webRootPath))
        {
            return webRootPath;
        }

        webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
        if (!Directory.Exists(webRootPath))
        {
            Directory.CreateDirectory(webRootPath);
        }

        return webRootPath;
    }

    private static bool IsStoredStudyMaterialPath(string path)
    {
        return path.StartsWith("/uploads/study-materials/", StringComparison.OrdinalIgnoreCase)
            || path.StartsWith("uploads/study-materials/", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsExternalUrl(string? path)
    {
        return Uri.TryCreate(path?.Trim(), UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }

    private static StudyContentDocumentDto BuildDocumentDto(dynamic document)
    {
        var createdAt = document.ThoiGianTao is DateTime createdTime
            ? createdTime
            : DateTime.UtcNow;
        var link = document.LinkTaiLieu?.ToString() ?? string.Empty;

        return new StudyContentDocumentDto
        {
            MaTaiLieu = document.MaTaiLieu,
            MaChuongHoc = document.MaChuongHoc,
            TenTaiLieu = document.TenTaiLieu ?? string.Empty,
            LinkTaiLieu = link,
            MoTa = document.MoTa,
            LoaiTaiLieu = ResolveDocumentType(document.TenTaiLieu?.ToString(), link),
            NgayDang = createdAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            NgayDangHienThi = createdAt.ToString("dd/MM/yyyy", VietnameseCulture),
            LaLinkNgoai = IsExternalUrl(link)
        };
    }

    private static string ResolveDocumentType(string? fileName, string? link)
    {
        var fileExtension = Path.GetExtension(fileName ?? string.Empty);
        var source = !string.IsNullOrWhiteSpace(fileExtension) ? fileName : link;
        var extension = Path.GetExtension(source ?? string.Empty).ToLowerInvariant();

        return extension switch
        {
            ".pdf" => "PDF",
            ".mp4" or ".mov" or ".avi" or ".mkv" or ".webm" => "Video",
            ".mp3" or ".wav" or ".m4a" => "Audio",
            ".doc" or ".docx" => "Word",
            ".xls" or ".xlsx" => "Excel",
            ".ppt" or ".pptx" => "PowerPoint",
            ".txt" => "Text",
            ".zip" or ".rar" => "Archive",
            _ => "File"
        };
    }

    private ObjectResult ForbiddenStudyContentResponse()
    {
        return StatusCode(StatusCodes.Status403Forbidden,
            ApiResponseDto<object>.Fail("Bạn không có quyền xem nội dung học tập của lớp này.", "STUDY_CONTENT_FORBIDDEN"));
    }
}
