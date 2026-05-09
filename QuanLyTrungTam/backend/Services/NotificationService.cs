using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace backend.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepo;
        private readonly AppDbContext _context; // Dùng để tìm user theo Role

        public NotificationService(INotificationRepository notificationRepo, AppDbContext context)
        {
            _notificationRepo = notificationRepo;
            _context = context;
        }

        // Hàm helper: Tìm danh sách ID người dùng theo Đối tượng (DoiTuong)
        private async Task<List<Guid>> GetUserIdsByTargetAsync(string doiTuong)
        {
            var query = _context.Nguoidungs.Where(u => u.DaXoa == null || u.DaXoa == false);

            // Map sang tên vai trò tiếng Anh trong database
            if (doiTuong == "Hoc_Sinh")
                query = query.Where(u => u.Nguoidungvaitros.Any(v => v.MaVaiTroNavigation.TenVaiTro == "Hoc_Sinh"));
            else if (doiTuong == "Giao_Vien")
                query = query.Where(u => u.Nguoidungvaitros.Any(v => v.MaVaiTroNavigation.TenVaiTro == "Giao_Vien"));
            else if (doiTuong == "Phu_Huynh")
                query = query.Where(u => u.Nguoidungvaitros.Any(v => v.MaVaiTroNavigation.TenVaiTro == "Phu_Huynh"));
            else if (doiTuong == "Admin")
                query = query.Where(u => u.Nguoidungvaitros.Any(v => v.MaVaiTroNavigation.TenVaiTro == "Admin"));
            // Nếu là "Tat_Ca", query sẽ không filter role, lấy toàn bộ user.

            return await query.Select(u => u.MaNguoiDung).ToListAsync();
        }

        public async Task<ApiResponseDto<List<NotificationDto>>> GetAllAsync()
        {
            var thongBaos = await _notificationRepo.GetAllActiveAsync();

            var result = thongBaos.Select(t => new NotificationDto
            {
                Id = t.MaThongBao,
                Title = t.TieuDe,
                Content = t.NoiDung,
                DoiTuong = t.DoiTuong ?? "Tat_Ca",
                CreatedAt = t.ThoiGianTao ?? DateTime.UtcNow
            }).ToList();

            return ApiResponseDto<List<NotificationDto>>.Ok(result, "Lấy danh sách thông báo thành công");
        }

        public async Task<ApiResponseDto<List<UserNotificationDto>>> GetUserNotificationsAsync(Guid userId)
        {
            var notifications = await _notificationRepo.GetNotificationsByUserIdAsync(userId);

            var result = notifications
                .Where(n => n.MaThongBaoNavigation != null)
                .Select(n => new UserNotificationDto
                {
                    Id = n.MaThongBao,
                    Title = n.MaThongBaoNavigation!.TieuDe,
                    Content = n.MaThongBaoNavigation!.NoiDung,
                    DoiTuong = n.MaThongBaoNavigation!.DoiTuong ?? "Tat_Ca",
                    CreatedAt = n.MaThongBaoNavigation!.ThoiGianTao ?? DateTime.UtcNow,
                    IsRead = n.DaDoc ?? false,
                    ReadAt = n.NgayDoc
                }).ToList();

            return ApiResponseDto<List<UserNotificationDto>>.Ok(result, "Lấy thông báo của người dùng thành công");
        }

        public async Task<ApiResponseDto<List<UserNotificationDto>>> GetUnreadNotificationsAsync(Guid userId)
        {
            var notifications = await _notificationRepo.GetUnreadNotificationsByUserIdAsync(userId);

            var result = notifications
                .Where(n => n.MaThongBaoNavigation != null)
                .Select(n => new UserNotificationDto
                {
                    Id = n.MaThongBao,
                    Title = n.MaThongBaoNavigation!.TieuDe,
                    Content = n.MaThongBaoNavigation!.NoiDung,
                    DoiTuong = n.MaThongBaoNavigation!.DoiTuong ?? "Tat_Ca",
                    CreatedAt = n.MaThongBaoNavigation!.ThoiGianTao ?? DateTime.UtcNow,
                    IsRead = n.DaDoc ?? false,
                    ReadAt = n.NgayDoc
                }).ToList();

            return ApiResponseDto<List<UserNotificationDto>>.Ok(result, "Lấy thông báo chưa đọc thành công");
        }

        public async Task<ApiResponseDto<int>> GetUnreadCountAsync(Guid userId)
        {
            var count = await _notificationRepo.GetUnreadCountAsync(userId);
            return ApiResponseDto<int>.Ok(count, "Lấy số lượng thông báo chưa đọc thành công");
        }

        public async Task<ApiResponseDto<object>> MarkAsReadAsync(Guid userId, Guid notificationId)
        {
            await _notificationRepo.MarkAsReadAsync(userId, notificationId);
            return ApiResponseDto<object>.Ok(null, "Đánh dấu thông báo là đã đọc thành công");
        }

        public async Task<ApiResponseDto<object>> MarkAllAsReadAsync(Guid userId)
        {
            await _notificationRepo.MarkAllAsReadAsync(userId);
            return ApiResponseDto<object>.Ok(null, "Đánh dấu tất cả thông báo là đã đọc thành công");
        }

        public async Task<ApiResponseDto<NotificationDto>> CreateAsync(Guid currentUserId, CreateNotificationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.Content))
                return ApiResponseDto<NotificationDto>.Fail("Tiêu đề và nội dung không được để trống", "INVALID_INPUT");

            var receiverIds = await GetUserIdsByTargetAsync(dto.DoiTuong);

            var thongBao = new Thongbao
            {
                MaThongBao = Guid.NewGuid(),
                TieuDe = dto.Title,
                NoiDung = dto.Content,
                DoiTuong = dto.DoiTuong ?? "Tat_Ca",
                NguoiTao = currentUserId,
                ThoiGianTao = DateTime.UtcNow,
                DaXoa = false,
                TrangThai = true
            };

            await _notificationRepo.CreateAsync(thongBao, receiverIds);

            var result = new NotificationDto
            {
                Id = thongBao.MaThongBao,
                Title = thongBao.TieuDe,
                Content = thongBao.NoiDung,
                DoiTuong = thongBao.DoiTuong ?? "Tat_Ca",
                CreatedAt = thongBao.ThoiGianTao.Value
            };

            return ApiResponseDto<NotificationDto>.Ok(result, "Tạo thông báo thành công");
        }

        public async Task<ApiResponseDto<NotificationDto>> UpdateAsync(Guid currentUserId, UpdateNotificationDto dto)
        {
            var thongBao = await _notificationRepo.GetByIdAsync(dto.Id);
            if (thongBao == null)
                return ApiResponseDto<NotificationDto>.Fail("Không tìm thấy thông báo", "NOT_FOUND");

            thongBao.TieuDe = dto.Title;
            thongBao.NoiDung = dto.Content;
            thongBao.DoiTuong = dto.DoiTuong ?? "Tat_Ca";
            thongBao.NguoiSua = currentUserId;
            thongBao.ThoiGianSua = DateTime.UtcNow;

            // Tính toán lại danh sách người nhận mới
            var newReceiverIds = await GetUserIdsByTargetAsync(dto.DoiTuong);

            await _notificationRepo.UpdateAsync(thongBao, newReceiverIds, currentUserId);

            var result = new NotificationDto
            {
                Id = thongBao.MaThongBao,
                Title = thongBao.TieuDe,
                Content = thongBao.NoiDung,
                DoiTuong = thongBao.DoiTuong ?? "Tat_Ca",
                CreatedAt = thongBao.ThoiGianTao ?? DateTime.UtcNow
            };

            return ApiResponseDto<NotificationDto>.Ok(result, "Cập nhật thành công");
        }

        public async Task<ApiResponseDto<object>> DeleteAsync(Guid id, Guid currentUserId)
        {
            var thongBao = await _notificationRepo.GetByIdAsync(id);
            if (thongBao == null)
                return ApiResponseDto<object>.Fail("Không tìm thấy thông báo", "NOT_FOUND");

            await _notificationRepo.DeleteSoftAsync(id, currentUserId);

            return ApiResponseDto<object>.Ok(null, "Xóa thông báo thành công");
        }
    }
}
