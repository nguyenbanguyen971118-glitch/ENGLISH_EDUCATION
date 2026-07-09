namespace backend.DTOs;

public class AdminAssignmentOptionDto
{
    public string Code { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public int? Order { get; set; }
}

public class AdminAssignmentCourseOptionDto
{
    public Guid MaKhoaHoc { get; set; }
    public string TenKhoaHoc { get; set; } = string.Empty;
    public int SoLopHoc { get; set; }
    public int SoBaiTap { get; set; }
    public int SoCauHoi { get; set; }
}

public class AdminAssignmentClassOptionDto
{
    public Guid MaLopHoc { get; set; }
    public Guid MaKhoaHoc { get; set; }
    public string TenLop { get; set; } = string.Empty;
    public string? TenGiangVien { get; set; }
    public int SoHocVien { get; set; }
}

public class AdminAssignmentQuestionBankItemDto
{
    public Guid MaCauHoi { get; set; }
    public Guid MaKhoaHoc { get; set; }
    public string TenKhoaHoc { get; set; } = string.Empty;
    public string LoaiCauHoiCode { get; set; } = string.Empty;
    public string LoaiCauHoiLabel { get; set; } = string.Empty;
    public string MucDoCode { get; set; } = string.Empty;
    public string MucDoLabel { get; set; } = string.Empty;
    public string NoiDungCauHoi { get; set; } = string.Empty;
    public string? GiaiThichDapAn { get; set; }
    public string? AmThanhLink { get; set; }
    public string? HinhAnhLink { get; set; }
    public Guid? MaCauHoiCha { get; set; }
    public int? ThuTu { get; set; }
    public bool LaCauHoiTuLuan { get; set; }
    public int SoDapAnLuaChon { get; set; }
    public int SoDapAnNhapLieu { get; set; }
    public DateTime? ThoiGianTao { get; set; }
}

public class AdminAssignmentQuestionChoiceAnswerDto
{
    public Guid MaDapAn { get; set; }
    public string? TenDapAn { get; set; }
    public string NoiDungDapAn { get; set; } = string.Empty;
    public bool LaDapAnDung { get; set; }
    public string? GiaTriDoiChieu { get; set; }
    public int? ThuTu { get; set; }
}

public class AdminAssignmentQuestionTextAnswerDto
{
    public Guid MaDapAnDien { get; set; }
    public string DapAnChuan { get; set; } = string.Empty;
    public string? DapAnThayThe { get; set; }
    public bool PhanBietHoaThuong { get; set; }
}

public class AdminAssignmentQuestionBankDetailDto : AdminAssignmentQuestionBankItemDto
{
    public string? NoiDungCauHoiCha { get; set; }
    public List<AdminAssignmentQuestionChoiceAnswerDto> ChoiceAnswers { get; set; } = new();
    public List<AdminAssignmentQuestionTextAnswerDto> TextAnswers { get; set; } = new();
}

public class AdminAssignmentSubmissionStatsDto
{
    public int TotalStudents { get; set; }
    public int Submitted { get; set; }
    public int LateSubmitted { get; set; }
    public int NeedsGrading { get; set; }
}

public class AdminAssignmentQuestionSelectionDto
{
    public Guid MaCauHoi { get; set; }
    public Guid MaKhoaHoc { get; set; }
    public string NoiDungCauHoi { get; set; } = string.Empty;
    public string LoaiCauHoiCode { get; set; } = string.Empty;
    public string LoaiCauHoiLabel { get; set; } = string.Empty;
    public string MucDoCode { get; set; } = string.Empty;
    public string MucDoLabel { get; set; } = string.Empty;
    public bool LaCauHoiTuLuan { get; set; }
    public int? ThuTu { get; set; }
    public decimal DiemCuaCau { get; set; }
}

public class AdminAssignmentSummaryDto
{
    public Guid MaBaiTap { get; set; }
    public string Code { get; set; } = string.Empty;
    public string TenBaiTap { get; set; } = string.Empty;
    public string? MoTa { get; set; }
    public Guid MaKhoaHoc { get; set; }
    public string TenKhoaHoc { get; set; } = string.Empty;
    public string LoaiBaiTapCode { get; set; } = string.Empty;
    public string LoaiBaiTapLabel { get; set; } = string.Empty;
    public string PublishMode { get; set; } = string.Empty;
    public string StatusKey { get; set; } = string.Empty;
    public int? ThoiGianLamBai { get; set; }
    public decimal DiemToiDa { get; set; }
    public int MaxAttempts { get; set; }
    public DateTime? OpenAt { get; set; }
    public DateTime? DueAt { get; set; }
    public int TotalQuestions { get; set; }
    public int TotalClasses { get; set; }
    public int TotalStudents { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public AdminAssignmentSubmissionStatsDto SubmissionStats { get; set; } = new();
}

public class AdminAssignmentDetailDto : AdminAssignmentSummaryDto
{
    public bool AllowLateSubmission { get; set; }
    public bool ShowScoreWhenDone { get; set; }
    public bool ShowAnswerAfterDeadline { get; set; }
    public bool ShuffleQuestions { get; set; }
    public bool ShuffleAnswers { get; set; }
    public bool AutoGradeObjective { get; set; }
    public bool RequireManualReview { get; set; }
    public string? AccessCode { get; set; }
    public string ScoreMode { get; set; } = string.Empty;
    public List<Guid> SelectedClassIds { get; set; } = new();
    public List<AdminAssignmentQuestionSelectionDto> SelectedQuestions { get; set; } = new();
}

public class AdminAssignmentBootstrapDto
{
    public List<AdminAssignmentCourseOptionDto> Courses { get; set; } = new();
    public List<AdminAssignmentClassOptionDto> Classes { get; set; } = new();
    public List<AdminAssignmentOptionDto> AssignmentTypes { get; set; } = new();
    public List<AdminAssignmentOptionDto> PublishModes { get; set; } = new();
    public List<AdminAssignmentOptionDto> QuestionTypes { get; set; } = new();
    public List<AdminAssignmentOptionDto> DifficultyLevels { get; set; } = new();
}

public class AdminAssignmentQuestionSelectionRequestDto
{
    public Guid MaCauHoi { get; set; }
    public int? ThuTu { get; set; }
    public decimal? DiemCuaCau { get; set; }
}

public class AdminAssignmentQuestionChoiceAnswerUpsertRequestDto
{
    public string? TenDapAn { get; set; }
    public string NoiDungDapAn { get; set; } = string.Empty;
    public bool LaDapAnDung { get; set; }
    public string? GiaTriDoiChieu { get; set; }
    public int? ThuTu { get; set; }
}

public class AdminAssignmentQuestionTextAnswerUpsertRequestDto
{
    public string DapAnChuan { get; set; } = string.Empty;
    public string? DapAnThayThe { get; set; }
    public bool PhanBietHoaThuong { get; set; }
}

public class AdminAssignmentQuestionBankUpsertRequestDto
{
    public Guid MaKhoaHoc { get; set; }
    public string LoaiCauHoiCode { get; set; } = string.Empty;
    public string MucDoCode { get; set; } = string.Empty;
    public string NoiDungCauHoi { get; set; } = string.Empty;
    public string? GiaiThichDapAn { get; set; }
    public string? AmThanhLink { get; set; }
    public string? HinhAnhLink { get; set; }
    public Guid? MaCauHoiCha { get; set; }
    public int? ThuTu { get; set; }
    public List<AdminAssignmentQuestionChoiceAnswerUpsertRequestDto> ChoiceAnswers { get; set; } = new();
    public List<AdminAssignmentQuestionTextAnswerUpsertRequestDto> TextAnswers { get; set; } = new();
}

public class AdminAssignmentQuestionBankImportRequestDto
{
    public List<AdminAssignmentQuestionBankUpsertRequestDto> Questions { get; set; } = new();
}

public class AdminAssignmentQuestionBankImportResultDto
{
    public int ImportedCount { get; set; }
    public List<AdminAssignmentQuestionBankDetailDto> ImportedQuestions { get; set; } = new();
}

public class AdminAssignmentUpsertRequestDto
{
    public string? Code { get; set; }
    public string TenBaiTap { get; set; } = string.Empty;
    public string? MoTa { get; set; }
    public Guid MaKhoaHoc { get; set; }
    public string LoaiBaiTapCode { get; set; } = "HOMEWORK";
    public string PublishMode { get; set; } = "DRAFT";
    public int? ThoiGianLamBai { get; set; }
    public decimal DiemToiDa { get; set; }
    public int MaxAttempts { get; set; } = 1;
    public DateTime? OpenAt { get; set; }
    public DateTime? DueAt { get; set; }
    public bool AllowLateSubmission { get; set; }
    public bool ShowScoreWhenDone { get; set; }
    public bool ShowAnswerAfterDeadline { get; set; }
    public bool ShuffleQuestions { get; set; } = true;
    public bool ShuffleAnswers { get; set; } = true;
    public bool AutoGradeObjective { get; set; } = true;
    public bool RequireManualReview { get; set; } = true;
    public string? AccessCode { get; set; }
    public string ScoreMode { get; set; } = "AUTO_EQUAL";
    public List<Guid> SelectedClassIds { get; set; } = new();
    public List<AdminAssignmentQuestionSelectionRequestDto> SelectedQuestions { get; set; } = new();
}
