# Technical Documentation - Notification System

## Architecture Overview

```
┌─────────────────────────────────────────────────────────────┐
│                    Frontend (React/Vue)                     │
│  NotificationPanel | NotificationBell | Toast Notifications│
└────────────────┬──────────────────────────────────────────┘
                 │
        ┌────────┴────────┐
        │                 │
   REST API         SignalR WebSocket
   (HTTP)           (Real-time)
        │                 │
┌───────▼─────────────────▼──────────────────────────────────┐
│                    Backend (C# .NET)                        │
│  ┌──────────────────────────────────────────────────────┐  │
│  │  Controllers (NotificationController)                │  │
│  │  - GET /api/notification                             │  │
│  │  - GET /api/notification/my-notifications            │  │
│  │  - GET /api/notification/unread                      │  │
│  │  - GET /api/notification/unread-count                │  │
│  │  - POST /api/notification/{id}/mark-as-read          │  │
│  │  - POST /api/notification/mark-all-as-read           │  │
│  │  - POST /api/notification (Admin)                    │  │
│  │  - PUT /api/notification/{id} (Admin)                │  │
│  │  - DELETE /api/notification/{id} (Admin)             │  │
│  └──────────────────────────────────────────────────────┘  │
│  ┌──────────────────────────────────────────────────────┐  │
│  │  Hubs (NotificationHub - SignalR)                    │  │
│  │  - OnConnectedAsync → Add to role groups             │  │
│  │  - MarkNotificationAsRead()                          │  │
│  │  - GetUnreadCount()                                  │  │
│  │  - Sends: NewNotification, UnreadCountUpdated        │  │
│  └──────────────────────────────────────────────────────┘  │
│  ┌──────────────────────────────────────────────────────┐  │
│  │  Services                                            │  │
│  │  ├─ NotificationService (INotificationService)      │  │
│  │  │  ├─ GetUserNotificationsAsync()                   │  │
│  │  │  ├─ GetUnreadNotificationsAsync()                 │  │
│  │  │  ├─ GetUnreadCountAsync()                         │  │
│  │  │  ├─ MarkAsReadAsync()                             │  │
│  │  │  ├─ MarkAllAsReadAsync()                          │  │
│  │  │  ├─ CreateAsync()                                 │  │
│  │  │  ├─ UpdateAsync()                                 │  │
│  │  │  └─ DeleteAsync()                                 │  │
│  │  └─ NotificationEventService                        │  │
│  │     ├─ NotifyNewAssignmentAsync()                    │  │
│  │     ├─ NotifyTeachersAsync()                         │  │
│  │     ├─ NotifyParentsAsync()                          │  │
│  │     ├─ NotifyAllAsync()                              │  │
│  │     ├─ NotifyUserAsync()                             │  │
│  │     ├─ NotifyScheduleChangeAsync()                   │  │
│  │     ├─ NotifyAssignmentSubmittedAsync()              │  │
│  │     └─ NotifyGradeAsync()                            │  │
│  └──────────────────────────────────────────────────────┘  │
│  ┌──────────────────────────────────────────────────────┐  │
│  │  Repositories (INotificationRepository)              │  │
│  │  ├─ GetAllActiveAsync()                              │  │
│  │  ├─ GetByIdAsync()                                   │  │
│  │  ├─ GetNotificationsByUserIdAsync()                  │  │
│  │  ├─ GetUnreadNotificationsByUserIdAsync()            │  │
│  │  ├─ GetUnreadCountAsync()                            │  │
│  │  ├─ MarkAsReadAsync()                                │  │
│  │  ├─ MarkAllAsReadAsync()                             │  │
│  │  ├─ CreateAsync()                                    │  │
│  │  ├─ UpdateAsync()                                    │  │
│  │  └─ DeleteSoftAsync()                                │  │
│  └──────────────────────────────────────────────────────┘  │
└──────────────────┬───────────────────────────────────────┘
                   │
┌──────────────────▼───────────────────────────────────────────┐
│                    Database (MySQL)                          │
│  ┌─────────────────────────────────────┐                    │
│  │  Thongbao (Notifications)           │                    │
│  │  - MaThongBao (PK)                  │                    │
│  │  - TieuDe (Title)                   │                    │
│  │  - NoiDung (Content)                │                    │
│  │  - DoiTuong (Target: Admin, etc)    │                    │
│  │  - NguoiTao (Creator)               │                    │
│  │  - ThoiGianTao (CreatedAt)          │                    │
│  │  - NguoiSua (LastModifier)          │                    │
│  │  - ThoiGianSua (LastModifiedAt)     │                    │
│  │  - TrangThai (Status)               │                    │
│  │  - DaXoa (Soft Delete)              │                    │
│  └─────────────────────────────────────┘                    │
│  ┌─────────────────────────────────────┐                    │
│  │  Nguoinhanthongbao (Recipients)     │                    │
│  │  - MaThongBao (FK)                  │                    │
│  │  - MaNguoiDung (FK)                 │                    │
│  │  - DaDoc (IsRead)                   │                    │
│  │  - NgayDoc (ReadAt)                 │                    │
│  │  - NguoiTao (Creator)               │                    │
│  │  - ThoiGianTao (CreatedAt)          │                    │
│  │  - NguoiSua (LastModifier)          │                    │
│  │  - ThoiGianSua (LastModifiedAt)     │                    │
│  │  - TrangThai (Status)               │                    │
│  │  - DaXoa (Soft Delete)              │                    │
│  └─────────────────────────────────────┘                    │
└─────────────────────────────────────────────────────────────┘
```

