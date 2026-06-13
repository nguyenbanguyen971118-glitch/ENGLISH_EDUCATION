-- Friendly demo data for Study Content
-- Purpose:
--   1. Replace old seed names in Study Content with more familiar course/class names.
--   2. Create enough demo data for Admin, Teacher, and Student flows.
--   3. Give the default student both current classes and a past class for "hoc chu dong".
--
-- Expected bootstrap accounts:
--   admin@qltt.local
--   giaovien@qltt.local
--   hocsinh@qltt.local
--
-- If your local database name is different, change the USE statement below.

USE quanlytrungtam;

SET NAMES utf8mb4;

SET @db_name = CONVERT(DATABASE() USING utf8mb3) COLLATE utf8mb3_general_ci;
SET @admin_email = 'admin@qltt.local';
SET @teacher_email = 'giaovien@qltt.local';
SET @student_email = 'hocsinh@qltt.local';
SET @now = UTC_TIMESTAMP();
SET @today = UTC_DATE();

-- Ensure schema supports class-scoped study content.
SET @ensure_column_sql = (
    SELECT IF(
        EXISTS(
            SELECT 1
            FROM information_schema.COLUMNS
            WHERE TABLE_SCHEMA COLLATE utf8mb3_general_ci = @db_name
              AND TABLE_NAME = 'chuonghoc'
              AND COLUMN_NAME = 'MaLopHoc'
        ),
        'SELECT ''chuonghoc.MaLopHoc already exists''',
        'ALTER TABLE chuonghoc ADD COLUMN MaLopHoc CHAR(36) CHARACTER SET ascii COLLATE ascii_general_ci NULL'
    )
);
PREPARE stmt FROM @ensure_column_sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

SET @ensure_column_definition_sql = (
    SELECT IF(
        EXISTS(
            SELECT 1
            FROM information_schema.COLUMNS
            WHERE TABLE_SCHEMA COLLATE utf8mb3_general_ci = @db_name
              AND TABLE_NAME = 'chuonghoc'
              AND COLUMN_NAME = 'MaLopHoc'
              AND COLUMN_TYPE = 'char(36)'
              AND IS_NULLABLE = 'YES'
              AND CHARACTER_SET_NAME = 'ascii'
              AND COLLATION_NAME = 'ascii_general_ci'
        ),
        'SELECT ''chuonghoc.MaLopHoc already matches lophoc.MaLopHoc''',
        'ALTER TABLE chuonghoc MODIFY COLUMN MaLopHoc CHAR(36) CHARACTER SET ascii COLLATE ascii_general_ci NULL'
    )
);
PREPARE stmt FROM @ensure_column_definition_sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

SET @ensure_index_sql = (
    SELECT IF(
        EXISTS(
            SELECT 1
            FROM information_schema.STATISTICS
            WHERE TABLE_SCHEMA COLLATE utf8mb3_general_ci = @db_name
              AND TABLE_NAME = 'chuonghoc'
              AND INDEX_NAME = 'idx_ChuongHoc_MaLopHoc'
        ),
        'SELECT ''idx_ChuongHoc_MaLopHoc already exists''',
        'CREATE INDEX idx_ChuongHoc_MaLopHoc ON chuonghoc(MaLopHoc)'
    )
);
PREPARE stmt FROM @ensure_index_sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

SET @ensure_fk_sql = (
    SELECT IF(
        EXISTS(
            SELECT 1
            FROM information_schema.REFERENTIAL_CONSTRAINTS
            WHERE CONSTRAINT_SCHEMA COLLATE utf8mb3_general_ci = @db_name
              AND TABLE_NAME = 'chuonghoc'
              AND CONSTRAINT_NAME = 'chuonghoc_ibfk_2'
        ),
        'SELECT ''chuonghoc_ibfk_2 already exists''',
        'ALTER TABLE chuonghoc ADD CONSTRAINT chuonghoc_ibfk_2 FOREIGN KEY (MaLopHoc) REFERENCES lophoc(MaLopHoc)'
    )
);
PREPARE stmt FROM @ensure_fk_sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

