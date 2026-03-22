using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Nguoinhansukien
{
    public Guid MaSuKien { get; set; }

    public Guid MaHocSinh { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual Hocsinh MaHocSinhNavigation { get; set; } = null!;

    public virtual Sukienlophoc MaSuKienNavigation { get; set; } = null!;
}