## Data Flow

### 1. Admin Creates Notification for Students

```
Admin (Frontend)
    │
    ├─> POST /api/notification
    │   {
    │     title: "New Assignment",
    │     content: "...",
    │     doiTuong: "Hoc_Sinh"
    │   }
    │
    ▼
NotificationController.Create()
    │
    ├─> GetCurrentUserId() → Admin ID
    │
    ├─> NotificationService.CreateAsync()
    │
    ├─> GetUserIdsByTargetAsync("Hoc_Sinh")
    │   Query: Find all users with role "Hoc_Sinh"
    │   Returns: [StudentId1, StudentId2, ...]
    │
    ├─> NotificationRepository.CreateAsync()
    │   - Create Thongbao record
    │   - Create Nguoinhanthongbao records for each student
    │
    ├─> Return response
    │
    ▼
Student (Frontend)
    │
    ├─> SignalR receives "NewNotification"
    │
    ├─> Show toast/notification
    │
    ├─> Update unread count badge
    │
    ▼
Student later clicks notification
    │
    ├─> POST /api/notification/{id}/mark-as-read
    │
    ├─> NotificationService.MarkAsReadAsync()
    │
    ├─> NotificationRepository.MarkAsReadAsync()
    │   - Update DaDoc = true
    │   - Update NgayDoc = DateTime.UtcNow
    │
    ▼
Database updated
```

### 2. Event-based Notification (e.g., Grade Posted)

```
Teacher (Frontend)
    │
    ├─> POST /api/grades
    │   {
    │     studentId: "...",
    │     score: 9.5
    │   }
    │
    ▼
GradeController.Create()
    │
    ├─> GradeService.CreateAsync()
    │   - Save grade to DB
    │
    ├─> NotificationEventService.NotifyGradeAsync()
    │   {
    │     creatorId: TeacherId,
    │     studentId: StudentId,
    │     assignmentTitle: "...",
    │     grade: "9.5"
    │   }
    │
    ├─> NotificationService.CreateAsync()
    │   - Create notification
    │   - Set DoiTuong = "Hoc_Sinh"
    │   - Find recipient by studentId
    │
    ├─> NotificationHub sends message
    │   await _hubContext.Clients
    │       .Group(GetUserGroup(studentId))
    │       .SendAsync("NewNotification", notification)
    │
    ▼
Student receives real-time notification
    │
    ├─> SignalR "NewNotification" event
    │
    ├─> Show notification immediately
    │
    ▼
Student marks as read
    │
    ├─> SignalR invokes MarkNotificationAsRead()
    │
    ├─> Update database
```

## Role-based Groups in SignalR

When a user connects to NotificationHub:

```csharp
public override async Task OnConnectedAsync()
{
    var userId = Context.User?.GetUserId();
    
    if (userId.HasValue)
    {
        // Add to personal group
        await Groups.AddToGroupAsync(Context.ConnectionId, GetUserGroup(userId.Value));
        // Result: "user:{userId}"
        
        // Add to role groups
        var userRoles = Context.User?.FindAll("role");
        foreach (var role in userRoles)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GetRoleGroup(role.Value));
            // Result: "role:Admin", "role:Giao_Vien", etc.
        }
    }
    
    await base.OnConnectedAsync();
}
```

### Group Names:
- `user:{userId}` - Individual user
- `role:Admin` - All admins
- `role:Giao_Vien` - All teachers
- `role:Hoc_Sinh` - All students
- `role:Phu_Huynh` - All parents

### Sending Notifications:
```csharp
// Send to specific user
await _hubContext.Clients.Group($"user:{userId}").SendAsync(...);

// Send to all teachers
await _hubContext.Clients.Group("role:Giao_Vien").SendAsync(...);

// Send to all users
await _hubContext.Clients.All.SendAsync(...);
```

## Database Schema

### Thongbao Table
```sql
CREATE TABLE Thongbao (
    MaThongBao CHAR(36) PRIMARY KEY,
    TieuDe NVARCHAR(255) NOT NULL,
    NoiDung LONGTEXT NOT NULL,
    DoiTuong VARCHAR(50) NOT NULL DEFAULT 'Tat_Ca',
    NguoiTao CHAR(36),
    ThoiGianTao DATETIME,
    NguoiSua CHAR(36),
    ThoiGianSua DATETIME,
    TrangThai BIT,
    DaXoa BIT DEFAULT FALSE,
    INDEX idx_doixuong (DoiTuong),
    INDEX idx_deleted (DaXoa),
    FOREIGN KEY (NguoiTao) REFERENCES NguoiDung(MaNguoiDung)
);
```

