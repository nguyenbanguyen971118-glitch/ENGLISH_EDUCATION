# Ví dụ sử dụng Hệ thống Thông báo

## 1️⃣ Ví dụ Backend - Tạo Bài Tập Mới

Khi giáo viên tạo một bài tập mới, hệ thống tự động gửi thông báo cho tất cả học sinh:

```csharp
using backend.DTOs;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AssignmentController : ControllerBase
    {
        private readonly AssignmentService _assignmentService;
        private readonly NotificationEventService _notificationService;

        public AssignmentController(
            AssignmentService assignmentService,
            NotificationEventService notificationService)
        {
            _assignmentService = assignmentService;
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssignment(
            [FromBody] CreateAssignmentDto dto)
        {
            // Tạo bài tập
            var assignment = await _assignmentService.CreateAsync(dto);

            // Gửi thông báo cho tất cả học sinh
            await _notificationService.NotifyNewAssignmentAsync(
                GetCurrentUserId(),
                assignment.Title,
                $"Bài tập mới: {assignment.Title}\n{assignment.Description}"
            );

            return Ok(assignment);
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }
    }
}
```

## 2️⃣ Ví dụ Backend - Admin gửi thông báo cho giáo viên

Admin muốn thông báo tất cả giáo viên về thay đổi lịch học:

```csharp
[HttpPost("announce-schedule")]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> AnnounceScheduleChange(
    [FromBody] ScheduleChangeDto dto)
{
    // Cập nhật lịch học
    await _scheduleService.UpdateAsync(dto);

    // Gửi thông báo cho giáo viên
    await _notificationService.NotifyTeachersAsync(
        GetCurrentUserId(),
        "Thay đổi lịch học",
        $"Lớp {dto.ClassName} có thay đổi lịch học:\n{dto.Details}"
    );

    return Ok(new { success = true });
}
```

## 3️⃣ Ví dụ Backend - Giáo viên ghi nhận điểm

Khi giáo viên ghi điểm cho học sinh:

```csharp
[HttpPost("grade")]
[Authorize(Roles = "Giao_Vien")]
public async Task<IActionResult> GradeAssignment(
    [FromBody] GradeDto dto)
{
    // Lưu điểm
    var grade = await _gradeService.SaveAsync(dto);

    // Thông báo học sinh
    await _notificationService.NotifyGradeAsync(
        GetCurrentUserId(),
        dto.StudentId,
        dto.AssignmentTitle,
        dto.Score
    );

    return Ok(grade);
}
```

## 4️⃣ Ví dụ Frontend - React/TypeScript

### Setup SignalR connection:

```typescript
// notificationService.ts
import { HubConnectionBuilder, HttpTransportType } from "@aspnetcore/signalr";

class NotificationService {
    private connection: any;

    async connect(token: string) {
        this.connection = new HubConnectionBuilder()
            .withUrl("http://localhost:5000/hubs/notification", {
                accessTokenFactory: () => token,
                transport: HttpTransportType.WebSockets
            })
            .withAutomaticReconnect()
            .build();

        this.connection.on("NewNotification", (notification) => {
            console.log("📬 Thông báo mới:", notification);
            this.handleNewNotification(notification);
        });

        this.connection.on("UnreadCountUpdated", (count) => {
            console.log("📊 Thông báo chưa đọc:", count);
            this.updateUnreadCount(count);
        });

        await this.connection.start();
        console.log("✅ Kết nối SignalR thành công");
    }

    markAsRead(notificationId: string) {
        return this.connection.invoke("MarkNotificationAsRead", notificationId);
    }

    getUnreadCount() {
        return this.connection.invoke("GetUnreadCount");
    }

    private handleNewNotification(notification: any) {
        // Cập nhật UI
        // Phát âm thanh
        // Hiển thị toast notification
    }

    private updateUnreadCount(count: number) {
        // Cập nhật badge số thông báo
    }
}

export default new NotificationService();
```

### Component hiển thị thông báo:

```tsx
// NotificationPanel.tsx
import React, { useState, useEffect } from 'react';
import notificationService from '../services/notificationService';
import { fetchAPI } from '../utils/api';

interface Notification {
    id: string;
    title: string;
    content: string;
    doiTuong: string;
    createdAt: string;
    isRead: boolean;
    readAt?: string;
}

const NotificationPanel: React.FC = () => {
    const [notifications, setNotifications] = useState<Notification[]>([]);
    const [unreadCount, setUnreadCount] = useState(0);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        loadNotifications();
        loadUnreadCount();
    }, []);

    const loadNotifications = async () => {
        try {
            const response = await fetchAPI('/api/notification/my-notifications');
            if (response.success) {
                setNotifications(response.data);
            }
        } catch (error) {
            console.error('Lỗi tải thông báo:', error);
        } finally {
            setLoading(false);
        }
    };

    const loadUnreadCount = async () => {
        try {
            const response = await fetchAPI('/api/notification/unread-count');
            if (response.success) {
                setUnreadCount(response.data);
            }
        } catch (error) {
            console.error('Lỗi tải số thông báo:', error);
        }
    };

    const handleMarkAsRead = async (id: string) => {
        try {
            await fetchAPI(`/api/notification/${id}/mark-as-read`, {
                method: 'POST'
            });
            loadNotifications();
            loadUnreadCount();
        } catch (error) {
            console.error('Lỗi đánh dấu đã đọc:', error);
        }
    };

    const handleMarkAllAsRead = async () => {
        try {
            await fetchAPI('/api/notification/mark-all-as-read', {
                method: 'POST'
            });
            loadNotifications();
            loadUnreadCount();
        } catch (error) {
            console.error('Lỗi đánh dấu tất cả:', error);
        }
    };

    return (
        <div className="notification-panel">
            <div className="header">
                <h2>Thông báo</h2>
                <span className="badge">{unreadCount}</span>
                {unreadCount > 0 && (
                    <button 
                        className="btn-mark-all"
                        onClick={handleMarkAllAsRead}
                    >
                        Đánh dấu tất cả là đã đọc
                    </button>
                )}
            </div>

            <div className="notifications">
                {loading ? (
                    <p>Đang tải...</p>
                ) : notifications.length === 0 ? (
                    <p>Không có thông báo</p>
                ) : (
                    notifications.map(notif => (
                        <div 
                            key={notif.id} 
                            className={`notification-item ${notif.isRead ? 'read' : 'unread'}`}
                        >
                            <div className="content">
                                <h3>{notif.title}</h3>
                                <p>{notif.content}</p>
                                <span className="time">
                                    {new Date(notif.createdAt).toLocaleString('vi-VN')}
                                </span>
                            </div>
                            {!notif.isRead && (
                                <button 
                                    className="btn-read"
                                    onClick={() => handleMarkAsRead(notif.id)}
                                >
                                    ✓
                                </button>
                            )}
                        </div>
                    ))
                )}
            </div>
        </div>
    );
};

export default NotificationPanel;
```

