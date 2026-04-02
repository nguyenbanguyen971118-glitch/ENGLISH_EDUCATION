-- ============================================================================
-- Script: Chuẩn hoá Phân quyền (VaiTro Names)
-- Purpose: Standardize role names to Admin, Giao_Vien, Hoc_Sinh, Phu_Huynh
-- ============================================================================

-- 1. Chuẩn hoá tên VaiTro - Xóa duplicate, cập nhật case
-- Ghi chú: Kiểm tra trước khi chạy
SELECT DISTINCT TenVaiTro FROM VaiTro WHERE DaXoa = 0 OR DaXoa IS NULL;

-- 2. Cập nhật Admin
UPDATE VaiTro 
SET TenVaiTro = 'Admin'
WHERE LOWER(TenVaiTro) IN ('admin', 'quanly', 'administrator')
  AND (DaXoa = 0 OR DaXoa IS NULL);

-- 3. Cập nhật Giáo viên
UPDATE VaiTro 
SET TenVaiTro = 'Giao_Vien'
WHERE LOWER(TenVaiTro) IN ('giao_vien', 'giaovien', 'teacher', 'giáo viên')
  AND (DaXoa = 0 OR DaXoa IS NULL);

-- 4. Cập nhật Học sinh
UPDATE VaiTro 
SET TenVaiTro = 'Hoc_Sinh'
WHERE LOWER(TenVaiTro) IN ('hoc_sinh', 'hocsinh', 'student', 'học sinh')
  AND (DaXoa = 0 OR DaXoa IS NULL);

-- 5. Cập nhật Phụ huynh
UPDATE VaiTro 
SET TenVaiTro = 'Phu_Huynh'
WHERE LOWER(TenVaiTro) IN ('phu_huynh', 'phuhuynh', 'parent', 'phụ huynh')
  AND (DaXoa = 0 OR DaXoa IS NULL);

-- 6. Verify kết quả
SELECT MaVaiTro, TenVaiTro, COUNT(*) AS SoLan 
FROM VaiTro 
WHERE DaXoa = 0 OR DaXoa IS NULL
GROUP BY MaVaiTro, TenVaiTro
ORDER BY TenVaiTro;

-- 7. Verify VaiTroQuyen mappings - Kiểm tra quyền của từng role
SELECT 
    vtr.MaVaiTro,
    vtr.TenVaiTro,
    q.MaQuyen,
    q.TenQuyen,
    cnc.MaChucNang,
    cnc.TenChucNang,
    vtq.DaXoa
FROM VaiTroQuyen vtq
INNER JOIN VaiTro vtr ON vtq.MaVaiTro = vtr.MaVaiTro
INNER JOIN Quyen q ON vtq.MaQuyen = q.MaQuyen
INNER JOIN Chucnang cnc ON q.MaChucNang = cnc.MaChucNang
WHERE vtq.DaXoa = 0 OR vtq.DaXoa IS NULL
ORDER BY vtr.TenVaiTro, cnc.TenChucNang, q.TenQuyen;
