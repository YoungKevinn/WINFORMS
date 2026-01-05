using System.Security.Cryptography;
using System.Text;

namespace API.Security
{
    public static class PasswordHasher
    {
        private const string Pepper = "hehe";

        public static string CreatePasswordHash(string password)
        {
            var input = Pepper + password;
            var bytes = Encoding.UTF8.GetBytes(input);

            using var md5 = MD5.Create();
            var hashBytes = md5.ComputeHash(bytes);

            return Convert.ToHexString(hashBytes);
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            var computed = CreatePasswordHash(password);
            return string.Equals(computed, storedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}