SET @admin_user_id = (
    SELECT nd.MaNguoiDung
    FROM nguoidung nd
    WHERE nd.Email = @admin_email
      AND IFNULL(nd.DaXoa, 0) = 0
    LIMIT 1
);

SET @teacher_user_id = (
    SELECT nd.MaNguoiDung
    FROM nguoidung nd
    WHERE nd.Email = @teacher_email
      AND IFNULL(nd.DaXoa, 0) = 0
    LIMIT 1
);

SET @student_user_id = (
    SELECT nd.MaNguoiDung
    FROM nguoidung nd
    WHERE nd.Email = @student_email
      AND IFNULL(nd.DaXoa, 0) = 0
    LIMIT 1
);

SET @teacher_id = COALESCE(
    (
        SELECT gv.MaGiangVien
        FROM giangvien gv
        WHERE gv.MaNguoiDung = @teacher_user_id
          AND IFNULL(gv.DaXoa, 0) = 0
        LIMIT 1
    ),
    'f6452395-0c1f-4d53-8d2d-500000000001'
);

SET @student_id = COALESCE(
    (
        SELECT hs.MaHocSinh
        FROM hocsinh hs
        WHERE hs.MaNguoiDung = @student_user_id
          AND IFNULL(hs.DaXoa, 0) = 0
        LIMIT 1
    ),
    'f6452395-0c1f-4d53-8d2d-600000000001'
);

SET @seed_owner_id = COALESCE(@admin_user_id, @teacher_user_id, @student_user_id);

SET @course_1 = 'f6452395-0c1f-4d53-8d2d-100000000001';
SET @course_2 = 'f6452395-0c1f-4d53-8d2d-100000000002';

SET @class_1 = 'f6452395-0c1f-4d53-8d2d-200000000001';
SET @class_2 = 'f6452395-0c1f-4d53-8d2d-200000000002';
SET @class_3 = 'f6452395-0c1f-4d53-8d2d-200000000003';

SET @chapter_1 = 'f6452395-0c1f-4d53-8d2d-300000000001';
SET @chapter_2 = 'f6452395-0c1f-4d53-8d2d-300000000002';
SET @chapter_3 = 'f6452395-0c1f-4d53-8d2d-300000000003';
SET @chapter_4 = 'f6452395-0c1f-4d53-8d2d-300000000004';

SET @document_1 = 'f6452395-0c1f-4d53-8d2d-400000000001';
SET @document_2 = 'f6452395-0c1f-4d53-8d2d-400000000002';
SET @document_3 = 'f6452395-0c1f-4d53-8d2d-400000000003';
SET @document_4 = 'f6452395-0c1f-4d53-8d2d-400000000004';

START TRANSACTION;

INSERT INTO khoahoc (
    MaKhoaHoc, TenKhoaHoc, MoTa, GiaCoBan,
    NguoiTao, ThoiGianTao, NguoiSua, ThoiGianSua,
    TrangThai, DaXoa
)
VALUES
(
    @course_1,
    'Tieng Anh Giao Tiep Co Ban',
    'Khoa hoc demo de kiem thu noi dung hoc tap theo lop cho admin, giao vien va hoc sinh.',
    1500000.00,
    @seed_owner_id, @now, @seed_owner_id, @now,
    1, 0
),
(
    @course_2,
    'TOEIC 650+',
    'Khoa hoc demo de kiem thu hoc lieu, giao bai va hoc chu dong.',
    1900000.00,
    @seed_owner_id, @now, @seed_owner_id, @now,
    1, 0
)
ON DUPLICATE KEY UPDATE
    TenKhoaHoc = VALUES(TenKhoaHoc),
    MoTa = VALUES(MoTa),
    GiaCoBan = VALUES(GiaCoBan),
    NguoiSua = VALUES(NguoiSua),
    ThoiGianSua = VALUES(ThoiGianSua),
    TrangThai = 1,
    DaXoa = 0;

