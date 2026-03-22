using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Buoihoc
{
    public Guid MaBuoiHoc { get; set; }

    public Guid MaLopHoc { get; set; }

    public Guid? MaPhongHoc { get; set; }

    public DateOnly NgayHoc { get; set; }

    public int MaTietBatDau { get; set; }

    public int MaTietKetThuc { get; set; }

    public string? TieuDe { get; set; }

    public string? NoiDung { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual ICollection<Diemdanh> Diemdanhs { get; set; } = new List<Diemdanh>();

    public virtual Lophoc MaLopHocNavigation { get; set; } = null!;

    public virtual Phonghoc? MaPhongHocNavigation { get; set; }

    public virtual Tiethoc MaTietBatDauNavigation { get; set; } = null!;

    public virtual Tiethoc MaTietKetThucNavigation { get; set; } = null!;

    public virtual ICollection<Yeucaulichday> Yeucaulichdays { get; set; } = new List<Yeucaulichday>();
}
