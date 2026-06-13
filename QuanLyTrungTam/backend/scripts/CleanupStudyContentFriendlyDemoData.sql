-- Cleanup demo data for Study Content
-- This script only soft-deletes the demo dataset used by:
--   SeedTeacherStudyContentTestData.sql
--   SeedStudyContentFriendlyDemoData.sql
--
-- It does NOT delete bootstrap accounts such as:
--   admin@qltt.local
--   giaovien@qltt.local
--   hocsinh@qltt.local

USE quanlytrungtam;

SET NAMES utf8mb4;
SET @OLD_SQL_SAFE_UPDATES = @@SQL_SAFE_UPDATES;
SET SQL_SAFE_UPDATES = 0;

SET @admin_email = 'admin@qltt.local';
SET @teacher_email = 'giaovien@qltt.local';
SET @student_email = 'hocsinh@qltt.local';
SET @now = UTC_TIMESTAMP();

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

UPDATE tailieu
SET
    TrangThai = 0,
    DaXoa = 1,
    NguoiSua = @seed_owner_id,
    ThoiGianSua = @now
WHERE MaTaiLieu IN (@document_1, @document_2, @document_3, @document_4)
   OR MaChuongHoc IN (
        SELECT demo_chapters.MaChuong
        FROM (
            SELECT ch.MaChuong
            FROM chuonghoc ch
            WHERE ch.MaKhoaHoc IN (@course_1, @course_2)
               OR ch.MaLopHoc IN (@class_1, @class_2, @class_3)
        ) AS demo_chapters
    )
   OR LinkTaiLieu LIKE '/uploads/study-materials/seed/teacher-study-content/%';

UPDATE chuonghoc
SET
    TrangThai = 0,
    DaXoa = 1,
    NguoiSua = @seed_owner_id,
    ThoiGianSua = @now
WHERE MaChuong IN (@chapter_1, @chapter_2, @chapter_3, @chapter_4)
   OR MaKhoaHoc IN (@course_1, @course_2)
   OR MaLopHoc IN (@class_1, @class_2, @class_3);

UPDATE hocsinhlophoc
SET
    TrangThai = 0,
    DaXoa = 1,
    NguoiSua = @seed_owner_id,
    ThoiGianSua = @now
WHERE MaLopHoc IN (@class_1, @class_2, @class_3);

UPDATE giangvienlophoc
SET
    TrangThai = 0,
    DaXoa = 1,
    NguoiSua = @seed_owner_id,
    ThoiGianSua = @now
WHERE MaLopHoc IN (@class_1, @class_2, @class_3);

UPDATE chitietkhoahoc_lophoc
SET
    TrangThai = 0,
    DaXoa = 1,
    NguoiSua = @seed_owner_id,
    ThoiGianSua = @now
WHERE (MaKhoaHoc = @course_1 AND MaLopHoc IN (@class_1, @class_2))
   OR (MaKhoaHoc = @course_2 AND MaLopHoc = @class_3);

UPDATE lophoc
SET
    TrangThai = 0,
    DaXoa = 1,
    NguoiSua = @seed_owner_id,
    ThoiGianSua = @now
WHERE MaLopHoc IN (@class_1, @class_2, @class_3);

UPDATE khoahoc
SET
    TrangThai = 0,
    DaXoa = 1,
    NguoiSua = @seed_owner_id,
    ThoiGianSua = @now
WHERE MaKhoaHoc IN (@course_1, @course_2);

COMMIT;

SELECT
    'Demo study content data cleaned up by soft delete.' AS message,
    @now AS cleaned_at_utc;

SELECT
    'khoahoc' AS table_name,
    COUNT(*) AS remaining_active_rows
FROM khoahoc
WHERE MaKhoaHoc IN (@course_1, @course_2)
  AND IFNULL(DaXoa, 0) = 0
  AND IFNULL(TrangThai, 1) = 1
UNION ALL
SELECT
    'lophoc' AS table_name,
    COUNT(*) AS remaining_active_rows
FROM lophoc
WHERE MaLopHoc IN (@class_1, @class_2, @class_3)
  AND IFNULL(DaXoa, 0) = 0
  AND IFNULL(TrangThai, 1) = 1
UNION ALL
SELECT
    'chuonghoc' AS table_name,
    COUNT(*) AS remaining_active_rows
FROM chuonghoc
WHERE MaChuong IN (@chapter_1, @chapter_2, @chapter_3, @chapter_4)
  AND IFNULL(DaXoa, 0) = 0
  AND IFNULL(TrangThai, 1) = 1
UNION ALL
SELECT
    'tailieu' AS table_name,
    COUNT(*) AS remaining_active_rows
FROM tailieu
WHERE MaTaiLieu IN (@document_1, @document_2, @document_3, @document_4)
  AND IFNULL(DaXoa, 0) = 0
  AND IFNULL(TrangThai, 1) = 1;

SET SQL_SAFE_UPDATES = @OLD_SQL_SAFE_UPDATES;
