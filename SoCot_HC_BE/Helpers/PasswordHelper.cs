using System.Security.Cryptography;
using System.Text;

namespace SoCot_HC_BE.Helpers
{
    public class PasswordHelper
    {

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);

                // Convert to hex string
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hash)
                    builder.Append(b.ToString("x2"));

                return builder.ToString();
            }
        }

        // Compare input password with stored hashed password
        public static bool VerifyPassword(string inputPassword, string storedHashedPassword)
        {
            var hashedInput = HashPassword(inputPassword);
            return string.Equals(hashedInput, storedHashedPassword, StringComparison.OrdinalIgnoreCase);
        }
    }
}
