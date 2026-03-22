using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Nopbai
{
    public Guid MaNopBai { get; set; }

    public Guid MaSuKien { get; set; }

    public Guid MaHocSinh { get; set; }

    public DateTime? ThoiGianBatDau { get; set; }

    public DateTime? ThoiGianNop { get; set; }

    public string? NhanXetGiaoVien { get; set; }

    public decimal? DiemSo { get; set; }

    public int? LanNop { get; set; }

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

    public virtual ICollection<Chitietnopbai> Chitietnopbais { get; set; } = new List<Chitietnopbai>();

    public virtual ICollection<Dinhkemnopbai> Dinhkemnopbais { get; set; } = new List<Dinhkemnopbai>();

    public virtual Hocsinh MaHocSinhNavigation { get; set; } = null!;

    public virtual Sukienlophoc MaSuKienNavigation { get; set; } = null!;

    public virtual Chitietdanhmuc? MaTrangThaiNavigation { get; set; }
}
