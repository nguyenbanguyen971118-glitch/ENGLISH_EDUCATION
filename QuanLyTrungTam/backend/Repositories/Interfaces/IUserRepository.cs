using backend.Models;

namespace backend.Repositories.Interfaces;

public interface IUserRepository
{
    Task<Nguoidung> GetByEmailAsync(string email);
    Task<Nguoidung> GetByIdAsync(Guid id);
    Task<Nguoidung> GetByUsernameAsync(string username);
    Task<IEnumerable<Nguoidung>> GetAllAsync();
    Task AddAsync(Nguoidung user);
    Task UpdateAsync(Nguoidung user);
    Task DeleteAsync(Guid id);
}
