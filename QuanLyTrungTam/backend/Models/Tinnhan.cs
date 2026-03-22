using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Tinnhan
{
    public Guid MaTinNhan { get; set; }

    public Guid MaHoiThoai { get; set; }

    public Guid MaNguoiDungGui { get; set; }

    public string NoiDung { get; set; } = null!;

    public bool? DaDoc { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual Hoithoai MaHoiThoaiNavigation { get; set; } = null!;

    public virtual Nguoidung MaNguoiDungGuiNavigation { get; set; } = null!;
}
