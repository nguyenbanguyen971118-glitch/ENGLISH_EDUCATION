using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionBankRichFieldsForAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CauTraLoiDienKhuyet",
                table: "chitietnopbai",
                newName: "CauTraLoiHocSinh");

            migrationBuilder.AddColumn<string>(
                name: "AmThanhLink",
                table: "nganhangcauhoi",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HinhAnhLink",
                table: "nganhangcauhoi",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "MaCauHoiCha",
                table: "nganhangcauhoi",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci")
                .Annotation("MySql:CharSet", "ascii");

            migrationBuilder.AddColumn<int>(
                name: "ThuTu",
                table: "nganhangcauhoi",
                type: "int",
                nullable: true,
                defaultValueSql: "'0'");

            migrationBuilder.AddColumn<string>(
                name: "GiaTriDoiChieu",
                table: "dapan",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TenDapAn",
                table: "dapan",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "ThuTu",
                table: "dapan",
                type: "int",
                nullable: true,
                defaultValueSql: "'0'");

            migrationBuilder.CreateTable(
                name: "dapandiendkhuyet",
                columns: table => new
                {
                    MaDapAnDien = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    MaCauHoi = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    DapAnChuan = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DapAnThayThe = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhanBietHoaThuong = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'"),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NguoiSua = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    ThoiGianSua = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MaDapAnDien);
                    table.ForeignKey(
                        name: "dapandiendkhuyet_ibfk_1",
                        column: x => x.MaCauHoi,
                        principalTable: "nganhangcauhoi",
                        principalColumn: "MaCauHoi");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "MaCauHoiCha",
                table: "nganhangcauhoi",
                column: "MaCauHoiCha");

            migrationBuilder.CreateIndex(
                name: "idx_DapAn_CauHoi_Active",
                table: "dapan",
                columns: new[] { "MaCauHoi", "DaXoa", "TrangThai" });

            migrationBuilder.CreateIndex(
                name: "idx_DapAnDienKhuyet_CauHoi_Active",
                table: "dapandiendkhuyet",
                columns: new[] { "MaCauHoi", "DaXoa", "TrangThai" });

            migrationBuilder.CreateIndex(
                name: "MaCauHoi1",
                table: "dapandiendkhuyet",
                column: "MaCauHoi");

            migrationBuilder.AddForeignKey(
                name: "nganhangcauhoi_ibfk_4",
                table: "nganhangcauhoi",
                column: "MaCauHoiCha",
                principalTable: "nganhangcauhoi",
                principalColumn: "MaCauHoi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "nganhangcauhoi_ibfk_4",
                table: "nganhangcauhoi");

            migrationBuilder.DropTable(
                name: "dapandiendkhuyet");

            migrationBuilder.DropIndex(
                name: "MaCauHoiCha",
                table: "nganhangcauhoi");

            migrationBuilder.DropIndex(
                name: "idx_DapAn_CauHoi_Active",
                table: "dapan");

            migrationBuilder.DropColumn(
                name: "AmThanhLink",
                table: "nganhangcauhoi");

            migrationBuilder.DropColumn(
                name: "HinhAnhLink",
                table: "nganhangcauhoi");

            migrationBuilder.DropColumn(
                name: "MaCauHoiCha",
                table: "nganhangcauhoi");

            migrationBuilder.DropColumn(
                name: "ThuTu",
                table: "nganhangcauhoi");

            migrationBuilder.DropColumn(
                name: "GiaTriDoiChieu",
                table: "dapan");

            migrationBuilder.DropColumn(
                name: "TenDapAn",
                table: "dapan");

            migrationBuilder.DropColumn(
                name: "ThuTu",
                table: "dapan");

            migrationBuilder.RenameColumn(
                name: "CauTraLoiHocSinh",
                table: "chitietnopbai",
                newName: "CauTraLoiDienKhuyet");
        }
    }
}
