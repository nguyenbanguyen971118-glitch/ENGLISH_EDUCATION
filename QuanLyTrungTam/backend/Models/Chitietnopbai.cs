using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Chitietnopbai
{
    public Guid MaChiTiet { get; set; }

    public Guid MaNopBai { get; set; }

    public Guid MaCauHoi { get; set; }

    public Guid? MaDapAnChon { get; set; }

    public string? CauTraLoiDienKhuyet { get; set; }

    public decimal? DiemDatDuoc { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual Nganhangcauhoi MaCauHoiNavigation { get; set; } = null!;

    public virtual Dapan? MaDapAnChonNavigation { get; set; }

    public virtual Nopbai MaNopBaiNavigation { get; set; } = null!;
}
