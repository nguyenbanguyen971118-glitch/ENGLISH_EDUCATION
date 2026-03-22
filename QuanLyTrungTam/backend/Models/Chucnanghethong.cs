using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Chucnanghethong
{
    public int MaChucNang { get; set; }

    public string MaChucNangCode { get; set; } = null!;

    public string TenChucNang { get; set; } = null!;

    public string MaTrang { get; set; } = null!;

    public string TenTrang { get; set; } = null!;

    public int ThuTu { get; set; }

    public virtual ICollection<Vaitro> MaVaiTros { get; set; } = new List<Vaitro>();
}
