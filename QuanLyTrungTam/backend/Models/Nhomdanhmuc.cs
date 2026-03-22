using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Nhomdanhmuc
{
    public int MaNhom { get; set; }

    /// <summary>
    /// VD: LOAI_PHONG, TRANG_THAI_KHOA_HOC
    /// </summary>
    public string MaCode { get; set; } = null!;

    public string TenNhom { get; set; } = null!;

    public string? GhiChu { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual ICollection<Chitietdanhmuc> Chitietdanhmucs { get; set; } = new List<Chitietdanhmuc>();
}
