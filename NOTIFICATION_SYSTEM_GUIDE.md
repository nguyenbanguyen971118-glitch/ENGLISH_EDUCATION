# Hướng dẫn sử dụng Hệ thống Thông báo (Notification System)

## 📋 Tổng quan

Hệ thống thông báo được thiết kế để gửi thông báo chính xác đến các nhóm người dùng khác nhau:
- **Admin** - Quản trị viên
- **Giao_Vien** - Giáo viên
- **Hoc_Sinh** - Học sinh
- **Phu_Huynh** - Phụ huynh
- **Tat_Ca** - Tất cả người dùng

## 🔌 SignalR Real-time Integration

### Kết nối đến NotificationHub

```javascript
// Frontend (JavaScript/TypeScript)
import { HubConnectionBuilder } from "@aspnetcore/signalr";

const connection = new HubConnectionBuilder()
    .withUrl("http://localhost:5000/hubs/notification", {
        accessTokenFactory: () => localStorage.getItem("token")
    })
    .withAutomaticReconnect()
    .build();

connection.start()
    .then(() => console.log("Connected to notification hub"))
    .catch(err => console.log(err));

// Lắng nghe thông báo mới
connection.on("NewNotification", (notification) => {
    console.log("Thông báo mới:", notification);
    // Cập nhật UI
});

// Lắng nghe thay đổi số lượng chưa đọc
connection.on("UnreadCountUpdated", (count) => {
    console.log("Số thông báo chưa đọc:", count);
});

// Đánh dấu thông báo là đã đọc
connection.invoke("MarkNotificationAsRead", notificationId)
    .catch(err => console.log(err));

// Lấy số lượng chưa đọc
connection.invoke("GetUnreadCount")
    .catch(err => console.log(err));
```

## 📡 REST API Endpoints

### 1. Lấy tất cả thông báo (Admin only)
```http
GET /api/notification
Authorization: Bearer {token}
```

**Response:**
```json
{
  "success": true,
  "message": "Lấy danh sách thông báo thành công",
  "data": [
    {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "title": "Thông báo quan trọng",
      "content": "Nội dung thông báo",
      "doiTuong": "Tat_Ca",
      "createdAt": "2026-05-09T10:30:00Z"
    }
  ]
}
```

### 2. Lấy thông báo của người dùng hiện tại
```http
GET /api/notification/my-notifications
Authorization: Bearer {token}
```

**Response:**
```json
{
  "success": true,
  "message": "Lấy thông báo của người dùng thành công",
  "data": [
    {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "title": "Thông báo mới",
      "content": "Nội dung",
      "doiTuong": "Tat_Ca",
      "createdAt": "2026-05-09T10:30:00Z",
      "isRead": false,
      "readAt": null
    }
  ]
}
```

### 3. Lấy thông báo chưa đọc
```http
GET /api/notification/unread
Authorization: Bearer {token}
```

### 4. Đếm thông báo chưa đọc
```http
GET /api/notification/unread-count
Authorization: Bearer {token}
```

**Response:**
```json
{
  "success": true,
  "message": "Lấy số lượng thông báo chưa đọc thành công",
  "data": 5
}
```

### 5. Đánh dấu một thông báo là đã đọc
```http
POST /api/notification/{notificationId}/mark-as-read
Authorization: Bearer {token}
```

### 6. Đánh dấu tất cả thông báo là đã đọc
```http
POST /api/notification/mark-all-as-read
Authorization: Bearer {token}
```

### 7. Tạo thông báo mới (Admin only)
```http
POST /api/notification
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "Tiêu đề thông báo",
  "content": "Nội dung thông báo",
  "doiTuong": "Hoc_Sinh"  // Admin, Giao_Vien, Hoc_Sinh, Phu_Huynh, Tat_Ca
}
```

### 8. Cập nhật thông báo (Admin only)
```http
PUT /api/notification/{notificationId}
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "Tiêu đề mới",
  "content": "Nội dung mới",
  "doiTuong": "Phu_Huynh"
}
```

### 9. Xóa thông báo (Admin only)
```http
DELETE /api/notification/{notificationId}
Authorization: Bearer {token}
```

## 🎯 Cách sử dụng NotificationEventService

`NotificationEventService` được sử dụng để xử lý các sự kiện tự động sinh ra thông báo.

### Ví dụ: Thông báo khi có bài tập mới

