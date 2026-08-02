using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingFieldsAndTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add missing columns to nganhangcauhoi
            migrationBuilder.Sql("SET FOREIGN_KEY_CHECKS=0;");
            
            migrationBuilder.Sql(@"ALTER TABLE nganhangcauhoi ADD COLUMN IF NOT EXISTS AmThanhLink VARCHAR(255) NULL;");
            migrationBuilder.Sql(@"ALTER TABLE nganhangcauhoi ADD COLUMN IF NOT EXISTS HinhAnhLink VARCHAR(255) NULL;");
            migrationBuilder.Sql(@"ALTER TABLE nganhangcauhoi ADD COLUMN IF NOT EXISTS MaCauHoiCha CHAR(36) NULL;");
            migrationBuilder.Sql(@"ALTER TABLE nganhangcauhoi ADD COLUMN IF NOT EXISTS ThuTu INT NULL DEFAULT 0;");

            // Add missing columns to dapan
            migrationBuilder.Sql(@"ALTER TABLE dapan ADD COLUMN IF NOT EXISTS GiaTriDoiChieu VARCHAR(255) NULL;");
            migrationBuilder.Sql(@"ALTER TABLE dapan ADD COLUMN IF NOT EXISTS TenDapAn VARCHAR(10) NULL;");
            migrationBuilder.Sql(@"ALTER TABLE dapan ADD COLUMN IF NOT EXISTS ThuTu INT NULL DEFAULT 0;");

            // Add missing column to chuonghoc
            migrationBuilder.Sql(@"ALTER TABLE chuonghoc ADD COLUMN IF NOT EXISTS MaLopHoc CHAR(36) NULL;");

            // Create missing tables
            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS dapandiendkhuyet (
  MaDapAnDien char(36) NOT NULL,
  MaCauHoi char(36) NOT NULL,
  DapAnChuan text NOT NULL,
  DapAnThayThe text,
  PhanBietHoaThuong tinyint(1) DEFAULT '0',
  NguoiTao char(36),
  ThoiGianTao datetime DEFAULT CURRENT_TIMESTAMP,
  NguoiSua char(36),
  ThoiGianSua datetime,
  TrangThai tinyint(1) DEFAULT '1',
  DaXoa tinyint(1) DEFAULT '0',
  PRIMARY KEY (MaDapAnDien),
  KEY MaCauHoi1 (MaCauHoi),
  KEY idx_DapAnDienKhuyet_CauHoi_Active (MaCauHoi,DaXoa,TrangThai),
  CONSTRAINT dapandiendkhuyet_ibfk_1 FOREIGN KEY (MaCauHoi) REFERENCES nganhangcauhoi (MaCauHoi)
);
            ");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS dinhkemthongbao (
  MaThongBao char(36) NOT NULL,
  MaTaiNguyen char(36) NOT NULL,
  NguoiTao char(36),
  ThoiGianTao datetime DEFAULT CURRENT_TIMESTAMP,
  TrangThai tinyint(1) DEFAULT '1',
  DaXoa tinyint(1) DEFAULT '0',
  PRIMARY KEY (MaThongBao,MaTaiNguyen),
  KEY idx_DinhKemThongBao_TaiNguyen_Active (MaTaiNguyen,DaXoa,TrangThai),
  CONSTRAINT dinhkemthongbao_ibfk_1 FOREIGN KEY (MaThongBao) REFERENCES thongbao (MaThongBao),
  CONSTRAINT dinhkemthongbao_ibfk_2 FOREIGN KEY (MaTaiNguyen) REFERENCES tainguyenluutru (MaTaiNguyen)
);
            ");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS giangvienkhoahoc (
  MaGiangVien char(36) NOT NULL,
  MaKhoaHoc char(36) NOT NULL,
  NguoiTao char(36),
  ThoiGianTao datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (MaGiangVien,MaKhoaHoc),
  KEY IX_giangvienkhoahoc_MaKhoaHoc (MaKhoaHoc)
);
            ");

            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS refresh_sessions (
  SessionId varchar(32) NOT NULL PRIMARY KEY,
  UserId char(36) NOT NULL,
  RefreshToken varchar(512) NOT NULL,
  ExpiresAtUtc datetime(6) NOT NULL,
  CreatedAtUtc datetime(6) NOT NULL,
  UpdatedAtUtc datetime(6)
);
            ");

            // Add indexes
            migrationBuilder.Sql(@"ALTER TABLE nganhangcauhoi ADD INDEX IF NOT EXISTS MaCauHoiCha (MaCauHoiCha);");
            migrationBuilder.Sql(@"ALTER TABLE dapan ADD INDEX IF NOT EXISTS idx_DapAn_CauHoi_Active (MaCauHoi, DaXoa, TrangThai);");
            migrationBuilder.Sql(@"ALTER TABLE chuonghoc ADD INDEX IF NOT EXISTS idx_ChuongHoc_MaLopHoc (MaLopHoc);");

            migrationBuilder.Sql("SET FOREIGN_KEY_CHECKS=1;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("SET FOREIGN_KEY_CHECKS=0;");

            migrationBuilder.Sql(@"DROP TABLE IF EXISTS dapandiendkhuyet;");
            migrationBuilder.Sql(@"DROP TABLE IF EXISTS dinhkemthongbao;");
            migrationBuilder.Sql(@"DROP TABLE IF EXISTS giangvienkhoahoc;");
            migrationBuilder.Sql(@"DROP TABLE IF EXISTS refresh_sessions;");

            migrationBuilder.Sql(@"ALTER TABLE nganhangcauhoi DROP COLUMN IF EXISTS AmThanhLink;");
            migrationBuilder.Sql(@"ALTER TABLE nganhangcauhoi DROP COLUMN IF EXISTS HinhAnhLink;");
            migrationBuilder.Sql(@"ALTER TABLE nganhangcauhoi DROP COLUMN IF EXISTS MaCauHoiCha;");
            migrationBuilder.Sql(@"ALTER TABLE nganhangcauhoi DROP COLUMN IF EXISTS ThuTu;");
            migrationBuilder.Sql(@"ALTER TABLE nganhangcauhoi DROP INDEX IF EXISTS MaCauHoiCha;");

            migrationBuilder.Sql(@"ALTER TABLE dapan DROP COLUMN IF EXISTS GiaTriDoiChieu;");
            migrationBuilder.Sql(@"ALTER TABLE dapan DROP COLUMN IF EXISTS TenDapAn;");
            migrationBuilder.Sql(@"ALTER TABLE dapan DROP COLUMN IF EXISTS ThuTu;");
            migrationBuilder.Sql(@"ALTER TABLE dapan DROP INDEX IF EXISTS idx_DapAn_CauHoi_Active;");

            migrationBuilder.Sql(@"ALTER TABLE chuonghoc DROP COLUMN IF EXISTS MaLopHoc;");
            migrationBuilder.Sql(@"ALTER TABLE chuonghoc DROP INDEX IF EXISTS idx_ChuongHoc_MaLopHoc;");

            migrationBuilder.Sql("SET FOREIGN_KEY_CHECKS=1;");
        }
    }
}