INSERT INTO lophoc (
    MaLopHoc, TenLop, NgayBatDau, NgayKetThuc,
    SiSoHienTai, SiSoToiDa,
    NguoiTao, ThoiGianTao, NguoiSua, ThoiGianSua,
    TrangThai, DaXoa
)
VALUES
(
    @class_1,
    'Giao tiep T2 - T4 - T6',
    DATE_SUB(@today, INTERVAL 21 DAY),
    DATE_ADD(@today, INTERVAL 60 DAY),
    18, 25,
    @seed_owner_id, @now, @seed_owner_id, @now,
    1, 0
),
(
    @class_2,
    'Giao tiep Cuoi tuan',
    DATE_SUB(@today, INTERVAL 140 DAY),
    DATE_SUB(@today, INTERVAL 30 DAY),
    14, 25,
    @seed_owner_id, @now, @seed_owner_id, @now,
    1, 0
),
(
    @class_3,
    'TOEIC 650+ T3 - T5 - T7',
    DATE_SUB(@today, INTERVAL 10 DAY),
    DATE_ADD(@today, INTERVAL 75 DAY),
    20, 30,
    @seed_owner_id, @now, @seed_owner_id, @now,
    1, 0
)
ON DUPLICATE KEY UPDATE
    TenLop = VALUES(TenLop),
    NgayBatDau = VALUES(NgayBatDau),
    NgayKetThuc = VALUES(NgayKetThuc),
    SiSoHienTai = VALUES(SiSoHienTai),
    SiSoToiDa = VALUES(SiSoToiDa),
    NguoiSua = VALUES(NguoiSua),
    ThoiGianSua = VALUES(ThoiGianSua),
    TrangThai = 1,
    DaXoa = 0;

INSERT INTO chitietkhoahoc_lophoc (
    MaKhoaHoc, MaLopHoc, GhiChu,
    NguoiTao, ThoiGianTao, NguoiSua, ThoiGianSua,
    TrangThai, DaXoa
)
VALUES
(
    @course_1, @class_1, 'Lop hien tai cua khoa Tieng Anh Giao Tiep Co Ban.',
    @seed_owner_id, @now, @seed_owner_id, @now,
    1, 0
),
(
    @course_1, @class_2, 'Lop da hoc truoc do de test hoc lieu lich su.',
    @seed_owner_id, @now, @seed_owner_id, @now,
    1, 0
),
(
    @course_2, @class_3, 'Lop hien tai de test giao vien va hoc sinh.',
    @seed_owner_id, @now, @seed_owner_id, @now,
    1, 0
)
ON DUPLICATE KEY UPDATE
    GhiChu = VALUES(GhiChu),
    NguoiSua = VALUES(NguoiSua),
    ThoiGianSua = VALUES(ThoiGianSua),
    TrangThai = 1,
    DaXoa = 0;

INSERT INTO giangvien (
    MaGiangVien, MaNguoiDung, SoDienThoai, QueQuan,
    TrinhDoChuyenMon, HocVi, KinhNghiemGiangDay,
    NguoiTao, ThoiGianTao, NguoiSua, ThoiGianSua,
    TrangThai, DaXoa
)
SELECT
    @teacher_id, @teacher_user_id, NULL, NULL,
    'Tieng Anh Tong Quat', 'Cu nhan', 'Du lieu demo cho Study Content',
    @seed_owner_id, @now, @seed_owner_id, @now,
    1, 0
FROM DUAL
WHERE @teacher_user_id IS NOT NULL
ON DUPLICATE KEY UPDATE
    TrinhDoChuyenMon = VALUES(TrinhDoChuyenMon),
    HocVi = VALUES(HocVi),
    KinhNghiemGiangDay = VALUES(KinhNghiemGiangDay),
    NguoiSua = VALUES(NguoiSua),
    ThoiGianSua = VALUES(ThoiGianSua),
    TrangThai = 1,
    DaXoa = 0;

