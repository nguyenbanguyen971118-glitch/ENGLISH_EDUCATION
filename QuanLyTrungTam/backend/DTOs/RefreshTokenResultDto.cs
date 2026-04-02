namespace backend.DTOs;


/// <summary>
/// Dto chuan cho ket qua refresh token, gom co cac token moi va thong tin session.
/// </summary>
//nmkhue - Ngay 29/3/2026
public class RefreshTokenResultDto
{
    public string Token { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string SessionId { get; set; } = string.Empty;
}
