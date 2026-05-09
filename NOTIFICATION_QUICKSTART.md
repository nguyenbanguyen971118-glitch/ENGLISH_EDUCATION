# ⚡ Quick Start - Hệ thống Thông báo

## 5 Phút Setup

### 1. Backend Startup

```bash
cd /Users/banhan/ENGLISH_EDUCATION/QuanLyTrungTam/backend

# Build
dotnet build

# Run
dotnet run

# Server running at: http://localhost:5000
```

### 2. Frontend Connection (React)

```javascript
// Install dependency
npm install @aspnetcore/signalr

// In your main component or service
import { HubConnectionBuilder } from "@aspnetcore/signalr";

const token = localStorage.getItem("token");
const connection = new HubConnectionBuilder()
    .withUrl("http://localhost:5000/hubs/notification", {
        accessTokenFactory: () => token
    })
    .withAutomaticReconnect()
    .build();

connection.start().then(() => {
    console.log("✅ Connected to notification hub");
}).catch(err => console.error(err));

// Listen for notifications
connection.on("NewNotification", (notification) => {
    console.log("📬", notification);
});
```

## 🎯 Common Tasks

### Admin gửi thông báo cho học sinh

**Backend:**
```csharp
[HttpPost("announce")]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> SendToStudents(
    [FromBody] CreateNotificationDto dto,
    [FromServices] NotificationService service)
{
    dto.DoiTuong = "Hoc_Sinh"; // Important!
    var result = await service.CreateAsync(GetCurrentUserId(), dto);
    return Ok(result);
}
```

**Frontend (API Call):**
```javascript
fetch("/api/notification", {
    method: "POST",
    headers: {
        "Authorization": `Bearer ${token}`,
        "Content-Type": "application/json"
    },
    body: JSON.stringify({
        title: "New Assignment",
        content: "Math homework due tomorrow",
        doiTuong: "Hoc_Sinh"
    })
})
.then(r => r.json())
.then(data => console.log(data));
```

### Lấy thông báo chưa đọc của người dùng

**API:**
```bash
GET /api/notification/unread
Authorization: Bearer {token}
```

**JavaScript:**
```javascript
fetch("/api/notification/unread", {
    headers: { "Authorization": `Bearer ${token}` }
})
.then(r => r.json())
.then(data => {
    console.log("Unread notifications:", data.data);
});
```

### Đánh dấu thông báo là đã đọc

**Via REST API:**
```bash
POST /api/notification/{id}/mark-as-read
Authorization: Bearer {token}
```

**Via SignalR:**
```javascript
connection.invoke("MarkNotificationAsRead", notificationId)
    .catch(err => console.error(err));
```

### Đếm thông báo chưa đọc

**API:**
```bash
GET /api/notification/unread-count
Authorization: Bearer {token}
```

**JavaScript:**
```javascript
fetch("/api/notification/unread-count", {
    headers: { "Authorization": `Bearer ${token}` }
})
.then(r => r.json())
.then(data => {
    console.log("Unread count:", data.data); // e.g., 5
});
```

## 🔧 Code Templates

### Template 1: Notification Component

```tsx
import React, { useState, useEffect } from 'react';

const NotificationBell: React.FC = () => {
    const [count, setCount] = useState(0);
    const token = localStorage.getItem('token');

    useEffect(() => {
        // Get unread count
        fetch('/api/notification/unread-count', {
            headers: { 'Authorization': `Bearer ${token}` }
        })
        .then(r => r.json())
        .then(data => setCount(data.data));
    }, []);

    return (
        <div style={{ position: 'relative' }}>
            <button>🔔</button>
            {count > 0 && (
                <span style={{
                    position: 'absolute',
                    top: '-5px',
                    right: '-5px',
                    background: 'red',
                    color: 'white',
                    borderRadius: '50%',
                    width: '20px',
                    height: '20px',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center'
                }}>
                    {count}
                </span>
            )}
        </div>
    );
};

export default NotificationBell;
```

### Template 2: Notification List

