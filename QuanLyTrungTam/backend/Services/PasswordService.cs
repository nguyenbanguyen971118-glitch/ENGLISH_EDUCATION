using backend.Services.Interfaces;

namespace backend.Services;

public class PasswordService : IPasswordService
{
    // - chuc nang: Bam mat khau plain text thanh bcrypt hash.
    // - nmkhue -29/2/2026
    public string HashPassword(string plainTextPassword)
    {
        return BCrypt.Net.BCrypt.HashPassword(plainTextPassword, workFactor: 12);
    }

    // - chuc nang: Xac thuc mat khau voi hash hoac plain text cho giai doan migration.
    // - nmkhue -29/2/2026
    public bool VerifyPassword(string plainTextPassword, string hashOrPlainText)
    {
        if (string.IsNullOrWhiteSpace(hashOrPlainText))
        {
            return false;
        }

        // Migration-safe: support old plain text rows until data is rehashed.
        if (hashOrPlainText.StartsWith("$2a$") || hashOrPlainText.StartsWith("$2b$") || hashOrPlainText.StartsWith("$2y$"))
        {
            return BCrypt.Net.BCrypt.Verify(plainTextPassword, hashOrPlainText);
        }

        return hashOrPlainText == plainTextPassword;
    }
}