### Badge hiển thị số thông báo chưa đọc:

```tsx
// NotificationBell.tsx
import React, { useState, useEffect } from 'react';
import notificationService from '../services/notificationService';
import { fetchAPI } from '../utils/api';

const NotificationBell: React.FC = () => {
    const [unreadCount, setUnreadCount] = useState(0);

    useEffect(() => {
        loadUnreadCount();
        
        // Lắng nghe sự kiện từ SignalR
        notificationService.connection?.on("UnreadCountUpdated", (count: number) => {
            setUnreadCount(count);
        });

        // Kiểm tra mỗi 30 giây
        const interval = setInterval(loadUnreadCount, 30000);

        return () => clearInterval(interval);
    }, []);

    const loadUnreadCount = async () => {
        try {
            const response = await fetchAPI('/api/notification/unread-count');
            if (response.success) {
                setUnreadCount(response.data);
            }
        } catch (error) {
            console.error('Error:', error);
        }
    };

    return (
        <div className="notification-bell">
            <button className="bell-icon">
                🔔
                {unreadCount > 0 && (
                    <span className="badge">{unreadCount > 99 ? '99+' : unreadCount}</span>
                )}
            </button>
        </div>
    );
};

export default NotificationBell;
```

## 5️⃣ Ví dụ API Request/Response

### Tạo thông báo cho giáo viên:

**Request:**
```bash
curl -X POST http://localhost:5000/api/notification \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Cuộc họp khẩn cấp",
    "content": "Có cuộc họp khẩn cấp vào lúc 14:00 hôm nay",
    "doiTuong": "Giao_Vien"
  }'
```

**Response:**
```json
{
  "success": true,
  "message": "Tạo thông báo thành công",
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "title": "Cuộc họp khẩn cấp",
    "content": "Có cuộc họp khẩn cấp vào lúc 14:00 hôm nay",
    "doiTuong": "Giao_Vien",
    "createdAt": "2026-05-09T10:30:00Z"
  }
}
```

### Lấy thông báo của người dùng:

**Request:**
```bash
curl -X GET http://localhost:5000/api/notification/my-notifications \
  -H "Authorization: Bearer USER_TOKEN"
```

**Response:**
```json
{
  "success": true,
  "message": "Lấy thông báo của người dùng thành công",
  "data": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440000",
      "title": "Bài tập mới",
      "content": "Có bài tập Math mới",
      "doiTuong": "Hoc_Sinh",
      "createdAt": "2026-05-09T10:30:00Z",
      "isRead": false,
      "readAt": null
    },
    {
      "id": "550e8400-e29b-41d4-a716-446655440001",
      "title": "Điểm số",
      "content": "Bạn đạt 9.5 điểm",
      "doiTuong": "Hoc_Sinh",
      "createdAt": "2026-05-08T15:00:00Z",
      "isRead": true,
      "readAt": "2026-05-08T15:30:00Z"
    }
  ]
}
```

## 6️⃣ HTML/CSS Style (tùy chọn)

```css
.notification-panel {
    background: white;
    border-radius: 8px;
    box-shadow: 0 2px 8px rgba(0,0,0,0.1);
    max-width: 500px;
    margin: 10px;
}

.notification-panel .header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 20px;
    border-bottom: 1px solid #eee;
}

.notification-panel .header h2 {
    margin: 0;
    font-size: 20px;
}

.notification-panel .badge {
    background: #ff4444;
    color: white;
    border-radius: 50%;
    padding: 2px 6px;
    font-size: 12px;
    font-weight: bold;
}

.notification-item {
    padding: 15px 20px;
    border-bottom: 1px solid #f0f0f0;
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.notification-item.unread {
    background-color: #f0f8ff;
    font-weight: 500;
}

.notification-item .content h3 {
    margin: 0 0 5px 0;
    font-size: 16px;
}

.notification-item .content p {
    margin: 0 0 8px 0;
    color: #666;
    font-size: 14px;
}

.notification-item .time {
    font-size: 12px;
    color: #999;
}

.notification-bell {
    position: relative;
}

.notification-bell .bell-icon {
    background: none;
    border: none;
    font-size: 24px;
    cursor: pointer;
    position: relative;
}

.notification-bell .badge {
    position: absolute;
    top: -5px;
    right: -5px;
    background: #ff4444;
    color: white;
    border-radius: 50%;
    width: 20px;
    height: 20px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 10px;
    font-weight: bold;
}
```

---

**Version**: 1.0.0  
**Ngày cập nhật**: 2026-05-09