```tsx
import React, { useState, useEffect } from 'react';

interface Notification {
    id: string;
    title: string;
    content: string;
    isRead: boolean;
    createdAt: string;
}

const NotificationList: React.FC = () => {
    const [notifications, setNotifications] = useState<Notification[]>([]);
    const token = localStorage.getItem('token');

    useEffect(() => {
        fetch('/api/notification/my-notifications', {
            headers: { 'Authorization': `Bearer ${token}` }
        })
        .then(r => r.json())
        .then(data => setNotifications(data.data));
    }, []);

    return (
        <div>
            {notifications.map(notif => (
                <div key={notif.id} style={{
                    padding: '10px',
                    borderBottom: '1px solid #eee',
                    background: notif.isRead ? '#fff' : '#f0f8ff'
                }}>
                    <strong>{notif.title}</strong>
                    <p>{notif.content}</p>
                    <small>{new Date(notif.createdAt).toLocaleString()}</small>
                </div>
            ))}
        </div>
    );
};

export default NotificationList;
```

### Template 3: Send Notification (Admin)

```tsx
import React, { useState } from 'react';

interface FormData {
    title: string;
    content: string;
    doiTuong: string;
}

const AdminNotificationForm: React.FC = () => {
    const [form, setForm] = useState<FormData>({
        title: '',
        content: '',
        doiTuong: 'Hoc_Sinh'
    });
    const token = localStorage.getItem('token');

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        
        const response = await fetch('/api/notification', {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(form)
        });
        
        const data = await response.json();
        if (data.success) {
            alert('✅ Notification sent!');
            setForm({ title: '', content: '', doiTuong: 'Hoc_Sinh' });
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <input
                type="text"
                placeholder="Title"
                value={form.title}
                onChange={(e) => setForm({...form, title: e.target.value})}
                required
            />
            <textarea
                placeholder="Content"
                value={form.content}
                onChange={(e) => setForm({...form, content: e.target.value})}
                required
            />
            <select
                value={form.doiTuong}
                onChange={(e) => setForm({...form, doiTuong: e.target.value})}
            >
                <option value="Hoc_Sinh">Students</option>
                <option value="Giao_Vien">Teachers</option>
                <option value="Phu_Huynh">Parents</option>
                <option value="Admin">Admins</option>
                <option value="Tat_Ca">Everyone</option>
            </select>
            <button type="submit">Send</button>
        </form>
    );
};

export default AdminNotificationForm;
```

## 📊 API Cheat Sheet

| Method | Endpoint | Purpose | Auth |
|--------|----------|---------|------|
| GET | `/api/notification` | Get all | Admin |
| GET | `/api/notification/my-notifications` | Get mine | User |
| GET | `/api/notification/unread` | Get unread | User |
| GET | `/api/notification/unread-count` | Count unread | User |
| POST | `/api/notification/{id}/mark-as-read` | Mark read | User |
| POST | `/api/notification/mark-all-as-read` | Mark all read | User |
| POST | `/api/notification` | Create | Admin |
| PUT | `/api/notification/{id}` | Update | Admin |
| DELETE | `/api/notification/{id}` | Delete | Admin |

## 🎓 Learning Path

1. **Understand the concept** (5 min)
   - Read `NOTIFICATION_COMPLETION.md`

2. **See examples** (10 min)
   - Check `NOTIFICATION_EXAMPLES.md`

3. **Technical deep dive** (30 min)
   - Study `NOTIFICATION_TECHNICAL_DOCS.md`

4. **Full documentation** (as needed)
   - Refer to `NOTIFICATION_SYSTEM_GUIDE.md`

## ✅ Testing Checklist

- [ ] Backend builds successfully
- [ ] Admin can create notification
- [ ] Notification appears in user's list
- [ ] Unread count is correct
- [ ] Mark as read works
- [ ] SignalR delivers real-time notification
- [ ] Different roles see correct notifications
- [ ] Soft delete works (DaXoa = true)

## 🆘 Troubleshooting

**Q: "Access token not found" error**
A: Make sure `accessTokenFactory` is set correctly in HubConnectionBuilder

**Q: Notifications not appearing**
A: Check `DoiTuong` matches user's role (Admin, Giao_Vien, Hoc_Sinh, Phu_Huynh, Tat_Ca)

**Q: SignalR won't connect**
A: Verify server is running on localhost:5000 and token is valid

**Q: Build errors**
A: Run `dotnet clean` then `dotnet build`

## 🚀 Next Steps

1. Integrate into your frontend
2. Add email notifications
3. Add push notifications (FCM)
4. Add notification preferences/settings
5. Add notification archive

---

**⏱️ Total Setup Time**: ~5-10 minutes  
**Ready to use**: ✅ YES  
**Version**: 1.0.0
