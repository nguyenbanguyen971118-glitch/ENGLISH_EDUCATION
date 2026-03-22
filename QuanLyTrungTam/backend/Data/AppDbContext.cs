using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chucnang> Chucnangs { get; set; }
    public virtual DbSet<Giangvien> Giangviens { get; set; }
    public virtual DbSet<Hocsinh> Hocsinhs { get; set; }
    public virtual DbSet<Nguoidung> Nguoidungs { get; set; }
    public virtual DbSet<Nguoidungvaitro> Nguoidungvaitros { get; set; }
    public virtual DbSet<Quyen> Quyens { get; set; }
    public virtual DbSet<Vaitro> Vaitros { get; set; }
    public virtual DbSet<Vaitroquyen> Vaitroquyens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chucnang>(entity =>
        {
            entity.HasKey(e => e.MaChucNang);
            entity.ToTable("chucnang");

            entity.Property(e => e.TenChucNang).HasMaxLength(100);
            entity.Property(e => e.MoTa).HasMaxLength(255);
        });

        modelBuilder.Entity<Nguoidung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung);
            entity.ToTable("nguoidung");

            entity.Property(e => e.MaNguoiDung).HasColumnType("char(36)");
            entity.Property(e => e.TenDangNhap).HasMaxLength(50);
            entity.Property(e => e.MatKhauHash).HasMaxLength(255);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.AnhDaiDien).HasMaxLength(255);
            entity.Property(e => e.TokenXacMinh).HasMaxLength(255);

            entity.Ignore(e => e.Binhluans);
            entity.Ignore(e => e.MaTrangThaiNavigation);
            entity.Ignore(e => e.Nguoinhanthongbaos);
            entity.Ignore(e => e.Phuhuynh);
            entity.Ignore(e => e.Sukienlophocs);
            entity.Ignore(e => e.Tainguyenluutrus);
            entity.Ignore(e => e.Thanhvienhoithoais);
            entity.Ignore(e => e.Tinnhans);
        });

        modelBuilder.Entity<Vaitro>(entity =>
        {
            entity.HasKey(e => e.MaVaiTro);
            entity.ToTable("vaitro");

            entity.Property(e => e.TenVaiTro).HasMaxLength(50);

            // Ignore legacy many-to-many nav that is not used by current flow.
            entity.Ignore(e => e.MaChucNangs);
        });

        modelBuilder.Entity<Quyen>(entity =>
        {
            entity.HasKey(e => e.MaQuyen);
            entity.ToTable("quyen");

            entity.Property(e => e.TenQuyen).HasMaxLength(100);
            entity.Property(e => e.MoTa).HasMaxLength(255);

            entity.HasOne(d => d.MaChucNangNavigation)
                .WithMany(p => p.Quyens)
                .HasForeignKey(d => d.MaChucNang)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Nguoidungvaitro>(entity =>
        {
            entity.HasKey(e => new { e.MaNguoiDung, e.MaVaiTro });
            entity.ToTable("nguoidungvaitro");

            entity.Property(e => e.MaNguoiDung).HasColumnType("char(36)");

            entity.HasOne(d => d.MaNguoiDungNavigation)
                .WithMany(p => p.Nguoidungvaitros)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.MaVaiTroNavigation)
                .WithMany(p => p.Nguoidungvaitros)
                .HasForeignKey(d => d.MaVaiTro)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Vaitroquyen>(entity =>
        {
            entity.HasKey(e => new { e.MaVaiTro, e.MaQuyen });
            entity.ToTable("vaitroquyen");

            entity.HasOne(d => d.MaVaiTroNavigation)
                .WithMany(p => p.Vaitroquyens)
                .HasForeignKey(d => d.MaVaiTro)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.MaQuyenNavigation)
                .WithMany(p => p.Vaitroquyens)
                .HasForeignKey(d => d.MaQuyen)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Hocsinh>(entity =>
        {
            entity.HasKey(e => e.MaHocSinh);
            entity.ToTable("hocsinh");

            entity.Property(e => e.MaHocSinh).HasColumnType("char(36)");
            entity.Property(e => e.MaNguoiDung).HasColumnType("char(36)");

            entity.HasOne(d => d.MaNguoiDungNavigation)
                .WithOne(p => p.Hocsinh)
                .HasForeignKey<Hocsinh>(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.Ignore(e => e.Diemdanhs);
            entity.Ignore(e => e.Hocsinhlophocs);
            entity.Ignore(e => e.Nguoinhansukiens);
            entity.Ignore(e => e.Nopbais);
            entity.Ignore(e => e.Phuhuynhhocsinhs);
        });

        modelBuilder.Entity<Giangvien>(entity =>
        {
            entity.HasKey(e => e.MaGiangVien);
            entity.ToTable("giangvien");

            entity.Property(e => e.MaGiangVien).HasColumnType("char(36)");
            entity.Property(e => e.MaNguoiDung).HasColumnType("char(36)");
            entity.Property(e => e.TrinhDoChuyenMon).HasMaxLength(255);

            entity.HasOne(d => d.MaNguoiDungNavigation)
                .WithOne(p => p.Giangvien)
                .HasForeignKey<Giangvien>(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.Ignore(e => e.Giangvienlophocs);
            entity.Ignore(e => e.Yeucaulichdays);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