INSERT INTO hocsinh (
    MaHocSinh, MaNguoiDung, NgaySinh, QueQuan,
    SoDienThoaiNguoiThan, TruongDangTheoHoc,
    NguoiTao, ThoiGianTao, NguoiSua, ThoiGianSua,
    TrangThai, DaXoa
)
SELECT
    @student_id, @student_user_id, DATE_SUB(@today, INTERVAL 16 YEAR), 'TP.HCM',
    '0900000000', 'Truong THPT Demo',
    @seed_owner_id, @now, @seed_owner_id, @now,
    1, 0
FROM DUAL
WHERE @student_user_id IS NOT NULL
ON DUPLICATE KEY UPDATE
    NgaySinh = VALUES(NgaySinh),
    QueQuan = VALUES(QueQuan),
    SoDienThoaiNguoiThan = VALUES(SoDienThoaiNguoiThan),
    TruongDangTheoHoc = VALUES(TruongDangTheoHoc),
    NguoiSua = VALUES(NguoiSua),
    ThoiGianSua = VALUES(ThoiGianSua),
    TrangThai = 1,
    DaXoa = 0;

INSERT INTO giangvienlophoc (
    MaLopHoc, MaGiangVien, LoaiVaiTro, NgayThamGia,
    NguoiTao, ThoiGianTao, NguoiSua, ThoiGianSua,
    TrangThai, DaXoa
)
SELECT
    seed.MaLopHoc,
    @teacher_id,
    1,
    DATE_SUB(@today, INTERVAL 30 DAY),
    @seed_owner_id, @now, @seed_owner_id, @now,
    1, 0
FROM
(
    SELECT @class_1 AS MaLopHoc
    UNION ALL SELECT @class_2
    UNION ALL SELECT @class_3
) seed
WHERE @teacher_user_id IS NOT NULL
ON DUPLICATE KEY UPDATE
    LoaiVaiTro = VALUES(LoaiVaiTro),
    NgayThamGia = VALUES(NgayThamGia),
    NguoiSua = VALUES(NguoiSua),
    ThoiGianSua = VALUES(ThoiGianSua),
    TrangThai = 1,
    DaXoa = 0;

INSERT INTO hocsinhlophoc (
    MaLopHoc, MaHocSinh, MaTrangThai, NgayThamGia, NgayRoiLop,
    NguoiTao, ThoiGianTao, NguoiSua, ThoiGianSua,
    TrangThai, DaXoa
)
SELECT
    seed.MaLopHoc,
    @student_id,
    NULL,
    seed.NgayThamGia,
    seed.NgayRoiLop,
    @seed_owner_id, @now, @seed_owner_id, @now,
    1, 0
FROM
(
    SELECT
        @class_1 AS MaLopHoc,
        DATE_SUB(@today, INTERVAL 21 DAY) AS NgayThamGia,
        NULL AS NgayRoiLop
    UNION ALL
    SELECT
        @class_2 AS MaLopHoc,
        DATE_SUB(@today, INTERVAL 140 DAY) AS NgayThamGia,
        DATE_SUB(@today, INTERVAL 30 DAY) AS NgayRoiLop
    UNION ALL
    SELECT
        @class_3 AS MaLopHoc,
        DATE_SUB(@today, INTERVAL 10 DAY) AS NgayThamGia,
        NULL AS NgayRoiLop
 ) seed
WHERE @student_user_id IS NOT NULL
ON DUPLICATE KEY UPDATE
    NgayThamGia = VALUES(NgayThamGia),
    NgayRoiLop = VALUES(NgayRoiLop),
    NguoiSua = VALUES(NguoiSua),
    ThoiGianSua = VALUES(ThoiGianSua),
    TrangThai = 1,
    DaXoa = 0;

