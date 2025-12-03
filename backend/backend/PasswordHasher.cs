using System.Security.Cryptography;

namespace backend.Services
{
    public static class PasswordHasher
    {
        // PBKDF2 with SHA256, 100k iterations
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            passwordSalt = RandomNumberGenerator.GetBytes(16);
            using var pbkdf2 = new Rfc2898DeriveBytes(password, passwordSalt, 100_000, HashAlgorithmName.SHA256);
            passwordHash = pbkdf2.GetBytes(32);
        }

        public static bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(password, passwordSalt, 100_000, HashAlgorithmName.SHA256);
            var computed = pbkdf2.GetBytes(32);
            return CryptographicOperations.FixedTimeEquals(computed, passwordHash);
        }
    }
}