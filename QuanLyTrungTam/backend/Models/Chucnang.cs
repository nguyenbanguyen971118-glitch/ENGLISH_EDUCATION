using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Chucnang
{
    public int MaChucNang { get; set; }

    public string TenChucNang { get; set; } = null!;

    public string? MoTa { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual ICollection<Quyen> Quyens { get; set; } = new List<Quyen>();
}
