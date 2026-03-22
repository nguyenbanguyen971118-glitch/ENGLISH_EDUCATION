using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Lophoc
{
    public Guid MaLopHoc { get; set; }

    public string TenLop { get; set; } = null!;

    public DateOnly? NgayBatDau { get; set; }

    public DateOnly? NgayKetThuc { get; set; }

    public int? SiSoHienTai { get; set; }

    public int? SiSoToiDa { get; set; }

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

    public virtual ICollection<Buoihoc> Buoihocs { get; set; } = new List<Buoihoc>();

    public virtual ICollection<ChitietkhoahocLophoc> ChitietkhoahocLophocs { get; set; } = new List<ChitietkhoahocLophoc>();

    public virtual ICollection<Giangvienlophoc> Giangvienlophocs { get; set; } = new List<Giangvienlophoc>();

    public virtual ICollection<Hocsinhlophoc> Hocsinhlophocs { get; set; } = new List<Hocsinhlophoc>();

    public virtual Chitietdanhmuc? MaTrangThaiNavigation { get; set; }

    public virtual ICollection<Sukienlophoc> Sukienlophocs { get; set; } = new List<Sukienlophoc>();

    public virtual ICollection<Yeucaulichday> Yeucaulichdays { get; set; } = new List<Yeucaulichday>();
}
