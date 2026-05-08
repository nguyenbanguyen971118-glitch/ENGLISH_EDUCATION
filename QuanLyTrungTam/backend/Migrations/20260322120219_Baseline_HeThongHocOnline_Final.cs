using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class Baseline_HeThongHocOnline_Final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "chucnang",
                columns: table => new
                {
                    MaChucNang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenChucNang = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MoTa = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaChucNang);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "chucnanghethong",
                columns: table => new
                {
                    maChucNang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    maChucNangCode = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tenChucNang = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    maTrang = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tenTrang = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    thuTu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.maChucNang);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "hoithoai",
                columns: table => new
                {
                    MaHoiThoai = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TieuDe = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaHoiThoai);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "nhomdanhmuc",
                columns: table => new
                {
                    MaNhom = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MaCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "VD: LOAI_PHONG, TRANG_THAI_KHOA_HOC", collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TenNhom = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GhiChu = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaNhom);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "thongbao",
                columns: table => new
                {
                    MaThongBao = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TieuDe = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NoiDung = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaThongBao);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "tiethoc",
                columns: table => new
                {
                    MaTiet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenTiet = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GioBatDau = table.Column<TimeOnly>(type: "time", nullable: false),
                    GioKetThuc = table.Column<TimeOnly>(type: "time", nullable: false),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaTiet);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "vaitro",
                columns: table => new
                {
                    MaVaiTro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenVaiTro = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaVaiTro);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "quyen",
                columns: table => new
                {
                    MaQuyen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MaChucNang = table.Column<int>(type: "int", nullable: false),
                    TenQuyen = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MoTa = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaQuyen);
                    table.ForeignKey(
                        name: "quyen_ibfk_1",
                        column: x => x.MaChucNang,
                        principalTable: "chucnang",
                        principalColumn: "MaChucNang");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "chitietdanhmuc",
                columns: table => new
                {
                    MaChiTiet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MaNhom = table.Column<int>(type: "int", nullable: false),
                    MaCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "VD: PH_LAB, TKH_OPEN", collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TenChiTiet = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ThuTu = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'0'"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaChiTiet);
                    table.ForeignKey(
                        name: "chitietdanhmuc_ibfk_1",
                        column: x => x.MaNhom,
                        principalTable: "nhomdanhmuc",
                        principalColumn: "MaNhom");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "vaitrochucnang",
                columns: table => new
                {
                    maVaiTro = table.Column<int>(type: "int", nullable: false),
                    maChucNang = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.maVaiTro, x.maChucNang })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "fk_vtc_chucnang",
                        column: x => x.maChucNang,
                        principalTable: "chucnanghethong",
                        principalColumn: "maChucNang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_vtc_vaitro",
                        column: x => x.maVaiTro,
                        principalTable: "vaitro",
                        principalColumn: "MaVaiTro",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "vaitroquyen",
                columns: table => new
                {
                    MaVaiTro = table.Column<int>(type: "int", nullable: false),
                    MaQuyen = table.Column<int>(type: "int", nullable: false),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.MaVaiTro, x.MaQuyen })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "vaitroquyen_ibfk_1",
                        column: x => x.MaVaiTro,
                        principalTable: "vaitro",
                        principalColumn: "MaVaiTro");
                    table.ForeignKey(
                        name: "vaitroquyen_ibfk_2",
                        column: x => x.MaQuyen,
                        principalTable: "quyen",
                        principalColumn: "MaQuyen");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "khoahoc",
                columns: table => new
                {
                    MaKhoaHoc = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenKhoaHoc = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MoTa = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GiaCoBan = table.Column<decimal>(type: "decimal(14,2)", precision: 14, scale: 2, nullable: true),
                    MaTrangThai = table.Column<int>(type: "int", nullable: true, comment: "Trỏ về ChiTietDanhMuc"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaKhoaHoc);
                    table.ForeignKey(
                        name: "khoahoc_ibfk_1",
                        column: x => x.MaTrangThai,
                        principalTable: "chitietdanhmuc",
                        principalColumn: "MaChiTiet");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "lophoc",
                columns: table => new
                {
                    MaLopHoc = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenLop = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NgayBatDau = table.Column<DateOnly>(type: "date", nullable: true),
                    NgayKetThuc = table.Column<DateOnly>(type: "date", nullable: true),
                    SiSoHienTai = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'0'"),
                    SiSoToiDa = table.Column<int>(type: "int", nullable: true),
                    MaTrangThai = table.Column<int>(type: "int", nullable: true, comment: "Trỏ về ChiTietDanhMuc"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaLopHoc);
                    table.ForeignKey(
                        name: "lophoc_ibfk_1",
                        column: x => x.MaTrangThai,
                        principalTable: "chitietdanhmuc",
                        principalColumn: "MaChiTiet");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "nguoidung",
                columns: table => new
                {
                    MaNguoiDung = table.Column<Guid>(type: "char(36)", nullable: false, comment: "GUID cho tài khoản", collation: "ascii_general_ci"),
                    LoaiTaiKhoan = table.Column<sbyte>(type: "tinyint", nullable: false, comment: "1: Admin, 2: GiangVien, 3: HocSinh, 4: PhuHuynh"),
                    TenDangNhap = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MatKhauHash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HoTen = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AnhDaiDien = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaTrangThai = table.Column<int>(type: "int", nullable: true, comment: "Trỏ về ChiTietDanhMuc"),
                    TokenXacMinh = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DaXacMinhEmail = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaNguoiDung);
                    table.ForeignKey(
                        name: "nguoidung_ibfk_1",
                        column: x => x.MaTrangThai,
                        principalTable: "chitietdanhmuc",
                        principalColumn: "MaChiTiet");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "phonghoc",
                columns: table => new
                {
                    MaPhongHoc = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenPhong = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SucChua = table.Column<int>(type: "int", nullable: true),
                    LoaiPhong = table.Column<int>(type: "int", nullable: true, comment: "Trỏ về ChiTietDanhMuc"),
                    Link = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaPhongHoc);
                    table.ForeignKey(
                        name: "phonghoc_ibfk_1",
                        column: x => x.LoaiPhong,
                        principalTable: "chitietdanhmuc",
                        principalColumn: "MaChiTiet");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "baitap",
                columns: table => new
                {
                    MaBaiTap = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaKhoaHoc = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenBaiTap = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MoTa = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LoaiBaiTap = table.Column<int>(type: "int", nullable: true, comment: "Trỏ về ChiTietDanhMuc"),
                    ThoiGianLamBai = table.Column<int>(type: "int", nullable: true),
                    DiemToiDa = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaBaiTap);
                    table.ForeignKey(
                        name: "baitap_ibfk_1",
                        column: x => x.MaKhoaHoc,
                        principalTable: "khoahoc",
                        principalColumn: "MaKhoaHoc");
                    table.ForeignKey(
                        name: "baitap_ibfk_2",
                        column: x => x.LoaiBaiTap,
                        principalTable: "chitietdanhmuc",
                        principalColumn: "MaChiTiet");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "chuonghoc",
                columns: table => new
                {
                    MaChuong = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaKhoaHoc = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenChuong = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MoTa = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ThuTu = table.Column<int>(type: "int", nullable: true),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaChuong);
                    table.ForeignKey(
                        name: "chuonghoc_ibfk_1",
                        column: x => x.MaKhoaHoc,
                        principalTable: "khoahoc",
                        principalColumn: "MaKhoaHoc");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "nganhangcauhoi",
                columns: table => new
                {
                    MaCauHoi = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaKhoaHoc = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LoaiCauHoi = table.Column<int>(type: "int", nullable: true, comment: "Trỏ về ChiTietDanhMuc"),
                    MucDo = table.Column<int>(type: "int", nullable: true, comment: "Trỏ về ChiTietDanhMuc"),
                    MucDichSuDung = table.Column<sbyte>(type: "tinyint", nullable: true, defaultValueSql: "'1'"),
                    NoiDungCauHoi = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GiaiThichDapAn = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaCauHoi);
                    table.ForeignKey(
                        name: "nganhangcauhoi_ibfk_1",
                        column: x => x.MaKhoaHoc,
                        principalTable: "khoahoc",
                        principalColumn: "MaKhoaHoc");
                    table.ForeignKey(
                        name: "nganhangcauhoi_ibfk_2",
                        column: x => x.LoaiCauHoi,
                        principalTable: "chitietdanhmuc",
                        principalColumn: "MaChiTiet");
                    table.ForeignKey(
                        name: "nganhangcauhoi_ibfk_3",
                        column: x => x.MucDo,
                        principalTable: "chitietdanhmuc",
                        principalColumn: "MaChiTiet");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "chitietkhoahoc_lophoc",
                columns: table => new
                {
                    MaKhoaHoc = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaLopHoc = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    GhiChu = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.MaKhoaHoc, x.MaLopHoc })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "chitietkhoahoc_lophoc_ibfk_1",
                        column: x => x.MaKhoaHoc,
                        principalTable: "khoahoc",
                        principalColumn: "MaKhoaHoc");
                    table.ForeignKey(
                        name: "chitietkhoahoc_lophoc_ibfk_2",
                        column: x => x.MaLopHoc,
                        principalTable: "lophoc",
                        principalColumn: "MaLopHoc");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "giangvien",
                columns: table => new
                {
                    MaGiangVien = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaNguoiDung = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SoDienThoai = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    QueQuan = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TrinhDoChuyenMon = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HocVi = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KinhNghiemGiangDay = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaGiangVien);
                    table.ForeignKey(
                        name: "giangvien_ibfk_1",
                        column: x => x.MaNguoiDung,
                        principalTable: "nguoidung",
                        principalColumn: "MaNguoiDung");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "hocsinh",
                columns: table => new
                {
                    MaHocSinh = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaNguoiDung = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    NgaySinh = table.Column<DateOnly>(type: "date", nullable: true),
                    QueQuan = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SoDienThoaiNguoiThan = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TruongDangTheoHoc = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaHocSinh);
                    table.ForeignKey(
                        name: "hocsinh_ibfk_1",
                        column: x => x.MaNguoiDung,
                        principalTable: "nguoidung",
                        principalColumn: "MaNguoiDung");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "nguoidungvaitro",
                columns: table => new
                {
                    MaNguoiDung = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaVaiTro = table.Column<int>(type: "int", nullable: false),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.MaNguoiDung, x.MaVaiTro })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "nguoidungvaitro_ibfk_1",
                        column: x => x.MaNguoiDung,
                        principalTable: "nguoidung",
                        principalColumn: "MaNguoiDung");
                    table.ForeignKey(
                        name: "nguoidungvaitro_ibfk_2",
                        column: x => x.MaVaiTro,
                        principalTable: "vaitro",
                        principalColumn: "MaVaiTro");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "nguoinhanthongbao",
                columns: table => new
                {
                    MaThongBao = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaNguoiDung = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DaDoc = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'"),
                    NgayDoc = table.Column<DateTime>(type: "datetime", nullable: true),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.MaThongBao, x.MaNguoiDung })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "nguoinhanthongbao_ibfk_1",
                        column: x => x.MaThongBao,
                        principalTable: "thongbao",
                        principalColumn: "MaThongBao");
                    table.ForeignKey(
                        name: "nguoinhanthongbao_ibfk_2",
                        column: x => x.MaNguoiDung,
                        principalTable: "nguoidung",
                        principalColumn: "MaNguoiDung");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "phuhuynh",
                columns: table => new
                {
                    MaPhuHuynh = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaNguoiDung = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SoDienThoai = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DiaChiLienHe = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NgheNghiep = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaPhuHuynh);
                    table.ForeignKey(
                        name: "phuhuynh_ibfk_1",
                        column: x => x.MaNguoiDung,
                        principalTable: "nguoidung",
                        principalColumn: "MaNguoiDung");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "tainguyenluutru",
                columns: table => new
                {
                    MaTaiNguyen = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaNguoiDung = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenTaiNguyen = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Link = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaTaiNguyen);
                    table.ForeignKey(
                        name: "tainguyenluutru_ibfk_1",
                        column: x => x.MaNguoiDung,
                        principalTable: "nguoidung",
                        principalColumn: "MaNguoiDung");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "thanhvienhoithoai",
                columns: table => new
                {
                    MaHoiThoai = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaNguoiDung = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.MaHoiThoai, x.MaNguoiDung })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "thanhvienhoithoai_ibfk_1",
                        column: x => x.MaHoiThoai,
                        principalTable: "hoithoai",
                        principalColumn: "MaHoiThoai");
                    table.ForeignKey(
                        name: "thanhvienhoithoai_ibfk_2",
                        column: x => x.MaNguoiDung,
                        principalTable: "nguoidung",
                        principalColumn: "MaNguoiDung");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "tinnhan",
                columns: table => new
                {
                    MaTinNhan = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaHoiThoai = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaNguoiDungGui = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    NoiDung = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DaDoc = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaTinNhan);
                    table.ForeignKey(
                        name: "tinnhan_ibfk_1",
                        column: x => x.MaHoiThoai,
                        principalTable: "hoithoai",
                        principalColumn: "MaHoiThoai");
                    table.ForeignKey(
                        name: "tinnhan_ibfk_2",
                        column: x => x.MaNguoiDungGui,
                        principalTable: "nguoidung",
                        principalColumn: "MaNguoiDung");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "buoihoc",
                columns: table => new
                {
                    MaBuoiHoc = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaLopHoc = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaPhongHoc = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    NgayHoc = table.Column<DateOnly>(type: "date", nullable: false),
                    MaTietBatDau = table.Column<int>(type: "int", nullable: false),
                    MaTietKetThuc = table.Column<int>(type: "int", nullable: false),
                    TieuDe = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NoiDung = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaBuoiHoc);
                    table.ForeignKey(
                        name: "buoihoc_ibfk_1",
                        column: x => x.MaLopHoc,
                        principalTable: "lophoc",
                        principalColumn: "MaLopHoc");
                    table.ForeignKey(
                        name: "buoihoc_ibfk_2",
                        column: x => x.MaPhongHoc,
                        principalTable: "phonghoc",
                        principalColumn: "MaPhongHoc");
                    table.ForeignKey(
                        name: "buoihoc_ibfk_3",
                        column: x => x.MaTietBatDau,
                        principalTable: "tiethoc",
                        principalColumn: "MaTiet");
                    table.ForeignKey(
                        name: "buoihoc_ibfk_4",
                        column: x => x.MaTietKetThuc,
                        principalTable: "tiethoc",
                        principalColumn: "MaTiet");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "sukienlophoc",
                columns: table => new
                {
                    MaSuKien = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaLopHoc = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaNguoiDung = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DangSuKien = table.Column<int>(type: "int", nullable: true, comment: "Trỏ về ChiTietDanhMuc"),
                    MaBaiTap = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    NoiDung = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HanNop = table.Column<DateTime>(type: "datetime", nullable: true),
                    MaTrangThai = table.Column<int>(type: "int", nullable: true, comment: "Trỏ về ChiTietDanhMuc"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaSuKien);
                    table.ForeignKey(
                        name: "sukienlophoc_ibfk_1",
                        column: x => x.MaLopHoc,
                        principalTable: "lophoc",
                        principalColumn: "MaLopHoc");
                    table.ForeignKey(
                        name: "sukienlophoc_ibfk_2",
                        column: x => x.MaNguoiDung,
                        principalTable: "nguoidung",
                        principalColumn: "MaNguoiDung");
                    table.ForeignKey(
                        name: "sukienlophoc_ibfk_3",
                        column: x => x.DangSuKien,
                        principalTable: "chitietdanhmuc",
                        principalColumn: "MaChiTiet");
                    table.ForeignKey(
                        name: "sukienlophoc_ibfk_4",
                        column: x => x.MaBaiTap,
                        principalTable: "baitap",
                        principalColumn: "MaBaiTap");
                    table.ForeignKey(
                        name: "sukienlophoc_ibfk_5",
                        column: x => x.MaTrangThai,
                        principalTable: "chitietdanhmuc",
                        principalColumn: "MaChiTiet");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "tailieu",
                columns: table => new
                {
                    MaTaiLieu = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaChuongHoc = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenTaiLieu = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LinkTaiLieu = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MoTa = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaTaiLieu);
                    table.ForeignKey(
                        name: "tailieu_ibfk_1",
                        column: x => x.MaChuongHoc,
                        principalTable: "chuonghoc",
                        principalColumn: "MaChuong");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "baitapcauhoi",
                columns: table => new
                {
                    MaBaiTap = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaCauHoi = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DiemCuaCau = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    ThuTu = table.Column<int>(type: "int", nullable: true),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.MaBaiTap, x.MaCauHoi })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "baitapcauhoi_ibfk_1",
                        column: x => x.MaBaiTap,
                        principalTable: "baitap",
                        principalColumn: "MaBaiTap");
                    table.ForeignKey(
                        name: "baitapcauhoi_ibfk_2",
                        column: x => x.MaCauHoi,
                        principalTable: "nganhangcauhoi",
                        principalColumn: "MaCauHoi");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "dapan",
                columns: table => new
                {
                    MaDapAn = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaCauHoi = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    NoiDungDapAn = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LaDapAnDung = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaDapAn);
                    table.ForeignKey(
                        name: "dapan_ibfk_1",
                        column: x => x.MaCauHoi,
                        principalTable: "nganhangcauhoi",
                        principalColumn: "MaCauHoi");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "giangvienlophoc",
                columns: table => new
                {
                    MaLopHoc = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaGiangVien = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LoaiVaiTro = table.Column<sbyte>(type: "tinyint", nullable: true, defaultValueSql: "'1'"),
                    NgayThamGia = table.Column<DateOnly>(type: "date", nullable: true),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.MaLopHoc, x.MaGiangVien })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "giangvienlophoc_ibfk_1",
                        column: x => x.MaLopHoc,
                        principalTable: "lophoc",
                        principalColumn: "MaLopHoc");
                    table.ForeignKey(
                        name: "giangvienlophoc_ibfk_2",
                        column: x => x.MaGiangVien,
                        principalTable: "giangvien",
                        principalColumn: "MaGiangVien");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "hocsinhlophoc",
                columns: table => new
                {
                    MaLopHoc = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaHocSinh = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaTrangThai = table.Column<int>(type: "int", nullable: true, comment: "Trỏ về ChiTietDanhMuc"),
                    NgayThamGia = table.Column<DateOnly>(type: "date", nullable: true),
                    NgayRoiLop = table.Column<DateOnly>(type: "date", nullable: true),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.MaLopHoc, x.MaHocSinh })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "hocsinhlophoc_ibfk_1",
                        column: x => x.MaLopHoc,
                        principalTable: "lophoc",
                        principalColumn: "MaLopHoc");
                    table.ForeignKey(
                        name: "hocsinhlophoc_ibfk_2",
                        column: x => x.MaHocSinh,
                        principalTable: "hocsinh",
                        principalColumn: "MaHocSinh");
                    table.ForeignKey(
                        name: "hocsinhlophoc_ibfk_3",
                        column: x => x.MaTrangThai,
                        principalTable: "chitietdanhmuc",
                        principalColumn: "MaChiTiet");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "phuhuynhhocsinh",
                columns: table => new
                {
                    MaPhuHuynh = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaHocSinh = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaQuanHe = table.Column<int>(type: "int", nullable: true, comment: "Trỏ về ChiTietDanhMuc"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.MaPhuHuynh, x.MaHocSinh })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "phuhuynhhocsinh_ibfk_1",
                        column: x => x.MaPhuHuynh,
                        principalTable: "phuhuynh",
                        principalColumn: "MaPhuHuynh");
                    table.ForeignKey(
                        name: "phuhuynhhocsinh_ibfk_2",
                        column: x => x.MaHocSinh,
                        principalTable: "hocsinh",
                        principalColumn: "MaHocSinh");
                    table.ForeignKey(
                        name: "phuhuynhhocsinh_ibfk_3",
                        column: x => x.MaQuanHe,
                        principalTable: "chitietdanhmuc",
                        principalColumn: "MaChiTiet");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "diemdanh",
                columns: table => new
                {
                    MaBuoiHoc = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaHocSinh = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaTrangThai = table.Column<int>(type: "int", nullable: true, comment: "Trỏ về ChiTietDanhMuc"),
                    GhiChu = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.MaBuoiHoc, x.MaHocSinh })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "diemdanh_ibfk_1",
                        column: x => x.MaBuoiHoc,
                        principalTable: "buoihoc",
                        principalColumn: "MaBuoiHoc");
                    table.ForeignKey(
                        name: "diemdanh_ibfk_2",
                        column: x => x.MaHocSinh,
                        principalTable: "hocsinh",
                        principalColumn: "MaHocSinh");
                    table.ForeignKey(
                        name: "diemdanh_ibfk_3",
                        column: x => x.MaTrangThai,
                        principalTable: "chitietdanhmuc",
                        principalColumn: "MaChiTiet");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "yeucaulichday",
                columns: table => new
                {
                    MaYeuCau = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaGiangVien = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaLopHoc = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaBuoiHoc = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    NgayHocDeXuat = table.Column<DateOnly>(type: "date", nullable: false),
                    MaTietBatDauDeXuat = table.Column<int>(type: "int", nullable: false),
                    MaTietKetThucDeXuat = table.Column<int>(type: "int", nullable: false),
                    LyDo = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TrangThaiDuyet = table.Column<sbyte>(type: "tinyint", nullable: true, defaultValueSql: "'0'"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaYeuCau);
                    table.ForeignKey(
                        name: "yeucaulichday_ibfk_1",
                        column: x => x.MaGiangVien,
                        principalTable: "giangvien",
                        principalColumn: "MaGiangVien");
                    table.ForeignKey(
                        name: "yeucaulichday_ibfk_2",
                        column: x => x.MaLopHoc,
                        principalTable: "lophoc",
                        principalColumn: "MaLopHoc");
                    table.ForeignKey(
                        name: "yeucaulichday_ibfk_3",
                        column: x => x.MaBuoiHoc,
                        principalTable: "buoihoc",
                        principalColumn: "MaBuoiHoc");
                    table.ForeignKey(
                        name: "yeucaulichday_ibfk_4",
                        column: x => x.MaTietBatDauDeXuat,
                        principalTable: "tiethoc",
                        principalColumn: "MaTiet");
                    table.ForeignKey(
                        name: "yeucaulichday_ibfk_5",
                        column: x => x.MaTietKetThucDeXuat,
                        principalTable: "tiethoc",
                        principalColumn: "MaTiet");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "binhluan",
                columns: table => new
                {
                    MaBinhLuan = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaSuKien = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaNguoiDung = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    NoiDung = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaBinhLuan);
                    table.ForeignKey(
                        name: "binhluan_ibfk_1",
                        column: x => x.MaSuKien,
                        principalTable: "sukienlophoc",
                        principalColumn: "MaSuKien");
                    table.ForeignKey(
                        name: "binhluan_ibfk_2",
                        column: x => x.MaNguoiDung,
                        principalTable: "nguoidung",
                        principalColumn: "MaNguoiDung");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "dinhkem",
                columns: table => new
                {
                    MaSuKien = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaTaiNguyen = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.MaSuKien, x.MaTaiNguyen })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "dinhkem_ibfk_1",
                        column: x => x.MaSuKien,
                        principalTable: "sukienlophoc",
                        principalColumn: "MaSuKien");
                    table.ForeignKey(
                        name: "dinhkem_ibfk_2",
                        column: x => x.MaTaiNguyen,
                        principalTable: "tainguyenluutru",
                        principalColumn: "MaTaiNguyen");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "nguoinhansukien",
                columns: table => new
                {
                    MaSuKien = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaHocSinh = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.MaSuKien, x.MaHocSinh })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "nguoinhansukien_ibfk_1",
                        column: x => x.MaSuKien,
                        principalTable: "sukienlophoc",
                        principalColumn: "MaSuKien");
                    table.ForeignKey(
                        name: "nguoinhansukien_ibfk_2",
                        column: x => x.MaHocSinh,
                        principalTable: "hocsinh",
                        principalColumn: "MaHocSinh");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "nopbai",
                columns: table => new
                {
                    MaNopBai = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaSuKien = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaHocSinh = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ThoiGianBatDau = table.Column<DateTime>(type: "datetime", nullable: true),
                    ThoiGianNop = table.Column<DateTime>(type: "datetime", nullable: true),
                    NhanXetGiaoVien = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DiemSo = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    LanNop = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'1'"),
                    MaTrangThai = table.Column<int>(type: "int", nullable: true, comment: "Trỏ về ChiTietDanhMuc"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaNopBai);
                    table.ForeignKey(
                        name: "nopbai_ibfk_1",
                        column: x => x.MaSuKien,
                        principalTable: "sukienlophoc",
                        principalColumn: "MaSuKien");
                    table.ForeignKey(
                        name: "nopbai_ibfk_2",
                        column: x => x.MaHocSinh,
                        principalTable: "hocsinh",
                        principalColumn: "MaHocSinh");
                    table.ForeignKey(
                        name: "nopbai_ibfk_3",
                        column: x => x.MaTrangThai,
                        principalTable: "chitietdanhmuc",
                        principalColumn: "MaChiTiet");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "chitietnopbai",
                columns: table => new
                {
                    MaChiTiet = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaNopBai = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaCauHoi = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaDapAnChon = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CauTraLoiDienKhuyet = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DiemDatDuoc = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true, defaultValueSql: "'0.00'"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaChiTiet);
                    table.ForeignKey(
                        name: "chitietnopbai_ibfk_1",
                        column: x => x.MaNopBai,
                        principalTable: "nopbai",
                        principalColumn: "MaNopBai");
                    table.ForeignKey(
                        name: "chitietnopbai_ibfk_2",
                        column: x => x.MaCauHoi,
                        principalTable: "nganhangcauhoi",
                        principalColumn: "MaCauHoi");
                    table.ForeignKey(
                        name: "chitietnopbai_ibfk_3",
                        column: x => x.MaDapAnChon,
                        principalTable: "dapan",
                        principalColumn: "MaDapAn");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "dinhkemnopbai",
                columns: table => new
                {
                    MaNopBai = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaTaiNguyen = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.MaNopBai, x.MaTaiNguyen })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "dinhkemnopbai_ibfk_1",
                        column: x => x.MaNopBai,
                        principalTable: "nopbai",
                        principalColumn: "MaNopBai");
                    table.ForeignKey(
                        name: "dinhkemnopbai_ibfk_2",
                        column: x => x.MaTaiNguyen,
                        principalTable: "tainguyenluutru",
                        principalColumn: "MaTaiNguyen");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "LoaiBaiTap",
                table: "baitap",
                column: "LoaiBaiTap");

            migrationBuilder.CreateIndex(
                name: "MaKhoaHoc",
                table: "baitap",
                column: "MaKhoaHoc");

            migrationBuilder.CreateIndex(
                name: "idx_BaiTapCauHoi_CauHoi_Active",
                table: "baitapcauhoi",
                columns: new[] { "MaCauHoi", "DaXoa", "TrangThai" });

            migrationBuilder.CreateIndex(
                name: "MaNguoiDung",
                table: "binhluan",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "MaSuKien",
                table: "binhluan",
                column: "MaSuKien");

            migrationBuilder.CreateIndex(
                name: "MaLopHoc",
                table: "buoihoc",
                column: "MaLopHoc");

            migrationBuilder.CreateIndex(
                name: "MaPhongHoc",
                table: "buoihoc",
                column: "MaPhongHoc");

            migrationBuilder.CreateIndex(
                name: "MaTietBatDau",
                table: "buoihoc",
                column: "MaTietBatDau");

            migrationBuilder.CreateIndex(
                name: "MaTietKetThuc",
                table: "buoihoc",
                column: "MaTietKetThuc");

            migrationBuilder.CreateIndex(
                name: "MaNhom",
                table: "chitietdanhmuc",
                column: "MaNhom");

            migrationBuilder.CreateIndex(
                name: "idx_ChiTietKHLH_LopHoc_Active",
                table: "chitietkhoahoc_lophoc",
                columns: new[] { "MaLopHoc", "DaXoa", "TrangThai" });

            migrationBuilder.CreateIndex(
                name: "idx_ChiTietNopBai_CauHoi_Active",
                table: "chitietnopbai",
                columns: new[] { "MaCauHoi", "DaXoa", "TrangThai" });

            migrationBuilder.CreateIndex(
                name: "MaDapAnChon",
                table: "chitietnopbai",
                column: "MaDapAnChon");

            migrationBuilder.CreateIndex(
                name: "MaNopBai",
                table: "chitietnopbai",
                column: "MaNopBai");

            migrationBuilder.CreateIndex(
                name: "uq_chucnang_code",
                table: "chucnanghethong",
                column: "maChucNangCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "MaKhoaHoc1",
                table: "chuonghoc",
                column: "MaKhoaHoc");

            migrationBuilder.CreateIndex(
                name: "MaCauHoi",
                table: "dapan",
                column: "MaCauHoi");

            migrationBuilder.CreateIndex(
                name: "idx_DiemDanh_HocSinh_Active",
                table: "diemdanh",
                columns: new[] { "MaHocSinh", "DaXoa", "TrangThai" });

            migrationBuilder.CreateIndex(
                name: "MaTrangThai",
                table: "diemdanh",
                column: "MaTrangThai");

            migrationBuilder.CreateIndex(
                name: "idx_DinhKem_TaiNguyen_Active",
                table: "dinhkem",
                columns: new[] { "MaTaiNguyen", "DaXoa", "TrangThai" });

            migrationBuilder.CreateIndex(
                name: "idx_DinhKemNopBai_TaiNguyen_Active",
                table: "dinhkemnopbai",
                columns: new[] { "MaTaiNguyen", "DaXoa", "TrangThai" });

            migrationBuilder.CreateIndex(
                name: "MaNguoiDung1",
                table: "giangvien",
                column: "MaNguoiDung",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_GiangVienLopHoc_GiangVien_Active",
                table: "giangvienlophoc",
                columns: new[] { "MaGiangVien", "DaXoa", "TrangThai" });

            migrationBuilder.CreateIndex(
                name: "MaNguoiDung2",
                table: "hocsinh",
                column: "MaNguoiDung",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_HocSinhLopHoc_HocSinh_Active",
                table: "hocsinhlophoc",
                columns: new[] { "MaHocSinh", "DaXoa", "TrangThai" });

            migrationBuilder.CreateIndex(
                name: "MaTrangThai1",
                table: "hocsinhlophoc",
                column: "MaTrangThai");

            migrationBuilder.CreateIndex(
                name: "MaTrangThai2",
                table: "khoahoc",
                column: "MaTrangThai");

            migrationBuilder.CreateIndex(
                name: "MaTrangThai3",
                table: "lophoc",
                column: "MaTrangThai");

            migrationBuilder.CreateIndex(
                name: "LoaiCauHoi",
                table: "nganhangcauhoi",
                column: "LoaiCauHoi");

            migrationBuilder.CreateIndex(
                name: "MaKhoaHoc2",
                table: "nganhangcauhoi",
                column: "MaKhoaHoc");

            migrationBuilder.CreateIndex(
                name: "MucDo",
                table: "nganhangcauhoi",
                column: "MucDo");

            migrationBuilder.CreateIndex(
                name: "Email",
                table: "nguoidung",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "MaTrangThai4",
                table: "nguoidung",
                column: "MaTrangThai");

            migrationBuilder.CreateIndex(
                name: "TenDangNhap",
                table: "nguoidung",
                column: "TenDangNhap",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_NguoiDungVaiTro_VaiTro_Active",
                table: "nguoidungvaitro",
                columns: new[] { "MaVaiTro", "DaXoa", "TrangThai" });

            migrationBuilder.CreateIndex(
                name: "idx_NguoiNhanSuKien_HocSinh_Active",
                table: "nguoinhansukien",
                columns: new[] { "MaHocSinh", "DaXoa", "TrangThai" });

            migrationBuilder.CreateIndex(
                name: "idx_NguoiNhanThongBao_User_DaDoc",
                table: "nguoinhanthongbao",
                columns: new[] { "MaNguoiDung", "DaDoc", "DaXoa" });

            migrationBuilder.CreateIndex(
                name: "MaCode",
                table: "nhomdanhmuc",
                column: "MaCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "MaHocSinh",
                table: "nopbai",
                column: "MaHocSinh");

            migrationBuilder.CreateIndex(
                name: "MaSuKien1",
                table: "nopbai",
                column: "MaSuKien");

            migrationBuilder.CreateIndex(
                name: "MaTrangThai5",
                table: "nopbai",
                column: "MaTrangThai");

            migrationBuilder.CreateIndex(
                name: "LoaiPhong",
                table: "phonghoc",
                column: "LoaiPhong");

            migrationBuilder.CreateIndex(
                name: "MaNguoiDung3",
                table: "phuhuynh",
                column: "MaNguoiDung",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_PhuHuynhHocSinh_HocSinh_Active",
                table: "phuhuynhhocsinh",
                columns: new[] { "MaHocSinh", "DaXoa", "TrangThai" });

            migrationBuilder.CreateIndex(
                name: "MaQuanHe",
                table: "phuhuynhhocsinh",
                column: "MaQuanHe");

            migrationBuilder.CreateIndex(
                name: "MaChucNang",
                table: "quyen",
                column: "MaChucNang");

            migrationBuilder.CreateIndex(
                name: "DangSuKien",
                table: "sukienlophoc",
                column: "DangSuKien");

            migrationBuilder.CreateIndex(
                name: "MaBaiTap",
                table: "sukienlophoc",
                column: "MaBaiTap");

            migrationBuilder.CreateIndex(
                name: "MaLopHoc1",
                table: "sukienlophoc",
                column: "MaLopHoc");

            migrationBuilder.CreateIndex(
                name: "MaNguoiDung4",
                table: "sukienlophoc",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "MaTrangThai6",
                table: "sukienlophoc",
                column: "MaTrangThai");

            migrationBuilder.CreateIndex(
                name: "MaChuongHoc",
                table: "tailieu",
                column: "MaChuongHoc");

            migrationBuilder.CreateIndex(
                name: "MaNguoiDung5",
                table: "tainguyenluutru",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "idx_ThanhVienHoiThoai_User_Active",
                table: "thanhvienhoithoai",
                columns: new[] { "MaNguoiDung", "DaXoa", "TrangThai" });

            migrationBuilder.CreateIndex(
                name: "MaHoiThoai",
                table: "tinnhan",
                column: "MaHoiThoai");

            migrationBuilder.CreateIndex(
                name: "MaNguoiDungGui",
                table: "tinnhan",
                column: "MaNguoiDungGui");

            migrationBuilder.CreateIndex(
                name: "fk_vtc_chucnang",
                table: "vaitrochucnang",
                column: "maChucNang");

            migrationBuilder.CreateIndex(
                name: "idx_VaiTroQuyen_Quyen_Active",
                table: "vaitroquyen",
                columns: new[] { "MaQuyen", "DaXoa", "TrangThai" });

            migrationBuilder.CreateIndex(
                name: "MaBuoiHoc",
                table: "yeucaulichday",
                column: "MaBuoiHoc");

            migrationBuilder.CreateIndex(
                name: "MaGiangVien",
                table: "yeucaulichday",
                column: "MaGiangVien");

            migrationBuilder.CreateIndex(
                name: "MaLopHoc2",
                table: "yeucaulichday",
                column: "MaLopHoc");

            migrationBuilder.CreateIndex(
                name: "MaTietBatDauDeXuat",
                table: "yeucaulichday",
                column: "MaTietBatDauDeXuat");

            migrationBuilder.CreateIndex(
                name: "MaTietKetThucDeXuat",
                table: "yeucaulichday",
                column: "MaTietKetThucDeXuat");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "baitapcauhoi");

            migrationBuilder.DropTable(
                name: "binhluan");

            migrationBuilder.DropTable(
                name: "chitietkhoahoc_lophoc");

            migrationBuilder.DropTable(
                name: "chitietnopbai");

            migrationBuilder.DropTable(
                name: "diemdanh");

            migrationBuilder.DropTable(
                name: "dinhkem");

            migrationBuilder.DropTable(
                name: "dinhkemnopbai");

            migrationBuilder.DropTable(
                name: "giangvienlophoc");

            migrationBuilder.DropTable(
                name: "hocsinhlophoc");

            migrationBuilder.DropTable(
                name: "nguoidungvaitro");

            migrationBuilder.DropTable(
                name: "nguoinhansukien");

            migrationBuilder.DropTable(
                name: "nguoinhanthongbao");

            migrationBuilder.DropTable(
                name: "phuhuynhhocsinh");

            migrationBuilder.DropTable(
                name: "tailieu");

            migrationBuilder.DropTable(
                name: "thanhvienhoithoai");

            migrationBuilder.DropTable(
                name: "tinnhan");

            migrationBuilder.DropTable(
                name: "vaitrochucnang");

            migrationBuilder.DropTable(
                name: "vaitroquyen");

            migrationBuilder.DropTable(
                name: "yeucaulichday");

            migrationBuilder.DropTable(
                name: "dapan");

            migrationBuilder.DropTable(
                name: "nopbai");

            migrationBuilder.DropTable(
                name: "tainguyenluutru");

            migrationBuilder.DropTable(
                name: "thongbao");

            migrationBuilder.DropTable(
                name: "phuhuynh");

            migrationBuilder.DropTable(
                name: "chuonghoc");

            migrationBuilder.DropTable(
                name: "hoithoai");

            migrationBuilder.DropTable(
                name: "chucnanghethong");

            migrationBuilder.DropTable(
                name: "vaitro");

            migrationBuilder.DropTable(
                name: "quyen");

            migrationBuilder.DropTable(
                name: "giangvien");

            migrationBuilder.DropTable(
                name: "buoihoc");

            migrationBuilder.DropTable(
                name: "nganhangcauhoi");

            migrationBuilder.DropTable(
                name: "sukienlophoc");

            migrationBuilder.DropTable(
                name: "hocsinh");

            migrationBuilder.DropTable(
                name: "chucnang");

            migrationBuilder.DropTable(
                name: "phonghoc");

            migrationBuilder.DropTable(
                name: "tiethoc");

            migrationBuilder.DropTable(
                name: "lophoc");

            migrationBuilder.DropTable(
                name: "baitap");

            migrationBuilder.DropTable(
                name: "nguoidung");

            migrationBuilder.DropTable(
                name: "khoahoc");

            migrationBuilder.DropTable(
                name: "chitietdanhmuc");

            migrationBuilder.DropTable(
                name: "nhomdanhmuc");
        }
    }
}
