using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Quyen
{
    public int MaQuyen { get; set; }

    public int MaChucNang { get; set; }

    public string TenQuyen { get; set; } = null!;

    public string? MoTa { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual Chucnang MaChucNangNavigation { get; set; } = null!;

    public virtual ICollection<Vaitroquyen> Vaitroquyens { get; set; } = new List<Vaitroquyen>();
}
