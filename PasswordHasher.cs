using System;
using System.Security.Cryptography;

namespace MyStore.Services
{
    /// <summary>
    /// PBKDF2-HMAC-SHA256: 16 byte salt + 16 byte derived key.
    /// Encoded as "P1$" + Base64(32 bytes) = 47 ASCII chars, fitting
    /// the teacher's dbo.User.Password NCHAR(50) schema.
    /// NCHAR pads with spaces, hence TrimEnd() on verification.
    /// This is a STUDY PROJECT compatibility format, not a substitute
    /// for a modern production identity system (e.g. ASP.NET Identity).
    /// </summary>
    public static class PasswordHasher
    {
        private const string Prefix = "P1$";
        private const int SaltBytes = 16;
        private const int KeyBytes = 16;
        private const int Iterations = 150000;

        public static string Hash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Mật khẩu không được để trống.", nameof(password));

            var salt = new byte[SaltBytes];
            using (var rng = RandomNumberGenerator.Create()) rng.GetBytes(salt);

            byte[] key;
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
                key = pbkdf2.GetBytes(KeyBytes);

            var output = new byte[SaltBytes + KeyBytes];
            Buffer.BlockCopy(salt, 0, output, 0, SaltBytes);
            Buffer.BlockCopy(key, 0, output, SaltBytes, KeyBytes);
            return Prefix + Convert.ToBase64String(output);
        }

        public static bool Verify(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash)) return false;
            storedHash = storedHash.TrimEnd(); // NCHAR(50) padding
            if (!storedHash.StartsWith(Prefix, StringComparison.Ordinal)) return false;

            try
            {
                var all = Convert.FromBase64String(storedHash.Substring(Prefix.Length));
                if (all.Length != SaltBytes + KeyBytes) return false;
                var salt = new byte[SaltBytes];
                Buffer.BlockCopy(all, 0, salt, 0, SaltBytes);
                byte[] expected;
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
                    expected = pbkdf2.GetBytes(KeyBytes);

                // Compare all key bytes, avoiding early return on a mismatched byte.
                int diff = 0;
                for (int i = 0; i < KeyBytes; i++) diff |= expected[i] ^ all[SaltBytes + i];
                return diff == 0;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
