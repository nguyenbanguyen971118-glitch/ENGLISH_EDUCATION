using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace backend.Data;
using backend.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Baitap> Baitaps { get; set; }

    public virtual DbSet<Baitapcauhoi> Baitapcauhois { get; set; }

    public virtual DbSet<Binhluan> Binhluans { get; set; }

    public virtual DbSet<Buoihoc> Buoihocs { get; set; }

    public virtual DbSet<Chitietdanhmuc> Chitietdanhmucs { get; set; }

    public virtual DbSet<ChitietkhoahocLophoc> ChitietkhoahocLophocs { get; set; }

    public virtual DbSet<Chitietnopbai> Chitietnopbais { get; set; }

    public virtual DbSet<Chucnang> Chucnangs { get; set; }

    public virtual DbSet<Chucnanghethong> Chucnanghethongs { get; set; }

    public virtual DbSet<Chuonghoc> Chuonghocs { get; set; }

    public virtual DbSet<Dapan> Dapans { get; set; }

    public virtual DbSet<Dapandiendkhuyet> Dapandiendkhuyets { get; set; }

    public virtual DbSet<Diemdanh> Diemdanhs { get; set; }

    public virtual DbSet<Dinhkem> Dinhkems { get; set; }

    public virtual DbSet<Dinhkemnopbai> Dinhkemnopbais { get; set; }

    public virtual DbSet<Dinhkemthongbao> Dinhkemthongbaos { get; set; }

    public virtual DbSet<Giangvien> Giangviens { get; set; }

    public virtual DbSet<Giangvienlophoc> Giangvienlophocs { get; set; }

    public virtual DbSet<Hocsinh> Hocsinhs { get; set; }

    public virtual DbSet<Hocsinhlophoc> Hocsinhlophocs { get; set; }

    public virtual DbSet<Hoithoai> Hoithoais { get; set; }

    public virtual DbSet<Khoahoc> Khoahocs { get; set; }

    public virtual DbSet<Lophoc> Lophocs { get; set; }

    public virtual DbSet<Nganhangcauhoi> Nganhangcauhois { get; set; }

    public virtual DbSet<Nguoidung> Nguoidungs { get; set; }

    public virtual DbSet<Nguoidungvaitro> Nguoidungvaitros { get; set; }

    public virtual DbSet<Nguoinhansukien> Nguoinhansukiens { get; set; }

    public virtual DbSet<Nguoinhanthongbao> Nguoinhanthongbaos { get; set; }

    public virtual DbSet<Nhomdanhmuc> Nhomdanhmucs { get; set; }

    public virtual DbSet<Nopbai> Nopbais { get; set; }

    public virtual DbSet<Phonghoc> Phonghocs { get; set; }

    public virtual DbSet<Phuhuynh> Phuhuynhs { get; set; }

    public virtual DbSet<Phuhuynhhocsinh> Phuhuynhhocsinhs { get; set; }

    public virtual DbSet<Quyen> Quyens { get; set; }

    public virtual DbSet<Sukienlophoc> Sukienlophocs { get; set; }

    public virtual DbSet<Tailieu> Tailieus { get; set; }

    public virtual DbSet<Tainguyenluutru> Tainguyenluutrus { get; set; }

    public virtual DbSet<Thanhvienhoithoai> Thanhvienhoithoais { get; set; }

    public virtual DbSet<Thongbao> Thongbaos { get; set; }

    public virtual DbSet<Tiethoc> Tiethocs { get; set; }

    public virtual DbSet<Tinnhan> Tinnhans { get; set; }

    public virtual DbSet<Vaitro> Vaitros { get; set; }

    public virtual DbSet<Vaitroquyen> Vaitroquyens { get; set; }

    public virtual DbSet<Yeucaulichday> Yeucaulichdays { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Connection được cấu hình từ Program.cs qua AddDbContext.
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Baitap>(entity =>
        {
            entity.HasKey(e => e.MaBaiTap).HasName("PRIMARY");

            entity.ToTable("baitap");

            entity.HasIndex(e => e.LoaiBaiTap, "LoaiBaiTap");

            entity.HasIndex(e => e.MaKhoaHoc, "MaKhoaHoc");

            entity.Property(e => e.MaBaiTap)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.DiemToiDa).HasPrecision(5, 2);
            entity.Property(e => e.LoaiBaiTap).HasComment("Trỏ về ChiTietDanhMuc");
            entity.Property(e => e.MaKhoaHoc)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MoTa).HasColumnType("text");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.TenBaiTap).HasMaxLength(255);
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.LoaiBaiTapNavigation).WithMany(p => p.Baitaps)
                .HasForeignKey(d => d.LoaiBaiTap)
                .HasConstraintName("baitap_ibfk_2");

            entity.HasOne(d => d.MaKhoaHocNavigation).WithMany(p => p.Baitaps)
                .HasForeignKey(d => d.MaKhoaHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("baitap_ibfk_1");
        });

        modelBuilder.Entity<Baitapcauhoi>(entity =>
        {
            entity.HasKey(e => new { e.MaBaiTap, e.MaCauHoi })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("baitapcauhoi");

            entity.HasIndex(e => new { e.MaCauHoi, e.DaXoa, e.TrangThai }, "idx_BaiTapCauHoi_CauHoi_Active");

            entity.Property(e => e.MaBaiTap)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaCauHoi)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.DiemCuaCau).HasPrecision(5, 2);
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaBaiTapNavigation).WithMany(p => p.Baitapcauhois)
                .HasForeignKey(d => d.MaBaiTap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("baitapcauhoi_ibfk_1");

            entity.HasOne(d => d.MaCauHoiNavigation).WithMany(p => p.Baitapcauhois)
                .HasForeignKey(d => d.MaCauHoi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("baitapcauhoi_ibfk_2");
        });

        modelBuilder.Entity<Binhluan>(entity =>
        {
            entity.HasKey(e => e.MaBinhLuan).HasName("PRIMARY");

            entity.ToTable("binhluan");

            entity.HasIndex(e => e.MaNguoiDung, "MaNguoiDung");

            entity.HasIndex(e => e.MaSuKien, "MaSuKien");

            entity.Property(e => e.MaBinhLuan)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.MaNguoiDung)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaSuKien)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NoiDung).HasColumnType("text");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.Binhluans)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("binhluan_ibfk_2");

            entity.HasOne(d => d.MaSuKienNavigation).WithMany(p => p.Binhluans)
                .HasForeignKey(d => d.MaSuKien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("binhluan_ibfk_1");
        });

        modelBuilder.Entity<Buoihoc>(entity =>
        {
            entity.HasKey(e => e.MaBuoiHoc).HasName("PRIMARY");

            entity.ToTable("buoihoc");

            entity.HasIndex(e => e.MaLopHoc, "MaLopHoc");

            entity.HasIndex(e => e.MaPhongHoc, "MaPhongHoc");

            entity.HasIndex(e => e.MaTietBatDau, "MaTietBatDau");

            entity.HasIndex(e => e.MaTietKetThuc, "MaTietKetThuc");

            entity.Property(e => e.MaBuoiHoc)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.MaLopHoc)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaPhongHoc)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NoiDung).HasColumnType("text");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TieuDe).HasMaxLength(255);
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaLopHocNavigation).WithMany(p => p.Buoihocs)
                .HasForeignKey(d => d.MaLopHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("buoihoc_ibfk_1");

            entity.HasOne(d => d.MaPhongHocNavigation).WithMany(p => p.Buoihocs)
                .HasForeignKey(d => d.MaPhongHoc)
                .HasConstraintName("buoihoc_ibfk_2");

            entity.HasOne(d => d.MaTietBatDauNavigation).WithMany(p => p.BuoihocMaTietBatDauNavigations)
                .HasForeignKey(d => d.MaTietBatDau)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("buoihoc_ibfk_3");

            entity.HasOne(d => d.MaTietKetThucNavigation).WithMany(p => p.BuoihocMaTietKetThucNavigations)
                .HasForeignKey(d => d.MaTietKetThuc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("buoihoc_ibfk_4");
        });

        modelBuilder.Entity<Chitietdanhmuc>(entity =>
        {
            entity.HasKey(e => e.MaChiTiet).HasName("PRIMARY");

            entity.ToTable("chitietdanhmuc");

            entity.HasIndex(e => e.MaNhom, "MaNhom");

            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.MaCode)
                .HasMaxLength(50)
                .HasComment("VD: PH_LAB, TKH_OPEN");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.TenChiTiet).HasMaxLength(255);
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.ThuTu).HasDefaultValueSql("'0'");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaNhomNavigation).WithMany(p => p.Chitietdanhmucs)
                .HasForeignKey(d => d.MaNhom)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chitietdanhmuc_ibfk_1");
        });

        modelBuilder.Entity<ChitietkhoahocLophoc>(entity =>
        {
            entity.HasKey(e => new { e.MaKhoaHoc, e.MaLopHoc })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("chitietkhoahoc_lophoc");

            entity.HasIndex(e => new { e.MaLopHoc, e.DaXoa, e.TrangThai }, "idx_ChiTietKHLH_LopHoc_Active");

            entity.Property(e => e.MaKhoaHoc)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaLopHoc)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.GhiChu).HasMaxLength(255);
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaKhoaHocNavigation).WithMany(p => p.ChitietkhoahocLophocs)
                .HasForeignKey(d => d.MaKhoaHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chitietkhoahoc_lophoc_ibfk_1");

            entity.HasOne(d => d.MaLopHocNavigation).WithMany(p => p.ChitietkhoahocLophocs)
                .HasForeignKey(d => d.MaLopHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chitietkhoahoc_lophoc_ibfk_2");
        });

        modelBuilder.Entity<Chitietnopbai>(entity =>
        {
            entity.HasKey(e => e.MaChiTiet).HasName("PRIMARY");

            entity.ToTable("chitietnopbai");

            entity.HasIndex(e => e.MaDapAnChon, "MaDapAnChon");

            entity.HasIndex(e => e.MaNopBai, "MaNopBai");

            entity.HasIndex(e => new { e.MaCauHoi, e.DaXoa, e.TrangThai }, "idx_ChiTietNopBai_CauHoi_Active");

            entity.Property(e => e.MaChiTiet)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.CauTraLoiHocSinh).HasColumnType("text");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.DiemDatDuoc)
                .HasPrecision(5, 2)
                .HasDefaultValueSql("'0.00'");
            entity.Property(e => e.MaCauHoi)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaDapAnChon)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaNopBai)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaCauHoiNavigation).WithMany(p => p.Chitietnopbais)
                .HasForeignKey(d => d.MaCauHoi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chitietnopbai_ibfk_2");

            entity.HasOne(d => d.MaDapAnChonNavigation).WithMany(p => p.Chitietnopbais)
                .HasForeignKey(d => d.MaDapAnChon)
                .HasConstraintName("chitietnopbai_ibfk_3");

            entity.HasOne(d => d.MaNopBaiNavigation).WithMany(p => p.Chitietnopbais)
                .HasForeignKey(d => d.MaNopBai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chitietnopbai_ibfk_1");
        });

        modelBuilder.Entity<Chucnang>(entity =>
        {
            entity.HasKey(e => e.MaChucNang).HasName("PRIMARY");

            entity.ToTable("chucnang");

            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.TenChucNang).HasMaxLength(100);
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");
        });

        modelBuilder.Entity<Chucnanghethong>(entity =>
        {
            entity.HasKey(e => e.MaChucNang).HasName("PRIMARY");

            entity.ToTable("chucnanghethong");

            entity.HasIndex(e => e.MaChucNangCode, "uq_chucnang_code").IsUnique();

            entity.Property(e => e.MaChucNang).HasColumnName("maChucNang");
            entity.Property(e => e.MaChucNangCode)
                .HasMaxLength(100)
                .HasColumnName("maChucNangCode");
            entity.Property(e => e.MaTrang)
                .HasMaxLength(100)
                .HasColumnName("maTrang");
            entity.Property(e => e.TenChucNang)
                .HasMaxLength(255)
                .HasColumnName("tenChucNang");
            entity.Property(e => e.TenTrang)
                .HasMaxLength(255)
                .HasColumnName("tenTrang");
            entity.Property(e => e.ThuTu).HasColumnName("thuTu");
        });

        modelBuilder.Entity<Chuonghoc>(entity =>
        {
            entity.HasKey(e => e.MaChuong).HasName("PRIMARY");

            entity.ToTable("chuonghoc");

            entity.HasIndex(e => e.MaKhoaHoc, "MaKhoaHoc1");

            entity.HasIndex(e => e.MaLopHoc, "idx_ChuongHoc_MaLopHoc");

            entity.Property(e => e.MaChuong)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.MaKhoaHoc)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaLopHoc)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MoTa).HasColumnType("text");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.TenChuong).HasMaxLength(255);
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaKhoaHocNavigation).WithMany(p => p.Chuonghocs)
                .HasForeignKey(d => d.MaKhoaHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chuonghoc_ibfk_1");

            entity.HasOne(d => d.MaLopHocNavigation).WithMany(p => p.Chuonghocs)
                .HasForeignKey(d => d.MaLopHoc)
                .HasConstraintName("chuonghoc_ibfk_2");
        });

        modelBuilder.Entity<Dapan>(entity =>
        {
            entity.HasKey(e => e.MaDapAn).HasName("PRIMARY");

            entity.ToTable("dapan");

            entity.HasIndex(e => e.MaCauHoi, "MaCauHoi");

            entity.HasIndex(e => new { e.MaCauHoi, e.DaXoa, e.TrangThai }, "idx_DapAn_CauHoi_Active");

            entity.Property(e => e.MaDapAn)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.GiaTriDoiChieu).HasMaxLength(255);
            entity.Property(e => e.LaDapAnDung).HasDefaultValueSql("'0'");
            entity.Property(e => e.MaCauHoi)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NoiDungDapAn).HasColumnType("text");
            entity.Property(e => e.TenDapAn).HasMaxLength(10);
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.ThuTu).HasDefaultValueSql("'0'");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaCauHoiNavigation).WithMany(p => p.Dapans)
                .HasForeignKey(d => d.MaCauHoi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dapan_ibfk_1");
        });

        modelBuilder.Entity<Dapandiendkhuyet>(entity =>
        {
            entity.HasKey(e => e.MaDapAnDien).HasName("PRIMARY");

            entity.ToTable("dapandiendkhuyet");

            entity.HasIndex(e => e.MaCauHoi, "MaCauHoi1");

            entity.HasIndex(e => new { e.MaCauHoi, e.DaXoa, e.TrangThai }, "idx_DapAnDienKhuyet_CauHoi_Active");

            entity.Property(e => e.MaDapAnDien)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.DapAnChuan).HasColumnType("text");
            entity.Property(e => e.DapAnThayThe).HasColumnType("text");
            entity.Property(e => e.MaCauHoi)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.PhanBietHoaThuong).HasDefaultValueSql("'0'");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaCauHoiNavigation).WithMany(p => p.Dapandiendkhuyets)
                .HasForeignKey(d => d.MaCauHoi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dapandiendkhuyet_ibfk_1");
        });

        modelBuilder.Entity<Diemdanh>(entity =>
        {
            entity.HasKey(e => new { e.MaBuoiHoc, e.MaHocSinh })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("diemdanh");

            entity.HasIndex(e => e.MaTrangThai, "MaTrangThai");

            entity.HasIndex(e => new { e.MaHocSinh, e.DaXoa, e.TrangThai }, "idx_DiemDanh_HocSinh_Active");

            entity.Property(e => e.MaBuoiHoc)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaHocSinh)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.GhiChu).HasMaxLength(255);
            entity.Property(e => e.MaTrangThai).HasComment("Trỏ về ChiTietDanhMuc");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaBuoiHocNavigation).WithMany(p => p.Diemdanhs)
                .HasForeignKey(d => d.MaBuoiHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("diemdanh_ibfk_1");

            entity.HasOne(d => d.MaHocSinhNavigation).WithMany(p => p.Diemdanhs)
                .HasForeignKey(d => d.MaHocSinh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("diemdanh_ibfk_2");

            entity.HasOne(d => d.MaTrangThaiNavigation).WithMany(p => p.Diemdanhs)
                .HasForeignKey(d => d.MaTrangThai)
                .HasConstraintName("diemdanh_ibfk_3");
        });

        modelBuilder.Entity<Dinhkem>(entity =>
        {
            entity.HasKey(e => new { e.MaSuKien, e.MaTaiNguyen })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("dinhkem");

            entity.HasIndex(e => new { e.MaTaiNguyen, e.DaXoa, e.TrangThai }, "idx_DinhKem_TaiNguyen_Active");

            entity.Property(e => e.MaSuKien)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaTaiNguyen)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaSuKienNavigation).WithMany(p => p.Dinhkems)
                .HasForeignKey(d => d.MaSuKien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dinhkem_ibfk_1");

            entity.HasOne(d => d.MaTaiNguyenNavigation).WithMany(p => p.Dinhkems)
                .HasForeignKey(d => d.MaTaiNguyen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dinhkem_ibfk_2");
        });

        modelBuilder.Entity<Dinhkemnopbai>(entity =>
        {
            entity.HasKey(e => new { e.MaNopBai, e.MaTaiNguyen })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("dinhkemnopbai");

            entity.HasIndex(e => new { e.MaTaiNguyen, e.DaXoa, e.TrangThai }, "idx_DinhKemNopBai_TaiNguyen_Active");

            entity.Property(e => e.MaNopBai)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaTaiNguyen)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaNopBaiNavigation).WithMany(p => p.Dinhkemnopbais)
                .HasForeignKey(d => d.MaNopBai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dinhkemnopbai_ibfk_1");

            entity.HasOne(d => d.MaTaiNguyenNavigation).WithMany(p => p.Dinhkemnopbais)
                .HasForeignKey(d => d.MaTaiNguyen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dinhkemnopbai_ibfk_2");
        });

        modelBuilder.Entity<Dinhkemthongbao>(entity =>
        {
            // MaThongBao/MaTaiNguyen deliberately left on the table's default charset
            // (utf8mb4_general_ci) instead of the "ascii" override used elsewhere in this
            // file — the actual thongbao/tainguyenluutru tables in this DB use utf8mb4 for
            // their GUID PKs, and an ascii FK column here would fail with errno 150.
            entity.HasKey(e => new { e.MaThongBao, e.MaTaiNguyen })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("dinhkemthongbao");

            entity.HasIndex(e => new { e.MaTaiNguyen, e.DaXoa, e.TrangThai }, "idx_DinhKemThongBao_TaiNguyen_Active");

            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaThongBaoNavigation).WithMany(p => p.Dinhkemthongbaos)
                .HasForeignKey(d => d.MaThongBao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dinhkemthongbao_ibfk_1");

            entity.HasOne(d => d.MaTaiNguyenNavigation).WithMany(p => p.Dinhkemthongbaos)
                .HasForeignKey(d => d.MaTaiNguyen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dinhkemthongbao_ibfk_2");
        });

        modelBuilder.Entity<Giangvien>(entity =>
        {
            entity.HasKey(e => e.MaGiangVien).HasName("PRIMARY");

            entity.ToTable("giangvien");

            entity.HasIndex(e => e.MaNguoiDung, "MaNguoiDung1").IsUnique();

            entity.Property(e => e.MaGiangVien)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.HocVi).HasMaxLength(100);
            entity.Property(e => e.KinhNghiemGiangDay).HasColumnType("text");
            entity.Property(e => e.MaNguoiDung)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.QueQuan).HasMaxLength(255);
            entity.Property(e => e.SoDienThoai).HasMaxLength(15);
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");
            entity.Property(e => e.TrinhDoChuyenMon).HasMaxLength(255);

            entity.HasOne(d => d.MaNguoiDungNavigation).WithOne(p => p.Giangvien)
                .HasForeignKey<Giangvien>(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("giangvien_ibfk_1");
        });

        modelBuilder.Entity<Giangvienlophoc>(entity =>
        {
            entity.HasKey(e => new { e.MaLopHoc, e.MaGiangVien })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("giangvienlophoc");

            entity.HasIndex(e => new { e.MaGiangVien, e.DaXoa, e.TrangThai }, "idx_GiangVienLopHoc_GiangVien_Active");

            entity.Property(e => e.MaLopHoc)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaGiangVien)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.LoaiVaiTro).HasDefaultValueSql("'1'");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaGiangVienNavigation).WithMany(p => p.Giangvienlophocs)
                .HasForeignKey(d => d.MaGiangVien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("giangvienlophoc_ibfk_2");

            entity.HasOne(d => d.MaLopHocNavigation).WithMany(p => p.Giangvienlophocs)
                .HasForeignKey(d => d.MaLopHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("giangvienlophoc_ibfk_1");
        });

        modelBuilder.Entity<Hocsinh>(entity =>
        {
            entity.HasKey(e => e.MaHocSinh).HasName("PRIMARY");

            entity.ToTable("hocsinh");

            entity.HasIndex(e => e.MaNguoiDung, "MaNguoiDung2").IsUnique();

            entity.Property(e => e.MaHocSinh)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.MaNguoiDung)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.QueQuan).HasMaxLength(255);
            entity.Property(e => e.SoDienThoaiNguoiThan).HasMaxLength(15);
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");
            entity.Property(e => e.TruongDangTheoHoc).HasMaxLength(255);

            entity.HasOne(d => d.MaNguoiDungNavigation).WithOne(p => p.Hocsinh)
                .HasForeignKey<Hocsinh>(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hocsinh_ibfk_1");
        });

        modelBuilder.Entity<Hocsinhlophoc>(entity =>
        {
            entity.HasKey(e => new { e.MaLopHoc, e.MaHocSinh })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("hocsinhlophoc");

            entity.HasIndex(e => e.MaTrangThai, "MaTrangThai1");

            entity.HasIndex(e => new { e.MaHocSinh, e.DaXoa, e.TrangThai }, "idx_HocSinhLopHoc_HocSinh_Active");

            entity.Property(e => e.MaLopHoc)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaHocSinh)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.MaTrangThai).HasComment("Trỏ về ChiTietDanhMuc");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaHocSinhNavigation).WithMany(p => p.Hocsinhlophocs)
                .HasForeignKey(d => d.MaHocSinh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hocsinhlophoc_ibfk_2");

            entity.HasOne(d => d.MaLopHocNavigation).WithMany(p => p.Hocsinhlophocs)
                .HasForeignKey(d => d.MaLopHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hocsinhlophoc_ibfk_1");

            entity.HasOne(d => d.MaTrangThaiNavigation).WithMany(p => p.Hocsinhlophocs)
                .HasForeignKey(d => d.MaTrangThai)
                .HasConstraintName("hocsinhlophoc_ibfk_3");
        });

        modelBuilder.Entity<Hoithoai>(entity =>
        {
            entity.HasKey(e => e.MaHoiThoai).HasName("PRIMARY");

            entity.ToTable("hoithoai");

            entity.Property(e => e.MaHoiThoai)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TieuDe).HasMaxLength(255);
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");
        });

        modelBuilder.Entity<Khoahoc>(entity =>
        {
            entity.HasKey(e => e.MaKhoaHoc).HasName("PRIMARY");

            entity.ToTable("khoahoc");

            entity.HasIndex(e => e.MaTrangThai, "MaTrangThai2");

            entity.Property(e => e.MaKhoaHoc)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.GiaCoBan).HasPrecision(14, 2);
            entity.Property(e => e.MaTrangThai).HasComment("Trỏ về ChiTietDanhMuc");
            entity.Property(e => e.MoTa).HasColumnType("text");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.TenKhoaHoc).HasMaxLength(255);
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaTrangThaiNavigation).WithMany(p => p.Khoahocs)
                .HasForeignKey(d => d.MaTrangThai)
                .HasConstraintName("khoahoc_ibfk_1");
        });

        modelBuilder.Entity<Lophoc>(entity =>
        {
            entity.HasKey(e => e.MaLopHoc).HasName("PRIMARY");

            entity.ToTable("lophoc");

            entity.HasIndex(e => e.MaTrangThai, "MaTrangThai3");

            entity.Property(e => e.MaLopHoc)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.MaTrangThai).HasComment("Trỏ về ChiTietDanhMuc");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.SiSoHienTai).HasDefaultValueSql("'0'");
            entity.Property(e => e.TenLop).HasMaxLength(100);
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaTrangThaiNavigation).WithMany(p => p.Lophocs)
                .HasForeignKey(d => d.MaTrangThai)
                .HasConstraintName("lophoc_ibfk_1");
        });

        modelBuilder.Entity<Nganhangcauhoi>(entity =>
        {
            entity.HasKey(e => e.MaCauHoi).HasName("PRIMARY");

            entity.ToTable("nganhangcauhoi");

            entity.HasIndex(e => e.LoaiCauHoi, "LoaiCauHoi");

            entity.HasIndex(e => e.MaKhoaHoc, "MaKhoaHoc2");

            entity.HasIndex(e => e.MaCauHoiCha, "MaCauHoiCha");

            entity.HasIndex(e => e.MucDo, "MucDo");

            entity.Property(e => e.MaCauHoi)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.AmThanhLink).HasMaxLength(255);
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.GiaiThichDapAn).HasColumnType("text");
            entity.Property(e => e.HinhAnhLink).HasMaxLength(255);
            entity.Property(e => e.LoaiCauHoi).HasComment("Trỏ về ChiTietDanhMuc");
            entity.Property(e => e.MaCauHoiCha)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaKhoaHoc)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MucDichSuDung).HasDefaultValueSql("'1'");
            entity.Property(e => e.MucDo).HasComment("Trỏ về ChiTietDanhMuc");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NoiDungCauHoi).HasColumnType("text");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.ThuTu).HasDefaultValueSql("'0'");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.LoaiCauHoiNavigation).WithMany(p => p.NganhangcauhoiLoaiCauHoiNavigations)
                .HasForeignKey(d => d.LoaiCauHoi)
                .HasConstraintName("nganhangcauhoi_ibfk_2");

            entity.HasOne(d => d.MaCauHoiChaNavigation).WithMany(p => p.InverseMaCauHoiChaNavigation)
                .HasForeignKey(d => d.MaCauHoiCha)
                .HasConstraintName("nganhangcauhoi_ibfk_4");

            entity.HasOne(d => d.MaKhoaHocNavigation).WithMany(p => p.Nganhangcauhois)
                .HasForeignKey(d => d.MaKhoaHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("nganhangcauhoi_ibfk_1");

            entity.HasOne(d => d.MucDoNavigation).WithMany(p => p.NganhangcauhoiMucDoNavigations)
                .HasForeignKey(d => d.MucDo)
                .HasConstraintName("nganhangcauhoi_ibfk_3");
        });

        modelBuilder.Entity<Nguoidung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PRIMARY");

            entity.ToTable("nguoidung");

            entity.HasIndex(e => e.Email, "Email").IsUnique();

            entity.HasIndex(e => e.MaTrangThai, "MaTrangThai4");

            entity.HasIndex(e => e.TenDangNhap, "TenDangNhap").IsUnique();

            entity.Property(e => e.MaNguoiDung)
                .HasComment("GUID cho tài khoản")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.AnhDaiDien).HasMaxLength(255);
            entity.Property(e => e.DaXacMinhEmail).HasDefaultValueSql("'0'");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.LoaiTaiKhoan).HasComment("1: Admin, 2: GiangVien, 3: HocSinh, 4: PhuHuynh");
            entity.Property(e => e.MaTrangThai).HasComment("Trỏ về ChiTietDanhMuc");
            entity.Property(e => e.MatKhauHash).HasMaxLength(255);
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.TenDangNhap).HasMaxLength(50);
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TokenXacMinh).HasMaxLength(255);
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaTrangThaiNavigation).WithMany(p => p.Nguoidungs)
                .HasForeignKey(d => d.MaTrangThai)
                .HasConstraintName("nguoidung_ibfk_1");
        });

        modelBuilder.Entity<Nguoidungvaitro>(entity =>
        {
            entity.HasKey(e => new { e.MaNguoiDung, e.MaVaiTro })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("nguoidungvaitro");

            entity.HasIndex(e => new { e.MaVaiTro, e.DaXoa, e.TrangThai }, "idx_NguoiDungVaiTro_VaiTro_Active");

            entity.Property(e => e.MaNguoiDung)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.Nguoidungvaitros)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("nguoidungvaitro_ibfk_1");

            entity.HasOne(d => d.MaVaiTroNavigation).WithMany(p => p.Nguoidungvaitros)
                .HasForeignKey(d => d.MaVaiTro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("nguoidungvaitro_ibfk_2");
        });

        modelBuilder.Entity<Nguoinhansukien>(entity =>
        {
            entity.HasKey(e => new { e.MaSuKien, e.MaHocSinh })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("nguoinhansukien");

            entity.HasIndex(e => new { e.MaHocSinh, e.DaXoa, e.TrangThai }, "idx_NguoiNhanSuKien_HocSinh_Active");

            entity.Property(e => e.MaSuKien)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaHocSinh)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaHocSinhNavigation).WithMany(p => p.Nguoinhansukiens)
                .HasForeignKey(d => d.MaHocSinh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("nguoinhansukien_ibfk_2");

            entity.HasOne(d => d.MaSuKienNavigation).WithMany(p => p.Nguoinhansukiens)
                .HasForeignKey(d => d.MaSuKien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("nguoinhansukien_ibfk_1");
        });

        modelBuilder.Entity<Nguoinhanthongbao>(entity =>
        {
            entity.HasKey(e => new { e.MaThongBao, e.MaNguoiDung })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("nguoinhanthongbao");

            entity.HasIndex(e => new { e.MaNguoiDung, e.DaDoc, e.DaXoa }, "idx_NguoiNhanThongBao_User_DaDoc");

            entity.Property(e => e.MaThongBao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaNguoiDung)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaDoc).HasDefaultValueSql("'0'");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.NgayDoc).HasColumnType("datetime");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.Nguoinhanthongbaos)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("nguoinhanthongbao_ibfk_2");

            entity.HasOne(d => d.MaThongBaoNavigation).WithMany(p => p.Nguoinhanthongbaos)
                .HasForeignKey(d => d.MaThongBao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("nguoinhanthongbao_ibfk_1");
        });

        modelBuilder.Entity<Nhomdanhmuc>(entity =>
        {
            entity.HasKey(e => e.MaNhom).HasName("PRIMARY");

            entity.ToTable("nhomdanhmuc");

            entity.HasIndex(e => e.MaCode, "MaCode").IsUnique();

            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.GhiChu).HasColumnType("text");
            entity.Property(e => e.MaCode)
                .HasMaxLength(50)
                .HasComment("VD: LOAI_PHONG, TRANG_THAI_KHOA_HOC");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.TenNhom).HasMaxLength(255);
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");
        });

        modelBuilder.Entity<Nopbai>(entity =>
        {
            entity.HasKey(e => e.MaNopBai).HasName("PRIMARY");

            entity.ToTable("nopbai");

            entity.HasIndex(e => e.MaHocSinh, "MaHocSinh");

            entity.HasIndex(e => e.MaSuKien, "MaSuKien1");

            entity.HasIndex(e => e.MaTrangThai, "MaTrangThai5");

            entity.Property(e => e.MaNopBai)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.DiemSo).HasPrecision(5, 2);
            entity.Property(e => e.LanNop).HasDefaultValueSql("'1'");
            entity.Property(e => e.MaHocSinh)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaSuKien)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaTrangThai).HasComment("Trỏ về ChiTietDanhMuc");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NhanXetGiaoVien).HasColumnType("text");
            entity.Property(e => e.ThoiGianBatDau).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianNop).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaHocSinhNavigation).WithMany(p => p.Nopbais)
                .HasForeignKey(d => d.MaHocSinh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("nopbai_ibfk_2");

            entity.HasOne(d => d.MaSuKienNavigation).WithMany(p => p.Nopbais)
                .HasForeignKey(d => d.MaSuKien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("nopbai_ibfk_1");

            entity.HasOne(d => d.MaTrangThaiNavigation).WithMany(p => p.Nopbais)
                .HasForeignKey(d => d.MaTrangThai)
                .HasConstraintName("nopbai_ibfk_3");
        });

        modelBuilder.Entity<Phonghoc>(entity =>
        {
            entity.HasKey(e => e.MaPhongHoc).HasName("PRIMARY");

            entity.ToTable("phonghoc");

            entity.HasIndex(e => e.LoaiPhong, "LoaiPhong");

            entity.Property(e => e.MaPhongHoc)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.Link).HasMaxLength(255);
            entity.Property(e => e.LoaiPhong).HasComment("Trỏ về ChiTietDanhMuc");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.TenPhong).HasMaxLength(50);
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.LoaiPhongNavigation).WithMany(p => p.Phonghocs)
                .HasForeignKey(d => d.LoaiPhong)
                .HasConstraintName("phonghoc_ibfk_1");
        });

        modelBuilder.Entity<Phuhuynh>(entity =>
        {
            entity.HasKey(e => e.MaPhuHuynh).HasName("PRIMARY");

            entity.ToTable("phuhuynh");

            entity.HasIndex(e => e.MaNguoiDung, "MaNguoiDung3").IsUnique();

            entity.Property(e => e.MaPhuHuynh)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.DiaChiLienHe).HasColumnType("text");
            entity.Property(e => e.MaNguoiDung)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NgheNghiep).HasMaxLength(255);
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.SoDienThoai).HasMaxLength(15);
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithOne(p => p.Phuhuynh)
                .HasForeignKey<Phuhuynh>(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("phuhuynh_ibfk_1");
        });

        modelBuilder.Entity<Phuhuynhhocsinh>(entity =>
        {
            entity.HasKey(e => new { e.MaPhuHuynh, e.MaHocSinh })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("phuhuynhhocsinh");

            entity.HasIndex(e => e.MaQuanHe, "MaQuanHe");

            entity.HasIndex(e => new { e.MaHocSinh, e.DaXoa, e.TrangThai }, "idx_PhuHuynhHocSinh_HocSinh_Active");

            entity.Property(e => e.MaPhuHuynh)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaHocSinh)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.MaQuanHe).HasComment("Trỏ về ChiTietDanhMuc");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaHocSinhNavigation).WithMany(p => p.Phuhuynhhocsinhs)
                .HasForeignKey(d => d.MaHocSinh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("phuhuynhhocsinh_ibfk_2");

            entity.HasOne(d => d.MaPhuHuynhNavigation).WithMany(p => p.Phuhuynhhocsinhs)
                .HasForeignKey(d => d.MaPhuHuynh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("phuhuynhhocsinh_ibfk_1");

            entity.HasOne(d => d.MaQuanHeNavigation).WithMany(p => p.Phuhuynhhocsinhs)
                .HasForeignKey(d => d.MaQuanHe)
                .HasConstraintName("phuhuynhhocsinh_ibfk_3");
        });

        modelBuilder.Entity<Quyen>(entity =>
        {
            entity.HasKey(e => e.MaQuyen).HasName("PRIMARY");

            entity.ToTable("quyen");

            entity.HasIndex(e => e.MaChucNang, "MaChucNang");

            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.TenQuyen).HasMaxLength(100);
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaChucNangNavigation).WithMany(p => p.Quyens)
                .HasForeignKey(d => d.MaChucNang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("quyen_ibfk_1");
        });

        modelBuilder.Entity<Sukienlophoc>(entity =>
        {
            entity.HasKey(e => e.MaSuKien).HasName("PRIMARY");

            entity.ToTable("sukienlophoc");

            entity.HasIndex(e => e.DangSuKien, "DangSuKien");

            entity.HasIndex(e => e.MaBaiTap, "MaBaiTap");

            entity.HasIndex(e => e.MaLopHoc, "MaLopHoc1");

            entity.HasIndex(e => e.MaNguoiDung, "MaNguoiDung4");

            entity.HasIndex(e => e.MaTrangThai, "MaTrangThai6");

            entity.Property(e => e.MaSuKien)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.DangSuKien).HasComment("Trỏ về ChiTietDanhMuc");
            entity.Property(e => e.HanNop).HasColumnType("datetime");
            entity.Property(e => e.MaBaiTap)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaLopHoc)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaNguoiDung)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaTrangThai).HasComment("Trỏ về ChiTietDanhMuc");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NoiDung).HasColumnType("text");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.DangSuKienNavigation).WithMany(p => p.SukienlophocDangSuKienNavigations)
                .HasForeignKey(d => d.DangSuKien)
                .HasConstraintName("sukienlophoc_ibfk_3");

            entity.HasOne(d => d.MaBaiTapNavigation).WithMany(p => p.Sukienlophocs)
                .HasForeignKey(d => d.MaBaiTap)
                .HasConstraintName("sukienlophoc_ibfk_4");

            entity.HasOne(d => d.MaLopHocNavigation).WithMany(p => p.Sukienlophocs)
                .HasForeignKey(d => d.MaLopHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sukienlophoc_ibfk_1");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.Sukienlophocs)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sukienlophoc_ibfk_2");

            entity.HasOne(d => d.MaTrangThaiNavigation).WithMany(p => p.SukienlophocMaTrangThaiNavigations)
                .HasForeignKey(d => d.MaTrangThai)
                .HasConstraintName("sukienlophoc_ibfk_5");
        });

        modelBuilder.Entity<Tailieu>(entity =>
        {
            entity.HasKey(e => e.MaTaiLieu).HasName("PRIMARY");

            entity.ToTable("tailieu");

            entity.HasIndex(e => e.MaChuongHoc, "MaChuongHoc");

            entity.Property(e => e.MaTaiLieu)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.LinkTaiLieu).HasMaxLength(255);
            entity.Property(e => e.MaChuongHoc)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MoTa).HasColumnType("text");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.TenTaiLieu).HasMaxLength(255);
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaChuongHocNavigation).WithMany(p => p.Tailieus)
                .HasForeignKey(d => d.MaChuongHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tailieu_ibfk_1");
        });

        modelBuilder.Entity<Tainguyenluutru>(entity =>
        {
            entity.HasKey(e => e.MaTaiNguyen).HasName("PRIMARY");

            entity.ToTable("tainguyenluutru");

            entity.HasIndex(e => e.MaNguoiDung, "MaNguoiDung5");

            entity.Property(e => e.MaTaiNguyen)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.Link).HasMaxLength(255);
            entity.Property(e => e.MaNguoiDung)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.TenTaiNguyen).HasMaxLength(255);
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.Tainguyenluutrus)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tainguyenluutru_ibfk_1");
        });

        modelBuilder.Entity<Thanhvienhoithoai>(entity =>
        {
            entity.HasKey(e => new { e.MaHoiThoai, e.MaNguoiDung })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("thanhvienhoithoai");

            entity.HasIndex(e => new { e.MaNguoiDung, e.DaXoa, e.TrangThai }, "idx_ThanhVienHoiThoai_User_Active");

            entity.Property(e => e.MaHoiThoai)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaNguoiDung)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaHoiThoaiNavigation).WithMany(p => p.Thanhvienhoithoais)
                .HasForeignKey(d => d.MaHoiThoai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("thanhvienhoithoai_ibfk_1");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.Thanhvienhoithoais)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("thanhvienhoithoai_ibfk_2");
        });

        modelBuilder.Entity<Thongbao>(entity =>
        {
            entity.HasKey(e => e.MaThongBao).HasName("PRIMARY");

            entity.ToTable("thongbao");

            entity.Property(e => e.MaThongBao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NoiDung).HasColumnType("text");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TieuDe).HasMaxLength(255);
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");
        });

        modelBuilder.Entity<Tiethoc>(entity =>
        {
            entity.HasKey(e => e.MaTiet).HasName("PRIMARY");

            entity.ToTable("tiethoc");

            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.GioBatDau).HasColumnType("time");
            entity.Property(e => e.GioKetThuc).HasColumnType("time");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.TenTiet).HasMaxLength(50);
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");
        });

        modelBuilder.Entity<Tinnhan>(entity =>
        {
            entity.HasKey(e => e.MaTinNhan).HasName("PRIMARY");

            entity.ToTable("tinnhan");

            entity.HasIndex(e => e.MaHoiThoai, "MaHoiThoai");

            entity.HasIndex(e => e.MaNguoiDungGui, "MaNguoiDungGui");

            entity.Property(e => e.MaTinNhan)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaDoc).HasDefaultValueSql("'0'");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.MaHoiThoai)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaNguoiDungGui)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NoiDung).HasColumnType("text");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaHoiThoaiNavigation).WithMany(p => p.Tinnhans)
                .HasForeignKey(d => d.MaHoiThoai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tinnhan_ibfk_1");

            entity.HasOne(d => d.MaNguoiDungGuiNavigation).WithMany(p => p.Tinnhans)
                .HasForeignKey(d => d.MaNguoiDungGui)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tinnhan_ibfk_2");
        });

        modelBuilder.Entity<Vaitro>(entity =>
        {
            entity.HasKey(e => e.MaVaiTro).HasName("PRIMARY");

            entity.ToTable("vaitro");

            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.TenVaiTro).HasMaxLength(50);
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasMany(d => d.MaChucNangs).WithMany(p => p.MaVaiTros)
                .UsingEntity<Dictionary<string, object>>(
                    "Vaitrochucnang",
                    r => r.HasOne<Chucnanghethong>().WithMany()
                        .HasForeignKey("MaChucNang")
                        .HasConstraintName("fk_vtc_chucnang"),
                    l => l.HasOne<Vaitro>().WithMany()
                        .HasForeignKey("MaVaiTro")
                        .HasConstraintName("fk_vtc_vaitro"),
                    j =>
                    {
                        j.HasKey("MaVaiTro", "MaChucNang")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("vaitrochucnang");
                        j.HasIndex(new[] { "MaChucNang" }, "fk_vtc_chucnang");
                        j.IndexerProperty<int>("MaVaiTro").HasColumnName("maVaiTro");
                        j.IndexerProperty<int>("MaChucNang").HasColumnName("maChucNang");
                    });
        });

        modelBuilder.Entity<Vaitroquyen>(entity =>
        {
            entity.HasKey(e => new { e.MaVaiTro, e.MaQuyen })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("vaitroquyen");

            entity.HasIndex(e => new { e.MaQuyen, e.DaXoa, e.TrangThai }, "idx_VaiTroQuyen_Quyen_Active");

            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.MaQuyenNavigation).WithMany(p => p.Vaitroquyens)
                .HasForeignKey(d => d.MaQuyen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vaitroquyen_ibfk_2");

            entity.HasOne(d => d.MaVaiTroNavigation).WithMany(p => p.Vaitroquyens)
                .HasForeignKey(d => d.MaVaiTro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vaitroquyen_ibfk_1");
        });

        modelBuilder.Entity<Yeucaulichday>(entity =>
        {
            entity.HasKey(e => e.MaYeuCau).HasName("PRIMARY");

            entity.ToTable("yeucaulichday");

            entity.HasIndex(e => e.MaBuoiHoc, "MaBuoiHoc");

            entity.HasIndex(e => e.MaGiangVien, "MaGiangVien");

            entity.HasIndex(e => e.MaLopHoc, "MaLopHoc2");

            entity.HasIndex(e => e.MaTietBatDauDeXuat, "MaTietBatDauDeXuat");

            entity.HasIndex(e => e.MaTietKetThucDeXuat, "MaTietKetThucDeXuat");

            entity.Property(e => e.MaYeuCau)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.DaXoa).HasDefaultValueSql("'0'");
            entity.Property(e => e.LyDo).HasColumnType("text");
            entity.Property(e => e.MaBuoiHoc)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaGiangVien)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MaLopHoc)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiSua)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.NguoiTao)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ThoiGianSua).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasDefaultValueSql("'1'");
            entity.Property(e => e.TrangThaiDuyet).HasDefaultValueSql("'0'");
            entity.Property(e => e.GhiChuAdmin).HasColumnType("longtext");
            // MaPhongHocDeXuat được để charset mặc định (utf8mb4) của bảng, khớp với
            // phonghoc.MaPhongHoc thực tế trong DB — không dùng override "ascii" như các
            // cột GUID khác trong entity này vì sẽ gây lỗi FK (errno 150) lúc migrate.
            entity.Property(e => e.MaPhongHocDeXuat);

            entity.HasOne(d => d.MaPhongHocDeXuatNavigation).WithMany()
                .HasForeignKey(d => d.MaPhongHocDeXuat)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_yeucaulichday_phonghoc_MaPhongHocDeXuat");

            entity.HasOne(d => d.MaBuoiHocNavigation).WithMany(p => p.Yeucaulichdays)
                .HasForeignKey(d => d.MaBuoiHoc)
                .HasConstraintName("yeucaulichday_ibfk_3");

            entity.HasOne(d => d.MaGiangVienNavigation).WithMany(p => p.Yeucaulichdays)
                .HasForeignKey(d => d.MaGiangVien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("yeucaulichday_ibfk_1");

            entity.HasOne(d => d.MaLopHocNavigation).WithMany(p => p.Yeucaulichdays)
                .HasForeignKey(d => d.MaLopHoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("yeucaulichday_ibfk_2");

            entity.HasOne(d => d.MaTietBatDauDeXuatNavigation).WithMany(p => p.YeucaulichdayMaTietBatDauDeXuatNavigations)
                .HasForeignKey(d => d.MaTietBatDauDeXuat)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("yeucaulichday_ibfk_4");

            entity.HasOne(d => d.MaTietKetThucDeXuatNavigation).WithMany(p => p.YeucaulichdayMaTietKetThucDeXuatNavigations)
                .HasForeignKey(d => d.MaTietKetThucDeXuat)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("yeucaulichday_ibfk_5");

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
