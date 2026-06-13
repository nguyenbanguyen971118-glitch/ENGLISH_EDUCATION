using Microsoft.AspNetCore.Http;

namespace backend.DTOs;

public class CreateStudyChapterDto
{
    public Guid MaKhoaHoc { get; set; }
    public Guid MaLopHoc { get; set; }
    public string TenChuong { get; set; } = string.Empty;
    public string? MoTa { get; set; }
    public int? ThuTu { get; set; }
}

public class UpdateStudyChapterDto
{
    public Guid MaLopHoc { get; set; }
    public string TenChuong { get; set; } = string.Empty;
    public string? MoTa { get; set; }
    public int? ThuTu { get; set; }
}

public class CreateStudyDocumentDto
{
    public Guid MaChuongHoc { get; set; }
    public Guid MaLopHoc { get; set; }
    public string? TenTaiLieu { get; set; }
    public string? MoTa { get; set; }
    public string? LinkTaiLieu { get; set; }
    public IFormFile? File { get; set; }
}

public class UpdateStudyDocumentDto
{
    public Guid MaLopHoc { get; set; }
    public string? TenTaiLieu { get; set; }
    public string? MoTa { get; set; }
    public string? LinkTaiLieu { get; set; }
    public IFormFile? File { get; set; }
}
