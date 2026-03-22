using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Tiethoc
{
    public int MaTiet { get; set; }

    public string TenTiet { get; set; } = null!;

    public TimeOnly GioBatDau { get; set; }

    public TimeOnly GioKetThuc { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual ICollection<Buoihoc> BuoihocMaTietBatDauNavigations { get; set; } = new List<Buoihoc>();

    public virtual ICollection<Buoihoc> BuoihocMaTietKetThucNavigations { get; set; } = new List<Buoihoc>();

    public virtual ICollection<Yeucaulichday> YeucaulichdayMaTietBatDauDeXuatNavigations { get; set; } = new List<Yeucaulichday>();

    public virtual ICollection<Yeucaulichday> YeucaulichdayMaTietKetThucDeXuatNavigations { get; set; } = new List<Yeucaulichday>();
}
