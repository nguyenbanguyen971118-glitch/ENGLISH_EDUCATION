using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Dapandiendkhuyet
{
    public Guid MaDapAnDien { get; set; }

    public Guid MaCauHoi { get; set; }

    public string DapAnChuan { get; set; } = null!;

    public string? DapAnThayThe { get; set; }

    public bool? PhanBietHoaThuong { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual Nganhangcauhoi MaCauHoiNavigation { get; set; } = null!;
}
