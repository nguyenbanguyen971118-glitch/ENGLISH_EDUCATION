namespace backend.DTOs
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Target { get; set; } = "Tất cả";
        public string Date { get; set; } = null!; // yyyy-MM-dd
    }

    public class CreateNotificationDto
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Target { get; set; } = "Tất cả";
    }

    public class UpdateNotificationDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Target { get; set; } = "Tất cả";
    }

}
