using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Nguoidungvaitro
{
    public Guid MaNguoiDung { get; set; }

    public int MaVaiTro { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual Nguoidung MaNguoiDungNavigation { get; set; } = null!;

    public virtual Vaitro MaVaiTroNavigation { get; set; } = null!;
}
