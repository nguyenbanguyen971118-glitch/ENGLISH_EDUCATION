using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Binhluan
{
    public Guid MaBinhLuan { get; set; }

    public Guid MaSuKien { get; set; }

    public Guid MaNguoiDung { get; set; }

    public string NoiDung { get; set; } = null!;

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual Nguoidung MaNguoiDungNavigation { get; set; } = null!;

    public virtual Sukienlophoc MaSuKienNavigation { get; set; } = null!;
}
