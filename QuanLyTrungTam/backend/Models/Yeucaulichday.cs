using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Yeucaulichday
{
    public Guid MaYeuCau { get; set; }

    public Guid MaGiangVien { get; set; }

    public Guid MaLopHoc { get; set; }

    public Guid? MaBuoiHoc { get; set; }

    public DateOnly NgayHocDeXuat { get; set; }

    public int MaTietBatDauDeXuat { get; set; }

    public int MaTietKetThucDeXuat { get; set; }

    public string? LyDo { get; set; }

    public sbyte? TrangThaiDuyet { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual Buoihoc? MaBuoiHocNavigation { get; set; }

    public virtual Giangvien MaGiangVienNavigation { get; set; } = null!;

    public virtual Lophoc MaLopHocNavigation { get; set; } = null!;

    public virtual Tiethoc MaTietBatDauDeXuatNavigation { get; set; } = null!;

    public virtual Tiethoc MaTietKetThucDeXuatNavigation { get; set; } = null!;
}
