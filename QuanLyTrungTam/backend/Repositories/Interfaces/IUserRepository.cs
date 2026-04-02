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
    
    // 3-layer pattern: role & permission methods
    Task<Vaitro> GetUserRoleAsync(Guid userId);
    Task<IEnumerable<string>> GetUserPermissionsAsync(int roleId);
    Task<Guid?> GetUserProfileIdAsync(Guid userId, string roleName);
}
