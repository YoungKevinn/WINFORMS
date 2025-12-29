using System.Security.Cryptography;
using System.Text;

namespace API.Security
{
    public static class PasswordHasher
    {
        private const string Pepper = "hehe";

        // Tạo hash từ password
        public static string CreatePasswordHash(string password)
        {
            // Gộp pepper + password
            var input = Pepper + password;
            var bytes = Encoding.UTF8.GetBytes(input);

            using var md5 = MD5.Create();
            var hashBytes = md5.ComputeHash(bytes);

            // Trả về dạng hex cho dễ nhìn (VD: "5F4DCC3B5AA765D61D8327DEB882CF99")
            return Convert.ToHexString(hashBytes);
        }

        // So sánh password nhập vào với hash đã lưu
        public static bool VerifyPassword(string password, string storedHash)
        {
            var computed = CreatePasswordHash(password);
            // có thể ToUpperInvariant() để chắc kèo
            return string.Equals(computed, storedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}
