# Kiểm Tra Tính Năng: Thêm Lớp Học và Xếp Lịch Học

## Tổng Quan Yêu Cầu
Hệ thống cho phép Admin tạo lớp học và tự động sinh lịch học dựa trên cấu hình.

---

## Kiểm Tra Yêu Cầu Nghiệp Vụ

### BR01: Mỗi khóa học phải có tổng số tiết xác định
- **Frontend**: ✅ Trường "Số tiết" (sotiet) cho phép Admin nhập tổng số tiết
- **Backend**: ✅ DTO chứa `TotalPeriods` (nullable int)
- **Xác minh**: Admin nhập "30" tiết
- **Kỳ vọng**: Hệ thống sinh lịch đủ 30 tiết

### BR02: Tự động hiển thị hoặc yêu cầu nhập số tiết
- **Frontend**: ✅ Trường nhập tự do (không bắt buộc)
- **Helper text**: ✅ "Số tiết là tổng số tiết của khóa học..."
- **Xác minh**: Có thể nhập hoặc bỏ trống

### BR03: Admin chọn nhiều ngày học trong tuần
- **Frontend**: ✅ Multi-day dropdown với checkbox cho 7 ngày
- **Xác minh**: Chọn "Thu 2", "Thu 4", "Thu 6"
- **Kỳ vọng**: Lịch được sinh cho các ngày này lặp lại

### BR04: Mỗi ngày được phép chọn nhiều tiết
- **Frontend**: ✅ Multi-period picker với checkbox cho Tiết 1-14
- **Xác minh**: Chọn "Tiết 1, 2, 3" cho "Thu 2"
- **Kỳ vọng**: Mỗi Thu 2 trong lịch có 3 tiết liên tiếp

### BR05: Tự động lặp lịch cho đến khi đủ tổng số tiết
- **Backend**: ✅ ScheduleGeneratorService lặp tuần-by-tuần
- **Xác minh**: 
  - Tạo lớp: Thu 2, 4, 6 với 3 tiết/ngày = 9 tiết/tuần
  - TotalPeriods = 30
  - Kỳ vọng: ~3-4 tuần lịch (tùy startDate)
- **Kiểm tra DB**: SELECT COUNT(*) FROM Buoihoc WHERE MaLopHoc = [classId]
- **Kỳ vọng**: ≥ 30 tiết (có thể hơn do rounding)

### BR06: Ngày kết thúc tự động tính
- **Frontend**: ✅ Trường "Ngày kết thúc" readonly
- **Backend**: ✅ Controller gán `classEntity.NgayKetThuc = EndDate.Value`
- **Xác minh**: 
  - Tạo lớp với startDate = 24/06/2026
  - Xem endDate trên trang `/admin/classes`
  - Kỳ vọng: endDate ≠ null và ≥ startDate

### BR07: Không trùng giáo viên, phòng, lịch lớp
- **Backend**: ✅ ScheduleGeneratorService kiểm tra 3 loại conflict:
  1. Class conflict: cùng lớp, ngày, tiết trùng
  2. Teacher conflict: giáo viên dạy trùng ngày/tiết
  3. Room conflict: phòng học dùng trùng ngày/tiết
- **Xác minh**: 
  - Tạo 2 lớp cùng giáo viên + cùng Thu 2 lúc 7:00-8:45
  - Lớp 2 phải có lỗi "Trung lich giang vien"
- **Kiểm tra**: Xem log từ browser console hoặc backend error message

### BR08: Hiển thị lỗi nếu dữ liệu không hợp lệ
- **Frontend**: ✅ Validation trước khi submit (tên lớp, khóa, giáo viên, ngày, tiết)
- **Backend**: ✅ Trả lỗi nếu generation thất bại
- **Xác minh**: 
  - Tạo lớp mà không chọn ngày: → lỗi "Vui long chon day..."
  - Tạo lớp có conflict: → lỗi từ backend (conflict message)

---

## Quy Trình Kiểm Tra Chi Tiết (Step-by-Step)

### Bước 1: Chuẩn Bị
1. Khởi động backend: `cd QuanLyTrungTam/backend && dotnet run`
2. Khởi động frontend: `cd QuanLyTrungTam/frontend && npm run dev`
3. Mở browser: `http://localhost:3000/admin/classes/create`

### Bước 2: Tạo Lớp Học Cơ Bản
**Nhập:**
- Tên lớp: `Test Schedule 1`
- Khóa học: `Lộ trình IELTS chuyên sâu` (hoặc khóa có sẵn)
- Giáo viên: Chọn một giáo viên
- Sức chứa: `30`
- **Số tiết: `30`**
- Ngày bắt đầu: `24/06/2026`
- Ngày kết thúc: _(để trống, sẽ tự động)_

