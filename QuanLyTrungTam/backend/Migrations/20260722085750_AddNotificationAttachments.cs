using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationAttachments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dinhkemthongbao",
                columns: table => new
                {
                    MaThongBao = table.Column<Guid>(type: "char(36)", nullable: false),
                    MaTaiNguyen = table.Column<Guid>(type: "char(36)", nullable: false),
                    NguoiTao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    TrangThai = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'1'"),
                    DaXoa = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.MaThongBao, x.MaTaiNguyen })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "dinhkemthongbao_ibfk_1",
                        column: x => x.MaThongBao,
                        principalTable: "thongbao",
                        principalColumn: "MaThongBao");
                    table.ForeignKey(
                        name: "dinhkemthongbao_ibfk_2",
                        column: x => x.MaTaiNguyen,
                        principalTable: "tainguyenluutru",
                        principalColumn: "MaTaiNguyen");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "idx_DinhKemThongBao_TaiNguyen_Active",
                table: "dinhkemthongbao",
                columns: new[] { "MaTaiNguyen", "DaXoa", "TrangThai" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dinhkemthongbao");
        }
    }
}
