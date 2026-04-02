using System.Security.Claims;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Helpers;

/// <summary>
/// Helper dùng để truy vấn và kiểm tra quyền của người dùng.
/// Helper này đọc trực tiếp từ DbContext nên chỉ nên dùng trong tầng nghiệp vụ/hạ tầng,
/// không gọi từ controller.
/// </summary>
public class PermissionHelper
{
    private readonly AppDbContext _context;

    public PermissionHelper(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Kiểm tra người dùng có MaQuyen cụ thể trong một chức năng hay không.
    /// </summary>
    /// <param name="userId">Mã người dùng.</param>
    /// <param name="maChucNang">Mã chức năng cần kiểm tra. Truyền giá trị <= 0 để chỉ kiểm tra theo MaQuyen.</param>
    /// <param name="permissionId">Mã quyền (MaQuyen) cần kiểm tra.</param>
    /// <returns>True nếu người dùng có quyền, ngược lại trả về false.</returns>
    public async Task<bool> HasPermissionByIdAsync(Guid userId, int maChucNang, int permissionId)
    {
        try
        {
            if (permissionId <= 0)
            {
                return false;
            }

            var user = await _context.Nguoidungs
                .Include(x => x.Nguoidungvaitros)
                .FirstOrDefaultAsync(x => x.MaNguoiDung == userId);

            if (user == null)
                return false;

            var roleIds = user.Nguoidungvaitros
                .Where(x => x.DaXoa == null || x.DaXoa == false)
                .Select(x => x.MaVaiTro)
                .ToList();

            if (!roleIds.Any())
                return false;

            return await _context.Vaitroquyens
                .Include(x => x.MaQuyenNavigation)
                .AnyAsync(x =>
                    roleIds.Contains(x.MaVaiTro) &&
                    x.MaQuyen == permissionId &&
                    (maChucNang <= 0 || x.MaQuyenNavigation.MaChucNang == maChucNang) &&
                    (x.DaXoa == null || x.DaXoa == false)
                );
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Kiểm tra người dùng có quyền truy cập một chức năng cụ thể hay không.
    /// </summary>
    /// <param name="userId">Mã người dùng.</param>
    /// <param name="maChucNang">Mã chức năng cần kiểm tra. Truyền giá trị <= 0 để kiểm tra theo mã quyền, không ép chức năng.</param>
    /// <param name="permissionCode">Mã quyền kỹ thuật, ví dụ: USERS_CREATE, PAGE_ADMIN_USERS_VIEW.</param>
    /// <returns>True nếu người dùng có quyền, ngược lại trả về false.</returns>
    public async Task<bool> HasPermissionAsync(Guid userId, int maChucNang, string permissionCode)
    {
        try
        {
            var normalizedPermissionCode = (permissionCode ?? string.Empty).Trim().ToUpperInvariant();
            if (string.IsNullOrWhiteSpace(normalizedPermissionCode))
            {
                return false;
            }

            var user = await _context.Nguoidungs
                .Include(x => x.Nguoidungvaitros)
                .FirstOrDefaultAsync(x => x.MaNguoiDung == userId);

            if (user == null)
                return false;

            // Lấy tất cả vai trò đang còn hiệu lực của người dùng.
            var roleIds = user.Nguoidungvaitros
                .Where(x => x.DaXoa == null || x.DaXoa == false)
                .Select(x => x.MaVaiTro)
                .ToList();

            if (!roleIds.Any())
                return false;

            // Kiểm tra xem có vai trò nào gán quyền này cho đúng chức năng hay không.
            var hasPermission = await _context.Vaitroquyens
                .Include(x => x.MaQuyenNavigation)
                .AnyAsync(x =>
                    roleIds.Contains(x.MaVaiTro) &&
                    (maChucNang <= 0 || x.MaQuyenNavigation.MaChucNang == maChucNang) &&
                    ((x.MaQuyenNavigation.TenQuyen ?? string.Empty).Trim().ToUpper()) == normalizedPermissionCode &&
                    (x.DaXoa == null || x.DaXoa == false)
                );

            return hasPermission;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Lấy toàn bộ quyền của người dùng theo từng chức năng.
    /// </summary>
    /// <param name="userId">Mã người dùng.</param>
    /// <returns>Dictionary có dạng { MaChucNang: ["Xem", "Tạo", ...] }.</returns>
    public async Task<Dictionary<int, List<string>>> GetUserPermissionsAsync(Guid userId)
    {
        var result = new Dictionary<int, List<string>>();

        try
        {
            var user = await _context.Nguoidungs
                .Include(x => x.Nguoidungvaitros)
                .FirstOrDefaultAsync(x => x.MaNguoiDung == userId);

            if (user == null)
                return result;

            var roleIds = user.Nguoidungvaitros
                .Where(x => x.DaXoa == null || x.DaXoa == false)
                .Select(x => x.MaVaiTro)
                .ToList();

            if (!roleIds.Any())
                return result;

            var permissions = await _context.Vaitroquyens
                .Include(x => x.MaQuyenNavigation)
                .Where(x =>
                    roleIds.Contains(x.MaVaiTro) &&
                    (x.DaXoa == null || x.DaXoa == false)
                )
                .Select(x => new
                {
                    x.MaQuyenNavigation.MaChucNang,
                    x.MaQuyenNavigation.TenQuyen
                })
                .ToListAsync();

            foreach (var perm in permissions)
            {
                if (!result.ContainsKey(perm.MaChucNang))
                {
                    result[perm.MaChucNang] = new List<string>();
                }

                if (!result[perm.MaChucNang].Contains(perm.TenQuyen))
                {
                    result[perm.MaChucNang].Add(perm.TenQuyen);
                }
            }
        }
        catch
        {
            // Có thể bổ sung logging ở đây nếu hệ thống có logger dùng chung.
        }

        return result;
    }

    /// <summary>
    /// Lấy vai trò chính của người dùng.
    /// </summary>
    /// <param name="userId">Mã người dùng.</param>
    /// <returns>Tên vai trò như Admin, Giao_Vien, Hoc_Sinh, Phu_Huynh.</returns>
    public async Task<string?> GetUserPrimaryRoleAsync(Guid userId)
    {
        var role = await _context.Nguoidungs
            .Where(x => x.MaNguoiDung == userId)
            .Include(x => x.Nguoidungvaitros)
            .ThenInclude(x => x.MaVaiTroNavigation)
            .SelectMany(x => x.Nguoidungvaitros)
            .Where(x => x.DaXoa == null || x.DaXoa == false)
            .Select(x => x.MaVaiTroNavigation.TenVaiTro)
            .FirstOrDefaultAsync();

        return role;
    }

    /// <summary>
    /// Kiểm tra người dùng có mang một vai trò cụ thể hay không.
    /// </summary>
    /// <param name="userId">Mã người dùng.</param>
    /// <param name="roleName">Tên vai trò cần kiểm tra.</param>
    /// <returns>True nếu người dùng có vai trò này, ngược lại trả về false.</returns>
    public async Task<bool> HasRoleAsync(Guid userId, string roleName)
    {
        return await _context.Nguoidungs
            .Where(x => x.MaNguoiDung == userId)
            .Include(x => x.Nguoidungvaitros)
            .ThenInclude(x => x.MaVaiTroNavigation)
            .SelectMany(x => x.Nguoidungvaitros)
            .AnyAsync(x =>
                x.MaVaiTroNavigation.TenVaiTro == roleName &&
                (x.DaXoa == null || x.DaXoa == false)
            );
    }
}
