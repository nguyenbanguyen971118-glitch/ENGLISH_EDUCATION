using backend.Data;
using backend.DTOs;
using backend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.StaticFiles;
using System.Globalization;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminStudyContentController : ControllerBase
{
    private static readonly CultureInfo VietnameseCulture = CultureInfo.GetCultureInfo("vi-VN");
    private static readonly FileExtensionContentTypeProvider ContentTypeProvider = new();

    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public AdminStudyContentController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [HttpGet("courses")]
    public async Task<IActionResult> GetCourses()
    {
        var courses = await _context.Khoahocs
            .AsNoTracking()
            .Where(x => x.DaXoa != true && x.TrangThai != false)
            .OrderBy(x => x.TenKhoaHoc)
            .Select(x => new StudyContentCourseOptionDto
            {
                MaKhoaHoc = x.MaKhoaHoc,
                TenKhoaHoc = x.TenKhoaHoc,
                SoLopHoc = x.ChitietkhoahocLophocs.Count(c => c.DaXoa != true && c.TrangThai != false),
                SoChuongHoc = x.Chuonghocs.Count(c => c.DaXoa != true && c.TrangThai != false)
            })
            .ToListAsync();

        return Ok(ApiResponseDto<List<StudyContentCourseOptionDto>>.Ok(courses, "Lấy danh sách khóa học thành công."));
    }

    [HttpGet("courses/{courseId:guid}/classes")]
    public async Task<IActionResult> GetClassesByCourse(Guid courseId)
    {
        var courseExists = await _context.Khoahocs
            .AnyAsync(x => x.MaKhoaHoc == courseId && x.DaXoa != true && x.TrangThai != false);

        if (!courseExists)
        {
            return NotFound(ApiResponseDto<object>.Fail("Không tìm thấy khóa học.", "STUDY_CONTENT_COURSE_NOT_FOUND"));
        }

        var classes = await _context.ChitietkhoahocLophocs
            .AsNoTracking()
            .Where(x => x.MaKhoaHoc == courseId && x.DaXoa != true && x.TrangThai != false)
            .Select(x => new StudyContentClassOptionDto
            {
                MaLopHoc = x.MaLopHoc,
                MaKhoaHoc = x.MaKhoaHoc,
                TenLop = x.MaLopHocNavigation.TenLop,
                NgayBatDau = x.MaLopHocNavigation.NgayBatDau.HasValue
                    ? x.MaLopHocNavigation.NgayBatDau.Value.ToString("yyyy-MM-dd")
                    : null,
                NgayKetThuc = x.MaLopHocNavigation.NgayKetThuc.HasValue
                    ? x.MaLopHocNavigation.NgayKetThuc.Value.ToString("yyyy-MM-dd")
                    : null
            })
            .OrderBy(x => x.TenLop)
            .ToListAsync();

        return Ok(ApiResponseDto<List<StudyContentClassOptionDto>>.Ok(classes, "Lấy danh sách lớp học thành công."));
    }

    [HttpGet("contents")]
    public async Task<IActionResult> GetContents([FromQuery] Guid courseId, [FromQuery] Guid? classId)
    {
        var course = await _context.Khoahocs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.MaKhoaHoc == courseId && x.DaXoa != true && x.TrangThai != false);

        if (course == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Không tìm thấy khóa học.", "STUDY_CONTENT_COURSE_NOT_FOUND"));
        }

        string? className = null;
        if (classId.HasValue)
        {
            var classMapping = await _context.ChitietkhoahocLophocs
                .AsNoTracking()
                .Where(x =>
                    x.MaKhoaHoc == courseId &&
                    x.MaLopHoc == classId.Value &&
                    x.DaXoa != true &&
                    x.TrangThai != false)
                .Select(x => new
                {
                    x.MaLopHoc,
                    x.MaLopHocNavigation.TenLop
                })
                .FirstOrDefaultAsync();

            if (classMapping == null)
            {
                return NotFound(ApiResponseDto<object>.Fail("Lớp học không thuộc khóa học đã chọn.", "STUDY_CONTENT_CLASS_NOT_FOUND"));
            }

            className = classMapping.TenLop;
        }

        var chaptersQuery = _context.Chuonghocs
            .AsNoTracking()
            .Where(x => x.MaKhoaHoc == courseId && x.DaXoa != true && x.TrangThai != false);

        if (classId.HasValue)
        {
            chaptersQuery = chaptersQuery.Where(x => x.MaLopHoc == classId.Value);
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

        var overview = new StudyContentOverviewDto
        {
            MaKhoaHoc = course.MaKhoaHoc,
            TenKhoaHoc = course.TenKhoaHoc,
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

        return Ok(ApiResponseDto<StudyContentOverviewDto>.Ok(overview, "Lấy nội dung học tập thành công."));
    }

    [HttpGet("documents/{documentId:guid}/download")]
    public async Task<IActionResult> DownloadDocument(Guid documentId)
    {
        var document = await _context.Tailieus
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.MaTaiLieu == documentId && x.DaXoa != true && x.TrangThai != false);

        if (document == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Không tìm thấy tài liệu.", "STUDY_CONTENT_DOCUMENT_NOT_FOUND"));
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

    private static string ResolveDocumentDisplayName(string? requestedName, string? uploadedFileName, string? link, string? fallbackName = null)
    {
        if (!string.IsNullOrWhiteSpace(requestedName))
        {
            return requestedName.Trim();
        }

        if (!string.IsNullOrWhiteSpace(uploadedFileName))
        {
            return uploadedFileName.Trim();
        }

        if (!string.IsNullOrWhiteSpace(link))
        {
            try
            {
                if (Uri.TryCreate(link, UriKind.Absolute, out var uri))
                {
                    var lastSegment = uri.Segments.LastOrDefault();
                    if (!string.IsNullOrWhiteSpace(lastSegment))
                    {
                        return Uri.UnescapeDataString(lastSegment.Trim('/'));
                    }
                }

                return Path.GetFileName(link);
            }
            catch
            {
                // Fall through to fallback name.
            }
        }

        return fallbackName?.Trim() ?? "tai-lieu";
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
        var source = !string.IsNullOrWhiteSpace(fileName) ? fileName : link;
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
}
