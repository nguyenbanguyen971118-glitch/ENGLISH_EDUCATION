using backend.Data;
using backend.Models;
using backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace backend.Repositories
{
    public class NotificationRepository : INotificationRepository
    {

        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

       public async Task<List<Thongbao>> GetAllActiveAsync()
        {
            // Chỉ lấy những thông báo chưa bị xóa mềm
            return await _context.Thongbaos
                .Include(t => t.Nguoinhanthongbaos)
                .Include(t => t.Dinhkemthongbaos.Where(a => a.DaXoa != true))
                .ThenInclude(a => a.MaTaiNguyenNavigation)
                .Where(t => t.DaXoa == null || t.DaXoa == false)
                .OrderByDescending(t => t.ThoiGianTao)
                .ToListAsync();
        }

        public async Task<Thongbao?> GetByIdAsync(Guid id)
        {
            return await _context.Thongbaos
                .Include(t => t.Nguoinhanthongbaos)
                .FirstOrDefaultAsync(t => t.MaThongBao == id && (t.DaXoa == null || t.DaXoa == false));
        }

        public async Task<List<Nguoinhanthongbao>> GetNotificationsByUserIdAsync(Guid userId)
        {
            return await _context.Nguoinhanthongbaos
                .Include(n => n.MaThongBaoNavigation)
                .ThenInclude(t => t.Dinhkemthongbaos.Where(a => a.DaXoa != true))
                .ThenInclude(a => a.MaTaiNguyenNavigation)
                .Where(n => n.MaNguoiDung == userId
                    && (n.DaXoa == null || n.DaXoa == false)
                    && (n.MaThongBaoNavigation.DaXoa == null || n.MaThongBaoNavigation.DaXoa == false))
                .OrderByDescending(n => n.MaThongBaoNavigation.ThoiGianTao)
                .ToListAsync();
        }

        public async Task<List<Nguoinhanthongbao>> GetUnreadNotificationsByUserIdAsync(Guid userId)
        {
            return await _context.Nguoinhanthongbaos
                .Include(n => n.MaThongBaoNavigation)
                .ThenInclude(t => t.Dinhkemthongbaos.Where(a => a.DaXoa != true))
                .ThenInclude(a => a.MaTaiNguyenNavigation)
                .Where(n => n.MaNguoiDung == userId
                    && (n.DaDoc == null || n.DaDoc == false)
                    && (n.DaXoa == null || n.DaXoa == false)
                    && (n.MaThongBaoNavigation.DaXoa == null || n.MaThongBaoNavigation.DaXoa == false))
                .OrderByDescending(n => n.MaThongBaoNavigation.ThoiGianTao)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(Guid userId)
        {
            return await _context.Nguoinhanthongbaos
                .Where(n => n.MaNguoiDung == userId 
                    && (n.DaDoc == null || n.DaDoc == false)
                    && (n.DaXoa == null || n.DaXoa == false))
                .CountAsync();
        }

        public async Task MarkAsReadAsync(Guid userId, Guid notificationId)
        {
            var record = await _context.Nguoinhanthongbaos
                .FirstOrDefaultAsync(n => n.MaNguoiDung == userId && n.MaThongBao == notificationId);

            if (record != null)
            {
                record.DaDoc = true;
                record.NgayDoc = DateTime.UtcNow;
                record.NguoiSua = userId;
                record.ThoiGianSua = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAllAsReadAsync(Guid userId)
        {
            var records = await _context.Nguoinhanthongbaos
                .Where(n => n.MaNguoiDung == userId && (n.DaDoc == null || n.DaDoc == false))
                .ToListAsync();

            foreach (var record in records)
            {
                record.DaDoc = true;
                record.NgayDoc = DateTime.UtcNow;
                record.NguoiSua = userId;
                record.ThoiGianSua = DateTime.UtcNow;
            }

            if (records.Count > 0)
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Guid> CreateAsync(Thongbao thongBao, List<Guid> receiverIds)
        {
            await _context.Thongbaos.AddAsync(thongBao);

            // Thêm danh sách người nhận
            var nguoiNhans = receiverIds.Select(userId => new Nguoinhanthongbao
            {
                MaThongBao = thongBao.MaThongBao,
                MaNguoiDung = userId,
                DaDoc = false,
                ThoiGianTao = DateTime.UtcNow,
                NguoiTao = thongBao.NguoiTao,
                DaXoa = false,
                TrangThai = true
            }).ToList();

            await _context.Nguoinhanthongbaos.AddRangeAsync(nguoiNhans);
            await _context.SaveChangesAsync();

            return thongBao.MaThongBao;
        }

        public async Task UpdateAsync(Thongbao thongBao, List<Guid> newReceiverIds, Guid currentUserId)
        {
            // 1. Tạm thời đánh dấu xóa mềm toàn bộ người nhận hiện tại
            foreach (var old in thongBao.Nguoinhanthongbaos)
            {
                old.DaXoa = true;
                old.NguoiSua = currentUserId;
                old.ThoiGianSua = DateTime.UtcNow;
            }

            // 2. Duyệt qua danh sách người nhận mới để Thêm mới hoặc Khôi phục
            foreach (var userId in newReceiverIds)
            {
                // Tìm xem người dùng này đã từng có trong danh sách nhận thông báo chưa
                var existingReceiver = thongBao.Nguoinhanthongbaos.FirstOrDefault(n => n.MaNguoiDung == userId);

                if (existingReceiver != null)
                {
                    // Nếu đã từng tồn tại -> Khôi phục trạng thái (tránh lỗi trùng Khóa chính)
                    existingReceiver.DaXoa = false;
                    existingReceiver.NguoiSua = currentUserId;
                    existingReceiver.ThoiGianSua = DateTime.UtcNow;
                }
                else
                {
                    // Nếu chưa từng tồn tại -> Thêm mới hoàn toàn
                    thongBao.Nguoinhanthongbaos.Add(new Nguoinhanthongbao
                    {
                        MaThongBao = thongBao.MaThongBao,
                        MaNguoiDung = userId,
                        DaDoc = false,
                        ThoiGianTao = DateTime.UtcNow,
                        NguoiTao = currentUserId,
                        DaXoa = false,
                        TrangThai = true
                    });
                }
            }

            _context.Thongbaos.Update(thongBao);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSoftAsync(Guid id, Guid currentUserId)
        {
            var thongBao = await _context.Thongbaos
                .Include(t => t.Nguoinhanthongbaos)
                .FirstOrDefaultAsync(t => t.MaThongBao == id);

            if (thongBao != null)
            {
                // 1. Xóa mềm bảng ThongBao
                thongBao.DaXoa = true;
                thongBao.NguoiSua = currentUserId;
                thongBao.ThoiGianSua = DateTime.UtcNow;

                // 2. Xóa mềm bảng NguoiNhanThongBao liên quan
                foreach (var nn in thongBao.Nguoinhanthongbaos)
                {
                    nn.DaXoa = true;
                    nn.NguoiSua = currentUserId;
                    nn.ThoiGianSua = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
