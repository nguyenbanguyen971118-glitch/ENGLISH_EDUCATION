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

        // Hàm helper: Tìm danh sách ID người dùng theo Đối tượng (Target)
        private async Task<List<Guid>> GetUserIdsByTargetAsync(string target)
        {
            var query = _context.Nguoidungs.Where(u => u.DaXoa == null || u.DaXoa == false);

            if (target == "Học sinh")
                query = query.Where(u => u.Nguoidungvaitros.Any(v => v.MaVaiTroNavigation.TenVaiTro == "Hoc_Sinh"));
            else if (target == "Giáo viên")
                query = query.Where(u => u.Nguoidungvaitros.Any(v => v.MaVaiTroNavigation.TenVaiTro == "Giao_Vien"));
            else if (target == "Phụ huynh")
                query = query.Where(u => u.Nguoidungvaitros.Any(v => v.MaVaiTroNavigation.TenVaiTro == "Phu_Huynh"));
            // Nếu là "Tất cả", query sẽ không filter role, lấy toàn bộ user.

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
                // Tạm thời trả về "Tất cả" hoặc nội suy. Ở đây mình giả lập dựa theo tổng số lượng người nhận.
                Target = "Tất cả", // TODO: Thêm cột DoiTuong vào DB để map chuẩn xác nhất
                Date = (t.ThoiGianTao ?? DateTime.UtcNow).ToString("yyyy-MM-dd")
            }).ToList();

            return ApiResponseDto<List<NotificationDto>>.Ok(result, "Lấy danh sách thông báo thành công");
        }

        public async Task<ApiResponseDto<NotificationDto>> CreateAsync(Guid currentUserId, CreateNotificationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.Content))
                return ApiResponseDto<NotificationDto>.Fail("Tiêu đề và nội dung không được để trống", "INVALID_INPUT");

            var receiverIds = await GetUserIdsByTargetAsync(dto.Target);

            var thongBao = new Thongbao
            {
                MaThongBao = Guid.NewGuid(),
                TieuDe = dto.Title,
                NoiDung = dto.Content,
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
                Target = dto.Target,
                Date = thongBao.ThoiGianTao.Value.ToString("yyyy-MM-dd")
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
            thongBao.NguoiSua = currentUserId;
            thongBao.ThoiGianSua = DateTime.UtcNow;

            // Tính toán lại danh sách người nhận mới
            var newReceiverIds = await GetUserIdsByTargetAsync(dto.Target);

            await _notificationRepo.UpdateAsync(thongBao, newReceiverIds, currentUserId);

            var result = new NotificationDto
            {
                Id = thongBao.MaThongBao,
                Title = thongBao.TieuDe,
                Content = thongBao.NoiDung,
                Target = dto.Target,
                Date = (thongBao.ThoiGianTao ?? DateTime.UtcNow).ToString("yyyy-MM-dd")
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
