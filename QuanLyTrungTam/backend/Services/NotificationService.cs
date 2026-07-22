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
        private const long MaxAttachmentFileSize = 20 * 1024 * 1024; // 20MB mỗi file
        private const int MaxAttachmentCount = 10; // tối đa 10 file mỗi thông báo
        private static readonly string[] AllowedAttachmentExtensions =
        [
            ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf"
        ];

        private readonly INotificationRepository _notificationRepo;
        private readonly AppDbContext _context; // Dùng để tìm user theo Role
        private readonly IWebHostEnvironment _environment;

        public NotificationService(INotificationRepository notificationRepo, AppDbContext context, IWebHostEnvironment environment)
        {
            _notificationRepo = notificationRepo;
            _context = context;
            _environment = environment;
        }

        private static List<NotificationAttachmentDto> MapAttachments(IEnumerable<Dinhkemthongbao> attachments)
        {
            return attachments
                .Where(a => a.DaXoa != true && a.MaTaiNguyenNavigation != null)
                .Select(a => new NotificationAttachmentDto
                {
                    Id = a.MaTaiNguyen,
                    FileName = a.MaTaiNguyenNavigation.TenTaiNguyen
                })
                .ToList();
        }

        private string EnsureWebRootPath()
        {
            var webRootPath = _environment.WebRootPath;
            if (!string.IsNullOrWhiteSpace(webRootPath))
            {
                return webRootPath;
            }

            webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
            Directory.CreateDirectory(webRootPath);
            return webRootPath;
        }

        private async Task<(List<Dinhkemthongbao>? Attachments, string? Error)> SaveAttachmentsAsync(
            Guid thongBaoId, List<Microsoft.AspNetCore.Http.IFormFile>? files, Guid userId, DateTime now)
        {
            if (files == null || files.Count == 0)
            {
                return (new List<Dinhkemthongbao>(), null);
            }

            if (files.Count > MaxAttachmentCount)
            {
                return (null, $"Chỉ được đính kèm tối đa {MaxAttachmentCount} file cho mỗi thông báo.");
            }

            foreach (var file in files)
            {
                if (file.Length == 0)
                {
                    return (null, $"File '{file.FileName}' đang rỗng.");
                }

                if (file.Length > MaxAttachmentFileSize)
                {
                    return (null, $"File '{file.FileName}' vượt quá dung lượng cho phép (20MB).");
                }

                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!AllowedAttachmentExtensions.Contains(ext))
                {
                    return (null, $"File '{file.FileName}' không đúng định dạng cho phép (Word, Excel, PowerPoint, PDF).");
                }
            }

            var webRootPath = EnsureWebRootPath();
            var folderPath = Path.Combine(webRootPath, "uploads", "notifications");
            Directory.CreateDirectory(folderPath);

            var links = new List<Dinhkemthongbao>();
            foreach (var file in files)
            {
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                var storedFileName = $"notif_{userId:N}_{Guid.NewGuid():N}{ext}";
                var physicalPath = Path.Combine(folderPath, storedFileName);
                var relativePath = $"/uploads/notifications/{storedFileName}";

                await using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var resource = new Tainguyenluutru
                {
                    MaTaiNguyen = Guid.NewGuid(),
                    MaNguoiDung = userId,
                    TenTaiNguyen = file.FileName,
                    Link = relativePath,
                    NguoiTao = userId,
                    ThoiGianTao = now,
                    TrangThai = true,
                    DaXoa = false
                };
                _context.Tainguyenluutrus.Add(resource);

                links.Add(new Dinhkemthongbao
                {
                    MaThongBao = thongBaoId,
                    MaTaiNguyen = resource.MaTaiNguyen,
                    NguoiTao = userId,
                    ThoiGianTao = now,
                    TrangThai = true,
                    DaXoa = false
                });
            }

            _context.Dinhkemthongbaos.AddRange(links);
            return (links, null);
        }

        public async Task<(string? PhysicalPath, string? FileName, ApiResponseDto<object>? Error)> GetAttachmentForDownloadAsync(Guid userId, bool isAdmin, Guid resourceId)
        {
            var resource = await _context.Tainguyenluutrus
                .Include(r => r.Dinhkemthongbaos.Where(a => a.DaXoa != true))
                .FirstOrDefaultAsync(r => r.MaTaiNguyen == resourceId && r.DaXoa != true);

            var link = resource?.Dinhkemthongbaos.FirstOrDefault();
            if (resource == null || link == null)
            {
                return (null, null, ApiResponseDto<object>.Fail("Không tìm thấy file đính kèm.", "ATTACHMENT_NOT_FOUND"));
            }

            if (!isAdmin)
            {
                var isRecipient = await _context.Nguoinhanthongbaos.AnyAsync(n =>
                    n.MaThongBao == link.MaThongBao && n.MaNguoiDung == userId && n.DaXoa != true);
                if (!isRecipient)
                {
                    return (null, null, ApiResponseDto<object>.Fail("Bạn không có quyền tải file này.", "ATTACHMENT_FORBIDDEN"));
                }
            }

            var webRootPath = EnsureWebRootPath();
            var relativePhysicalPath = resource.Link.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var physicalPath = Path.Combine(webRootPath, relativePhysicalPath);

            if (!File.Exists(physicalPath))
            {
                return (null, null, ApiResponseDto<object>.Fail("Không tìm thấy file trên máy chủ.", "ATTACHMENT_FILE_NOT_FOUND"));
            }

            return (physicalPath, resource.TenTaiNguyen, null);
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
                CreatedAt = t.ThoiGianTao ?? DateTime.UtcNow,
                Attachments = MapAttachments(t.Dinhkemthongbaos)
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
                    ReadAt = n.NgayDoc,
                    Attachments = MapAttachments(n.MaThongBaoNavigation!.Dinhkemthongbaos)
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
                    ReadAt = n.NgayDoc,
                    Attachments = MapAttachments(n.MaThongBaoNavigation!.Dinhkemthongbaos)
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

            var now = DateTime.UtcNow;
            var thongBao = new Thongbao
            {
                MaThongBao = Guid.NewGuid(),
                TieuDe = dto.Title,
                NoiDung = dto.Content,
                DoiTuong = dto.DoiTuong ?? "Tat_Ca",
                NguoiTao = currentUserId,
                ThoiGianTao = now,
                DaXoa = false,
                TrangThai = true
            };

            await _notificationRepo.CreateAsync(thongBao, receiverIds);

            var (attachments, attachmentError) = await SaveAttachmentsAsync(thongBao.MaThongBao, dto.Files, currentUserId, now);
            if (attachmentError != null)
            {
                return ApiResponseDto<NotificationDto>.Fail(attachmentError, "ATTACHMENT_INVALID");
            }
            await _context.SaveChangesAsync();

            var result = new NotificationDto
            {
                Id = thongBao.MaThongBao,
                Title = thongBao.TieuDe,
                Content = thongBao.NoiDung,
                DoiTuong = thongBao.DoiTuong ?? "Tat_Ca",
                CreatedAt = thongBao.ThoiGianTao.Value,
                Attachments = MapAttachments(attachments ?? new List<Dinhkemthongbao>())
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
