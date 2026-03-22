using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Nguoidung
{
    /// <summary>
    /// GUID cho tài khoản
    /// </summary>
    public Guid MaNguoiDung { get; set; }

    /// <summary>
    /// 1: Admin, 2: GiangVien, 3: HocSinh, 4: PhuHuynh
    /// </summary>
    public sbyte LoaiTaiKhoan { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string MatKhauHash { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string? Email { get; set; }

    public string? AnhDaiDien { get; set; }

    /// <summary>
    /// Trỏ về ChiTietDanhMuc
    /// </summary>
    public int? MaTrangThai { get; set; }

    public string? TokenXacMinh { get; set; }

    public bool? DaXacMinhEmail { get; set; }

    public Guid? NguoiTao { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public Guid? NguoiSua { get; set; }

    public DateTime? ThoiGianSua { get; set; }

    public bool? TrangThai { get; set; }

    public bool? DaXoa { get; set; }

    public virtual ICollection<Binhluan> Binhluans { get; set; } = new List<Binhluan>();

    public virtual Giangvien? Giangvien { get; set; }

    public virtual Hocsinh? Hocsinh { get; set; }

    public virtual Chitietdanhmuc? MaTrangThaiNavigation { get; set; }

    public virtual ICollection<Nguoidungvaitro> Nguoidungvaitros { get; set; } = new List<Nguoidungvaitro>();

    public virtual ICollection<Nguoinhanthongbao> Nguoinhanthongbaos { get; set; } = new List<Nguoinhanthongbao>();

    public virtual Phuhuynh? Phuhuynh { get; set; }

    public virtual ICollection<Sukienlophoc> Sukienlophocs { get; set; } = new List<Sukienlophoc>();

    public virtual ICollection<Tainguyenluutru> Tainguyenluutrus { get; set; } = new List<Tainguyenluutru>();

    public virtual ICollection<Thanhvienhoithoai> Thanhvienhoithoais { get; set; } = new List<Thanhvienhoithoai>();

    public virtual ICollection<Tinnhan> Tinnhans { get; set; } = new List<Tinnhan>();
}
