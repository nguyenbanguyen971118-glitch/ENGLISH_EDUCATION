using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class GiangvienKhoahoc
{
    public Guid MaGiangVien { get; set; }

    public Guid MaKhoaHoc { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public virtual Giangvien MaGiangVienNavigation { get; set; } = null!;

    public virtual Khoahoc MaKhoaHocNavigation { get; set; } = null!;
}
