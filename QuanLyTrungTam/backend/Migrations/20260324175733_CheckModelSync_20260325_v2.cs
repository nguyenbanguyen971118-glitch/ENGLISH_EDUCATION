using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class CheckModelSync_20260325_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "yeucaulichday",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "yeucaulichday",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "yeucaulichday",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaLopHoc",
                table: "yeucaulichday",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaGiangVien",
                table: "yeucaulichday",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaBuoiHoc",
                table: "yeucaulichday",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaYeuCau",
                table: "yeucaulichday",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "vaitroquyen",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "vaitroquyen",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "vaitroquyen",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "vaitro",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "vaitro",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "vaitro",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "MaVaiTro",
                table: "vaitro",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "tinnhan",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "tinnhan",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "tinnhan",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguoiDungGui",
                table: "tinnhan",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaHoiThoai",
                table: "tinnhan",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaTinNhan",
                table: "tinnhan",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "tiethoc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "tiethoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "tiethoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "MaTiet",
                table: "tiethoc",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "thongbao",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "thongbao",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "thongbao",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaThongBao",
                table: "thongbao",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "thanhvienhoithoai",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "thanhvienhoithoai",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "thanhvienhoithoai",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguoiDung",
                table: "thanhvienhoithoai",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaHoiThoai",
                table: "thanhvienhoithoai",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "tainguyenluutru",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "tainguyenluutru",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "tainguyenluutru",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguoiDung",
                table: "tainguyenluutru",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaTaiNguyen",
                table: "tainguyenluutru",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "tailieu",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "tailieu",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "tailieu",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaChuongHoc",
                table: "tailieu",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaTaiLieu",
                table: "tailieu",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "sukienlophoc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "sukienlophoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "sukienlophoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguoiDung",
                table: "sukienlophoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaLopHoc",
                table: "sukienlophoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaBaiTap",
                table: "sukienlophoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaSuKien",
                table: "sukienlophoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "quyen",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "quyen",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "quyen",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "MaQuyen",
                table: "quyen",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "phuhuynhhocsinh",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "phuhuynhhocsinh",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "phuhuynhhocsinh",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaHocSinh",
                table: "phuhuynhhocsinh",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaPhuHuynh",
                table: "phuhuynhhocsinh",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "phuhuynh",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "phuhuynh",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "phuhuynh",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguoiDung",
                table: "phuhuynh",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaPhuHuynh",
                table: "phuhuynh",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "phonghoc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "phonghoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "phonghoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaPhongHoc",
                table: "phonghoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "nopbai",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "nopbai",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "nopbai",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaSuKien",
                table: "nopbai",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaHocSinh",
                table: "nopbai",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNopBai",
                table: "nopbai",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "nhomdanhmuc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "nhomdanhmuc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "nhomdanhmuc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "MaNhom",
                table: "nhomdanhmuc",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "nguoinhanthongbao",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "nguoinhanthongbao",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "nguoinhanthongbao",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguoiDung",
                table: "nguoinhanthongbao",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaThongBao",
                table: "nguoinhanthongbao",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "nguoinhansukien",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "nguoinhansukien",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "nguoinhansukien",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaHocSinh",
                table: "nguoinhansukien",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaSuKien",
                table: "nguoinhansukien",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "nguoidungvaitro",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "nguoidungvaitro",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "nguoidungvaitro",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguoiDung",
                table: "nguoidungvaitro",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "nguoidung",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "nguoidung",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "nguoidung",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguoiDung",
                table: "nguoidung",
                type: "char(36)",
                nullable: false,
                comment: "GUID cho tài khoản",
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldComment: "GUID cho tài khoản")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "nganhangcauhoi",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "nganhangcauhoi",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "nganhangcauhoi",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaKhoaHoc",
                table: "nganhangcauhoi",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaCauHoi",
                table: "nganhangcauhoi",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "lophoc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "lophoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "lophoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaLopHoc",
                table: "lophoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "khoahoc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "khoahoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "khoahoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaKhoaHoc",
                table: "khoahoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "hoithoai",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "hoithoai",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "hoithoai",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaHoiThoai",
                table: "hoithoai",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "hocsinhlophoc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "hocsinhlophoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "hocsinhlophoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaHocSinh",
                table: "hocsinhlophoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaLopHoc",
                table: "hocsinhlophoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "hocsinh",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "hocsinh",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "hocsinh",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguoiDung",
                table: "hocsinh",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaHocSinh",
                table: "hocsinh",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "giangvienlophoc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "giangvienlophoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "giangvienlophoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaGiangVien",
                table: "giangvienlophoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaLopHoc",
                table: "giangvienlophoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "giangvien",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "giangvien",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "giangvien",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguoiDung",
                table: "giangvien",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaGiangVien",
                table: "giangvien",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "dinhkemnopbai",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "dinhkemnopbai",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "dinhkemnopbai",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaTaiNguyen",
                table: "dinhkemnopbai",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNopBai",
                table: "dinhkemnopbai",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "dinhkem",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "dinhkem",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "dinhkem",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaTaiNguyen",
                table: "dinhkem",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaSuKien",
                table: "dinhkem",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "diemdanh",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "diemdanh",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "diemdanh",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaHocSinh",
                table: "diemdanh",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaBuoiHoc",
                table: "diemdanh",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "dapan",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "dapan",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "dapan",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaCauHoi",
                table: "dapan",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaDapAn",
                table: "dapan",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "chuonghoc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "chuonghoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "chuonghoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaKhoaHoc",
                table: "chuonghoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaChuong",
                table: "chuonghoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "maChucNang",
                table: "chucnanghethong",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "chucnang",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "chucnang",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "chucnang",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "MaChucNang",
                table: "chucnang",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "chitietnopbai",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "chitietnopbai",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "chitietnopbai",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNopBai",
                table: "chitietnopbai",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaDapAnChon",
                table: "chitietnopbai",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaCauHoi",
                table: "chitietnopbai",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaChiTiet",
                table: "chitietnopbai",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "chitietkhoahoc_lophoc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "chitietkhoahoc_lophoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "chitietkhoahoc_lophoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaLopHoc",
                table: "chitietkhoahoc_lophoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaKhoaHoc",
                table: "chitietkhoahoc_lophoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "chitietdanhmuc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "chitietdanhmuc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "chitietdanhmuc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "MaChiTiet",
                table: "chitietdanhmuc",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "buoihoc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "buoihoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "buoihoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaPhongHoc",
                table: "buoihoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaLopHoc",
                table: "buoihoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaBuoiHoc",
                table: "buoihoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "binhluan",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "binhluan",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "binhluan",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaSuKien",
                table: "binhluan",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguoiDung",
                table: "binhluan",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaBinhLuan",
                table: "binhluan",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "baitapcauhoi",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "baitapcauhoi",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "baitapcauhoi",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaCauHoi",
                table: "baitapcauhoi",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaBaiTap",
                table: "baitapcauhoi",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "baitap",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "baitap",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "baitap",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaKhoaHoc",
                table: "baitap",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaBaiTap",
                table: "baitap",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "ascii")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "yeucaulichday",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "yeucaulichday",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "yeucaulichday",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaLopHoc",
                table: "yeucaulichday",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaGiangVien",
                table: "yeucaulichday",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaBuoiHoc",
                table: "yeucaulichday",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaYeuCau",
                table: "yeucaulichday",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "vaitroquyen",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "vaitroquyen",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "vaitroquyen",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "vaitro",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "vaitro",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "vaitro",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<int>(
                name: "MaVaiTro",
                table: "vaitro",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "tinnhan",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "tinnhan",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "tinnhan",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguoiDungGui",
                table: "tinnhan",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaHoiThoai",
                table: "tinnhan",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaTinNhan",
                table: "tinnhan",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "tiethoc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "tiethoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "tiethoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<int>(
                name: "MaTiet",
                table: "tiethoc",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "thongbao",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "thongbao",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "thongbao",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaThongBao",
                table: "thongbao",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "thanhvienhoithoai",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "thanhvienhoithoai",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "thanhvienhoithoai",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguoiDung",
                table: "thanhvienhoithoai",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaHoiThoai",
                table: "thanhvienhoithoai",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "tainguyenluutru",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "tainguyenluutru",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "tainguyenluutru",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguoiDung",
                table: "tainguyenluutru",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaTaiNguyen",
                table: "tainguyenluutru",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "tailieu",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "tailieu",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "tailieu",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaChuongHoc",
                table: "tailieu",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaTaiLieu",
                table: "tailieu",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "sukienlophoc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "sukienlophoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "sukienlophoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguoiDung",
                table: "sukienlophoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaLopHoc",
                table: "sukienlophoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaBaiTap",
                table: "sukienlophoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaSuKien",
                table: "sukienlophoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "quyen",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "quyen",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "quyen",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<int>(
                name: "MaQuyen",
                table: "quyen",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "phuhuynhhocsinh",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "phuhuynhhocsinh",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "phuhuynhhocsinh",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaHocSinh",
                table: "phuhuynhhocsinh",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaPhuHuynh",
                table: "phuhuynhhocsinh",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "phuhuynh",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "phuhuynh",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "phuhuynh",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguoiDung",
                table: "phuhuynh",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaPhuHuynh",
                table: "phuhuynh",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "phonghoc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "phonghoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "phonghoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaPhongHoc",
                table: "phonghoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "nopbai",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "nopbai",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "nopbai",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaSuKien",
                table: "nopbai",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaHocSinh",
                table: "nopbai",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNopBai",
                table: "nopbai",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "nhomdanhmuc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "nhomdanhmuc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "nhomdanhmuc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<int>(
                name: "MaNhom",
                table: "nhomdanhmuc",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "nguoinhanthongbao",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "nguoinhanthongbao",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "nguoinhanthongbao",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguoiDung",
                table: "nguoinhanthongbao",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaThongBao",
                table: "nguoinhanthongbao",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "nguoinhansukien",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "nguoinhansukien",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "nguoinhansukien",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaHocSinh",
                table: "nguoinhansukien",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaSuKien",
                table: "nguoinhansukien",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "nguoidungvaitro",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "nguoidungvaitro",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "nguoidungvaitro",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguoiDung",
                table: "nguoidungvaitro",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "nguoidung",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "nguoidung",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "nguoidung",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguoiDung",
                table: "nguoidung",
                type: "char(36)",
                nullable: false,
                comment: "GUID cho tài khoản",
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldComment: "GUID cho tài khoản",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "nganhangcauhoi",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "nganhangcauhoi",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "nganhangcauhoi",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaKhoaHoc",
                table: "nganhangcauhoi",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaCauHoi",
                table: "nganhangcauhoi",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "lophoc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "lophoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "lophoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaLopHoc",
                table: "lophoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "khoahoc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "khoahoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "khoahoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaKhoaHoc",
                table: "khoahoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "hoithoai",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "hoithoai",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "hoithoai",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaHoiThoai",
                table: "hoithoai",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "hocsinhlophoc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "hocsinhlophoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "hocsinhlophoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaHocSinh",
                table: "hocsinhlophoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaLopHoc",
                table: "hocsinhlophoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "hocsinh",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "hocsinh",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "hocsinh",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguoiDung",
                table: "hocsinh",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaHocSinh",
                table: "hocsinh",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "giangvienlophoc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "giangvienlophoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "giangvienlophoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaGiangVien",
                table: "giangvienlophoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaLopHoc",
                table: "giangvienlophoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "giangvien",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "giangvien",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "giangvien",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguoiDung",
                table: "giangvien",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaGiangVien",
                table: "giangvien",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "dinhkemnopbai",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "dinhkemnopbai",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "dinhkemnopbai",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaTaiNguyen",
                table: "dinhkemnopbai",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNopBai",
                table: "dinhkemnopbai",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "dinhkem",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "dinhkem",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "dinhkem",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaTaiNguyen",
                table: "dinhkem",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaSuKien",
                table: "dinhkem",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "diemdanh",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "diemdanh",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "diemdanh",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaHocSinh",
                table: "diemdanh",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaBuoiHoc",
                table: "diemdanh",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "dapan",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "dapan",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "dapan",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaCauHoi",
                table: "dapan",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaDapAn",
                table: "dapan",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "chuonghoc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "chuonghoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "chuonghoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaKhoaHoc",
                table: "chuonghoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaChuong",
                table: "chuonghoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<int>(
                name: "maChucNang",
                table: "chucnanghethong",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "chucnang",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "chucnang",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "chucnang",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<int>(
                name: "MaChucNang",
                table: "chucnang",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "chitietnopbai",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "chitietnopbai",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "chitietnopbai",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNopBai",
                table: "chitietnopbai",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaDapAnChon",
                table: "chitietnopbai",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaCauHoi",
                table: "chitietnopbai",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaChiTiet",
                table: "chitietnopbai",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "chitietkhoahoc_lophoc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "chitietkhoahoc_lophoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "chitietkhoahoc_lophoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaLopHoc",
                table: "chitietkhoahoc_lophoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaKhoaHoc",
                table: "chitietkhoahoc_lophoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "chitietdanhmuc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "chitietdanhmuc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "chitietdanhmuc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<int>(
                name: "MaChiTiet",
                table: "chitietdanhmuc",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "buoihoc",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "buoihoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "buoihoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaPhongHoc",
                table: "buoihoc",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaLopHoc",
                table: "buoihoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaBuoiHoc",
                table: "buoihoc",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "binhluan",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "binhluan",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "binhluan",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaSuKien",
                table: "binhluan",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguoiDung",
                table: "binhluan",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaBinhLuan",
                table: "binhluan",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "baitapcauhoi",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "baitapcauhoi",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "baitapcauhoi",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaCauHoi",
                table: "baitapcauhoi",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaBaiTap",
                table: "baitapcauhoi",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ThoiGianSua",
                table: "baitap",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiTao",
                table: "baitap",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "NguoiSua",
                table: "baitap",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true,
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaKhoaHoc",
                table: "baitap",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaBaiTap",
                table: "baitap",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldCollation: "ascii_general_ci")
                .OldAnnotation("MySql:CharSet", "ascii");
        }
    }
}
