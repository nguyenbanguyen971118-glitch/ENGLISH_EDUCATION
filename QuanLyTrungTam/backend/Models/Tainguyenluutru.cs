using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Tainguyenluutru
{
    public Guid MaTaiNguyen { get; set; }

    public Guid MaNguoiDung { get; set; }

    public string TenTaiNguyen { get; set; } = null!;

    public string Link { get; set; } = null!;

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual ICollection<Dinhkemnopbai> Dinhkemnopbais { get; set; } = new List<Dinhkemnopbai>();

    public virtual ICollection<Dinhkem> Dinhkems { get; set; } = new List<Dinhkem>();

    public virtual ICollection<Dinhkemthongbao> Dinhkemthongbaos { get; set; } = new List<Dinhkemthongbao>();

    public virtual Nguoidung MaNguoiDungNavigation { get; set; } = null!;
}