INSERT INTO chuonghoc (
    MaChuong, MaKhoaHoc, MaLopHoc, TenChuong, MoTa, ThuTu,
    NguoiTao, ThoiGianTao, NguoiSua, ThoiGianSua,
    TrangThai, DaXoa
)
VALUES
(
    @chapter_1,
    @course_1,
    @class_1,
    'Chao hoi va gioi thieu ban than',
    'Noi dung mo dau cua lop giao tiep hien tai.',
    1,
    @seed_owner_id, @now, @seed_owner_id, @now,
    1, 0
),
(
    @chapter_2,
    @course_1,
    @class_1,
    'Mau cau giao tiep hang ngay',
    'Tong hop cac tinh huong giao tiep don gian.',
    2,
    @seed_owner_id, @now, @seed_owner_id, @now,
    1, 0
),
(
    @chapter_3,
    @course_1,
    @class_2,
    'On tap chu de gia dinh va cong viec',
    'Hoc lieu cho lop da hoc de test xem lai tai lieu cu.',
    1,
    @seed_owner_id, @now, @seed_owner_id, @now,
    1, 0
),
(
    @chapter_4,
    @course_2,
    @class_3,
    'Ky nang nghe TOEIC Part 1 - Part 2',
    'Noi dung co ban cho lop TOEIC hien tai.',
    1,
    @seed_owner_id, @now, @seed_owner_id, @now,
    1, 0
)
ON DUPLICATE KEY UPDATE
    MaKhoaHoc = VALUES(MaKhoaHoc),
    MaLopHoc = VALUES(MaLopHoc),
    TenChuong = VALUES(TenChuong),
    MoTa = VALUES(MoTa),
    ThuTu = VALUES(ThuTu),
    NguoiSua = VALUES(NguoiSua),
    ThoiGianSua = VALUES(ThoiGianSua),
    TrangThai = 1,
    DaXoa = 0;

INSERT INTO tailieu (
    MaTaiLieu, MaChuongHoc, TenTaiLieu, LinkTaiLieu, MoTa,
    NguoiTao, ThoiGianTao, NguoiSua, ThoiGianSua,
    TrangThai, DaXoa
)
VALUES
(
    @document_1,
    @chapter_1,
    'Huong dan buoi 1 - Chao hoi va gioi thieu.pdf',
    '/uploads/study-materials/seed/teacher-study-content/class-a-intro-guide.pdf',
    'Tai lieu PDF demo cho chuong mo dau cua lop giao tiep.',
    @seed_owner_id, @now, @seed_owner_id, @now,
    1, 0
),
(
    @document_2,
    @chapter_2,
    'Checklist giao tiep hang ngay.docx',
    '/uploads/study-materials/seed/teacher-study-content/class-a-daily-conversation-checklist.docx',
    'Tai lieu Word demo cho chuong mau cau giao tiep hang ngay.',
    @seed_owner_id, @now, @seed_owner_id, @now,
    1, 0
),
(
    @document_3,
    @chapter_3,
    'Bang tu vung on tap cuoi tuan.xlsx',
    '/uploads/study-materials/seed/teacher-study-content/class-b-review-vocabulary.xlsx',
    'Tai lieu Excel demo cho lop da hoc truoc do.',
    @seed_owner_id, @now, @seed_owner_id, @now,
    1, 0
),
(
    @document_4,
    @chapter_4,
    'Luyen nghe TOEIC Part 1 - Part 2.wav',
    '/uploads/study-materials/seed/teacher-study-content/toeic-part1-part2-listening-practice.wav',
    'Tai lieu audio demo cho lop TOEIC hien tai.',
    @seed_owner_id, @now, @seed_owner_id, @now,
    1, 0
)
ON DUPLICATE KEY UPDATE
    MaChuongHoc = VALUES(MaChuongHoc),
    TenTaiLieu = VALUES(TenTaiLieu),
    LinkTaiLieu = VALUES(LinkTaiLieu),
    MoTa = VALUES(MoTa),
    NguoiSua = VALUES(NguoiSua),
    ThoiGianSua = VALUES(ThoiGianSua),
    TrangThai = 1,
    DaXoa = 0;

