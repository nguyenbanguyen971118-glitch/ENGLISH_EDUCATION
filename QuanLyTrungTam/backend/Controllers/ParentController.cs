using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data; // <--- SỬA: Phải là backend.Data thay vì QuanLyTrungTam.Data
using QuanLyTrungTam.DTOs;

namespace QuanLyTrungTam.Controllers
{
    [Authorize(Roles = "Phu_Huynh")]
    [Route("api/[controller]")]
    [ApiController]
    public class ParentController : ControllerBase
    {
        // SỬA: Đổi ApplicationDbContext thành AppDbContext
        private readonly AppDbContext _context;

        public ParentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("child-summary/{userId}")]
        public async Task<IActionResult> GetChildSummary(string userId)
        {
            if (!Guid.TryParse(userId, out var parsedUserId))
            {
                return BadRequest("userId không hợp lệ.");
            }

            // Logic lấy dữ liệu con cái từ _context
            var data = await (from hs in _context.Hocsinhs
                              join nd in _context.Nguoidungs on hs.MaNguoiDung equals nd.MaNguoiDung
                              where nd.MaNguoiDung == parsedUserId
                              select new ChildSummaryDto
                              {
                                  MaHocSinh = hs.MaHocSinh.ToString(),
                                  TenCon = nd.HoTen,
                                  MaLopHienThi = "N/A",
                                  TenKhoaHoc = "N/A",
                                  NhanXetMoiNhat = "Chưa có dữ liệu"
                              }).FirstOrDefaultAsync();

            if (data == null) return NotFound("Không tìm thấy dữ liệu.");
            return Ok(data);
        }
    }
}