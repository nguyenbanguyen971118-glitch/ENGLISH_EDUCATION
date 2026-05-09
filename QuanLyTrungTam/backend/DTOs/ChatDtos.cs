namespace backend.DTOs;

public class ChatUserDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
}

public class ChatMessageDto
{
    public Guid MessageId { get; set; }
    public Guid ConversationId { get; set; }
    public Guid SenderId { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public string SenderRole { get; set; } = string.Empty;
    public string? SenderAvatarUrl { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ChatConversationDto
{
    public Guid ConversationId { get; set; }
    public bool IsGroup { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? AvatarUrl { get; set; }
    public string LastMessage { get; set; } = string.Empty;
    public DateTime? LastMessageAt { get; set; }
    public int UnreadCount { get; set; }
    public List<ChatUserDto> Members { get; set; } = new();
}

public class SendMessageRequestDto
{
    public string Content { get; set; } = string.Empty;
    public List<string>? AttachmentUrls { get; set; }
}

public class CreateDirectConversationRequestDto
{
    public Guid RecipientUserId { get; set; }
    public string? InitialMessage { get; set; }
}

public class CreateGroupConversationRequestDto
{
    public string Title { get; set; } = string.Empty;
    public List<Guid> MemberUserIds { get; set; } = new();
    public string? InitialMessage { get; set; }
}

public class RegisterDeviceTokenRequestDto
{
    public string DeviceToken { get; set; } = string.Empty;
}
