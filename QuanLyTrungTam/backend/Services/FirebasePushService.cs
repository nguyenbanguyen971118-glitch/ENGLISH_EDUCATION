using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using backend.DTOs;
using backend.Services.Interfaces;

namespace backend.Services;

public class FirebasePushService : IFirebasePushService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<FirebasePushService> _logger;

    private readonly ConcurrentDictionary<Guid, HashSet<string>> _deviceTokens = new();

    public FirebasePushService(
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory,
        ILogger<FirebasePushService> logger)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public Task RegisterDeviceTokenAsync(Guid userId, string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return Task.CompletedTask;
        }

        var set = _deviceTokens.GetOrAdd(userId, _ => new HashSet<string>(StringComparer.Ordinal));
        lock (set)
        {
            set.Add(token.Trim());
        }

        return Task.CompletedTask;
    }

    public async Task SendNewMessagePushAsync(ChatMessageDto message, IEnumerable<Guid> receiverUserIds, CancellationToken cancellationToken = default)
    {
        var enabled = _configuration.GetValue<bool>("Firebase:Enabled");
        var serverKey = _configuration["Firebase:ServerKey"];

        if (!enabled || string.IsNullOrWhiteSpace(serverKey))
        {
            return;
        }

        var targetTokens = new HashSet<string>(StringComparer.Ordinal);
        foreach (var userId in receiverUserIds.Distinct())
        {
            if (_deviceTokens.TryGetValue(userId, out var set))
            {
                lock (set)
                {
                    foreach (var token in set)
                    {
                        targetTokens.Add(token);
                    }
                }
            }
        }

        if (targetTokens.Count == 0)
        {
            return;
        }

        var payload = new
        {
            registration_ids = targetTokens,
            notification = new
            {
                title = $"Tin nhắn mới từ {message.SenderName}",
                body = message.Content,
                sound = "default"
            },
            data = new
            {
                type = "chat-message",
                conversationId = message.ConversationId,
                messageId = message.MessageId,
                senderId = message.SenderId
            }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send")
        {
            Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("key", $"={serverKey}");

        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogWarning("Firebase push gửi thất bại. Status: {Status}. Body: {Body}", response.StatusCode, body);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Có lỗi khi gửi push notification qua Firebase.");
        }
    }
}
