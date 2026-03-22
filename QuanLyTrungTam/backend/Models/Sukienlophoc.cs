using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Sukienlophoc
{
    public Guid MaSuKien { get; set; }

    public Guid MaLopHoc { get; set; }

    public Guid MaNguoiDung { get; set; }

    /// <summary>
    /// Trỏ về ChiTietDanhMuc
    /// </summary>
    public int? DangSuKien { get; set; }

    public Guid? MaBaiTap { get; set; }

    public string? NoiDung { get; set; }

    public DateTime? HanNop { get; set; }

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

    public virtual ICollection<Binhluan> Binhluans { get; set; } = new List<Binhluan>();

    public virtual Chitietdanhmuc? DangSuKienNavigation { get; set; }

    public virtual ICollection<Dinhkem> Dinhkems { get; set; } = new List<Dinhkem>();

    public virtual Baitap? MaBaiTapNavigation { get; set; }

    public virtual Lophoc MaLopHocNavigation { get; set; } = null!;

    public virtual Nguoidung MaNguoiDungNavigation { get; set; } = null!;

    public virtual Chitietdanhmuc? MaTrangThaiNavigation { get; set; }

    public virtual ICollection<Nguoinhansukien> Nguoinhansukiens { get; set; } = new List<Nguoinhansukien>();

    public virtual ICollection<Nopbai> Nopbais { get; set; } = new List<Nopbai>();
}
