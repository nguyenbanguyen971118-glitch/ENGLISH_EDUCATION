using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Phuhuynh
{
    public Guid MaPhuHuynh { get; set; }

    public Guid MaNguoiDung { get; set; }

    public string SoDienThoai { get; set; } = null!;

    public string? DiaChiLienHe { get; set; }

    public string? NgheNghiep { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual Nguoidung MaNguoiDungNavigation { get; set; } = null!;

    public virtual ICollection<Phuhuynhhocsinh> Phuhuynhhocsinhs { get; set; } = new List<Phuhuynhhocsinh>();
}