COMMIT;

SELECT
    @teacher_email AS teacher_email,
    @teacher_user_id AS teacher_user_id,
    @teacher_id AS teacher_id,
    CASE
        WHEN @teacher_user_id IS NULL THEN 'Teacher account not found. Class assignment skipped.'
        ELSE 'Teacher demo data is ready.'
    END AS teacher_status,
    @student_email AS student_email,
    @student_user_id AS student_user_id,
    @student_id AS student_id,
    CASE
        WHEN @student_user_id IS NULL THEN 'Student account not found. Student enrollment skipped.'
        ELSE 'Student demo data is ready.'
    END AS student_status;

SELECT
    kh.TenKhoaHoc AS ten_khoa_hoc,
    lh.TenLop AS ten_lop,
    COUNT(DISTINCT ch.MaChuong) AS so_chuong,
    COUNT(DISTINCT tl.MaTaiLieu) AS so_tai_lieu
FROM khoahoc kh
JOIN chitietkhoahoc_lophoc cklh
    ON cklh.MaKhoaHoc = kh.MaKhoaHoc
   AND IFNULL(cklh.DaXoa, 0) = 0
   AND IFNULL(cklh.TrangThai, 1) = 1
JOIN lophoc lh
    ON lh.MaLopHoc = cklh.MaLopHoc
   AND IFNULL(lh.DaXoa, 0) = 0
   AND IFNULL(lh.TrangThai, 1) = 1
LEFT JOIN chuonghoc ch
    ON ch.MaKhoaHoc = kh.MaKhoaHoc
   AND ch.MaLopHoc = lh.MaLopHoc
   AND IFNULL(ch.DaXoa, 0) = 0
   AND IFNULL(ch.TrangThai, 1) = 1
LEFT JOIN tailieu tl
    ON tl.MaChuongHoc = ch.MaChuong
   AND IFNULL(tl.DaXoa, 0) = 0
   AND IFNULL(tl.TrangThai, 1) = 1
WHERE kh.MaKhoaHoc IN (@course_1, @course_2)
GROUP BY kh.TenKhoaHoc, lh.TenLop
ORDER BY kh.TenKhoaHoc, lh.TenLop;

SELECT
    nd.Email AS hoc_sinh_email,
    kh.TenKhoaHoc AS ten_khoa_hoc,
    lh.TenLop AS ten_lop,
    hslh.NgayThamGia AS ngay_tham_gia,
    hslh.NgayRoiLop AS ngay_roi_lop,
    CASE
        WHEN hslh.NgayRoiLop IS NULL OR hslh.NgayRoiLop >= @today THEN 'Dang hoc'
        ELSE 'Da hoc'
    END AS trang_thai_lop
FROM hocsinhlophoc hslh
JOIN hocsinh hs
    ON hs.MaHocSinh = hslh.MaHocSinh
JOIN nguoidung nd
    ON nd.MaNguoiDung = hs.MaNguoiDung
JOIN lophoc lh
    ON lh.MaLopHoc = hslh.MaLopHoc
JOIN chitietkhoahoc_lophoc cklh
    ON cklh.MaLopHoc = lh.MaLopHoc
   AND IFNULL(cklh.DaXoa, 0) = 0
   AND IFNULL(cklh.TrangThai, 1) = 1
JOIN khoahoc kh
    ON kh.MaKhoaHoc = cklh.MaKhoaHoc
WHERE hs.MaHocSinh = @student_id
  AND IFNULL(hslh.DaXoa, 0) = 0
  AND IFNULL(hslh.TrangThai, 1) = 1
ORDER BY kh.TenKhoaHoc, lh.TenLop;
