using backend.Data;
using backend.DTOs;
using backend.Helpers;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Giao_Vien")]
public class TeacherStudyContentController : ControllerBase
{
    private const long MaxStudyFileSize = 100 * 1024 * 1024;
    private static readonly string[] AllowedStudyFileExtensions =
    [
        ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx",
        ".mp3", ".wav", ".m4a", ".mp4", ".mov", ".avi", ".mkv", ".webm",
        ".txt", ".zip", ".rar"
    ];

    private static readonly CultureInfo VietnameseCulture = CultureInfo.GetCultureInfo("vi-VN");
    private static readonly FileExtensionContentTypeProvider ContentTypeProvider = new();

    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public TeacherStudyContentController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    // Giảng viên chỉ nhìn thấy các khóa học có ít nhất một lớp đang được phân công.
    [HttpGet("courses")]
    public async Task<IActionResult> GetCourses()
    {
        var auth = await ResolveCurrentTeacherAsync();
        if (auth.Error != null)
        {
            return auth.Error;
        }

        var assignedCourses = await _context.Giangvienlophocs
            .AsNoTracking()
            .Where(x =>
                x.MaGiangVien == auth.TeacherId &&
                x.DaXoa != true &&
                x.TrangThai != false)
            .Join(
                _context.ChitietkhoahocLophocs.AsNoTracking().Where(x => x.DaXoa != true && x.TrangThai != false),
                teacherClass => teacherClass.MaLopHoc,
                courseClass => courseClass.MaLopHoc,
                (teacherClass, courseClass) => new
                {
                    courseClass.MaKhoaHoc,
                    courseClass.MaLopHoc,
                    TenKhoaHoc = courseClass.MaKhoaHocNavigation.TenKhoaHoc,
                    KhoaHocDaXoa = courseClass.MaKhoaHocNavigation.DaXoa,
                    KhoaHocTrangThai = courseClass.MaKhoaHocNavigation.TrangThai
                })
            .Where(x => x.KhoaHocDaXoa != true && x.KhoaHocTrangThai != false)
            .ToListAsync();

        var courseIds = assignedCourses
            .Select(x => x.MaKhoaHoc)
            .Distinct()
            .ToList();

        var assignedClassKeys = assignedCourses
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
                .Where(x => x.MaLopHoc.HasValue && assignedClassKeys.Contains($"{x.MaKhoaHoc:N}:{x.MaLopHoc.Value:N}"))
                .GroupBy(x => x.MaKhoaHoc)
                .ToDictionary(x => x.Key, x => x.Count());

        var courses = assignedCourses
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

        return Ok(ApiResponseDto<List<StudyContentCourseOptionDto>>.Ok(courses, "Lấy danh sách khóa học giảng dạy thành công."));
    }

    // Chỉ trả các lớp thuộc khóa học và thuộc phân công của chính giảng viên hiện tại.
    [HttpGet("courses/{courseId:guid}/classes")]
    public async Task<IActionResult> GetClassesByCourse(Guid courseId)
    {
        var auth = await ResolveCurrentTeacherAsync();
        if (auth.Error != null)
        {
            return auth.Error;
        }

        var classRows = await _context.Giangvienlophocs
            .AsNoTracking()
            .Where(x =>
                x.MaGiangVien == auth.TeacherId &&
                x.DaXoa != true &&
                x.TrangThai != false)
            .Join(
                _context.ChitietkhoahocLophocs.AsNoTracking().Where(x =>
                    x.MaKhoaHoc == courseId &&
                    x.DaXoa != true &&
                    x.TrangThai != false),
                teacherClass => teacherClass.MaLopHoc,
                courseClass => courseClass.MaLopHoc,
                (teacherClass, courseClass) => new
                {
                    courseClass.MaLopHoc,
                    courseClass.MaKhoaHoc,
                    TenLop = courseClass.MaLopHocNavigation.TenLop,
                    NgayBatDau = courseClass.MaLopHocNavigation.NgayBatDau.HasValue
                        ? courseClass.MaLopHocNavigation.NgayBatDau.Value.ToString("yyyy-MM-dd")
                        : null,
                    NgayKetThuc = courseClass.MaLopHocNavigation.NgayKetThuc.HasValue
                        ? courseClass.MaLopHocNavigation.NgayKetThuc.Value.ToString("yyyy-MM-dd")
                        : null
                })
            .OrderBy(x => x.TenLop)
            .ToListAsync();

        var classes = classRows
            .GroupBy(x => new { x.MaLopHoc, x.MaKhoaHoc, x.TenLop, x.NgayBatDau, x.NgayKetThuc })
            .Select(x => new StudyContentClassOptionDto
            {
                MaLopHoc = x.Key.MaLopHoc,
                MaKhoaHoc = x.Key.MaKhoaHoc,
                TenLop = x.Key.TenLop,
                NgayBatDau = x.Key.NgayBatDau,
                NgayKetThuc = x.Key.NgayKetThuc
            })
            .OrderBy(x => x.TenLop)
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

        return Ok(ApiResponseDto<List<StudyContentClassOptionDto>>.Ok(classes, "Lấy danh sách lớp học phụ trách thành công."));
    }

