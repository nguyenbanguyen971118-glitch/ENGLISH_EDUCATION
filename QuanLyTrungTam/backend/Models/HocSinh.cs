using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Hocsinh
{
    public Guid MaHocSinh { get; set; }

    public Guid MaNguoiDung { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? QueQuan { get; set; }

    public string? SoDienThoaiNguoiThan { get; set; }

    public string? TruongDangTheoHoc { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual ICollection<Diemdanh> Diemdanhs { get; set; } = new List<Diemdanh>();

    public virtual ICollection<Hocsinhlophoc> Hocsinhlophocs { get; set; } = new List<Hocsinhlophoc>();

    public virtual Nguoidung MaNguoiDungNavigation { get; set; } = null!;

    public virtual ICollection<Nguoinhansukien> Nguoinhansukiens { get; set; } = new List<Nguoinhansukien>();

    public virtual ICollection<Nopbai> Nopbais { get; set; } = new List<Nopbai>();

    public virtual ICollection<Phuhuynhhocsinh> Phuhuynhhocsinhs { get; set; } = new List<Phuhuynhhocsinh>();
}
