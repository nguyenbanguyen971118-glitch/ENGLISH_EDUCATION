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
}
