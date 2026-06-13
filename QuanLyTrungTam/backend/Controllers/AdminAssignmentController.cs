using System.Text.Json;
using backend.Data;
using backend.DTOs;
using backend.Helpers;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminAssignmentController : ControllerBase
{
    private const string AssignmentMetadataSchema = "ADMIN_ASSIGNMENT_V1";

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly AppDbContext _context;

    public AdminAssignmentController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("bootstrap")]
    public async Task<IActionResult> GetBootstrap()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var courses = await _context.Khoahocs
            .AsNoTracking()
            .Where(x => x.DaXoa != true && x.TrangThai != false)
            .OrderBy(x => x.TenKhoaHoc)
            .Select(x => new AdminAssignmentCourseOptionDto
            {
                MaKhoaHoc = x.MaKhoaHoc,
                TenKhoaHoc = x.TenKhoaHoc,
                SoLopHoc = x.ChitietkhoahocLophocs.Count(c => c.DaXoa != true && c.TrangThai != false),
                SoBaiTap = x.Baitaps.Count(b => b.DaXoa != true && b.TrangThai != false),
                SoCauHoi = x.Nganhangcauhois.Count(q => q.DaXoa != true && q.TrangThai != false)
            })
            .ToListAsync();

        var classes = await _context.ChitietkhoahocLophocs
            .AsNoTracking()
            .Where(x => x.DaXoa != true && x.TrangThai != false)
            .OrderBy(x => x.MaLopHocNavigation.TenLop)
            .Select(x => new AdminAssignmentClassOptionDto
            {
                MaLopHoc = x.MaLopHoc,
                MaKhoaHoc = x.MaKhoaHoc,
                TenLop = x.MaLopHocNavigation.TenLop,
                TenGiangVien = x.MaLopHocNavigation.Giangvienlophocs
                    .Where(g => g.DaXoa != true && g.TrangThai != false)
                    .OrderBy(g => g.LoaiVaiTro)
                    .Select(g => g.MaGiangVienNavigation.MaNguoiDungNavigation.HoTen)
                    .FirstOrDefault(),
                SoHocVien = x.MaLopHocNavigation.Hocsinhlophocs.Count(h =>
                    h.DaXoa != true &&
                    h.TrangThai != false &&
                    (!h.NgayThamGia.HasValue || h.NgayThamGia.Value <= today) &&
                    (!h.NgayRoiLop.HasValue || h.NgayRoiLop.Value >= today))
            })
            .ToListAsync();

        var optionRows = await _context.Chitietdanhmucs
            .AsNoTracking()
            .Where(x =>
                x.DaXoa != true &&
                x.TrangThai != false &&
                x.MaNhomNavigation.DaXoa != true &&
                x.MaNhomNavigation.TrangThai != false &&
                new[]
                {
                    "LOAI_BAI_TAP",
                    "TRANG_THAI_SU_KIEN",
                    "LOAI_CAU_HOI",
                    "MUC_DO_CAU_HOI"
                }.Contains(x.MaNhomNavigation.MaCode))
            .Select(x => new
            {
                GroupCode = x.MaNhomNavigation.MaCode,
                Code = x.MaCode ?? string.Empty,
                Label = x.TenChiTiet,
                x.ThuTu
            })
            .ToListAsync();

        var data = new AdminAssignmentBootstrapDto
        {
            Courses = courses,
            Classes = classes,
            AssignmentTypes = optionRows
                .Where(x => x.GroupCode == "LOAI_BAI_TAP")
                .OrderBy(x => x.ThuTu ?? int.MaxValue)
                .Select(MapOption)
                .ToList(),
            PublishModes = optionRows
                .Where(x => x.GroupCode == "TRANG_THAI_SU_KIEN")
                .OrderBy(x => x.ThuTu ?? int.MaxValue)
                .Select(MapOption)
                .ToList(),
            QuestionTypes = optionRows
                .Where(x => x.GroupCode == "LOAI_CAU_HOI")
                .OrderBy(x => x.ThuTu ?? int.MaxValue)
                .Select(MapOption)
                .ToList(),
            DifficultyLevels = optionRows
                .Where(x => x.GroupCode == "MUC_DO_CAU_HOI")
                .OrderBy(x => x.ThuTu ?? int.MaxValue)
                .Select(MapOption)
                .ToList()
        };

        return Ok(ApiResponseDto<AdminAssignmentBootstrapDto>.Ok(data, "Lay bootstrap bai tap admin thanh cong."));
    }

    [HttpGet("question-bank")]
    public async Task<IActionResult> GetQuestionBank(
        [FromQuery] Guid? courseId,
        [FromQuery] string? search,
        [FromQuery] string? questionType,
        [FromQuery] string? difficulty)
    {
        var query = _context.Nganhangcauhois
            .AsNoTracking()
            .Where(x => x.DaXoa != true && x.TrangThai != false);

        if (courseId.HasValue)
        {
            query = query.Where(x => x.MaKhoaHoc == courseId.Value);
        }

        if (!string.IsNullOrWhiteSpace(questionType))
        {
            query = query.Where(x => x.LoaiCauHoiNavigation != null && x.LoaiCauHoiNavigation.MaCode == questionType);
        }

        if (!string.IsNullOrWhiteSpace(difficulty))
        {
            query = query.Where(x => x.MucDoNavigation != null && x.MucDoNavigation.MaCode == difficulty);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var keyword = search.Trim();
            query = query.Where(x => x.NoiDungCauHoi.Contains(keyword));
        }

        var items = await query
            .OrderBy(x => x.MaKhoaHocNavigation.TenKhoaHoc)
            .ThenBy(x => x.ThuTu ?? int.MaxValue)
            .ThenBy(x => x.ThoiGianTao ?? DateTime.MinValue)
            .Select(x => new AdminAssignmentQuestionBankItemDto
            {
                MaCauHoi = x.MaCauHoi,
                MaKhoaHoc = x.MaKhoaHoc,
                TenKhoaHoc = x.MaKhoaHocNavigation.TenKhoaHoc,
                LoaiCauHoiCode = x.LoaiCauHoiNavigation != null ? x.LoaiCauHoiNavigation.MaCode ?? string.Empty : string.Empty,
                LoaiCauHoiLabel = x.LoaiCauHoiNavigation != null ? x.LoaiCauHoiNavigation.TenChiTiet : string.Empty,
                MucDoCode = x.MucDoNavigation != null ? x.MucDoNavigation.MaCode ?? string.Empty : string.Empty,
                MucDoLabel = x.MucDoNavigation != null ? x.MucDoNavigation.TenChiTiet : string.Empty,
                NoiDungCauHoi = x.NoiDungCauHoi,
                GiaiThichDapAn = x.GiaiThichDapAn,
                AmThanhLink = x.AmThanhLink,
                HinhAnhLink = x.HinhAnhLink,
                MaCauHoiCha = x.MaCauHoiCha,
                ThuTu = x.ThuTu,
                LaCauHoiTuLuan = x.LoaiCauHoiNavigation != null && IsEssayTypeCode(x.LoaiCauHoiNavigation.MaCode),
                SoDapAnLuaChon = x.Dapans.Count(d => d.DaXoa != true && d.TrangThai != false),
                SoDapAnNhapLieu = x.Dapandiendkhuyets.Count(d => d.DaXoa != true && d.TrangThai != false),
                ThoiGianTao = x.ThoiGianTao
            })
            .ToListAsync();

        return Ok(ApiResponseDto<List<AdminAssignmentQuestionBankItemDto>>.Ok(items, "Lay ngan hang cau hoi thanh cong."));
    }

    [HttpGet("assignments")]
    public async Task<IActionResult> GetAssignments(
        [FromQuery] Guid? courseId,
        [FromQuery] string? assignmentType,
        [FromQuery] string? status,
        [FromQuery] string? search)
    {
        var query = _context.Baitaps
            .AsNoTracking()
            .Where(x => x.DaXoa != true && x.TrangThai != false);

        if (courseId.HasValue)
        {
            query = query.Where(x => x.MaKhoaHoc == courseId.Value);
        }

        if (!string.IsNullOrWhiteSpace(assignmentType))
        {
            query = query.Where(x => x.LoaiBaiTapNavigation != null && x.LoaiBaiTapNavigation.MaCode == assignmentType);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var keyword = search.Trim();
            query = query.Where(x => x.TenBaiTap.Contains(keyword) || (x.MoTa != null && x.MoTa.Contains(keyword)));
        }

        var assignmentIds = await query
            .OrderByDescending(x => x.ThoiGianSua ?? x.ThoiGianTao ?? DateTime.MinValue)
            .Select(x => x.MaBaiTap)
            .ToListAsync();

        var results = new List<AdminAssignmentSummaryDto>();
        foreach (var assignmentId in assignmentIds)
        {
            var detail = await BuildAssignmentDetailAsync(assignmentId);
            if (detail == null)
            {
                continue;
            }

            if (!string.IsNullOrWhiteSpace(status) && !string.Equals(detail.StatusKey, status, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            results.Add(ToSummary(detail));
        }

        return Ok(ApiResponseDto<List<AdminAssignmentSummaryDto>>.Ok(results, "Lay danh sach bai tap admin thanh cong."));
    }

    [HttpGet("assignments/{assignmentId:guid}")]
    public async Task<IActionResult> GetAssignmentDetail(Guid assignmentId)
    {
        var detail = await BuildAssignmentDetailAsync(assignmentId);
        if (detail == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Khong tim thay bai tap.", "ADMIN_ASSIGNMENT_NOT_FOUND"));
        }

        return Ok(ApiResponseDto<AdminAssignmentDetailDto>.Ok(detail, "Lay chi tiet bai tap thanh cong."));
    }

    [HttpPost("assignments")]
    public async Task<IActionResult> CreateAssignment([FromBody] AdminAssignmentUpsertRequestDto request)
    {
        var result = await UpsertAssignmentAsync(null, request);
        if (result.ErrorResult != null)
        {
            return result.ErrorResult;
        }

        return Ok(ApiResponseDto<AdminAssignmentDetailDto>.Ok(result.Detail!, "Tao bai tap thanh cong."));
    }

    [HttpPut("assignments/{assignmentId:guid}")]
    public async Task<IActionResult> UpdateAssignment(Guid assignmentId, [FromBody] AdminAssignmentUpsertRequestDto request)
    {
        var result = await UpsertAssignmentAsync(assignmentId, request);
        if (result.ErrorResult != null)
        {
            return result.ErrorResult;
        }

        return Ok(ApiResponseDto<AdminAssignmentDetailDto>.Ok(result.Detail!, "Cap nhat bai tap thanh cong."));
    }

    [HttpDelete("assignments/{assignmentId:guid}")]
    public async Task<IActionResult> DeleteAssignment(Guid assignmentId)
    {
        var userId = User.GetUserId();
        if (!userId.HasValue)
        {
            return Unauthorized(ApiResponseDto<object>.Fail("Khong xac dinh duoc nguoi dung hien tai.", "ADMIN_ASSIGNMENT_USER_INVALID"));
        }

        var assignment = await _context.Baitaps
            .Include(x => x.Baitapcauhois)
            .Include(x => x.Sukienlophocs)
                .ThenInclude(sk => sk.Nguoinhansukiens)
            .FirstOrDefaultAsync(x => x.MaBaiTap == assignmentId && x.DaXoa != true && x.TrangThai != false);

        if (assignment == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Khong tim thay bai tap.", "ADMIN_ASSIGNMENT_NOT_FOUND"));
        }

        var now = DateTime.Now;
        assignment.DaXoa = true;
        assignment.TrangThai = false;
        assignment.NguoiSua = userId.Value;
        assignment.ThoiGianSua = now;

        foreach (var question in assignment.Baitapcauhois)
        {
            question.DaXoa = true;
            question.TrangThai = false;
            question.NguoiSua = userId.Value;
            question.ThoiGianSua = now;
        }

        foreach (var eventRow in assignment.Sukienlophocs)
        {
            eventRow.DaXoa = true;
            eventRow.TrangThai = false;
            eventRow.NguoiSua = userId.Value;
            eventRow.ThoiGianSua = now;

            foreach (var receiver in eventRow.Nguoinhansukiens)
            {
                receiver.DaXoa = true;
                receiver.TrangThai = false;
                receiver.NguoiSua = userId.Value;
                receiver.ThoiGianSua = now;
            }
        }

        await _context.SaveChangesAsync();
        return Ok(ApiResponseDto<object>.Ok(new { assignmentId }, "Xoa mem bai tap thanh cong."));
    }

    private async Task<UpsertResult> UpsertAssignmentAsync(Guid? assignmentId, AdminAssignmentUpsertRequestDto request)
    {
        var userId = User.GetUserId();
        if (!userId.HasValue)
        {
            return UpsertResult.Fail(Unauthorized(ApiResponseDto<object>.Fail("Khong xac dinh duoc nguoi dung hien tai.", "ADMIN_ASSIGNMENT_USER_INVALID")));
        }

        var validationError = await ValidateRequestAsync(request);
        if (validationError != null)
        {
            return UpsertResult.Fail(validationError);
        }

        var assignmentTypeId = await ResolveCategoryIdAsync("LOAI_BAI_TAP", request.LoaiBaiTapCode);
        var publishModeId = await ResolveCategoryIdAsync("TRANG_THAI_SU_KIEN", request.PublishMode);
        var eventTypeId = await ResolveCategoryIdAsync("DANG_SU_KIEN", ResolveEventTypeCode(request.LoaiBaiTapCode));

        if (!assignmentTypeId.HasValue || !publishModeId.HasValue || !eventTypeId.HasValue)
        {
            return UpsertResult.Fail(BadRequest(ApiResponseDto<object>.Fail("Thieu danh muc cau hinh bai tap.", "ADMIN_ASSIGNMENT_CATEGORY_MISSING")));
        }

        var assignment = assignmentId.HasValue
            ? await _context.Baitaps
                .Include(x => x.Baitapcauhois)
                .Include(x => x.Sukienlophocs)
                    .ThenInclude(sk => sk.Nguoinhansukiens)
                .FirstOrDefaultAsync(x => x.MaBaiTap == assignmentId.Value && x.DaXoa != true && x.TrangThai != false)
            : null;

        if (assignmentId.HasValue && assignment == null)
        {
            return UpsertResult.Fail(NotFound(ApiResponseDto<object>.Fail("Khong tim thay bai tap.", "ADMIN_ASSIGNMENT_NOT_FOUND")));
        }

        var now = DateTime.Now;
        assignment ??= new Baitap
        {
            MaBaiTap = Guid.NewGuid(),
            NguoiTao = userId.Value,
            ThoiGianTao = now,
            TrangThai = true,
            DaXoa = false
        };

        var resolvedCode = string.IsNullOrWhiteSpace(request.Code)
            ? await GenerateAssignmentCodeAsync(request.LoaiBaiTapCode)
            : request.Code.Trim();

        assignment.MaKhoaHoc = request.MaKhoaHoc;
        assignment.TenBaiTap = request.TenBaiTap.Trim();
        assignment.LoaiBaiTap = assignmentTypeId.Value;
        assignment.ThoiGianLamBai = NormalizeDuration(request.LoaiBaiTapCode, request.ThoiGianLamBai);
        assignment.DiemToiDa = request.DiemToiDa;
        assignment.NguoiSua = userId.Value;
        assignment.ThoiGianSua = now;
        assignment.TrangThai = true;
        assignment.DaXoa = false;
        assignment.MoTa = SerializeMetadata(new AssignmentMetadataEnvelope
        {
            Code = resolvedCode,
            Description = NormalizeOptional(request.MoTa),
            PublishMode = request.PublishMode,
            OpenAt = request.OpenAt,
            DueAt = ResolveDueAt(request),
            MaxAttempts = request.MaxAttempts,
            AllowLateSubmission = request.AllowLateSubmission,
            ShowScoreWhenDone = request.ShowScoreWhenDone,
            ShowAnswerAfterDeadline = request.ShowAnswerAfterDeadline,
            ShuffleQuestions = request.ShuffleQuestions,
            ShuffleAnswers = request.ShuffleAnswers,
            AutoGradeObjective = request.AutoGradeObjective,
            RequireManualReview = request.RequireManualReview,
            AccessCode = NormalizeOptional(request.AccessCode),
            ScoreMode = request.ScoreMode
        });

        if (!assignmentId.HasValue)
        {
            _context.Baitaps.Add(assignment);
        }

        SyncQuestionSelections(assignment, request, userId.Value, now);
        await SyncClassEventsAsync(assignment, request, userId.Value, now, publishModeId.Value, eventTypeId.Value);

        await _context.SaveChangesAsync();

        var detail = await BuildAssignmentDetailAsync(assignment.MaBaiTap);
        return UpsertResult.Success(detail!);
    }

    private async Task<IActionResult?> ValidateRequestAsync(AdminAssignmentUpsertRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.TenBaiTap))
        {
            return BadRequest(ApiResponseDto<object>.Fail("Ten bai tap khong duoc de trong.", "ADMIN_ASSIGNMENT_TITLE_REQUIRED"));
        }

        if (request.DiemToiDa <= 0)
        {
            return BadRequest(ApiResponseDto<object>.Fail("Diem toi da phai lon hon 0.", "ADMIN_ASSIGNMENT_SCORE_INVALID"));
        }

        if (!string.Equals(request.LoaiBaiTapCode, "HOMEWORK", StringComparison.OrdinalIgnoreCase) &&
            (!request.ThoiGianLamBai.HasValue || request.ThoiGianLamBai.Value <= 0))
        {
            return BadRequest(ApiResponseDto<object>.Fail("Thoi gian lam bai phai lon hon 0.", "ADMIN_ASSIGNMENT_DURATION_INVALID"));
        }

        if (request.OpenAt.HasValue && request.DueAt.HasValue && request.DueAt.Value <= request.OpenAt.Value)
        {
            return BadRequest(ApiResponseDto<object>.Fail("Han nop phai sau gio mo bai.", "ADMIN_ASSIGNMENT_DUE_DATE_INVALID"));
        }

        var courseExists = await _context.Khoahocs
            .AnyAsync(x => x.MaKhoaHoc == request.MaKhoaHoc && x.DaXoa != true && x.TrangThai != false);

        if (!courseExists)
        {
            return NotFound(ApiResponseDto<object>.Fail("Khong tim thay khoa hoc.", "ADMIN_ASSIGNMENT_COURSE_NOT_FOUND"));
        }

        var classIds = request.SelectedClassIds
            .Where(x => x != Guid.Empty)
            .Distinct()
            .ToList();

        if (classIds.Count > 0)
        {
            var matchedClassIds = await _context.ChitietkhoahocLophocs
                .AsNoTracking()
                .Where(x =>
                    x.MaKhoaHoc == request.MaKhoaHoc &&
                    classIds.Contains(x.MaLopHoc) &&
                    x.DaXoa != true &&
                    x.TrangThai != false)
                .Select(x => x.MaLopHoc)
                .ToListAsync();

            if (matchedClassIds.Count != classIds.Count)
            {
                return BadRequest(ApiResponseDto<object>.Fail("Co lop hoc khong thuoc khoa hoc da chon.", "ADMIN_ASSIGNMENT_CLASS_INVALID"));
            }
        }

        var questionIds = request.SelectedQuestions
            .Select(x => x.MaCauHoi)
            .Where(x => x != Guid.Empty)
            .Distinct()
            .ToList();

        if (questionIds.Count > 0)
        {
            var matchedQuestionIds = await _context.Nganhangcauhois
                .AsNoTracking()
                .Where(x =>
                    x.MaKhoaHoc == request.MaKhoaHoc &&
                    questionIds.Contains(x.MaCauHoi) &&
                    x.DaXoa != true &&
                    x.TrangThai != false)
                .Select(x => x.MaCauHoi)
                .ToListAsync();

            if (matchedQuestionIds.Count != questionIds.Count)
            {
                return BadRequest(ApiResponseDto<object>.Fail("Co cau hoi khong thuoc khoa hoc da chon.", "ADMIN_ASSIGNMENT_QUESTION_INVALID"));
            }
        }

        return null;
    }

    private void SyncQuestionSelections(Baitap assignment, AdminAssignmentUpsertRequestDto request, Guid userId, DateTime now)
    {
        var requestedMap = request.SelectedQuestions
            .Where(x => x.MaCauHoi != Guid.Empty)
            .GroupBy(x => x.MaCauHoi)
            .ToDictionary(
                x => x.Key,
                x =>
                {
                    var item = x.First();
                    return new
                    {
                        ThuTu = item.ThuTu ?? 0,
                        Diem = item.DiemCuaCau ?? 0m
                    };
                });

        foreach (var existing in assignment.Baitapcauhois)
        {
            if (!requestedMap.ContainsKey(existing.MaCauHoi))
            {
                existing.DaXoa = true;
                existing.TrangThai = false;
                existing.NguoiSua = userId;
                existing.ThoiGianSua = now;
                continue;
            }

            var requestItem = requestedMap[existing.MaCauHoi];
            existing.ThuTu = requestItem.ThuTu;
            existing.DiemCuaCau = requestItem.Diem;
            existing.DaXoa = false;
            existing.TrangThai = true;
            existing.NguoiSua = userId;
            existing.ThoiGianSua = now;
        }

        var existingQuestionIds = assignment.Baitapcauhois
            .Select(x => x.MaCauHoi)
            .ToHashSet();

        foreach (var requestItem in requestedMap)
        {
            if (existingQuestionIds.Contains(requestItem.Key))
            {
                continue;
            }

            assignment.Baitapcauhois.Add(new Baitapcauhoi
            {
                MaBaiTap = assignment.MaBaiTap,
                MaCauHoi = requestItem.Key,
                ThuTu = requestItem.Value.ThuTu,
                DiemCuaCau = requestItem.Value.Diem,
                NguoiTao = userId,
                ThoiGianTao = now,
                TrangThai = true,
                DaXoa = false
            });
        }
    }

    private async Task SyncClassEventsAsync(
        Baitap assignment,
        AdminAssignmentUpsertRequestDto request,
        Guid userId,
        DateTime now,
        int publishModeId,
        int eventTypeId)
    {
        var dueAt = ResolveDueAt(request);
        var requestedClassIds = request.SelectedClassIds
            .Where(x => x != Guid.Empty)
            .Distinct()
            .ToList();

        foreach (var existingEvent in assignment.Sukienlophocs)
        {
            if (!requestedClassIds.Contains(existingEvent.MaLopHoc))
            {
                existingEvent.DaXoa = true;
                existingEvent.TrangThai = false;
                existingEvent.NguoiSua = userId;
                existingEvent.ThoiGianSua = now;

                foreach (var receiver in existingEvent.Nguoinhansukiens)
                {
                    receiver.DaXoa = true;
                    receiver.TrangThai = false;
                    receiver.NguoiSua = userId;
                    receiver.ThoiGianSua = now;
                }
            }
        }

        var activeStudentsByClass = await LoadActiveStudentsByClassAsync(requestedClassIds);
        var existingEventsByClass = assignment.Sukienlophocs.ToDictionary(x => x.MaLopHoc, x => x);

        foreach (var classId in requestedClassIds)
        {
            if (!existingEventsByClass.TryGetValue(classId, out var eventRow))
            {
                eventRow = new Sukienlophoc
                {
                    MaSuKien = Guid.NewGuid(),
                    MaLopHoc = classId,
                    MaNguoiDung = userId,
                    MaBaiTap = assignment.MaBaiTap,
                    NguoiTao = userId,
                    ThoiGianTao = now,
                    TrangThai = true,
                    DaXoa = false
                };
                assignment.Sukienlophocs.Add(eventRow);
            }

            eventRow.MaNguoiDung = userId;
            eventRow.DangSuKien = eventTypeId;
            eventRow.HanNop = dueAt;
            eventRow.MaTrangThai = publishModeId;
            eventRow.NoiDung = BuildEventContent(request);
            eventRow.NguoiSua = userId;
            eventRow.ThoiGianSua = now;
            eventRow.TrangThai = true;
            eventRow.DaXoa = false;

            SyncEventReceivers(
                eventRow,
                activeStudentsByClass.TryGetValue(classId, out var studentIds) ? studentIds : [],
                userId,
                now);
        }
    }

    private void SyncEventReceivers(Sukienlophoc eventRow, IReadOnlyCollection<Guid> studentIds, Guid userId, DateTime now)
    {
        var studentSet = studentIds.ToHashSet();
        foreach (var receiver in eventRow.Nguoinhansukiens)
        {
            if (!studentSet.Contains(receiver.MaHocSinh))
            {
                receiver.DaXoa = true;
                receiver.TrangThai = false;
                receiver.NguoiSua = userId;
                receiver.ThoiGianSua = now;
                continue;
            }

            receiver.DaXoa = false;
            receiver.TrangThai = true;
            receiver.NguoiSua = userId;
            receiver.ThoiGianSua = now;
        }

        var existingStudentIds = eventRow.Nguoinhansukiens
            .Select(x => x.MaHocSinh)
            .ToHashSet();

        foreach (var studentId in studentSet)
        {
            if (existingStudentIds.Contains(studentId))
            {
                continue;
            }

            eventRow.Nguoinhansukiens.Add(new Nguoinhansukien
            {
                MaSuKien = eventRow.MaSuKien,
                MaHocSinh = studentId,
                NguoiTao = userId,
                ThoiGianTao = now,
                TrangThai = true,
                DaXoa = false
            });
        }
    }

    private async Task<Dictionary<Guid, List<Guid>>> LoadActiveStudentsByClassAsync(List<Guid> classIds)
    {
        if (classIds.Count == 0)
        {
            return new Dictionary<Guid, List<Guid>>();
        }

        var today = DateOnly.FromDateTime(DateTime.Today);
        var rows = await _context.Hocsinhlophocs
            .AsNoTracking()
            .Where(x =>
                classIds.Contains(x.MaLopHoc) &&
                x.DaXoa != true &&
                x.TrangThai != false &&
                (!x.NgayThamGia.HasValue || x.NgayThamGia.Value <= today) &&
                (!x.NgayRoiLop.HasValue || x.NgayRoiLop.Value >= today))
            .Select(x => new { x.MaLopHoc, x.MaHocSinh })
            .ToListAsync();

        return rows
            .GroupBy(x => x.MaLopHoc)
            .ToDictionary(x => x.Key, x => x.Select(item => item.MaHocSinh).Distinct().ToList());
    }

    private async Task<AdminAssignmentDetailDto?> BuildAssignmentDetailAsync(Guid assignmentId)
    {
        var assignment = await _context.Baitaps
            .AsNoTracking()
            .Include(x => x.MaKhoaHocNavigation)
            .Include(x => x.LoaiBaiTapNavigation)
            .FirstOrDefaultAsync(x => x.MaBaiTap == assignmentId && x.DaXoa != true && x.TrangThai != false);

        if (assignment == null)
        {
            return null;
        }

        var metadata = ParseMetadata(assignment.MoTa);

        var questionRows = await _context.Baitapcauhois
            .AsNoTracking()
            .Where(x => x.MaBaiTap == assignmentId && x.DaXoa != true && x.TrangThai != false)
            .Include(x => x.MaCauHoiNavigation)
                .ThenInclude(q => q.LoaiCauHoiNavigation)
            .Include(x => x.MaCauHoiNavigation)
                .ThenInclude(q => q.MucDoNavigation)
            .OrderBy(x => x.ThuTu ?? int.MaxValue)
            .ThenBy(x => x.MaCauHoiNavigation.NoiDungCauHoi)
            .ToListAsync();

        var eventRows = await _context.Sukienlophocs
            .AsNoTracking()
            .Where(x => x.MaBaiTap == assignmentId && x.DaXoa != true && x.TrangThai != false)
            .Select(x => new
            {
                x.MaSuKien,
                x.MaLopHoc,
                x.HanNop,
                ReceiverCount = x.Nguoinhansukiens.Count(r => r.DaXoa != true && r.TrangThai != false)
            })
            .ToListAsync();

        var selectedClassIds = eventRows
            .Select(x => x.MaLopHoc)
            .Distinct()
            .ToList();

        var totalStudents = eventRows.Sum(x => x.ReceiverCount);
        if (totalStudents == 0 && selectedClassIds.Count > 0)
        {
            var fallbackStudents = await LoadActiveStudentsByClassAsync(selectedClassIds);
            totalStudents = fallbackStudents.Sum(x => x.Value.Count);
        }

        var eventIds = eventRows.Select(x => x.MaSuKien).ToList();
        var submissions = eventIds.Count == 0
            ? []
            : await _context.Nopbais
                .AsNoTracking()
                .Where(x => eventIds.Contains(x.MaSuKien) && x.DaXoa != true && x.TrangThai != false)
                .Select(x => new
                {
                    x.MaSuKien,
                    x.MaHocSinh,
                    x.ThoiGianNop,
                    x.DiemSo
                })
                .ToListAsync();

        var dueAtLookup = eventRows.ToDictionary(x => x.MaSuKien, x => x.HanNop);
        var submitted = submissions.Select(x => x.MaHocSinh).Distinct().Count();
        var lateSubmitted = submissions
            .Where(x =>
                x.ThoiGianNop.HasValue &&
                dueAtLookup.TryGetValue(x.MaSuKien, out var dueAt) &&
                dueAt.HasValue &&
                x.ThoiGianNop.Value > dueAt.Value)
            .Select(x => x.MaHocSinh)
            .Distinct()
            .Count();

        var requiresManualReview = metadata.RequireManualReview || questionRows.Any(x => IsEssayTypeCode(x.MaCauHoiNavigation.LoaiCauHoiNavigation?.MaCode));
        var needsGrading = requiresManualReview
            ? submissions.Where(x => x.DiemSo == null).Select(x => x.MaHocSinh).Distinct().Count()
            : 0;

        var typeCode = assignment.LoaiBaiTapNavigation?.MaCode ?? string.Empty;
        var detail = new AdminAssignmentDetailDto
        {
            MaBaiTap = assignment.MaBaiTap,
            Code = metadata.Code ?? BuildFallbackAssignmentCode(typeCode, assignment.MaBaiTap),
            TenBaiTap = assignment.TenBaiTap,
            MoTa = metadata.Description,
            MaKhoaHoc = assignment.MaKhoaHoc,
            TenKhoaHoc = assignment.MaKhoaHocNavigation.TenKhoaHoc,
            LoaiBaiTapCode = typeCode,
            LoaiBaiTapLabel = assignment.LoaiBaiTapNavigation?.TenChiTiet ?? string.Empty,
            PublishMode = metadata.PublishMode ?? "DRAFT",
            StatusKey = ComputeStatusKey(metadata.PublishMode, selectedClassIds.Count, questionRows.Count, metadata.OpenAt, metadata.DueAt),
            ThoiGianLamBai = assignment.ThoiGianLamBai,
            DiemToiDa = assignment.DiemToiDa ?? 0m,
            MaxAttempts = metadata.MaxAttempts,
            OpenAt = metadata.OpenAt,
            DueAt = metadata.DueAt,
            TotalQuestions = questionRows.Count,
            TotalClasses = selectedClassIds.Count,
            TotalStudents = totalStudents,
            UpdatedAt = assignment.ThoiGianSua ?? assignment.ThoiGianTao,
            AllowLateSubmission = metadata.AllowLateSubmission,
            ShowScoreWhenDone = metadata.ShowScoreWhenDone,
            ShowAnswerAfterDeadline = metadata.ShowAnswerAfterDeadline,
            ShuffleQuestions = metadata.ShuffleQuestions,
            ShuffleAnswers = metadata.ShuffleAnswers,
            AutoGradeObjective = metadata.AutoGradeObjective,
            RequireManualReview = metadata.RequireManualReview,
            AccessCode = metadata.AccessCode,
            ScoreMode = metadata.ScoreMode,
            SelectedClassIds = selectedClassIds,
            SelectedQuestions = questionRows.Select(x => new AdminAssignmentQuestionSelectionDto
            {
                MaCauHoi = x.MaCauHoi,
                MaKhoaHoc = x.MaCauHoiNavigation.MaKhoaHoc,
                NoiDungCauHoi = x.MaCauHoiNavigation.NoiDungCauHoi,
                LoaiCauHoiCode = x.MaCauHoiNavigation.LoaiCauHoiNavigation?.MaCode ?? string.Empty,
                LoaiCauHoiLabel = x.MaCauHoiNavigation.LoaiCauHoiNavigation?.TenChiTiet ?? string.Empty,
                MucDoCode = x.MaCauHoiNavigation.MucDoNavigation?.MaCode ?? string.Empty,
                MucDoLabel = x.MaCauHoiNavigation.MucDoNavigation?.TenChiTiet ?? string.Empty,
                LaCauHoiTuLuan = IsEssayTypeCode(x.MaCauHoiNavigation.LoaiCauHoiNavigation?.MaCode),
                ThuTu = x.ThuTu,
                DiemCuaCau = x.DiemCuaCau ?? 0m
            }).ToList(),
            SubmissionStats = new AdminAssignmentSubmissionStatsDto
            {
                TotalStudents = totalStudents,
                Submitted = submitted,
                LateSubmitted = lateSubmitted,
                NeedsGrading = needsGrading
            }
        };

        return detail;
    }

    private static AdminAssignmentSummaryDto ToSummary(AdminAssignmentDetailDto detail)
    {
        return new AdminAssignmentSummaryDto
        {
            MaBaiTap = detail.MaBaiTap,
            Code = detail.Code,
            TenBaiTap = detail.TenBaiTap,
            MoTa = detail.MoTa,
            MaKhoaHoc = detail.MaKhoaHoc,
            TenKhoaHoc = detail.TenKhoaHoc,
            LoaiBaiTapCode = detail.LoaiBaiTapCode,
            LoaiBaiTapLabel = detail.LoaiBaiTapLabel,
            PublishMode = detail.PublishMode,
            StatusKey = detail.StatusKey,
            ThoiGianLamBai = detail.ThoiGianLamBai,
            DiemToiDa = detail.DiemToiDa,
            MaxAttempts = detail.MaxAttempts,
            OpenAt = detail.OpenAt,
            DueAt = detail.DueAt,
            TotalQuestions = detail.TotalQuestions,
            TotalClasses = detail.TotalClasses,
            TotalStudents = detail.TotalStudents,
            UpdatedAt = detail.UpdatedAt,
            SubmissionStats = detail.SubmissionStats
        };
    }

    private async Task<int?> ResolveCategoryIdAsync(string groupCode, string detailCode)
    {
        if (string.IsNullOrWhiteSpace(groupCode) || string.IsNullOrWhiteSpace(detailCode))
        {
            return null;
        }

        return await _context.Chitietdanhmucs
            .AsNoTracking()
            .Where(x =>
                x.DaXoa != true &&
                x.TrangThai != false &&
                x.MaCode == detailCode &&
                x.MaNhomNavigation.DaXoa != true &&
                x.MaNhomNavigation.TrangThai != false &&
                x.MaNhomNavigation.MaCode == groupCode)
            .Select(x => (int?)x.MaChiTiet)
            .FirstOrDefaultAsync();
    }

    private async Task<string> GenerateAssignmentCodeAsync(string assignmentTypeCode)
    {
        var prefix = assignmentTypeCode switch
        {
            "EXAM" => "EX",
            "QUIZ" => "KT",
            _ => "BT"
        };

        var count = await _context.Baitaps
            .CountAsync(x =>
                x.DaXoa != true &&
                x.TrangThai != false &&
                x.LoaiBaiTapNavigation != null &&
                x.LoaiBaiTapNavigation.MaCode == assignmentTypeCode);

        return $"{prefix}-{(count + 1):000}";
    }

    private static AssignmentMetadataEnvelope ParseMetadata(string? rawValue)
    {
        if (string.IsNullOrWhiteSpace(rawValue))
        {
            return new AssignmentMetadataEnvelope();
        }

        try
        {
            var envelope = JsonSerializer.Deserialize<AssignmentMetadataEnvelope>(rawValue, JsonOptions);
            if (envelope != null && string.Equals(envelope.Schema, AssignmentMetadataSchema, StringComparison.OrdinalIgnoreCase))
            {
                return envelope;
            }
        }
        catch
        {
            // Legacy plain-text description is supported below.
        }

        return new AssignmentMetadataEnvelope
        {
            Description = rawValue
        };
    }

    private static string SerializeMetadata(AssignmentMetadataEnvelope envelope)
    {
        envelope.Schema = AssignmentMetadataSchema;
        return JsonSerializer.Serialize(envelope, JsonOptions);
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private static int NormalizeDuration(string assignmentTypeCode, int? durationMinutes)
    {
        return string.Equals(assignmentTypeCode, "HOMEWORK", StringComparison.OrdinalIgnoreCase)
            ? 0
            : Math.Max(0, durationMinutes ?? 0);
    }

    private static DateTime? ResolveDueAt(AdminAssignmentUpsertRequestDto request)
    {
        if (request.DueAt.HasValue)
        {
            return request.DueAt.Value;
        }

        if (string.Equals(request.LoaiBaiTapCode, "HOMEWORK", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        if (request.OpenAt.HasValue && request.ThoiGianLamBai.HasValue && request.ThoiGianLamBai.Value > 0)
        {
            return request.OpenAt.Value.AddMinutes(request.ThoiGianLamBai.Value);
        }

        return null;
    }

    private static string ResolveEventTypeCode(string assignmentTypeCode)
    {
        return assignmentTypeCode switch
        {
            "EXAM" => "TO_CHUC_THI",
            "QUIZ" => "TO_CHUC_KIEM_TRA",
            _ => "GIAO_BAI"
        };
    }

    private static string ComputeStatusKey(string? publishMode, int classCount, int questionCount, DateTime? openAt, DateTime? dueAt)
    {
        if (string.Equals(publishMode, "DRAFT", StringComparison.OrdinalIgnoreCase))
        {
            return "DRAFT";
        }

        if (classCount == 0 || questionCount == 0)
        {
            return "NEEDS_SETUP";
        }

        var now = DateTime.Now;
        if (openAt.HasValue && now < openAt.Value)
        {
            return "SCHEDULED";
        }

        if (dueAt.HasValue && now > dueAt.Value)
        {
            return "CLOSED";
        }

        return "ACTIVE";
    }

    private static bool IsEssayTypeCode(string? questionTypeCode)
    {
        return string.Equals(questionTypeCode, "LCH_TRANSFORM", StringComparison.OrdinalIgnoreCase);
    }

    private static string BuildFallbackAssignmentCode(string assignmentTypeCode, Guid assignmentId)
    {
        var prefix = assignmentTypeCode switch
        {
            "EXAM" => "EX",
            "QUIZ" => "KT",
            _ => "BT"
        };

        return $"{prefix}-{assignmentId.ToString("N")[..6].ToUpperInvariant()}";
    }

    private static string BuildEventContent(AdminAssignmentUpsertRequestDto request)
    {
        var title = string.IsNullOrWhiteSpace(request.TenBaiTap) ? "Bai tap" : request.TenBaiTap.Trim();
        return $"{title} [{request.LoaiBaiTapCode}]";
    }

    private static AdminAssignmentOptionDto MapOption(dynamic row)
    {
        return new AdminAssignmentOptionDto
        {
            Code = row.Code,
            Label = row.Label,
            Order = row.ThuTu
        };
    }

    private sealed class AssignmentMetadataEnvelope
    {
        public string Schema { get; set; } = AssignmentMetadataSchema;
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? PublishMode { get; set; } = "DRAFT";
        public DateTime? OpenAt { get; set; }
        public DateTime? DueAt { get; set; }
        public int MaxAttempts { get; set; } = 1;
        public bool AllowLateSubmission { get; set; }
        public bool ShowScoreWhenDone { get; set; }
        public bool ShowAnswerAfterDeadline { get; set; }
        public bool ShuffleQuestions { get; set; } = true;
        public bool ShuffleAnswers { get; set; } = true;
        public bool AutoGradeObjective { get; set; } = true;
        public bool RequireManualReview { get; set; } = true;
        public string? AccessCode { get; set; }
        public string ScoreMode { get; set; } = "AUTO_EQUAL";
    }

    private sealed class UpsertResult
    {
        public IActionResult? ErrorResult { get; private init; }
        public AdminAssignmentDetailDto? Detail { get; private init; }

        public static UpsertResult Fail(IActionResult errorResult) => new() { ErrorResult = errorResult };

        public static UpsertResult Success(AdminAssignmentDetailDto detail) => new() { Detail = detail };
    }
}
