using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Dinhkemnopbai
{
    public Guid MaNopBai { get; set; }

    public Guid MaTaiNguyen { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual Nopbai MaNopBaiNavigation { get; set; } = null!;

    public virtual Tainguyenluutru MaTaiNguyenNavigation { get; set; } = null!;
}
