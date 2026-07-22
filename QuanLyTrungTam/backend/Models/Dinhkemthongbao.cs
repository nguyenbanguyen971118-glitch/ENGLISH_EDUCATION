using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Dinhkemthongbao
{
    public Guid MaThongBao { get; set; }

    public Guid MaTaiNguyen { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual Thongbao MaThongBaoNavigation { get; set; } = null!;

    public virtual Tainguyenluutru MaTaiNguyenNavigation { get; set; } = null!;
}
