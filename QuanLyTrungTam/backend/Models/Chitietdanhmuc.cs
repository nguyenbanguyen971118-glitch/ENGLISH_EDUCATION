using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Chitietdanhmuc
{
    public int MaChiTiet { get; set; }

    public int MaNhom { get; set; }

    /// <summary>
    /// VD: PH_LAB, TKH_OPEN
    /// </summary>
    public string? MaCode { get; set; }

    public string TenChiTiet { get; set; } = null!;

    public int? ThuTu { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual ICollection<Baitap> Baitaps { get; set; } = new List<Baitap>();

    public virtual ICollection<Diemdanh> Diemdanhs { get; set; } = new List<Diemdanh>();

    public virtual ICollection<Hocsinhlophoc> Hocsinhlophocs { get; set; } = new List<Hocsinhlophoc>();

    public virtual ICollection<Khoahoc> Khoahocs { get; set; } = new List<Khoahoc>();

    public virtual ICollection<Lophoc> Lophocs { get; set; } = new List<Lophoc>();

    public virtual Nhomdanhmuc MaNhomNavigation { get; set; } = null!;

    public virtual ICollection<Nganhangcauhoi> NganhangcauhoiLoaiCauHoiNavigations { get; set; } = new List<Nganhangcauhoi>();

    public virtual ICollection<Nganhangcauhoi> NganhangcauhoiMucDoNavigations { get; set; } = new List<Nganhangcauhoi>();

    public virtual ICollection<Nguoidung> Nguoidungs { get; set; } = new List<Nguoidung>();

    public virtual ICollection<Nopbai> Nopbais { get; set; } = new List<Nopbai>();

    public virtual ICollection<Phonghoc> Phonghocs { get; set; } = new List<Phonghoc>();

    public virtual ICollection<Phuhuynhhocsinh> Phuhuynhhocsinhs { get; set; } = new List<Phuhuynhhocsinh>();

    public virtual ICollection<Sukienlophoc> SukienlophocDangSuKienNavigations { get; set; } = new List<Sukienlophoc>();

    public virtual ICollection<Sukienlophoc> SukienlophocMaTrangThaiNavigations { get; set; } = new List<Sukienlophoc>();
}