    // Nội dung học tập được quản lý theo từng lớp; classId quyết định đúng phạm vi dữ liệu được xem/sửa.
    [HttpGet("contents")]
    public async Task<IActionResult> GetContents([FromQuery] Guid courseId, [FromQuery] Guid? classId)
    {
        var auth = await ResolveCurrentTeacherAsync();
        if (auth.Error != null)
        {
            return auth.Error;
        }

        string? className = null;
        List<Guid>? visibleClassIds = null;
        if (classId.HasValue)
        {
            var classAccess = await EnsureTeacherHasClassInCourseAsync(auth.TeacherId!.Value, courseId, classId.Value);
            if (classAccess.Error != null)
            {
                return classAccess.Error;
            }

            className = classAccess.ClassName;
            visibleClassIds = [classId.Value];
        }
        else
        {
            visibleClassIds = await GetTeacherClassIdsByCourseAsync(auth.TeacherId!.Value, courseId);
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

    [HttpPost("chapters")]
    public async Task<IActionResult> CreateChapter([FromBody] CreateStudyChapterDto dto)
    {
        var auth = await ResolveCurrentTeacherAsync();
        if (auth.Error != null)
        {
            return auth.Error;
        }

        if (dto.MaKhoaHoc == Guid.Empty || dto.MaLopHoc == Guid.Empty)
        {
            return BadRequest(ApiResponseDto<object>.Fail("Thiếu khóa học hoặc lớp học để tạo chương.", "STUDY_CONTENT_CONTEXT_REQUIRED"));
        }

        if (string.IsNullOrWhiteSpace(dto.TenChuong))
        {
            return BadRequest(ApiResponseDto<object>.Fail("Tên chương là bắt buộc.", "STUDY_CONTENT_CHAPTER_NAME_REQUIRED"));
        }

        var classAccess = await EnsureTeacherHasClassInCourseAsync(auth.TeacherId!.Value, dto.MaKhoaHoc, dto.MaLopHoc);
        if (classAccess.Error != null)
        {
            return classAccess.Error;
        }

        var now = DateTime.UtcNow;
        var chapter = new Chuonghoc
        {
            MaChuong = Guid.NewGuid(),
            MaKhoaHoc = dto.MaKhoaHoc,
            MaLopHoc = dto.MaLopHoc,
            TenChuong = dto.TenChuong.Trim(),
            MoTa = NormalizeOptional(dto.MoTa),
            ThuTu = dto.ThuTu ?? await GetNextChapterOrderAsync(dto.MaKhoaHoc, dto.MaLopHoc),
            NguoiTao = auth.UserId,
            ThoiGianTao = now,
            TrangThai = true,
            DaXoa = false
        };

        _context.Chuonghocs.Add(chapter);
        await _context.SaveChangesAsync();

        return Ok(ApiResponseDto<StudyContentChapterDto>.Ok(
            new StudyContentChapterDto
            {
                MaChuong = chapter.MaChuong,
                MaKhoaHoc = chapter.MaKhoaHoc,
                MaLopHoc = chapter.MaLopHoc,
                TenLop = classAccess.ClassName,
                TenChuong = chapter.TenChuong,
                MoTa = chapter.MoTa,
                ThuTu = chapter.ThuTu,
                TaiLieu = []
            },
            "Tạo chương học thành công."
        ));
    }

    [HttpPut("chapters/{chapterId:guid}")]
    public async Task<IActionResult> UpdateChapter(Guid chapterId, [FromBody] UpdateStudyChapterDto dto)
    {
        var auth = await ResolveCurrentTeacherAsync();
        if (auth.Error != null)
        {
            return auth.Error;
        }

        if (dto.MaLopHoc == Guid.Empty)
        {
            return BadRequest(ApiResponseDto<object>.Fail("Thiếu lớp học để cập nhật chương.", "STUDY_CONTENT_CLASS_REQUIRED"));
        }

        if (string.IsNullOrWhiteSpace(dto.TenChuong))
        {
            return BadRequest(ApiResponseDto<object>.Fail("Tên chương là bắt buộc.", "STUDY_CONTENT_CHAPTER_NAME_REQUIRED"));
        }

        var chapter = await _context.Chuonghocs
            .FirstOrDefaultAsync(x => x.MaChuong == chapterId && x.DaXoa != true && x.TrangThai != false);

        if (chapter == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Không tìm thấy chương học.", "STUDY_CONTENT_CHAPTER_NOT_FOUND"));
        }

        if (chapter.MaLopHoc != dto.MaLopHoc)
        {
            return NotFound(ApiResponseDto<object>.Fail("Chương học không thuộc lớp đã chọn.", "STUDY_CONTENT_CHAPTER_CLASS_MISMATCH"));
        }

        var classAccess = await EnsureTeacherHasClassInCourseAsync(auth.TeacherId!.Value, chapter.MaKhoaHoc, dto.MaLopHoc);
        if (classAccess.Error != null)
        {
            return classAccess.Error;
        }

        chapter.TenChuong = dto.TenChuong.Trim();
        chapter.MoTa = NormalizeOptional(dto.MoTa);
        chapter.ThuTu = dto.ThuTu ?? chapter.ThuTu;
        chapter.NguoiSua = auth.UserId;
        chapter.ThoiGianSua = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(ApiResponseDto<StudyContentChapterDto>.Ok(
            new StudyContentChapterDto
            {
                MaChuong = chapter.MaChuong,
                MaKhoaHoc = chapter.MaKhoaHoc,
                MaLopHoc = chapter.MaLopHoc,
                TenLop = classAccess.ClassName,
                TenChuong = chapter.TenChuong,
                MoTa = chapter.MoTa,
                ThuTu = chapter.ThuTu,
                TaiLieu = []
            },
            "Cập nhật chương học thành công."
        ));
    }

    [HttpDelete("chapters/{chapterId:guid}")]
    public async Task<IActionResult> DeleteChapter(Guid chapterId, [FromQuery] Guid classId)
    {
        var auth = await ResolveCurrentTeacherAsync();
        if (auth.Error != null)
        {
            return auth.Error;
        }

        if (classId == Guid.Empty)
        {
            return BadRequest(ApiResponseDto<object>.Fail("Thiếu lớp học để xóa chương.", "STUDY_CONTENT_CLASS_REQUIRED"));
        }

        var chapter = await _context.Chuonghocs
            .Include(x => x.Tailieus.Where(document => document.DaXoa != true && document.TrangThai != false))
            .FirstOrDefaultAsync(x => x.MaChuong == chapterId && x.DaXoa != true && x.TrangThai != false);

        if (chapter == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Không tìm thấy chương học.", "STUDY_CONTENT_CHAPTER_NOT_FOUND"));
        }

        if (chapter.MaLopHoc != classId)
        {
            return NotFound(ApiResponseDto<object>.Fail("Chương học không thuộc lớp đã chọn.", "STUDY_CONTENT_CHAPTER_CLASS_MISMATCH"));
        }

        var classAccess = await EnsureTeacherHasClassInCourseAsync(auth.TeacherId!.Value, chapter.MaKhoaHoc, classId);
        if (classAccess.Error != null)
        {
            return classAccess.Error;
        }

        var now = DateTime.UtcNow;
        var webRootPath = EnsureWebRootPath();

        foreach (var document in chapter.Tailieus)
        {
            await CleanupDocumentFileAsync(document.LinkTaiLieu, auth.UserId!.Value, now, webRootPath);
            document.DaXoa = true;
            document.TrangThai = false;
            document.NguoiSua = auth.UserId;
            document.ThoiGianSua = now;
        }

        chapter.DaXoa = true;
        chapter.TrangThai = false;
        chapter.NguoiSua = auth.UserId;
        chapter.ThoiGianSua = now;

        await _context.SaveChangesAsync();

        return Ok(ApiResponseDto<object>.Ok(new { chapterId }, "Xóa chương học thành công."));
    }

    [HttpPost("documents")]
    public async Task<IActionResult> CreateDocument([FromForm] CreateStudyDocumentDto dto)
    {
        var auth = await ResolveCurrentTeacherAsync();
        if (auth.Error != null)
        {
            return auth.Error;
        }

        if (dto.MaChuongHoc == Guid.Empty || dto.MaLopHoc == Guid.Empty)
        {
            return BadRequest(ApiResponseDto<object>.Fail("Thiếu chương học hoặc lớp học để tạo tài liệu.", "STUDY_CONTENT_CONTEXT_REQUIRED"));
        }

        if (dto.File != null && !string.IsNullOrWhiteSpace(dto.LinkTaiLieu))
        {
            return BadRequest(ApiResponseDto<object>.Fail("Chỉ được chọn upload file hoặc nhập link ngoài.", "STUDY_CONTENT_DOCUMENT_SOURCE_CONFLICT"));
        }

        var chapter = await _context.Chuonghocs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.MaChuong == dto.MaChuongHoc && x.DaXoa != true && x.TrangThai != false);

        if (chapter == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Không tìm thấy chương học.", "STUDY_CONTENT_CHAPTER_NOT_FOUND"));
        }

        if (chapter.MaLopHoc != dto.MaLopHoc)
        {
            return NotFound(ApiResponseDto<object>.Fail("Chương học không thuộc lớp đã chọn.", "STUDY_CONTENT_CHAPTER_CLASS_MISMATCH"));
        }

        var classAccess = await EnsureTeacherHasClassInCourseAsync(auth.TeacherId!.Value, chapter.MaKhoaHoc, dto.MaLopHoc);
        if (classAccess.Error != null)
        {
            return classAccess.Error;
        }

        var documentSource = await ResolveDocumentSourceAsync(
            dto.File,
            dto.LinkTaiLieu,
            auth.UserId!.Value,
            DateTime.UtcNow);

        if (documentSource.Error != null)
        {
            return documentSource.Error;
        }

        var now = DateTime.UtcNow;
        var document = new Tailieu
        {
            MaTaiLieu = Guid.NewGuid(),
            MaChuongHoc = dto.MaChuongHoc,
            TenTaiLieu = ResolveDocumentDisplayName(dto.TenTaiLieu, dto.File?.FileName, dto.LinkTaiLieu),
            LinkTaiLieu = documentSource.Link,
            MoTa = NormalizeOptional(dto.MoTa),
            NguoiTao = auth.UserId,
            ThoiGianTao = now,
            TrangThai = true,
            DaXoa = false
        };

        _context.Tailieus.Add(document);
        await _context.SaveChangesAsync();

        return Ok(ApiResponseDto<StudyContentDocumentDto>.Ok(BuildDocumentDto(new
        {
            document.MaTaiLieu,
            document.MaChuongHoc,
            document.TenTaiLieu,
            document.LinkTaiLieu,
            document.MoTa,
            ThoiGianTao = document.ThoiGianTao ?? now
        }), "Tạo tài liệu thành công."));
    }

    [HttpPut("documents/{documentId:guid}")]
    public async Task<IActionResult> UpdateDocument(Guid documentId, [FromForm] UpdateStudyDocumentDto dto)
    {
        var auth = await ResolveCurrentTeacherAsync();
        if (auth.Error != null)
        {
            return auth.Error;
        }

        if (dto.MaLopHoc == Guid.Empty)
        {
            return BadRequest(ApiResponseDto<object>.Fail("Thiếu lớp học để cập nhật tài liệu.", "STUDY_CONTENT_CLASS_REQUIRED"));
        }

        if (dto.File != null && !string.IsNullOrWhiteSpace(dto.LinkTaiLieu))
        {
            return BadRequest(ApiResponseDto<object>.Fail("Chỉ được chọn upload file hoặc nhập link ngoài.", "STUDY_CONTENT_DOCUMENT_SOURCE_CONFLICT"));
        }

        var document = await _context.Tailieus
            .Include(x => x.MaChuongHocNavigation)
            .FirstOrDefaultAsync(x => x.MaTaiLieu == documentId && x.DaXoa != true && x.TrangThai != false);

        if (document == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Không tìm thấy tài liệu.", "STUDY_CONTENT_DOCUMENT_NOT_FOUND"));
        }

        if (document.MaChuongHocNavigation.MaLopHoc != dto.MaLopHoc)
        {
            return NotFound(ApiResponseDto<object>.Fail("Tài liệu không thuộc lớp đã chọn.", "STUDY_CONTENT_DOCUMENT_CLASS_MISMATCH"));
        }

        var classAccess = await EnsureTeacherHasClassInCourseAsync(auth.TeacherId!.Value, document.MaChuongHocNavigation.MaKhoaHoc, dto.MaLopHoc);
        if (classAccess.Error != null)
        {
            return classAccess.Error;
        }

        var now = DateTime.UtcNow;
        var webRootPath = EnsureWebRootPath();
        var nextLink = document.LinkTaiLieu;

        if (dto.File != null || !string.IsNullOrWhiteSpace(dto.LinkTaiLieu))
        {
            var documentSource = await ResolveDocumentSourceAsync(dto.File, dto.LinkTaiLieu, auth.UserId!.Value, now);
            if (documentSource.Error != null)
            {
                return documentSource.Error;
            }

            await CleanupDocumentFileAsync(document.LinkTaiLieu, auth.UserId!.Value, now, webRootPath);
            nextLink = documentSource.Link;
        }

        if (string.IsNullOrWhiteSpace(nextLink))
        {
            return BadRequest(ApiResponseDto<object>.Fail("Tài liệu phải có file upload hoặc link ngoài hợp lệ.", "STUDY_CONTENT_DOCUMENT_SOURCE_REQUIRED"));
        }

        document.TenTaiLieu = ResolveDocumentDisplayName(dto.TenTaiLieu, dto.File?.FileName, dto.LinkTaiLieu, document.TenTaiLieu);
        document.LinkTaiLieu = nextLink;
        document.MoTa = NormalizeOptional(dto.MoTa);
        document.NguoiSua = auth.UserId;
        document.ThoiGianSua = now;

        await _context.SaveChangesAsync();

        return Ok(ApiResponseDto<StudyContentDocumentDto>.Ok(BuildDocumentDto(new
        {
            document.MaTaiLieu,
            document.MaChuongHoc,
            document.TenTaiLieu,
            document.LinkTaiLieu,
            document.MoTa,
            ThoiGianTao = document.ThoiGianTao ?? now
        }), "Cập nhật tài liệu thành công."));
    }

    [HttpDelete("documents/{documentId:guid}")]
    public async Task<IActionResult> DeleteDocument(Guid documentId, [FromQuery] Guid classId)
    {
        var auth = await ResolveCurrentTeacherAsync();
        if (auth.Error != null)
        {
            return auth.Error;
        }

        if (classId == Guid.Empty)
        {
            return BadRequest(ApiResponseDto<object>.Fail("Thiếu lớp học để xóa tài liệu.", "STUDY_CONTENT_CLASS_REQUIRED"));
        }

        var document = await _context.Tailieus
            .Include(x => x.MaChuongHocNavigation)
            .FirstOrDefaultAsync(x => x.MaTaiLieu == documentId && x.DaXoa != true && x.TrangThai != false);

        if (document == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Không tìm thấy tài liệu.", "STUDY_CONTENT_DOCUMENT_NOT_FOUND"));
        }

        if (document.MaChuongHocNavigation.MaLopHoc != classId)
        {
            return NotFound(ApiResponseDto<object>.Fail("Tài liệu không thuộc lớp đã chọn.", "STUDY_CONTENT_DOCUMENT_CLASS_MISMATCH"));
        }

        var classAccess = await EnsureTeacherHasClassInCourseAsync(auth.TeacherId!.Value, document.MaChuongHocNavigation.MaKhoaHoc, classId);
        if (classAccess.Error != null)
        {
            return classAccess.Error;
        }

        var now = DateTime.UtcNow;
        await CleanupDocumentFileAsync(document.LinkTaiLieu, auth.UserId!.Value, now, EnsureWebRootPath());

        document.DaXoa = true;
        document.TrangThai = false;
        document.NguoiSua = auth.UserId;
        document.ThoiGianSua = now;

        await _context.SaveChangesAsync();

        return Ok(ApiResponseDto<object>.Ok(new { documentId }, "Xóa tài liệu thành công."));
    }

    [HttpGet("documents/{documentId:guid}/download")]
    public async Task<IActionResult> DownloadDocument(Guid documentId, [FromQuery] Guid? classId)
    {
        var auth = await ResolveCurrentTeacherAsync();
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

            var classAccess = await EnsureTeacherHasClassInCourseAsync(auth.TeacherId!.Value, document.MaKhoaHoc, classId.Value);
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

            var classAccess = await EnsureTeacherHasClassInCourseAsync(auth.TeacherId!.Value, document.MaKhoaHoc, document.MaLopHoc.Value);
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

    private async Task<(Guid? UserId, Guid? TeacherId, IActionResult? Error)> ResolveCurrentTeacherAsync()
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return (null, null, Unauthorized(ApiResponseDto<object>.Fail("Bạn chưa đăng nhập.", "AUTH_INVALID_TOKEN")));
        }

        var teacher = await _context.Giangviens
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.MaNguoiDung == userId.Value && x.DaXoa != true && x.TrangThai != false);

