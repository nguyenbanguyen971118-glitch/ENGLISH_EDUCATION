using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Vaitroquyen
{
    public int MaVaiTro { get; set; }

    public int MaQuyen { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual Quyen MaQuyenNavigation { get; set; } = null!;

    public virtual Vaitro MaVaiTroNavigation { get; set; } = null!;
}
