using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Chuonghoc
{
    public Guid MaChuong { get; set; }

    public Guid MaKhoaHoc { get; set; }

    public Guid? MaLopHoc { get; set; }

    public string TenChuong { get; set; } = null!;

    public string? MoTa { get; set; }

    public int? ThuTu { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual Khoahoc MaKhoaHocNavigation { get; set; } = null!;

    public virtual Lophoc? MaLopHocNavigation { get; set; }

    public virtual ICollection<Tailieu> Tailieus { get; set; } = new List<Tailieu>();
}
