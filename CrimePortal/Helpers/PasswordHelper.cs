using Microsoft.AspNetCore.Identity;

namespace CrimePortal.Helpers
{
    public static class PasswordHelper
    {
        private static PasswordHasher<string> hasher = new();

        public static string Hash(string password)
        {
            return hasher.HashPassword(null, password);
        }

        public static bool Verify(string storedPassword, string inputPassword)
        {
            if (string.IsNullOrWhiteSpace(storedPassword))
                return false;

            // 🔥 SAFE CHECK – plain text असेल तर direct compare
            if (!storedPassword.StartsWith("AQAAAA")) // Identity hash prefix
            {
                return storedPassword == inputPassword;
            }

            return hasher.VerifyHashedPassword(
                null,
                storedPassword,
                inputPassword
            ) == PasswordVerificationResult.Success;
        }
    }
}
