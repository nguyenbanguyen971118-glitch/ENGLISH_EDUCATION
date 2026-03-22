using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Phonghoc
{
    public Guid MaPhongHoc { get; set; }

    public string TenPhong { get; set; } = null!;

    public int? SucChua { get; set; }

    /// <summary>
    /// Trỏ về ChiTietDanhMuc
    /// </summary>
    public int? LoaiPhong { get; set; }

    public string? Link { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual ICollection<Buoihoc> Buoihocs { get; set; } = new List<Buoihoc>();

    public virtual Chitietdanhmuc? LoaiPhongNavigation { get; set; }
}
