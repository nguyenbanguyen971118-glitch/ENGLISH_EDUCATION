using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduleChangeRequestColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GhiChuAdmin",
                table: "yeucaulichday",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_general_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<sbyte>(
                name: "LoaiYeuCau",
                table: "yeucaulichday",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MaPhongHocDeXuat",
                table: "yeucaulichday",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_yeucaulichday_MaPhongHocDeXuat",
                table: "yeucaulichday",
                column: "MaPhongHocDeXuat");

            migrationBuilder.AddForeignKey(
                name: "FK_yeucaulichday_phonghoc_MaPhongHocDeXuat",
                table: "yeucaulichday",
                column: "MaPhongHocDeXuat",
                principalTable: "phonghoc",
                principalColumn: "MaPhongHoc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_yeucaulichday_phonghoc_MaPhongHocDeXuat",
                table: "yeucaulichday");

            migrationBuilder.DropIndex(
                name: "IX_yeucaulichday_MaPhongHocDeXuat",
                table: "yeucaulichday");

            migrationBuilder.DropColumn(
                name: "GhiChuAdmin",
                table: "yeucaulichday");

            migrationBuilder.DropColumn(
                name: "LoaiYeuCau",
                table: "yeucaulichday");

            migrationBuilder.DropColumn(
                name: "MaPhongHocDeXuat",
                table: "yeucaulichday");
        }
    }
}
