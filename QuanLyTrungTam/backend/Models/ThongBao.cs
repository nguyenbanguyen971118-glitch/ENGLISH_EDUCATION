using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Thongbao
{
    public Guid MaThongBao { get; set; }

    public string TieuDe { get; set; } = null!;

    public string NoiDung { get; set; } = null!;

    /// <summary>
    /// Đối tượng nhận thông báo: Admin, Giao_Vien, Hoc_Sinh, Phu_Huynh, Tat_Ca
    /// </summary>
    public string DoiTuong { get; set; } = "Tat_Ca";

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual ICollection<Nguoinhanthongbao> Nguoinhanthongbaos { get; set; } = new List<Nguoinhanthongbao>();
}
