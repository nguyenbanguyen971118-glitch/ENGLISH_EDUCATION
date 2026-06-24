-- Test multi-slot schedule (tiết 7-8) display and auto-room assignment
USE HeThongHocOnline_Final;

-- Lấy một lớp hiện có để test
SET @test_class = (SELECT MaLopHoc FROM LopHoc LIMIT 1);
SET @test_room_available = (SELECT MaPhongHoc FROM PhongHoc WHERE DaXoa = 0 LIMIT 1);

-- Insert một lịch học mới: tiết 7-8 (slotId=7, slotEndId=8)
-- Ngày hôm nay hoặc ngày gần nhất (để test dễ thấy)
INSERT INTO BuoiHoc (MaBuoiHoc, MaLopHoc, MaPhongHoc, NgayHoc, MaTietBatDau, MaTietKetThuc, TieuDe, NoiDung, ThoiGianTao, TrangThai, DaXoa)
VALUES (
    UUID(),
    @test_class,
    @test_room_available,
    CURDATE(),
    7,
    8,
    'Buổi Học Test - Tiết 7-8',
    'Kiểm tra hiển thị multi-slot và auto-room assign',
    NOW(),
    1,
    0
);

-- Verify insert thành công
SELECT COUNT(*) AS 'Total BuoiHoc' FROM BuoiHoc WHERE MaTietBatDau = 7 AND MaTietKetThuc = 8;
SELECT * FROM BuoiHoc WHERE MaTietBatDau = 7 AND MaTietKetThuc = 8 ORDER BY ThoiGianTao DESC LIMIT 1;