```csharp
// Trong AssignmentController.cs
[HttpPost]
public async Task<IActionResult> CreateAssignment(
    [FromBody] CreateAssignmentDto dto,
    [FromServices] NotificationEventService notificationService)
{
    var assignment = new Assignment
    {
        Title = dto.Title,
        Description = dto.Description,
        // ... các field khác
    };
    
    await _context.Assignments.AddAsync(assignment);
    await _context.SaveChangesAsync();
    
    // Thông báo cho tất cả sinh viên
    await notificationService.NotifyNewAssignmentAsync(
        GetCurrentUserId(),
        assignment.Title,
        $"Có bài tập mới: {assignment.Description}"
    );
    
    return Ok(assignment);
}
```

### Các phương thức có sẵn trong NotificationEventService:

1. **NotifyNewAssignmentAsync** - Thông báo bài tập mới cho sinh viên
2. **NotifyTeachersAsync** - Thông báo cho giáo viên
3. **NotifyParentsAsync** - Thông báo cho phụ huynh
4. **NotifyAllAsync** - Thông báo cho tất cả
5. **NotifyUserAsync** - Thông báo cho một người dùng cụ thể
6. **NotifyScheduleChangeAsync** - Thông báo thay đổi lịch học
7. **NotifyAssignmentSubmittedAsync** - Thông báo nộp bài
8. **NotifyGradeAsync** - Thông báo điểm số

## 🏗️ Cấu trúc thư mục

```
backend/
├── Controllers/
│   └── NotificationController.cs          # REST API endpoints
├── DTOs/
│   └── NotificationDto.cs                 # Data Transfer Objects
├── Hubs/
│   ├── ChatHub.cs                         # Chat Hub (existing)
│   └── NotificationHub.cs                 # Notification Hub (new)
├── Models/
│   ├── Thongbao.cs                        # Notification model
│   └── Nguoinhanthongbao.cs               # Notification recipient
├── Repositories/
│   ├── NotificationRepository.cs          # Data access
│   └── Interfaces/
│       └── INotificationRepository.cs     # Repository interface
└── Services/
    ├── NotificationService.cs             # Business logic
    ├── NotificationEventService.cs        # Event-based notifications
    └── Interfaces/
        └── INotificationService.cs        # Service interface
```

## 📊 Database Schema

### Bảng: Thongbao
```sql
CREATE TABLE Thongbao (
    MaThongBao GUID PRIMARY KEY,
    TieuDe NVARCHAR(255) NOT NULL,
    NoiDung NVARCHAR(MAX) NOT NULL,
    DoiTuong VARCHAR(50) DEFAULT 'Tat_Ca',  -- Admin, Giao_Vien, Hoc_Sinh, Phu_Huynh, Tat_Ca
    NguoiTao GUID,
    ThoiGianTao DATETIME,
    NguoiSua GUID,
    ThoiGianSua DATETIME,
    TrangThai BIT,
    DaXoa BIT
);
```

### Bảng: Nguoinhanthongbao
```sql
CREATE TABLE Nguoinhanthongbao (
    MaThongBao GUID,
    MaNguoiDung GUID,
    DaDoc BIT DEFAULT 0,
    NgayDoc DATETIME,
    NguoiTao GUID,
    ThoiGianTao DATETIME,
    NguoiSua GUID,
    ThoiGianSua DATETIME,
    TrangThai BIT,
    DaXoa BIT,
    PRIMARY KEY (MaThongBao, MaNguoiDung)
);
```

## 🔐 Bảo mật

- Tất cả endpoints yêu cầu xác thực JWT
- Admin có quyền tạo, cập nhật, xóa thông báo
- Người dùng chỉ có thể xem thông báo của chính mình
- SignalR sử dụng JWT token qua query string: `?access_token={token}`

## 🐛 Troubleshooting

### SignalR không kết nối
- Kiểm tra token JWT có hợp lệ không
- Đảm bảo URL đúng: `/hubs/notification`
- Kiểm tra CORS settings trong Program.cs

### Thông báo không gửi đến đúng người dùng
- Xác nhận rằng `DoiTuong` được ghi đúng trong database
- Kiểm tra tên vai trò phải chính xác: `Admin`, `Giao_Vien`, `Hoc_Sinh`, `Phu_Huynh`

### Real-time không hoạt động
- Đảm bảo SignalR Hub được map đúng trong Program.cs
- Kiểm tra firewall không chặn WebSocket connections

## 📝 Ghi chú

- Hệ thống sử dụng soft delete (đánh dấu `DaXoa = true`)
- Tất cả timestamp sử dụng UTC time
- Hỗ trợ pagination trong các endpoint lấy danh sách (tuỳ chỉnh thêm nếu cần)

---

**Version**: 1.0.0  
**Last Updated**: 2026-05-09
