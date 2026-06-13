using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Nganhangcauhoi
{
    public Guid MaCauHoi { get; set; }

    public Guid MaKhoaHoc { get; set; }

    /// <summary>
    /// Trỏ về ChiTietDanhMuc
    /// </summary>
    public int? LoaiCauHoi { get; set; }

    /// <summary>
    /// Trỏ về ChiTietDanhMuc
    /// </summary>
    public int? MucDo { get; set; }

    public sbyte? MucDichSuDung { get; set; }

    public string NoiDungCauHoi { get; set; } = null!;

    public string? GiaiThichDapAn { get; set; }

    public string? AmThanhLink { get; set; }

    public string? HinhAnhLink { get; set; }

    public Guid? MaCauHoiCha { get; set; }

    public int? ThuTu { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual ICollection<Baitapcauhoi> Baitapcauhois { get; set; } = new List<Baitapcauhoi>();

    public virtual ICollection<Chitietnopbai> Chitietnopbais { get; set; } = new List<Chitietnopbai>();

    public virtual ICollection<Dapan> Dapans { get; set; } = new List<Dapan>();

    public virtual ICollection<Dapandiendkhuyet> Dapandiendkhuyets { get; set; } = new List<Dapandiendkhuyet>();

    public virtual ICollection<Nganhangcauhoi> InverseMaCauHoiChaNavigation { get; set; } = new List<Nganhangcauhoi>();

    public virtual Chitietdanhmuc? LoaiCauHoiNavigation { get; set; }

    public virtual Nganhangcauhoi? MaCauHoiChaNavigation { get; set; }

    public virtual Khoahoc MaKhoaHocNavigation { get; set; } = null!;

    public virtual Chitietdanhmuc? MucDoNavigation { get; set; }
}
