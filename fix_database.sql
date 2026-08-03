-- Fix missing columns and tables

-- Add columns to nganhangcauhoi if they don't exist
ALTER TABLE nganhangcauhoi ADD COLUMN AmThanhLink VARCHAR(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL;
ALTER TABLE nganhangcauhoi ADD COLUMN HinhAnhLink VARCHAR(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL;
ALTER TABLE nganhangcauhoi ADD COLUMN MaCauHoiCha CHAR(36) COLLATE ascii_general_ci NULL;
ALTER TABLE nganhangcauhoi ADD COLUMN ThuTu INT NULL DEFAULT 0;

-- Add columns to dapan if they don't exist
ALTER TABLE dapan ADD COLUMN GiaTriDoiChieu VARCHAR(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL;
ALTER TABLE dapan ADD COLUMN TenDapAn VARCHAR(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL;
ALTER TABLE dapan ADD COLUMN ThuTu INT NULL DEFAULT 0;

-- Add column to chuonghoc if it doesn't exist
ALTER TABLE chuonghoc ADD COLUMN MaLopHoc CHAR(36) COLLATE ascii_general_ci NULL;

-- Create dapandiendkhuyet table if it doesn't exist
CREATE TABLE IF NOT EXISTS dapandiendkhuyet (
    MaDapAnDien CHAR(36) NOT NULL,
    MaCauHoi CHAR(36) NOT NULL,
    DapAnChuan TEXT NOT NULL CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
    DapAnThayThe TEXT NULL CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
    PhanBietHoaThuong TINYINT(1) NULL DEFAULT 0,
    NguoiTao CHAR(36) COLLATE ascii_general_ci NULL,
    ThoiGianTao DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
    NguoiSua CHAR(36) COLLATE ascii_general_ci NULL,
    ThoiGianSua DATETIME NULL,
    TrangThai TINYINT(1) NULL DEFAULT 1,
    DaXoa TINYINT(1) NULL DEFAULT 0,
    PRIMARY KEY (MaDapAnDien),
    FOREIGN KEY (MaCauHoi) REFERENCES nganhangcauhoi(MaCauHoi),
    INDEX MaCauHoi1 (MaCauHoi),
    INDEX idx_DapAnDienKhuyet_CauHoi_Active (MaCauHoi, DaXoa, TrangThai)
) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

-- Create dinhkemthongbao table if it doesn't exist
CREATE TABLE IF NOT EXISTS dinhkemthongbao (
    MaThongBao CHAR(36) NOT NULL,
    MaTaiNguyen CHAR(36) NOT NULL,
    NguoiTao CHAR(36) COLLATE ascii_general_ci NULL,
    ThoiGianTao DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
    TrangThai TINYINT(1) NULL DEFAULT 1,
    DaXoa TINYINT(1) NULL DEFAULT 0,
    PRIMARY KEY (MaThongBao, MaTaiNguyen),
    FOREIGN KEY (MaThongBao) REFERENCES thongbao(MaThongBao),
    FOREIGN KEY (MaTaiNguyen) REFERENCES tainguyenluutru(MaTaiNguyen),
    INDEX idx_DinhKemThongBao_TaiNguyen_Active (MaTaiNguyen, DaXoa, TrangThai)
) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

-- Create giangvienkhoahoc table if it doesn't exist
CREATE TABLE IF NOT EXISTS giangvienkhoahoc (
    MaGiangVien CHAR(36) NOT NULL,
    MaKhoaHoc CHAR(36) NOT NULL,
    NguoiTao CHAR(36) COLLATE ascii_general_ci NULL,
    ThoiGianTao DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (MaGiangVien, MaKhoaHoc),
    FOREIGN KEY (MaGiangVien) REFERENCES giangvien(MaGiangVien) ON DELETE CASCADE,
    FOREIGN KEY (MaKhoaHoc) REFERENCES khoahoc(MaKhoaHoc) ON DELETE CASCADE,
    INDEX IX_giangvienkhoahoc_MaKhoaHoc (MaKhoaHoc)
) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

-- Create refresh_sessions table if it doesn't exist
CREATE TABLE IF NOT EXISTS refresh_sessions (
    SessionId VARCHAR(32) NOT NULL PRIMARY KEY,
    UserId CHAR(36) NOT NULL,
    RefreshToken VARCHAR(512) NOT NULL,
    ExpiresAtUtc DATETIME(6) NOT NULL,
    CreatedAtUtc DATETIME(6) NOT NULL,
    UpdatedAtUtc DATETIME(6) NULL
) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

-- Add indexes if they don't exist
ALTER TABLE nganhangcauhoi ADD INDEX IF NOT EXISTS MaCauHoiCha (MaCauHoiCha);
ALTER TABLE dapan ADD INDEX IF NOT EXISTS idx_DapAn_CauHoi_Active (MaCauHoi, DaXoa, TrangThai);
ALTER TABLE chuonghoc ADD INDEX IF NOT EXISTS idx_ChuongHoc_MaLopHoc (MaLopHoc);

-- Add foreign keys if they don't exist
ALTER TABLE nganhangcauhoi ADD CONSTRAINT nganhangcauhoi_ibfk_4 FOREIGN KEY (MaCauHoiCha) REFERENCES nganhangcauhoi(MaCauHoi);
ALTER TABLE chuonghoc ADD CONSTRAINT chuonghoc_ibfk_2 FOREIGN KEY (MaLopHoc) REFERENCES lophoc(MaLopHoc);
