using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Baitap
{
    public Guid MaBaiTap { get; set; }

    public Guid MaKhoaHoc { get; set; }

    public string TenBaiTap { get; set; } = null!;

    public string? MoTa { get; set; }

    /// <summary>
    /// Trỏ về ChiTietDanhMuc
    /// </summary>
    public int? LoaiBaiTap { get; set; }

    public int? ThoiGianLamBai { get; set; }

    public decimal? DiemToiDa { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual ICollection<Baitapcauhoi> Baitapcauhois { get; set; } = new List<Baitapcauhoi>();

    public virtual Chitietdanhmuc? LoaiBaiTapNavigation { get; set; }

    public virtual Khoahoc MaKhoaHocNavigation { get; set; } = null!;

    public virtual ICollection<Sukienlophoc> Sukienlophocs { get; set; } = new List<Sukienlophoc>();
}
