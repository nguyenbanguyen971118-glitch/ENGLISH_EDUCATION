using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

[Table("refresh_sessions")]
public class RefreshSession
{
    [Key]
    [MaxLength(32)]
    public string SessionId { get; set; } = string.Empty;

    public Guid UserId { get; set; }

    [MaxLength(512)]
    public string RefreshToken { get; set; } = string.Empty;

    public DateTime ExpiresAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
}