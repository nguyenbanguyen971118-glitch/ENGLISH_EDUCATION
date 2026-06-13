namespace backend.DTOs;

public class StudyContentCourseOptionDto
{
    public Guid MaKhoaHoc { get; set; }
    public string TenKhoaHoc { get; set; } = string.Empty;
    public int SoLopHoc { get; set; }
    public int SoChuongHoc { get; set; }
}

public class StudyContentClassOptionDto
{
    public Guid MaLopHoc { get; set; }
    public Guid MaKhoaHoc { get; set; }
    public string TenLop { get; set; } = string.Empty;
    public string? NgayBatDau { get; set; }
    public string? NgayKetThuc { get; set; }
    public bool LaLopHienTai { get; set; }
}

public class StudyContentDocumentDto
{
    public Guid MaTaiLieu { get; set; }
    public Guid MaChuongHoc { get; set; }
    public string TenTaiLieu { get; set; } = string.Empty;
    public string LinkTaiLieu { get; set; } = string.Empty;
    public string? MoTa { get; set; }
    public string LoaiTaiLieu { get; set; } = string.Empty;
    public string NgayDang { get; set; } = string.Empty;
    public string NgayDangHienThi { get; set; } = string.Empty;
    public bool LaLinkNgoai { get; set; }
}

public class StudyContentChapterDto
{
    public Guid MaChuong { get; set; }
    public Guid MaKhoaHoc { get; set; }
    public Guid? MaLopHoc { get; set; }
    public string? TenLop { get; set; }
    public string TenChuong { get; set; } = string.Empty;
    public string? MoTa { get; set; }
    public int? ThuTu { get; set; }
    public List<StudyContentDocumentDto> TaiLieu { get; set; } = new();
}

public class StudyContentOverviewDto
{
    public Guid MaKhoaHoc { get; set; }
    public string TenKhoaHoc { get; set; } = string.Empty;
    public Guid? MaLopHoc { get; set; }
    public string? TenLop { get; set; }
    public List<StudyContentChapterDto> NoiDungHocTap { get; set; } = new();
}
