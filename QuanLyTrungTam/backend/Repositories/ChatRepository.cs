using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly AppDbContext _context;

    public ChatRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> UserExistsAsync(Guid userId)
    {
        return await _context.Nguoidungs
            .AnyAsync(u => u.MaNguoiDung == userId && u.DaXoa != true && u.TrangThai != false);
    }

    public async Task<List<ChatUserDto>> GetChatUsersAsync(Guid currentUserId)
    {
        var rows = await (
            from u in _context.Nguoidungs
            where u.MaNguoiDung != currentUserId && u.DaXoa != true && u.TrangThai != false
            join ur in _context.Nguoidungvaitros.Where(x => x.DaXoa != true && x.TrangThai != false)
                on u.MaNguoiDung equals ur.MaNguoiDung
            join r in _context.Vaitros.Where(x => x.DaXoa != true && x.TrangThai != false)
                on ur.MaVaiTro equals r.MaVaiTro
            where r.TenVaiTro == "Admin" || r.TenVaiTro == "Giao_Vien" || r.TenVaiTro == "Phu_Huynh"
            select new ChatUserDto
            {
                UserId = u.MaNguoiDung,
                FullName = u.HoTen,
                Role = r.TenVaiTro,
                AvatarUrl = u.AnhDaiDien
            }
        ).ToListAsync();

        return rows
            .GroupBy(x => x.UserId)
            .Select(g => g.First())
            .OrderBy(x => x.FullName)
            .ToList();
    }

    public async Task<ChatUserDto?> GetUserProfileAsync(Guid userId)
    {
        return (await GetUserProfilesAsync(new[] { userId })).FirstOrDefault();
    }

    public async Task<List<ChatUserDto>> GetUserProfilesAsync(IEnumerable<Guid> userIds)
    {
        var ids = userIds.Distinct().ToList();
        if (!ids.Any())
        {
            return new List<ChatUserDto>();
        }

        var rows = await (
            from u in _context.Nguoidungs
            where ids.Contains(u.MaNguoiDung) && u.DaXoa != true && u.TrangThai != false
            join ur in _context.Nguoidungvaitros.Where(x => x.DaXoa != true && x.TrangThai != false)
                on u.MaNguoiDung equals ur.MaNguoiDung into userRoles
            from ur in userRoles.DefaultIfEmpty()
            join r in _context.Vaitros.Where(x => x.DaXoa != true && x.TrangThai != false)
                on ur.MaVaiTro equals r.MaVaiTro into roles
            from r in roles.DefaultIfEmpty()
            select new
            {
                u.MaNguoiDung,
                u.HoTen,
                u.AnhDaiDien,
                RoleName = r != null ? r.TenVaiTro : "User"
            }
        ).ToListAsync();

        return rows
            .GroupBy(x => x.MaNguoiDung)
            .Select(g =>
            {
                var first = g.First();
                return new ChatUserDto
                {
                    UserId = first.MaNguoiDung,
                    FullName = first.HoTen,
                    Role = first.RoleName,
                    AvatarUrl = first.AnhDaiDien
                };
            })
            .ToList();
    }

    public async Task<Guid?> FindDirectConversationIdAsync(Guid userA, Guid userB)
    {
        var aConversationIds = await _context.Thanhvienhoithoais
            .Where(tv => tv.MaNguoiDung == userA && tv.DaXoa != true && tv.TrangThai != false)
            .Select(tv => tv.MaHoiThoai)
            .ToListAsync();

        if (!aConversationIds.Any())
        {
            return null;
        }

        var bConversationIds = await _context.Thanhvienhoithoais
            .Where(tv => tv.MaNguoiDung == userB && tv.DaXoa != true && tv.TrangThai != false)
            .Select(tv => tv.MaHoiThoai)
            .ToListAsync();

        var commonIds = aConversationIds.Intersect(bConversationIds).ToList();
        if (!commonIds.Any())
        {
            return null;
        }

        var matchedId = await _context.Hoithoais
            .Where(h => commonIds.Contains(h.MaHoiThoai) && h.DaXoa != true && h.TrangThai != false)
            .Where(h => string.IsNullOrEmpty(h.TieuDe))
            .Where(h => _context.Thanhvienhoithoais.Count(tv => tv.MaHoiThoai == h.MaHoiThoai && tv.DaXoa != true && tv.TrangThai != false) == 2)
            .Select(h => (Guid?)h.MaHoiThoai)
            .FirstOrDefaultAsync();

        return matchedId;
    }

    public async Task<Guid> CreateConversationAsync(string? title, Guid creatorId, IEnumerable<Guid> memberIds)
    {
        var now = DateTime.UtcNow;
        var conversationId = Guid.NewGuid();

        var conversation = new Hoithoai
        {
            MaHoiThoai = conversationId,
            TieuDe = string.IsNullOrWhiteSpace(title) ? null : title.Trim(),
            NguoiTao = creatorId,
            ThoiGianTao = now,
            NguoiSua = creatorId,
            ThoiGianSua = now,
            TrangThai = true,
            DaXoa = false
        };

        var allMembers = memberIds.Append(creatorId).Distinct().ToList();
        var memberRows = allMembers.Select(memberId => new Thanhvienhoithoai
        {
            MaHoiThoai = conversationId,
            MaNguoiDung = memberId,
            NguoiTao = creatorId,
            ThoiGianTao = now,
            NguoiSua = creatorId,
            ThoiGianSua = now,
            TrangThai = true,
            DaXoa = false
        });

        await _context.Hoithoais.AddAsync(conversation);
        await _context.Thanhvienhoithoais.AddRangeAsync(memberRows);
        await _context.SaveChangesAsync();

        return conversationId;
    }

    public async Task<bool> IsUserInConversationAsync(Guid conversationId, Guid userId)
    {
        return await _context.Thanhvienhoithoais
            .AnyAsync(tv => tv.MaHoiThoai == conversationId && tv.MaNguoiDung == userId && tv.DaXoa != true && tv.TrangThai != false);
    }

    public async Task<ChatConversationDto?> GetConversationByIdAsync(Guid conversationId, Guid currentUserId)
    {
        var data = await BuildConversationDtosAsync(new[] { conversationId }, currentUserId);
        return data.FirstOrDefault();
    }

    public async Task<List<ChatConversationDto>> GetConversationsAsync(Guid currentUserId)
    {
        var conversationIds = await _context.Thanhvienhoithoais
            .Where(tv => tv.MaNguoiDung == currentUserId && tv.DaXoa != true && tv.TrangThai != false)
            .Select(tv => tv.MaHoiThoai)
            .Distinct()
            .ToListAsync();

        return await BuildConversationDtosAsync(conversationIds, currentUserId);
    }

    public async Task<ChatMessageDto> CreateMessageAsync(Guid conversationId, Guid senderId, string content)
    {
        var now = DateTime.UtcNow;
        var message = new Tinnhan
        {
            MaTinNhan = Guid.NewGuid(),
            MaHoiThoai = conversationId,
            MaNguoiDungGui = senderId,
            NoiDung = content.Trim(),
            DaDoc = false,
            NguoiTao = senderId,
            ThoiGianTao = now,
            NguoiSua = senderId,
            ThoiGianSua = now,
            TrangThai = true,
            DaXoa = false
        };

        await _context.Tinnhans.AddAsync(message);
        await _context.SaveChangesAsync();

        var sender = await GetUserProfileAsync(senderId);

        return new ChatMessageDto
        {
            MessageId = message.MaTinNhan,
            ConversationId = message.MaHoiThoai,
            SenderId = senderId,
            SenderName = sender?.FullName ?? "Người dùng",
            SenderRole = sender?.Role ?? "User",
            SenderAvatarUrl = sender?.AvatarUrl,
            Content = message.NoiDung,
            IsRead = message.DaDoc == true,
            CreatedAt = message.ThoiGianTao ?? now
        };
    }

    public async Task<List<ChatMessageDto>> GetMessagesAsync(Guid conversationId, int take)
    {
        var rows = await (
            from m in _context.Tinnhans
            where m.MaHoiThoai == conversationId && m.DaXoa != true && m.TrangThai != false
            join u in _context.Nguoidungs on m.MaNguoiDungGui equals u.MaNguoiDung
            join ur in _context.Nguoidungvaitros.Where(x => x.DaXoa != true && x.TrangThai != false)
                on u.MaNguoiDung equals ur.MaNguoiDung into userRoles
            from ur in userRoles.DefaultIfEmpty()
            join r in _context.Vaitros.Where(x => x.DaXoa != true && x.TrangThai != false)
                on ur.MaVaiTro equals r.MaVaiTro into roles
            from r in roles.DefaultIfEmpty()
            orderby m.ThoiGianTao descending
            select new ChatMessageDto
            {
                MessageId = m.MaTinNhan,
                ConversationId = m.MaHoiThoai,
                SenderId = m.MaNguoiDungGui,
                SenderName = u.HoTen,
                SenderRole = r != null ? r.TenVaiTro : "User",
                SenderAvatarUrl = u.AnhDaiDien,
                Content = m.NoiDung,
                IsRead = m.DaDoc == true,
                CreatedAt = m.ThoiGianTao ?? DateTime.UtcNow
            }
        ).Take(take).ToListAsync();

        rows.Reverse();
        return rows;
    }

    public async Task<int> MarkMessagesAsReadAsync(Guid conversationId, Guid currentUserId)
    {
        var now = DateTime.UtcNow;
        var unreadMessages = await _context.Tinnhans
            .Where(m => m.MaHoiThoai == conversationId
                        && m.MaNguoiDungGui != currentUserId
                        && m.DaXoa != true
                        && m.TrangThai != false
                        && (m.DaDoc == null || m.DaDoc == false))
            .ToListAsync();

        foreach (var message in unreadMessages)
        {
            message.DaDoc = true;
            message.NguoiSua = currentUserId;
            message.ThoiGianSua = now;
        }

        if (unreadMessages.Count > 0)
        {
            await _context.SaveChangesAsync();
        }

        return unreadMessages.Count;
    }

    private async Task<List<ChatConversationDto>> BuildConversationDtosAsync(IEnumerable<Guid> conversationIds, Guid currentUserId)
    {
        var ids = conversationIds.Distinct().ToList();
        if (!ids.Any())
        {
            return new List<ChatConversationDto>();
        }

        var conversations = await _context.Hoithoais
            .Where(h => ids.Contains(h.MaHoiThoai) && h.DaXoa != true && h.TrangThai != false)
            .ToListAsync();

        var memberships = await _context.Thanhvienhoithoais
            .Where(tv => ids.Contains(tv.MaHoiThoai) && tv.DaXoa != true && tv.TrangThai != false)
            .Select(tv => new { tv.MaHoiThoai, tv.MaNguoiDung })
            .ToListAsync();

        var memberIds = memberships.Select(x => x.MaNguoiDung).Distinct().ToList();
        var userProfiles = await GetUserProfilesAsync(memberIds);
        var userProfileMap = userProfiles.ToDictionary(x => x.UserId, x => x);

        var unreadCounts = await _context.Tinnhans
            .Where(m => ids.Contains(m.MaHoiThoai)
                        && m.DaXoa != true
                        && m.TrangThai != false
                        && m.MaNguoiDungGui != currentUserId
                        && (m.DaDoc == null || m.DaDoc == false))
            .GroupBy(m => m.MaHoiThoai)
            .Select(g => new { ConversationId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.ConversationId, x => x.Count);

        var lastMessages = await (
            from m in _context.Tinnhans
            where ids.Contains(m.MaHoiThoai) && m.DaXoa != true && m.TrangThai != false
            join u in _context.Nguoidungs on m.MaNguoiDungGui equals u.MaNguoiDung
            orderby m.ThoiGianTao descending
            select new
            {
                m.MaHoiThoai,
                m.NoiDung,
                m.ThoiGianTao,
                m.MaNguoiDungGui,
                SenderName = u.HoTen
            }
        ).ToListAsync();

        var result = new List<ChatConversationDto>();

        foreach (var conversation in conversations)
        {
            var members = memberships
                .Where(x => x.MaHoiThoai == conversation.MaHoiThoai)
                .Select(x => userProfileMap.GetValueOrDefault(x.MaNguoiDung))
                .Where(x => x != null)
                .Cast<ChatUserDto>()
                .ToList();

            var isGroup = members.Count > 2 || !string.IsNullOrWhiteSpace(conversation.TieuDe);
            var otherMember = members.FirstOrDefault(m => m.UserId != currentUserId);

            var latest = lastMessages.FirstOrDefault(x => x.MaHoiThoai == conversation.MaHoiThoai);
            var latestText = latest?.NoiDung ?? string.Empty;
            if (latest != null && latest.MaNguoiDungGui != currentUserId && isGroup)
            {
                latestText = $"{latest.SenderName}: {latestText}";
            }

            result.Add(new ChatConversationDto
            {
                ConversationId = conversation.MaHoiThoai,
                IsGroup = isGroup,
                Title = conversation.TieuDe,
                DisplayName = isGroup
                    ? (conversation.TieuDe ?? $"Nhóm ({members.Count} thành viên)")
                    : (otherMember?.FullName ?? "Cuộc trò chuyện"),
                AvatarUrl = isGroup ? null : otherMember?.AvatarUrl,
                LastMessage = latestText,
                LastMessageAt = latest?.ThoiGianTao,
                UnreadCount = unreadCounts.GetValueOrDefault(conversation.MaHoiThoai, 0),
                Members = members
            });
        }

        return result
            .OrderByDescending(x => x.LastMessageAt ?? DateTime.MinValue)
            .ToList();
    }
}
