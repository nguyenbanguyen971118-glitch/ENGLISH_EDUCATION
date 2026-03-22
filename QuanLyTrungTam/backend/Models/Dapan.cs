using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Dapan
{
    public Guid MaDapAn { get; set; }

    public Guid MaCauHoi { get; set; }

    public string NoiDungDapAn { get; set; } = null!;

    public bool? LaDapAnDung { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual ICollection<Chitietnopbai> Chitietnopbais { get; set; } = new List<Chitietnopbai>();

    public virtual Nganhangcauhoi MaCauHoiNavigation { get; set; } = null!;
}
