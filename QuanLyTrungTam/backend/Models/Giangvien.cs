using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Giangvien
{
    public Guid MaGiangVien { get; set; }

    public Guid MaNguoiDung { get; set; }

    public string? SoDienThoai { get; set; }

    public string? QueQuan { get; set; }

    public string TrinhDoChuyenMon { get; set; } = null!;

    public string? HocVi { get; set; }

    public string? KinhNghiemGiangDay { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual ICollection<Giangvienlophoc> Giangvienlophocs { get; set; } = new List<Giangvienlophoc>();

    public virtual Nguoidung MaNguoiDungNavigation { get; set; } = null!;

    public virtual ICollection<Yeucaulichday> Yeucaulichdays { get; set; } = new List<Yeucaulichday>();
}
