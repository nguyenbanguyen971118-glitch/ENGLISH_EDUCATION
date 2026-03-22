using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Hoithoai
{
    public Guid MaHoiThoai { get; set; }

    public string? TieuDe { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual ICollection<Thanhvienhoithoai> Thanhvienhoithoais { get; set; } = new List<Thanhvienhoithoai>();

    public virtual ICollection<Tinnhan> Tinnhans { get; set; } = new List<Tinnhan>();
}
