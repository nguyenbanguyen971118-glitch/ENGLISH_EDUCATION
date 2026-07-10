using backend.Attributes;
using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IPasswordService _passwordService;

    public UsersController(AppDbContext db, IPasswordService passwordService)
    {
        _db = db;
        _passwordService = passwordService;
    }

    [HttpGet]
    [AuthorizeByPermission("PAGE_ADMIN_USERS_VIEW")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _db.Nguoidungs
            .Where(u => u.DaXoa != true)
            .Include(u => u.Hocsinh)
            .Include(u => u.Giangvien)
            .Include(u => u.Phuhuynh)
            .Include(u => u.Nguoidungvaitros.Where(r => r.DaXoa != true))
            .ThenInclude(r => r.MaVaiTroNavigation)
            .OrderBy(u => u.HoTen)
            .Select(u => ToDto(u))
            .ToListAsync();

        return Ok(ApiResponseDto<List<UserListItemDto>>.Ok(users));
    }

    [HttpGet("{id:guid}")]
    [AuthorizeByPermission("PAGE_ADMIN_USERS_VIEW")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _db.Nguoidungs
            .Where(u => u.MaNguoiDung == id && u.DaXoa != true)
            .Include(u => u.Hocsinh)
            .Include(u => u.Giangvien)
            .Include(u => u.Phuhuynh)
            .Include(u => u.Nguoidungvaitros.Where(r => r.DaXoa != true))
            .ThenInclude(r => r.MaVaiTroNavigation)
            .FirstOrDefaultAsync();

        return user == null
            ? NotFound(ApiResponseDto<object>.Fail("Khong tim thay nguoi dung.", "USER_NOT_FOUND"))
            : Ok(ApiResponseDto<UserListItemDto>.Ok(ToDto(user)));
    }

    [HttpPost]
    [AuthorizeByPermission("USERS_CREATE")]
    public async Task<IActionResult> Create([FromBody] CreateUserRequestDto request)
    {
        var validation = await ValidateCreateAsync(request);
        if (validation != null)
        {
            return BadRequest(validation);
        }

        var now = DateTime.UtcNow;
        var user = new Nguoidung
        {
            MaNguoiDung = Guid.NewGuid(),
            TenDangNhap = request.Username.Trim(),
            MatKhauHash = _passwordService.HashPassword(request.Password),
            HoTen = request.FullName.Trim(),
            Email = NormalizeEmail(request.Email),
            LoaiTaiKhoan = request.AccountType,
            DaXacMinhEmail = true,
            TrangThai = true,
            DaXoa = false,
            ThoiGianTao = now
        };

        _db.Nguoidungs.Add(user);
        await _db.SaveChangesAsync();
        await ReplaceUserRolesAsync(user.MaNguoiDung, request.RoleIds);
        await EnsureRoleProfilesAsync(user.MaNguoiDung, request.RoleIds);

        var created = await LoadUserDtoAsync(user.MaNguoiDung);
        return CreatedAtAction(nameof(GetById), new { id = user.MaNguoiDung }, ApiResponseDto<UserListItemDto>.Ok(created!));
    }

    [HttpPut("{id:guid}")]
    [AuthorizeByPermission("USERS_EDIT")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequestDto request)
    {
        var user = await _db.Nguoidungs.FirstOrDefaultAsync(u => u.MaNguoiDung == id && u.DaXoa != true);
        if (user == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Khong tim thay nguoi dung.", "USER_NOT_FOUND"));
        }

        var validation = await ValidateUpdateAsync(id, request);
        if (validation != null)
        {
            return BadRequest(validation);
        }

        user.TenDangNhap = request.Username.Trim();
        user.HoTen = request.FullName.Trim();
        user.Email = NormalizeEmail(request.Email);
        user.LoaiTaiKhoan = request.AccountType;
        user.TrangThai = request.Active;
        user.ThoiGianSua = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        var updated = await LoadUserDtoAsync(id);
        return Ok(ApiResponseDto<UserListItemDto>.Ok(updated!));
    }

    [HttpDelete("{id:guid}")]
    [AuthorizeByPermission("USERS_DELETE")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await _db.Nguoidungs.FirstOrDefaultAsync(u => u.MaNguoiDung == id && u.DaXoa != true);
        if (user == null)
        {
            return NotFound(ApiResponseDto<object>.Fail("Khong tim thay nguoi dung.", "USER_NOT_FOUND"));
        }

        user.DaXoa = true;
        user.TrangThai = false;
        user.ThoiGianSua = DateTime.UtcNow;

        var mappings = await _db.Nguoidungvaitros.Where(r => r.MaNguoiDung == id && r.DaXoa != true).ToListAsync();
        foreach (var mapping in mappings)
        {
            mapping.DaXoa = true;
            mapping.TrangThai = false;
            mapping.ThoiGianSua = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();
        return Ok(ApiResponseDto<object>.Ok(new { id }, "Da xoa mem nguoi dung."));
    }

    [HttpPut("{id:guid}/roles")]
    [AuthorizeByPermission("USERS_EDIT")]
    public async Task<IActionResult> UpdateRoles(Guid id, [FromBody] UpdateUserRolesRequestDto request)
    {
        var exists = await _db.Nguoidungs.AnyAsync(u => u.MaNguoiDung == id && u.DaXoa != true);
        if (!exists)
        {
            return NotFound(ApiResponseDto<object>.Fail("Khong tim thay nguoi dung.", "USER_NOT_FOUND"));
        }

        var roleValidation = await ValidateRoleIdsAsync(request.RoleIds);
        if (roleValidation != null)
        {
            return BadRequest(roleValidation);
        }

        await ReplaceUserRolesAsync(id, request.RoleIds);
        await EnsureRoleProfilesAsync(id, request.RoleIds);

        var updated = await LoadUserDtoAsync(id);
        return Ok(ApiResponseDto<UserListItemDto>.Ok(updated!));
    }

    private async Task<ApiResponseDto<object>?> ValidateCreateAsync(CreateUserRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password) || string.IsNullOrWhiteSpace(request.FullName))
        {
            return ApiResponseDto<object>.Fail("Username, password va ho ten la bat buoc.", "USER_INVALID_INPUT");
        }

        if (await _db.Nguoidungs.AnyAsync(u => u.TenDangNhap == request.Username.Trim() && u.DaXoa != true))
        {
            return ApiResponseDto<object>.Fail("Username da ton tai.", "USER_USERNAME_EXISTS");
        }

        var email = NormalizeEmail(request.Email);
        if (!string.IsNullOrWhiteSpace(email) && await _db.Nguoidungs.AnyAsync(u => u.Email == email && u.DaXoa != true))
        {
            return ApiResponseDto<object>.Fail("Email da ton tai.", "USER_EMAIL_EXISTS");
        }

        return await ValidateRoleIdsAsync(request.RoleIds);
    }

    private async Task<ApiResponseDto<object>?> ValidateUpdateAsync(Guid id, UpdateUserRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.FullName))
        {
            return ApiResponseDto<object>.Fail("Username va ho ten la bat buoc.", "USER_INVALID_INPUT");
        }

        if (await _db.Nguoidungs.AnyAsync(u => u.MaNguoiDung != id && u.TenDangNhap == request.Username.Trim() && u.DaXoa != true))
        {
            return ApiResponseDto<object>.Fail("Username da ton tai.", "USER_USERNAME_EXISTS");
        }

        var email = NormalizeEmail(request.Email);
        if (!string.IsNullOrWhiteSpace(email) && await _db.Nguoidungs.AnyAsync(u => u.MaNguoiDung != id && u.Email == email && u.DaXoa != true))
        {
            return ApiResponseDto<object>.Fail("Email da ton tai.", "USER_EMAIL_EXISTS");
        }

        return null;
    }

    private async Task<ApiResponseDto<object>?> ValidateRoleIdsAsync(List<int> roleIds)
    {
        if (roleIds == null || roleIds.Count == 0)
        {
            return null;
        }

        var distinctIds = roleIds.Distinct().ToList();
        var existingCount = await _db.Vaitros.CountAsync(r => distinctIds.Contains(r.MaVaiTro) && r.DaXoa != true);
        return existingCount == distinctIds.Count
            ? null
            : ApiResponseDto<object>.Fail("Mot hoac nhieu role khong ton tai.", "ROLE_NOT_FOUND");
    }

    private async Task ReplaceUserRolesAsync(Guid userId, List<int> roleIds)
    {
        roleIds = roleIds?.Distinct().ToList() ?? new List<int>();
        var existing = await _db.Nguoidungvaitros.Where(r => r.MaNguoiDung == userId).ToListAsync();

        foreach (var mapping in existing)
        {
            var shouldActive = roleIds.Contains(mapping.MaVaiTro);
            mapping.DaXoa = !shouldActive;
            mapping.TrangThai = shouldActive;
            mapping.ThoiGianSua = DateTime.UtcNow;
        }

        var existingRoleIds = existing.Select(r => r.MaVaiTro).ToHashSet();
        foreach (var roleId in roleIds.Where(id => !existingRoleIds.Contains(id)))
        {
            _db.Nguoidungvaitros.Add(new Nguoidungvaitro
            {
                MaNguoiDung = userId,
                MaVaiTro = roleId,
                TrangThai = true,
                DaXoa = false,
                ThoiGianTao = DateTime.UtcNow
            });
        }

        await _db.SaveChangesAsync();
    }

    private async Task EnsureRoleProfilesAsync(Guid userId, List<int> roleIds)
    {
        var roleNames = await _db.Vaitros
            .Where(r => roleIds.Contains(r.MaVaiTro))
            .Select(r => r.TenVaiTro)
            .ToListAsync();

        if (roleNames.Contains("Giao_Vien") && !await _db.Giangviens.AnyAsync(x => x.MaNguoiDung == userId))
        {
            _db.Giangviens.Add(new Giangvien
            {
                MaGiangVien = Guid.NewGuid(),
                MaNguoiDung = userId,
                TrinhDoChuyenMon = "Chua cap nhat",
                TrangThai = true,
                DaXoa = false,
                ThoiGianTao = DateTime.UtcNow
            });
        }

        if (roleNames.Contains("Hoc_Sinh") && !await _db.Hocsinhs.AnyAsync(x => x.MaNguoiDung == userId))
        {
            _db.Hocsinhs.Add(new Hocsinh { MaHocSinh = Guid.NewGuid(), MaNguoiDung = userId, TrangThai = true, DaXoa = false, ThoiGianTao = DateTime.UtcNow });
        }

        if (roleNames.Contains("Phu_Huynh") && !await _db.Phuhuynhs.AnyAsync(x => x.MaNguoiDung == userId))
        {
            _db.Phuhuynhs.Add(new Phuhuynh
            {
                MaPhuHuynh = Guid.NewGuid(),
                MaNguoiDung = userId,
                SoDienThoai = "Chua cap nhat",
                TrangThai = true,
                DaXoa = false,
                ThoiGianTao = DateTime.UtcNow
            });
        }

        await _db.SaveChangesAsync();
    }

    private async Task<UserListItemDto?> LoadUserDtoAsync(Guid userId)
    {
        var user = await _db.Nguoidungs
            .Where(u => u.MaNguoiDung == userId && u.DaXoa != true)
            .Include(u => u.Hocsinh)
            .Include(u => u.Giangvien)
            .Include(u => u.Phuhuynh)
            .Include(u => u.Nguoidungvaitros.Where(r => r.DaXoa != true))
            .ThenInclude(r => r.MaVaiTroNavigation)
            .FirstOrDefaultAsync();

        return user == null ? null : ToDto(user);
    }

    private static UserListItemDto ToDto(Nguoidung user)
    {
        return new UserListItemDto
        {
            Id = user.MaNguoiDung,
            ProfileId = user.Hocsinh?.MaHocSinh ?? user.Giangvien?.MaGiangVien ?? user.Phuhuynh?.MaPhuHuynh,
            Username = user.TenDangNhap,
            FullName = user.HoTen,
            Email = user.Email,
            AccountType = user.LoaiTaiKhoan,
            Active = user.TrangThai != false,
            Roles = user.Nguoidungvaitros
                .Where(r => r.DaXoa != true)
                .Select(r => new RoleItemDto { Id = r.MaVaiTro, Name = r.MaVaiTroNavigation.TenVaiTro })
                .ToList()
        };
    }

    private static string? NormalizeEmail(string? email)
    {
        return string.IsNullOrWhiteSpace(email) ? null : email.Trim().ToLowerInvariant();
    }
}
