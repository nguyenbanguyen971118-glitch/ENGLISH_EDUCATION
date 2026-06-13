using backend.Data;
using backend.Models;
using backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Nguoidung> GetByEmailAsync(string email)
    {
        return await _context.Nguoidungs
            .FirstOrDefaultAsync(u => u.Email == email && (u.DaXoa == null || u.DaXoa == false));
    }

    public async Task<Nguoidung> GetByIdAsync(Guid id)
    {
        return await _context.Nguoidungs
            .FirstOrDefaultAsync(u => u.MaNguoiDung == id && (u.DaXoa == null || u.DaXoa == false));
    }

    public async Task<Nguoidung> GetByUsernameAsync(string username)
    {
        return await _context.Nguoidungs
            .FirstOrDefaultAsync(u => u.TenDangNhap == username && (u.DaXoa == null || u.DaXoa == false));
    }

    public async Task<IEnumerable<Nguoidung>> GetAllAsync()
    {
        return await _context.Nguoidungs
            .Where(u => u.DaXoa == null || u.DaXoa == false)
            .ToListAsync();
    }

    public async Task AddAsync(Nguoidung user)
    {
        await _context.Nguoidungs.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Nguoidung user)
    {
        _context.Nguoidungs.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await GetByIdAsync(id);
        if (user != null)
        {
            user.DaXoa = true;
            user.ThoiGianSua = DateTime.Now;
            await UpdateAsync(user);
        }
    }

    // 3-layer pattern implementations
    public async Task<Vaitro> GetUserRoleAsync(Guid userId)
    {
        return await _context.Nguoidungvaitros
            .Where(x => x.MaNguoiDung == userId && (x.DaXoa == null || x.DaXoa == false))
            .Select(x => x.MaVaiTroNavigation)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<string>> GetUserPermissionsAsync(int roleId)
    {
        return await _context.Vaitroquyens
            .Where(x => x.MaVaiTro == roleId && (x.DaXoa == null || x.DaXoa == false))
            .Select(x => x.MaQuyenNavigation.TenQuyen)
            .ToListAsync();
    }

    public async Task<Guid?> GetUserProfileIdAsync(Guid userId, string roleName)
    {
        if (roleName == "Hoc_Sinh")
        {
            // Trả ProfileId học sinh để đồng bộ với luồng login/refresh của các role còn lại.
            var hs = await _context.Hocsinhs.FirstOrDefaultAsync(h => h.MaNguoiDung == userId);
            return hs?.MaHocSinh;
        }
        else if (roleName == "Giao_Vien")
        {
            // Trả ProfileId giảng viên để đồng bộ với luồng login/refresh của các role còn lại.
            var gv = await _context.Giangviens
                .FirstOrDefaultAsync(g => g.MaNguoiDung == userId && (g.DaXoa == null || g.DaXoa == false));
            return gv?.MaGiangVien;
        }
        else if (roleName == "Phu_Huynh")
        {
            // Bổ sung ProfileId cho phụ huynh để đồng bộ với luồng login/refresh của các role khác.
            var ph = await _context.Phuhuynhs
                .FirstOrDefaultAsync(p => p.MaNguoiDung == userId && (p.DaXoa == null || p.DaXoa == false));
            return ph?.MaPhuHuynh;
        }
        return null;
    }
}
