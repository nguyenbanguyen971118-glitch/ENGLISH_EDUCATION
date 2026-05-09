namespace backend.DTOs
{
    using System.Text.Json.Serialization;
    using backend.Converters;

    public class NotificationDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string DoiTuong { get; set; } = "Tat_Ca"; // Admin, Giao_Vien, Hoc_Sinh, Phu_Huynh, Tat_Ca
        [JsonConverter(typeof(UtcDateTimeJsonConverter))]
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        [JsonConverter(typeof(UtcDateTimeJsonConverter))]
        public DateTime? ReadAt { get; set; }
    }

    public class CreateNotificationDto
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string DoiTuong { get; set; } = "Tat_Ca"; // Admin, Giao_Vien, Hoc_Sinh, Phu_Huynh, Tat_Ca
    }

    public class UpdateNotificationDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string DoiTuong { get; set; } = "Tat_Ca";
    }

    public class UserNotificationDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string DoiTuong { get; set; } = "Tat_Ca";
        [JsonConverter(typeof(UtcDateTimeJsonConverter))]
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        [JsonConverter(typeof(UtcDateTimeJsonConverter))]
        public DateTime? ReadAt { get; set; }
    }

}
