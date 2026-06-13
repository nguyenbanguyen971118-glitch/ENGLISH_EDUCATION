using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddClassScopedStudyContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                SET @schema_name = DATABASE();
                SET @column_exists = (
                    SELECT COUNT(*)
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_SCHEMA = @schema_name
                      AND TABLE_NAME = 'chuonghoc'
                      AND COLUMN_NAME = 'MaLopHoc'
                );
                SET @sql = IF(
                    @column_exists = 0,
                    'ALTER TABLE chuonghoc ADD COLUMN MaLopHoc char(36) CHARACTER SET ascii COLLATE ascii_general_ci NULL',
                    'SELECT 1'
                );
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
                """);

            migrationBuilder.Sql(
                """
                SET @schema_name = DATABASE();
                SET @index_exists = (
                    SELECT COUNT(*)
                    FROM INFORMATION_SCHEMA.STATISTICS
                    WHERE TABLE_SCHEMA = @schema_name
                      AND TABLE_NAME = 'chuonghoc'
                      AND INDEX_NAME = 'idx_ChuongHoc_MaLopHoc'
                );
                SET @sql = IF(
                    @index_exists = 0,
                    'CREATE INDEX idx_ChuongHoc_MaLopHoc ON chuonghoc (MaLopHoc)',
                    'SELECT 1'
                );
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
                """);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "chuonghoc_ibfk_2",
                table: "chuonghoc");

            migrationBuilder.DropIndex(
                name: "idx_ChuongHoc_MaLopHoc",
                table: "chuonghoc");

            migrationBuilder.DropColumn(
                name: "MaLopHoc",
                table: "chuonghoc");
        }
    }
}
