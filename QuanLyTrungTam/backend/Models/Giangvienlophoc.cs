using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Giangvienlophoc
{
    public Guid MaLopHoc { get; set; }

    public Guid MaGiangVien { get; set; }

    public sbyte? LoaiVaiTro { get; set; }

    public DateOnly? NgayThamGia { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual Giangvien MaGiangVienNavigation { get; set; } = null!;

    public virtual Lophoc MaLopHocNavigation { get; set; } = null!;
}
