using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Khoahoc
{
    public Guid MaKhoaHoc { get; set; }

    public string TenKhoaHoc { get; set; } = null!;

    public string? MoTa { get; set; }

    public decimal? GiaCoBan { get; set; }

    /// <summary>
    /// Trỏ về ChiTietDanhMuc
    /// </summary>
    public int? MaTrangThai { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual ICollection<Baitap> Baitaps { get; set; } = new List<Baitap>();

    public virtual ICollection<ChitietkhoahocLophoc> ChitietkhoahocLophocs { get; set; } = new List<ChitietkhoahocLophoc>();

    public virtual ICollection<Chuonghoc> Chuonghocs { get; set; } = new List<Chuonghoc>();

    public virtual Chitietdanhmuc? MaTrangThaiNavigation { get; set; }

    public virtual ICollection<Nganhangcauhoi> Nganhangcauhois { get; set; } = new List<Nganhangcauhoi>();
}
