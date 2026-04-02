using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

/// <summary>
/// Kho dữ liệu phục vụ toàn bộ nghiệp vụ phân quyền.
/// Lớp này chỉ chịu trách nhiệm truy xuất và ghi dữ liệu, không chứa logic điều phối.
/// </summary>
public class PermissionRepository : IPermissionRepository
{
    private readonly AppDbContext _context;

    private static string NormalizePermissionCode(string code)
    {
        return (code ?? string.Empty).Trim().ToUpperInvariant();
    }

    /// <summary>
    /// Khởi tạo repository với DbContext dùng chung của backend.
    /// </summary>
    /// <param name="context">DbContext của ứng dụng.</param>
    public PermissionRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy danh sách quyền thô từ cơ sở dữ liệu để dựng ma trận phân quyền.
    /// </summary>
    public async Task<List<PermissionFlatDto>> GetRawPermissionsAsync()
    {
        return await _context.Quyens
            .Include(x => x.MaChucNangNavigation)
            .OrderBy(x => x.MaChucNang)
            .ThenBy(x => x.MaQuyen)
            .Select(x => new PermissionFlatDto
            {
                MaChucNang = x.MaChucNang,
                MaQuyen = x.MaQuyen,
                PermissionCode = x.TenQuyen,
                TenChucNang = x.MaChucNangNavigation.TenChucNang,
                MaTrang = "func_" + x.MaChucNang,
                TenTrang = x.MaChucNangNavigation.TenChucNang,
                ThuTu = x.MaQuyen
            })
            .ToListAsync();
    }

    /// <summary>
    /// Lấy các vai trò còn hiệu lực để hiển thị và gán quyền.
    /// </summary>
    public async Task<List<RoleLiteDto>> GetActiveRolesAsync()
    {
        return await _context.Vaitros
            .Where(x => x.DaXoa == null || x.DaXoa == false)
            .Select(x => new RoleLiteDto
            {
                MaVaiTro = x.MaVaiTro,
                TenVaiTro = x.TenVaiTro
            })
            .ToListAsync();
    }

    /// <summary>
    /// Lấy toàn bộ mapping vai trò - quyền đang còn hiệu lực.
    /// </summary>
    public async Task<List<RolePermissionMapDto>> GetActiveMappingsAsync()
    {
        return await _context.Vaitroquyens
            .Where(x => x.DaXoa == null || x.DaXoa == false)
            .Select(x => new RolePermissionMapDto
            {
                MaVaiTro = x.MaVaiTro,
                MaQuyen = x.MaQuyen
            })
            .ToListAsync();
    }

    /// <summary>
    /// Tìm vai trò theo mã định danh.
    /// </summary>
    public async Task<Vaitro?> GetRoleByIdAsync(int roleId)
    {
        return await _context.Vaitros.FirstOrDefaultAsync(x => x.MaVaiTro == roleId);
    }

    /// <summary>
    /// Kiểm tra tên vai trò đã tồn tại hay chưa.
    /// </summary>
    public async Task<bool> RoleNameExistsAsync(string roleName)
    {
        return await _context.Vaitros.AnyAsync(x =>
            x.TenVaiTro == roleName &&
            (x.DaXoa == null || x.DaXoa == false));
    }

    /// <summary>
    /// Chuyển danh sách mã quyền sang danh sách ID quyền tương ứng trong bảng Quyền.
    /// </summary>
    public async Task<List<int>> GetPermissionIdsByCodesAsync(List<string> permissionCodes)
    {
        permissionCodes ??= new List<string>();
        var normalizedCodes = permissionCodes
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(NormalizePermissionCode)
            .Distinct()
            .ToList();

        if (!normalizedCodes.Any())
        {
            return new List<int>();
        }

        return await _context.Quyens
            .Where(x => normalizedCodes.Contains((x.TenQuyen ?? string.Empty).Trim().ToUpper()))
            .Select(x => x.MaQuyen)
            .ToListAsync();
    }

    /// <summary>
    /// Lấy danh sách mã quyền của một vai trò.
    /// </summary>
    public async Task<List<string>> GetPermissionCodesByRoleIdAsync(int roleId)
    {
        return await _context.Vaitroquyens
            .Where(x => x.MaVaiTro == roleId && (x.DaXoa == null || x.DaXoa == false))
            .Select(x => x.MaQuyenNavigation.TenQuyen)
            .ToListAsync();
    }

    /// <summary>
    /// Kiểm tra một vai trò có quyền cụ thể hay không.
    /// </summary>
    public async Task<bool> HasPermissionAsync(int roleId, string permissionCode)
    {
        var normalizedPermissionCode = NormalizePermissionCode(permissionCode);
        if (string.IsNullOrWhiteSpace(normalizedPermissionCode))
        {
            return false;
        }

        return await _context.Vaitroquyens.AnyAsync(x =>
            x.MaVaiTro == roleId
            && (x.DaXoa == null || x.DaXoa == false)
            && ((x.MaQuyenNavigation.TenQuyen ?? string.Empty).Trim().ToUpper()) == normalizedPermissionCode);
    }

    /// <summary>
    /// Tạo vai trò mới và trả về mã vai trò vừa sinh.
    /// </summary>
    public async Task<int> CreateRoleAsync(string roleName)
    {
        var role = new Vaitro { TenVaiTro = roleName };
        _context.Vaitros.Add(role);
        await _context.SaveChangesAsync();
        return role.MaVaiTro;
    }

    /// <summary>
    /// Ghi đè toàn bộ quyền của một vai trò bằng danh sách quyền mới.
    /// </summary>
    public async Task ReplaceRolePermissionsAsync(int roleId, List<int> permissionIds)
    {
        permissionIds ??= new List<int>();

        var oldMappings = _context.Vaitroquyens.Where(x => x.MaVaiTro == roleId);
        _context.Vaitroquyens.RemoveRange(oldMappings);

        foreach (var permissionId in permissionIds)
        {
            _context.Vaitroquyens.Add(new Vaitroquyen
            {
                MaVaiTro = roleId,
                MaQuyen = permissionId,
                TrangThai = true,
                DaXoa = false
            });
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Kiểm tra vai trò có còn người dùng nào đang được gán hay không.
    /// </summary>
    public async Task<bool> HasAssignedUsersAsync(int roleId)
    {
        return await _context.Nguoidungvaitros
            .AnyAsync(x => x.MaVaiTro == roleId && (x.DaXoa == null || x.DaXoa == false));
    }

    /// <summary>
    /// Xóa mềm vai trò và các mapping quyền liên quan.
    /// </summary>
    public async Task SoftDeleteRoleAsync(int roleId)
    {
        var role = await _context.Vaitros.FirstAsync(x => x.MaVaiTro == roleId);
        var mappings = await _context.Vaitroquyens
            .Where(x => x.MaVaiTro == roleId && (x.DaXoa == null || x.DaXoa == false))
            .ToListAsync();

        foreach (var mapping in mappings)
        {
            mapping.DaXoa = true;
            mapping.TrangThai = false;
            mapping.ThoiGianSua = DateTime.Now;
        }

        role.DaXoa = true;
        role.TrangThai = false;
        role.ThoiGianSua = DateTime.Now;

        await _context.SaveChangesAsync();
    }
}