        if (teacher == null)
        {
            return (userId, null, NotFound(ApiResponseDto<object>.Fail("Không tìm thấy hồ sơ giảng viên.", "TEACHER_PROFILE_NOT_FOUND")));
        }

        return (userId, teacher.MaGiangVien, null);
    }

    private async Task<bool> TeacherHasCourseAccessAsync(Guid teacherId, Guid courseId)
    {
        return await _context.Giangvienlophocs
            .AsNoTracking()
            .Where(x =>
                x.MaGiangVien == teacherId &&
                x.DaXoa != true &&
                x.TrangThai != false)
            .Join(
                _context.ChitietkhoahocLophocs.AsNoTracking().Where(x =>
                    x.MaKhoaHoc == courseId &&
                    x.DaXoa != true &&
                    x.TrangThai != false),
                teacherClass => teacherClass.MaLopHoc,
                courseClass => courseClass.MaLopHoc,
                (_, _) => true)
            .AnyAsync();
    }

    private async Task<List<Guid>> GetTeacherClassIdsByCourseAsync(Guid teacherId, Guid courseId)
    {
        return await _context.Giangvienlophocs
            .AsNoTracking()
            .Where(x =>
                x.MaGiangVien == teacherId &&
                x.DaXoa != true &&
                x.TrangThai != false)
            .Join(
                _context.ChitietkhoahocLophocs.AsNoTracking().Where(x =>
                    x.MaKhoaHoc == courseId &&
                    x.DaXoa != true &&
                    x.TrangThai != false),
                teacherClass => teacherClass.MaLopHoc,
                courseClass => courseClass.MaLopHoc,
                (_, courseClass) => courseClass.MaLopHoc)
            .Distinct()
            .ToListAsync();
    }

    private async Task<(string? ClassName, IActionResult? Error)> EnsureTeacherHasClassInCourseAsync(Guid teacherId, Guid courseId, Guid classId)
    {
        var classInfo = await _context.Giangvienlophocs
            .AsNoTracking()
            .Where(x =>
                x.MaGiangVien == teacherId &&
                x.MaLopHoc == classId &&
                x.DaXoa != true &&
                x.TrangThai != false)
            .Join(
                _context.ChitietkhoahocLophocs.AsNoTracking().Where(x =>
                    x.MaKhoaHoc == courseId &&
                    x.MaLopHoc == classId &&
                    x.DaXoa != true &&
                    x.TrangThai != false),
                teacherClass => teacherClass.MaLopHoc,
                courseClass => courseClass.MaLopHoc,
                (teacherClass, courseClass) => new
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

    private async Task<int> GetNextChapterOrderAsync(Guid courseId, Guid classId)
    {
        var maxOrder = await _context.Chuonghocs
            .AsNoTracking()
            .Where(x =>
                x.MaKhoaHoc == courseId &&
                x.MaLopHoc == classId &&
                x.DaXoa != true &&
                x.TrangThai != false)
            .MaxAsync(x => (int?)x.ThuTu);

        return (maxOrder ?? 0) + 1;
    }

    private async Task<(string? Link, IActionResult? Error)> ResolveDocumentSourceAsync(IFormFile? file, string? externalLink, Guid userId, DateTime now)
    {
        if (file == null && string.IsNullOrWhiteSpace(externalLink))
        {
            return (null, BadRequest(ApiResponseDto<object>.Fail("Tài liệu phải có file upload hoặc link ngoài hợp lệ.", "STUDY_CONTENT_DOCUMENT_SOURCE_REQUIRED")));
        }

        if (file != null)
        {
            var savedFile = await SaveStudyMaterialAsync(file, userId, now);
            return savedFile.Error != null
                ? (null, savedFile.Error)
                : (savedFile.RelativePath, null);
        }

        if (!IsExternalUrl(externalLink))
        {
            return (null, BadRequest(ApiResponseDto<object>.Fail("Link tài liệu ngoài phải là http hoặc https hợp lệ.", "STUDY_CONTENT_DOCUMENT_LINK_INVALID")));
        }

        return (externalLink!.Trim(), null);
    }

    private async Task<(string? RelativePath, IActionResult? Error)> SaveStudyMaterialAsync(IFormFile file, Guid userId, DateTime now)
    {
        if (file.Length == 0)
        {
            return (null, BadRequest(ApiResponseDto<object>.Fail("File tài liệu đang rỗng.", "STUDY_CONTENT_FILE_EMPTY")));
        }

        if (file.Length > MaxStudyFileSize)
        {
            return (null, BadRequest(ApiResponseDto<object>.Fail("Dung lượng tài liệu không vượt quá 100MB.", "STUDY_CONTENT_FILE_TOO_LARGE")));
        }

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedStudyFileExtensions.Contains(fileExtension))
        {
            return (null, BadRequest(ApiResponseDto<object>.Fail("Định dạng tài liệu chưa được hỗ trợ.", "STUDY_CONTENT_FILE_EXTENSION_INVALID")));
        }

        var webRootPath = EnsureWebRootPath();
        var studyMaterialsFolderPath = Path.Combine(webRootPath, "uploads", "study-materials");
        Directory.CreateDirectory(studyMaterialsFolderPath);

        var storedFileName = $"study_{userId:N}_{Guid.NewGuid():N}{fileExtension}";
        var physicalPath = Path.Combine(studyMaterialsFolderPath, storedFileName);
        var relativePath = $"/uploads/study-materials/{storedFileName}";

        await using (var stream = new FileStream(physicalPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        _context.Tainguyenluutrus.Add(new Tainguyenluutru
        {
            MaTaiNguyen = Guid.NewGuid(),
            MaNguoiDung = userId,
            TenTaiNguyen = storedFileName,
            Link = relativePath,
            NguoiTao = userId,
            ThoiGianTao = now,
            TrangThai = true,
            DaXoa = false
        });

        return (relativePath, null);
    }

    private async Task CleanupDocumentFileAsync(string? currentPath, Guid userId, DateTime now, string webRootPath)
    {
        if (string.IsNullOrWhiteSpace(currentPath))
        {
            return;
        }

        var trimmedPath = currentPath.Trim();
        if (IsStoredStudyMaterialPath(trimmedPath))
        {
            var relativePhysicalPath = trimmedPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var physicalPath = Path.Combine(webRootPath, relativePhysicalPath);

            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }
        }

        var resources = await _context.Tainguyenluutrus
            .Where(x => x.Link == trimmedPath && x.DaXoa != true)
            .ToListAsync();

        foreach (var resource in resources)
        {
            resource.DaXoa = true;
            resource.TrangThai = false;
            resource.NguoiSua = userId;
            resource.ThoiGianSua = now;
        }
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

                var fileName = Path.GetFileName(link);
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    return fileName;
                }
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

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private ObjectResult ForbiddenStudyContentResponse()
    {
        return StatusCode(StatusCodes.Status403Forbidden,
            ApiResponseDto<object>.Fail("Bạn không có quyền thao tác nội dung học tập của lớp này.", "STUDY_CONTENT_FORBIDDEN"));
    }
}
