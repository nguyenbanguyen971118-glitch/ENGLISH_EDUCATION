using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Tailieu
{
    public Guid MaTaiLieu { get; set; }

    public Guid MaChuongHoc { get; set; }

    public string TenTaiLieu { get; set; } = null!;

    public string? LinkTaiLieu { get; set; }

    public string? MoTa { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual Chuonghoc MaChuongHocNavigation { get; set; } = null!;
}
