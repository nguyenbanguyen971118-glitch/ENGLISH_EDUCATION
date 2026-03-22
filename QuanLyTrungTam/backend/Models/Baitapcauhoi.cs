using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Baitapcauhoi
{
    public Guid MaBaiTap { get; set; }

    public Guid MaCauHoi { get; set; }

    public decimal? DiemCuaCau { get; set; }

    public int? ThuTu { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual Baitap MaBaiTapNavigation { get; set; } = null!;

    public virtual Nganhangcauhoi MaCauHoiNavigation { get; set; } = null!;
}
