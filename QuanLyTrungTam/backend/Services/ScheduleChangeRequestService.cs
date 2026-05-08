using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Hubs;
using backend.Models;
using backend.Services.Interfaces;

namespace backend.Services
{
    public class ScheduleChangeRequestService : IScheduleChangeRequestService
    {
        private readonly AppDbContext _db;
        private readonly INotificationService _notificationService;
        private readonly IHubContext<ChatHub> _hubContext;

        public ScheduleChangeRequestService(AppDbContext db, INotificationService notificationService, IHubContext<ChatHub> hubContext)
        {
            _db = db;
            _notificationService = notificationService;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Tạo yêu cầu đổi lịch dạy từ giáo viên
        /// </summary>
        public async Task<ScheduleChangeRequestResponseDto> CreateScheduleChangeRequestAsync(
            Guid maGiangVien, 
            CreateScheduleChangeRequestDto dto)
        {
            var response = new ScheduleChangeRequestResponseDto
            {
                MaYeuCau = Guid.NewGuid(),
                Success = false
            };

            // Lấy buổi học hiện tại
            var buoiHoc = await _db.Buoihocs
                .Include(b => b.MaLopHocNavigation)
                .FirstOrDefaultAsync(b => b.MaBuoiHoc == dto.MaBuoiHoc && (b.DaXoa == null || !b.DaXoa.Value));

            if (buoiHoc == null)
            {
                response.Message = "Buổi học không tồn tại hoặc đã bị xóa";
                return response;
            }

            // Kiểm tra giáo viên có quyền yêu cầu đổi lịch buổi này không
            var giangvienLopHoc = await _db.Giangvienlophocs
                .FirstOrDefaultAsync(g => g.MaLopHoc == buoiHoc.MaLopHoc && g.MaGiangVien == maGiangVien);

            if (giangvienLopHoc == null)
            {
                response.Message = "Bạn không phải là giáo viên của lớp này";
                return response;
            }

            var teacherName = await _db.Giangviens
                .Where(g => g.MaGiangVien == maGiangVien)
                .Select(g => g.MaNguoiDungNavigation.HoTen)
                .FirstOrDefaultAsync() ?? "Giáo viên";

            var className = buoiHoc.MaLopHocNavigation?.TenLop ?? "lớp học";

            // Kiểm tra theo loại yêu cầu
            if (dto.LoaiYeuCau == 1) // Đổi thời gian
            {
                if (!dto.NgayHocDeXuat.HasValue || !dto.MaTietBatDauDeXuat.HasValue || !dto.MaTietKetThucDeXuat.HasValue)
                {
                    response.Message = "Yêu cầu đổi thời gian phải có ngày và ca học mới";
                    return response;
                }

                // Kiểm tra ngày đề xuất không được là quá khứ
                if (dto.NgayHocDeXuat.Value < DateOnly.FromDateTime(DateTime.Now))
                {
                    response.Message = "Ngày học mới không được là quá khứ";
                    return response;
                }

                // Kiểm tra tiết bắt đầu <= tiết kết thúc
                if (dto.MaTietBatDauDeXuat.Value > dto.MaTietKetThucDeXuat.Value)
                {
                    response.Message = "Tiết bắt đầu phải nhỏ hơn hoặc bằng tiết kết thúc";
                    return response;
                }

                // Kiểm tra xung đột với các buổi học khác của cùng lớp
                var conflictClass = await _db.Buoihocs
                    .FirstOrDefaultAsync(b => 
                        b.MaLopHoc == buoiHoc.MaLopHoc &&
                        b.NgayHoc == dto.NgayHocDeXuat.Value &&
                        b.MaTietBatDau <= dto.MaTietKetThucDeXuat.Value &&
                        b.MaTietKetThuc >= dto.MaTietBatDauDeXuat.Value &&
                        b.MaBuoiHoc != dto.MaBuoiHoc &&
                        (!b.DaXoa.HasValue || !b.DaXoa.Value));

                if (conflictClass != null)
                {
                    response.Message = "Lớp học này đã có buổi học khác trùng với thời gian đề xuất";
                    return response;
                }

                // Kiểm tra xung đột với các buổi học khác của giáo viên
                var conflictTeacher = await _db.Buoihocs
                    .Where(b => b.NgayHoc == dto.NgayHocDeXuat.Value &&
                                b.MaTietBatDau <= dto.MaTietKetThucDeXuat.Value &&
                                b.MaTietKetThuc >= dto.MaTietBatDauDeXuat.Value &&
                                b.MaBuoiHoc != dto.MaBuoiHoc &&
                                (!b.DaXoa.HasValue || !b.DaXoa.Value))
                    .Join(_db.Giangvienlophocs.Where(g => g.MaGiangVien == maGiangVien),
                        b => b.MaLopHoc,
                        g => g.MaLopHoc,
                        (b, g) => b)
                    .FirstOrDefaultAsync();

                if (conflictTeacher != null)
                {
                    response.Message = "Bạn đã có buổi học khác trùng với thời gian đề xuất";
                    return response;
                }
            }
            else if (dto.LoaiYeuCau == 2) // Đổi phòng
            {
                if (!dto.MaPhongHocDeXuat.HasValue)
                {
                    response.Message = "Yêu cầu đổi phòng phải có mã phòng mới";
                    return response;
                }

                // Kiểm tra phòng có tồn tại không
                var phongHoc = await _db.Phonghocs
                    .FirstOrDefaultAsync(p => p.MaPhongHoc == dto.MaPhongHocDeXuat.Value);

                if (phongHoc == null)
                {
                    response.Message = "Phòng học không tồn tại";
                    return response;
                }

                // Kiểm tra xung đột phòng học trên thời gian buổi học hiện tại
                var conflictRoom = await _db.Buoihocs
                    .FirstOrDefaultAsync(b =>
                        b.MaPhongHoc == dto.MaPhongHocDeXuat.Value &&
                        b.NgayHoc == buoiHoc.NgayHoc &&
                        b.MaTietBatDau <= buoiHoc.MaTietKetThuc &&
                        b.MaTietKetThuc >= buoiHoc.MaTietBatDau &&
                        b.MaBuoiHoc != dto.MaBuoiHoc &&
                        (!b.DaXoa.HasValue || !b.DaXoa.Value));

                if (conflictRoom != null)
                {
                    response.Message = "Phòng học này đã được sử dụng trong thời gian đó";
                    return response;
                }
            }
            else
            {
                response.Message = "Loại yêu cầu không hợp lệ (1: Đổi thời gian, 2: Đổi phòng)";
                return response;
            }

            // Lưu yêu cầu
            var yeuCau = new Yeucaulichday
            {
                MaYeuCau = response.MaYeuCau,
                MaGiangVien = maGiangVien,
                MaLopHoc = buoiHoc.MaLopHoc,
                MaBuoiHoc = dto.MaBuoiHoc,
                LoaiYeuCau = dto.LoaiYeuCau,
                NgayHocDeXuat = dto.NgayHocDeXuat ?? buoiHoc.NgayHoc,
                MaTietBatDauDeXuat = dto.MaTietBatDauDeXuat ?? buoiHoc.MaTietBatDau,
                MaTietKetThucDeXuat = dto.MaTietKetThucDeXuat ?? buoiHoc.MaTietKetThuc,
                MaPhongHocDeXuat = dto.MaPhongHocDeXuat,
                LyDo = dto.LyDo,
                TrangThaiDuyet = 0, // Chờ xử lý
                NguoiTao = maGiangVien,
                ThoiGianTao = DateTime.Now,
                TrangThai = true,
                DaXoa = false
            };

            _db.Yeucaulichdays.Add(yeuCau);
            await _db.SaveChangesAsync();

            response.Message = "Yêu cầu đổi lịch đã được tạo thành công, vui lòng chờ admin xử lý";
            response.Success = true;

            // Gửi thông báo và realtime notify tới admin
            await NotifyAdminAboutNewRequestAsync(yeuCau, teacherName, className, buoiHoc);

            return response;
        }

        /// <summary>
        /// Lấy danh sách yêu cầu chờ xử lý
        /// </summary>
        public async Task<List<ScheduleChangeRequestDetailDto>> GetPendingRequestsAsync()
        {
            var requests = await _db.Yeucaulichdays
                .Where(y => y.TrangThaiDuyet == 0 && (!y.DaXoa.HasValue || !y.DaXoa.Value))
                .Include(y => y.MaGiangVienNavigation)
                .ThenInclude(g => g.MaNguoiDungNavigation)
                .Include(y => y.MaLopHocNavigation)
                .Include(y => y.MaBuoiHocNavigation)
                .ThenInclude(b => b!.MaPhongHocNavigation)
                .Include(y => y.MaPhongHocDeXuatNavigation)
                .ToListAsync();

            return await MapYeuCauToDetailDtoAsync(requests);
        }

        /// <summary>
        /// Lấy danh sách yêu cầu của một giáo viên
        /// </summary>
        public async Task<List<ScheduleChangeRequestDetailDto>> GetRequestsByTeacherAsync(Guid maGiangVien)
        {
            var requests = await _db.Yeucaulichdays
                .Where(y => y.MaGiangVien == maGiangVien && (!y.DaXoa.HasValue || !y.DaXoa.Value))
                .Include(y => y.MaGiangVienNavigation)
                .ThenInclude(g => g.MaNguoiDungNavigation)
                .Include(y => y.MaLopHocNavigation)
                .Include(y => y.MaBuoiHocNavigation)
                .ThenInclude(b => b!.MaPhongHocNavigation)
                .Include(y => y.MaPhongHocDeXuatNavigation)
                .ToListAsync();

            return await MapYeuCauToDetailDtoAsync(requests);
        }

        /// <summary>
        /// Lấy chi tiết một yêu cầu
        /// </summary>
        public async Task<ScheduleChangeRequestDetailDto?> GetRequestByIdAsync(Guid maYeuCau)
        {
            var request = await _db.Yeucaulichdays
                .Where(y => y.MaYeuCau == maYeuCau && (!y.DaXoa.HasValue || !y.DaXoa.Value))
                .Include(y => y.MaGiangVienNavigation)
                .ThenInclude(g => g.MaNguoiDungNavigation)
                .Include(y => y.MaLopHocNavigation)
                .Include(y => y.MaBuoiHocNavigation)
                .ThenInclude(b => b!.MaPhongHocNavigation)
                .Include(y => y.MaPhongHocDeXuatNavigation)
                .FirstOrDefaultAsync();

            if (request == null)
                return null;

            var dtos = await MapYeuCauToDetailDtoAsync(new[] { request });
            return dtos.FirstOrDefault();
        }

        /// <summary>
        /// Duyệt yêu cầu đổi lịch
        /// </summary>
        public async Task<ScheduleChangeRequestResponseDto> ApproveRequestAsync(Guid maYeuCau, string? ghiChuAdmin = null)
        {
            var response = new ScheduleChangeRequestResponseDto
            {
                MaYeuCau = maYeuCau,
                Success = false
            };

            var yeuCau = await _db.Yeucaulichdays
                .Include(y => y.MaBuoiHocNavigation)
                .FirstOrDefaultAsync(y => y.MaYeuCau == maYeuCau);

            if (yeuCau == null || yeuCau.DaXoa == true)
            {
                response.Message = "Yêu cầu không tồn tại";
                return response;
            }

            if (yeuCau.TrangThaiDuyet != 0)
            {
                response.Message = "Yêu cầu này đã được xử lý";
                return response;
            }

            var buoiHoc = yeuCau.MaBuoiHocNavigation;
            if (buoiHoc == null)
            {
                response.Message = "Buổi học không tồn tại";
                return response;
            }

            try
            {
                // Cập nhật BuoiHoc theo loại yêu cầu
                if (yeuCau.LoaiYeuCau == 1) // Đổi thời gian
                {
                    buoiHoc.NgayHoc = yeuCau.NgayHocDeXuat;
                    buoiHoc.MaTietBatDau = yeuCau.MaTietBatDauDeXuat;
                    buoiHoc.MaTietKetThuc = yeuCau.MaTietKetThucDeXuat;
                    buoiHoc.NguoiSua = Guid.Empty; // Admin
                    buoiHoc.ThoiGianSua = DateTime.Now;
                }
                else if (yeuCau.LoaiYeuCau == 2) // Đổi phòng
                {
                    buoiHoc.MaPhongHoc = yeuCau.MaPhongHocDeXuat;
                    buoiHoc.NguoiSua = Guid.Empty; // Admin
                    buoiHoc.ThoiGianSua = DateTime.Now;
                }

                // Cập nhật trạng thái yêu cầu
                yeuCau.TrangThaiDuyet = 1; // Đã duyệt
                yeuCau.GhiChuAdmin = ghiChuAdmin;
                yeuCau.NguoiSua = Guid.Empty; // Admin
                yeuCau.ThoiGianSua = DateTime.Now;

                _db.Buoihocs.Update(buoiHoc);
                _db.Yeucaulichdays.Update(yeuCau);
                await _db.SaveChangesAsync();

                response.Message = yeuCau.LoaiYeuCau == 1 
                    ? "Yêu cầu đổi thời gian đã được duyệt. Lịch dạy đã được cập nhật"
                    : "Yêu cầu đổi phòng đã được duyệt. Phòng học đã được cập nhật";
                response.Success = true;

                // Gửi thông báo tới giáo viên và học sinh
                await NotifyApprovalAsync(yeuCau, buoiHoc);
            }
            catch (Exception ex)
            {
                response.Message = $"Lỗi khi xử lý yêu cầu: {ex.Message}";
            }

            return response;
        }

        /// <summary>
        /// Từ chối yêu cầu đổi lịch
        /// </summary>
        public async Task<ScheduleChangeRequestResponseDto> RejectRequestAsync(Guid maYeuCau, string? ghiChuAdmin = null)
        {
            var response = new ScheduleChangeRequestResponseDto
            {
                MaYeuCau = maYeuCau,
                Success = false
            };

            var yeuCau = await _db.Yeucaulichdays
                .FirstOrDefaultAsync(y => y.MaYeuCau == maYeuCau);

            if (yeuCau == null || yeuCau.DaXoa == true)
            {
                response.Message = "Yêu cầu không tồn tại";
                return response;
            }

            if (yeuCau.TrangThaiDuyet != 0)
            {
                response.Message = "Yêu cầu này đã được xử lý";
                return response;
            }

            try
            {
                yeuCau.TrangThaiDuyet = 2; // Từ chối
                yeuCau.GhiChuAdmin = ghiChuAdmin;
                yeuCau.NguoiSua = Guid.Empty; // Admin
                yeuCau.ThoiGianSua = DateTime.Now;

                _db.Yeucaulichdays.Update(yeuCau);
                await _db.SaveChangesAsync();

                response.Message = "Yêu cầu đã bị từ chối";
                response.Success = true;

                // Gửi thông báo tới giáo viên
                await NotifyRejectionAsync(yeuCau);
            }
            catch (Exception ex)
            {
                response.Message = $"Lỗi khi xử lý yêu cầu: {ex.Message}";
            }

            return response;
        }

        private async Task<List<ScheduleChangeRequestDetailDto>> MapYeuCauToDetailDtoAsync(IEnumerable<Yeucaulichday> yeuCaus)
        {
            var dtos = new List<ScheduleChangeRequestDetailDto>();

            foreach (var yeuCau in yeuCaus)
            {
                var dto = new ScheduleChangeRequestDetailDto
                {
                    MaYeuCau = yeuCau.MaYeuCau,
                    MaGiangVien = yeuCau.MaGiangVien,
                    TenGiangVien = yeuCau.MaGiangVienNavigation?.MaNguoiDungNavigation?.HoTen,
                    MaLopHoc = yeuCau.MaLopHoc,
                    TenLop = yeuCau.MaLopHocNavigation?.TenLop,
                    MaBuoiHoc = yeuCau.MaBuoiHoc ?? Guid.Empty,
                    LoaiYeuCau = yeuCau.LoaiYeuCau ?? 0,
                    NgayHocHienTai = yeuCau.MaBuoiHocNavigation?.NgayHoc.ToDateTime(TimeOnly.MinValue) ?? DateTime.Now,
                    MaTietBatDauHienTai = yeuCau.MaBuoiHocNavigation?.MaTietBatDau ?? 0,
                    MaTietKetThucHienTai = yeuCau.MaBuoiHocNavigation?.MaTietKetThuc ?? 0,
                    MaPhongHocHienTai = yeuCau.MaBuoiHocNavigation?.MaPhongHoc,
                    TenPhongHocHienTai = yeuCau.MaBuoiHocNavigation?.MaPhongHocNavigation?.TenPhong,
                    NgayHocDeXuat = yeuCau.NgayHocDeXuat.ToDateTime(TimeOnly.MinValue),
                    MaTietBatDauDeXuat = yeuCau.MaTietBatDauDeXuat,
                    MaTietKetThucDeXuat = yeuCau.MaTietKetThucDeXuat,
                    MaPhongHocDeXuat = yeuCau.MaPhongHocDeXuat,
                    TenPhongHocDeXuat = yeuCau.MaPhongHocDeXuatNavigation?.TenPhong,
                    LyDo = yeuCau.LyDo,
                    TrangThaiDuyet = yeuCau.TrangThaiDuyet,
                    GhiChuAdmin = yeuCau.GhiChuAdmin,
                    ThoiGianTao = yeuCau.ThoiGianTao,
                    ThoiGianSua = yeuCau.ThoiGianSua
                };

                dtos.Add(dto);
            }

            return dtos;
        }

        private async Task NotifyAdminAboutNewRequestAsync(Yeucaulichday yeuCau, string teacherName, string className, Buoihoc buoiHoc)
        {
            try
            {
                var roomName = yeuCau.MaPhongHocDeXuat.HasValue
                    ? await _db.Phonghocs.Where(r => r.MaPhongHoc == yeuCau.MaPhongHocDeXuat.Value)
                        .Select(r => r.TenPhong)
                        .FirstOrDefaultAsync()
                    : null;

                var requestedDetails = yeuCau.LoaiYeuCau == 1
                    ? $"đổi thời gian sang {yeuCau.NgayHocDeXuat:dd/MM/yyyy}, ca {yeuCau.MaTietBatDauDeXuat}-{yeuCau.MaTietKetThucDeXuat}"
                    : $"đổi phòng sang {roomName ?? "phòng mới"}";

                var currentSchedule = $"buổi {buoiHoc.NgayHoc:dd/MM/yyyy} ca {buoiHoc.MaTietBatDau}-{buoiHoc.MaTietKetThuc}";
                var notificationTitle = "Yêu cầu đổi lịch dạy mới";
                var notificationContent = $"{teacherName} yêu cầu đổi lịch lớp {className} ({currentSchedule}), {requestedDetails}. Lý do: {yeuCau.LyDo ?? "không có"}.";

                // Gửi thông báo tới tất cả admin
                var admins = await _db.Nguoidungs
                    .Where(n => n.LoaiTaiKhoan == 1)
                    .Select(n => n.MaNguoiDung)
                    .ToListAsync();

                var adminGroups = admins.Select(ChatHub.GetUserGroup).ToList();

                foreach (var adminId in admins)
                {
                    var notification = new Thongbao
                    {
                        MaThongBao = Guid.NewGuid(),
                        TieuDe = notificationTitle,
                        NoiDung = notificationContent,
                        TrangThai = true,
                        DaXoa = false,
                        ThoiGianTao = DateTime.Now
                    };

                    _db.Thongbaos.Add(notification);

                    var nguoiNhan = new Nguoinhanthongbao
                    {
                        MaThongBao = notification.MaThongBao,
                        MaNguoiDung = adminId,
                        DaDoc = false,
                        DaXoa = false
                    };

                    _db.Nguoinhanthongbaos.Add(nguoiNhan);
                }

                await _db.SaveChangesAsync();

                if (adminGroups.Any())
                {
                    await _hubContext.Clients.Groups(adminGroups)
                        .SendAsync("NewScheduleChangeRequest", new
                        {
                            yeuCau.MaYeuCau,
                            Teacher = teacherName,
                            ClassName = className,
                            CurrentSchedule = currentSchedule,
                            RequestedDetails = requestedDetails,
                            Reason = yeuCau.LyDo
                        });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending notification to admin: {ex.Message}");
            }
        }

        private async Task NotifyApprovalAsync(Yeucaulichday yeuCau, Buoihoc buoiHoc)
        {
            try
            {
                var message = yeuCau.LoaiYeuCau == 1
                    ? $"Yêu cầu đổi lịch dạy của bạn đã được duyệt. Lịch mới: {yeuCau.NgayHocDeXuat:dd/MM/yyyy}, ca {yeuCau.MaTietBatDauDeXuat}-{yeuCau.MaTietKetThucDeXuat}"
                    : $"Yêu cầu đổi phòng dạy của bạn đã được duyệt.";

                // Thông báo cho giáo viên
                var notificationTeacher = new Thongbao
                {
                    MaThongBao = Guid.NewGuid(),
                    TieuDe = "Yêu cầu đổi lịch được phê duyệt",
                    NoiDung = message,
                    TrangThai = true,
                    DaXoa = false,
                    ThoiGianTao = DateTime.Now
                };

                _db.Thongbaos.Add(notificationTeacher);

                var nguoiNhanTeacher = new Nguoinhanthongbao
                {
                    MaThongBao = notificationTeacher.MaThongBao,
                    MaNguoiDung = yeuCau.MaGiangVien,
                    DaDoc = false,
                    DaXoa = false
                };

                _db.Nguoinhanthongbaos.Add(nguoiNhanTeacher);

                // Thông báo cho học sinh
                var students = await _db.Hocsinhlophocs
                    .Where(hl => hl.MaLopHoc == yeuCau.MaLopHoc)
                    .Select(hl => hl.MaHocSinh)
                    .Distinct()
                    .Join(_db.Hocsinhs,
                        maHocSinh => maHocSinh,
                        hs => hs.MaHocSinh,
                        (maHocSinh, hs) => hs.MaNguoiDung)
                    .ToListAsync();

                foreach (var studentId in students)
                {
                    var notificationStudent = new Thongbao
                    {
                        MaThongBao = Guid.NewGuid(),
                        TieuDe = "Thông báo thay đổi lịch học",
                        NoiDung = $"Lịch học của lớp bạn đã được thay đổi.",
                        TrangThai = true,
                        DaXoa = false,
                        ThoiGianTao = DateTime.Now
                    };

                    _db.Thongbaos.Add(notificationStudent);

                    var nguoiNhanStudent = new Nguoinhanthongbao
                    {
                        MaThongBao = notificationStudent.MaThongBao,
                        MaNguoiDung = studentId,
                        DaDoc = false,
                        DaXoa = false
                    };

                    _db.Nguoinhanthongbaos.Add(nguoiNhanStudent);
                }

                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending approval notification: {ex.Message}");
            }
        }

        private async Task NotifyRejectionAsync(Yeucaulichday yeuCau)
        {
            try
            {
                var message = $"Yêu cầu đổi lịch của bạn đã bị từ chối.";
                if (!string.IsNullOrEmpty(yeuCau.GhiChuAdmin))
                    message += $" Lý do: {yeuCau.GhiChuAdmin}";

                var notification = new Thongbao
                {
                    MaThongBao = Guid.NewGuid(),
                    TieuDe = "Yêu cầu đổi lịch bị từ chối",
                    NoiDung = message,
                    TrangThai = true,
                    DaXoa = false,
                    ThoiGianTao = DateTime.Now
                };

                _db.Thongbaos.Add(notification);

                var nguoiNhan = new Nguoinhanthongbao
                {
                    MaThongBao = notification.MaThongBao,
                    MaNguoiDung = yeuCau.MaGiangVien,
                    DaDoc = false,
                    DaXoa = false
                };

                _db.Nguoinhanthongbaos.Add(nguoiNhan);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending rejection notification: {ex.Message}");
            }
        }
    }
}