### Bước 3: Cấu Hình Lịch
1. Nhấn "Them lich"
2. Chọn ngày: `Thu 2`, `Thu 4`, `Thu 6` (3 ngày)
3. Chọn tiết: `Tiết 1, 2, 3` (3 tiết/ngày)
4. Bấm "Tao lop hoc"

### Bước 4: Xác Minh Kết Quả
1. **Trên form**: Xem "Ngày kết thúc" (phải có giá trị, ví dụ: 30/07/2026)
2. **Trên trang `/admin/classes`**:
   - Tìm lớp vừa tạo
   - Xem cột "Thoi gian hoc" → phải hiển thị "24/06/2026 - 30/07/2026"
3. **Trên trang `/admin/schedules`** (hoặc Lich day va hoc):
   - Xem lịch của giáo viên → phải có ~27-30 buổi học
   - Các buổi học phải vào Thu 2, 4, 6 từ 7:00-8:45 (tiết 1-3)

### Bước 5: Kiểm Tra Conflict
1. Tạo lớp thứ 2 **cùng giáo viên** + **cùng Thu 2** 
2. Nhập "Số tiết" = 10
3. Bấm "Tao lop hoc"
4. **Kỳ vọng**: Lỗi "Trung lich giang vien..." hoặc lớp được tạo nhưng không có lịch

### Bước 6: Kiểm Tra Validation
1. Tạo lớp mà **không nhập "Số tiết"** (bỏ trống)
2. Bấn "Tao lop hoc"
3. **Kỳ vọng**: Lớp được tạo nhưng **không sinh lịch** (vì TotalPeriods = null/0)

---

## Danh Sách Kiểm Tra (Checklist)

| # | Yêu Cầu | Test Case | Kỳ Vọng | Kết Quả |
|---|---------|-----------|---------|---------|
| 1 | BR01 | Nhập Số tiết = 30 | Hệ thống nhận giá trị | ☐ |
| 2 | BR02 | Số tiết có helper text | Hiển thị text hướng dẫn | ☐ |
| 3 | BR03 | Chọn 3 ngày (Thu 2, 4, 6) | Lịch sinh cho 3 ngày | ☐ |
| 4 | BR04 | Chọn 3 tiết/ngày (1, 2, 3) | Lịch có 3 tiết liên tiếp | ☐ |
| 5 | BR05 | Tạo lớp với TotalPeriods=30 | Sinh ~30 buổi (30÷9 tuần) | ☐ |
| 6 | BR06 | Xem endDate sau tạo | endDate ≠ null | ☐ |
| 7 | BR06 | endDate hiển thị /admin/classes | Hiển thị như "24/06-30/07" | ☐ |
| 8 | BR07 | Tạo 2 lớp cùng GV + ngày | Lỗi hoặc không sinh lịch | ☐ |
| 9 | BR08 | Submit mà chưa chọn ngày | Lỗi validation | ☐ |
| 10 | BR08 | Submit mà không nhập Số tiết | Lớp tạo, không sinh lịch | ☐ |

---

## Ghi Chú Kỹ Thuật

### Database Validation
```sql
-- Kiểm tra lớp vừa tạo
SELECT MaLopHoc, TenLop, NgayBatDau, NgayKetThuc 
FROM Lophoc 
WHERE TenLop = 'Test Schedule 1';

-- Kiểm tra lịch sinh ra
SELECT COUNT(*) AS TotalSessions, 
       MIN(NgayHoc) AS FirstDate, 
       MAX(NgayHoc) AS LastDate
FROM Buoihoc 
WHERE MaLopHoc = '[ClassId từ trên]';
```

### Browser Console (DevTools)
Kiểm tra logs:
```javascript
// AdminCreateClass.jsx logs
console.debug('AdminCreateClass - create payload', payload)
console.debug('AdminCreateClass - create response', createdClass)
```

### Common Issues & Fixes
| Issue | Nguyên Nhân | Fix |
|-------|-----------|-----|
| endDate không hiển thị | DateOnly serialization | ✅ formatServerDate() thêm vào AdminClasses.jsx |
| Lịch không được sinh | TotalPeriods = null/0 hoặc ScheduleConfigs rỗng | ✅ Backend kiểm tra điều kiện |
| Conflict không được phát hiện | Query sai hoặc logic sai | ✅ Kiểm tra DB conflicts |
| Transaction rollback | SaveChangesAsync gọi sai chỗ | ✅ Loại bỏ SaveChangesAsync từ generator |

---

## Kết Luận
Sau khi pass tất cả kiểm tra trên, tính năng được coi là **PASS** và sẵn sàng production.

**Date Checked**: 24/06/2026  
**Tested By**: [User]  
**Status**: 🟡 Chưa kiểm tra thực tế