### Nguoinhanthongbao Table
```sql
CREATE TABLE Nguoinhanthongbao (
    MaThongBao CHAR(36),
    MaNguoiDung CHAR(36),
    DaDoc BIT DEFAULT FALSE,
    NgayDoc DATETIME,
    NguoiTao CHAR(36),
    ThoiGianTao DATETIME,
    NguoiSua CHAR(36),
    ThoiGianSua DATETIME,
    TrangThai BIT,
    DaXoa BIT DEFAULT FALSE,
    PRIMARY KEY (MaThongBao, MaNguoiDung),
    INDEX idx_user (MaNguoiDung),
    INDEX idx_unread (DaDoc, DaXoa),
    FOREIGN KEY (MaThongBao) REFERENCES Thongbao(MaThongBao),
    FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung)
);
```

## API Response Examples

### Success Response
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
    }
  ]
}
```

### Error Response
```json
{
  "success": false,
  "message": "Không tìm thấy thông báo",
  "errorCode": "NOT_FOUND"
}
```

## Authentication & Authorization

### JWT Token Claims
```json
{
  "sub": "user-id-here",
  "name": "John Doe",
  "role": ["Admin", "Giao_Vien"],
  "iat": 1620000000,
  "exp": 1620086400
}
```

### SignalR JWT
```javascript
const connection = new HubConnectionBuilder()
    .withUrl(`/hubs/notification?access_token=${token}`)
    .build();
```

### Authorization Rules
- **GET /api/notification** → Admin only
- **GET /api/notification/my-notifications** → Any authenticated user
- **GET /api/notification/unread** → Any authenticated user
- **GET /api/notification/unread-count** → Any authenticated user
- **POST /api/notification/{id}/mark-as-read** → Owner only
- **POST /api/notification/mark-all-as-read** → Any authenticated user
- **POST /api/notification** → Admin only
- **PUT /api/notification/{id}** → Admin only
- **DELETE /api/notification/{id}** → Admin only

## Performance Considerations

### Database Indexes
- `Thongbao.DoiTuong` - For filtering by target
- `Thongbao.DaXoa` - For soft delete filtering
- `Nguoinhanthongbao.MaNguoiDung` - For user lookups
- `Nguoinhanthongbao.DaDoc, DaXoa` - For unread queries

### Query Optimization
```csharp
// Good - Uses indexes
var unread = await _context.Nguoinhanthongbaos
    .Where(n => n.MaNguoiDung == userId 
        && (n.DaDoc == null || n.DaDoc == false)
        && (n.DaXoa == null || n.DaXoa == false))
    .CountAsync();

// Bad - No index, full table scan
var unread = await _context.Nguoinhanthongbaos
    .AsEnumerable() // Brings all data to memory
    .Where(n => n.MaNguoiDung == userId && !n.DaDoc)
    .Count();
```

### Pagination (Future Enhancement)
```csharp
public async Task<PagedResult<UserNotificationDto>> GetUserNotificationsAsync(
    Guid userId, 
    int pageNumber = 1, 
    int pageSize = 20)
{
    var query = _context.Nguoinhanthongbaos
        .Where(n => n.MaNguoiDung == userId && !n.DaXoa)
        .OrderByDescending(n => n.MaThongBaoNavigation.ThoiGianTao);

    var totalCount = await query.CountAsync();
    
    var notifications = await query
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return new PagedResult<UserNotificationDto>
    {
        Items = notifications.Select(MapToDto).ToList(),
        TotalCount = totalCount,
        PageNumber = pageNumber,
        PageSize = pageSize
    };
}
```

## Error Handling

### Try-Catch Example
```csharp
try
{
    var notifications = await _notificationService
        .GetUserNotificationsAsync(userId);
    return Ok(notifications);
}
catch (InvalidOperationException ex)
{
    return BadRequest(ApiResponseDto<object>.Fail(
        "Invalid operation", 
        "INVALID_OPERATION"));
}
catch (Exception ex)
{
    _logger.LogError(ex, "Unexpected error");
    return StatusCode(500, ApiResponseDto<object>.Fail(
        "An unexpected error occurred", 
        "INTERNAL_SERVER_ERROR"));
}
```

## Testing

### Unit Test Example
```csharp
[TestClass]
public class NotificationServiceTests
{
    private NotificationService _service;
    private Mock<INotificationRepository> _mockRepo;

    [TestInitialize]
    public void Setup()
    {
        _mockRepo = new Mock<INotificationRepository>();
        _service = new NotificationService(_mockRepo.Object, mockContext);
    }

    [TestMethod]
    public async Task CreateAsync_ValidDto_CreatesNotification()
    {
        // Arrange
        var dto = new CreateNotificationDto 
        { 
            Title = "Test", 
            Content = "Test content",
            DoiTuong = "Hoc_Sinh"
        };
        var userId = Guid.NewGuid();

        // Act
        var result = await _service.CreateAsync(userId, dto);

        // Assert
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.Data);
    }
}
```

---

**Version**: 1.0.0  
**Last Updated**: 2026-05-09
