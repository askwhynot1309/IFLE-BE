using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.Ultis
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 10000;

        public static (string Salt, string Hash) HashPassword(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                byte[] hash = rfc2898.GetBytes(HashSize);

                string saltString = BitConverter.ToString(salt).Replace("-", "");
                string hashString = BitConverter.ToString(hash).Replace("-", "");

                return (saltString, hashString);
            }
        }

        public static bool VerifyPassword(string password, string storedSaltString, string storedHashPassword)
        {
            byte[] storedSalt = ConvertStringToByteArray(storedSaltString);
            byte[] storedHash = ConvertStringToByteArray(storedHashPassword);

            using (var rfc2898 = new Rfc2898DeriveBytes(password, storedSalt, Iterations))
            {
                byte[] inputHash = rfc2898.GetBytes(HashSize);

                return inputHash.SequenceEqual(storedHash);
            }
        }

        private static byte[] ConvertStringToByteArray(string hexString)
        {
            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return bytes;
        }

        public static string GenerateRandomPassword()
        {
            int length = 12;
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
            char[] chars = new char[length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[length];
                rng.GetBytes(data);
                for (int i = 0; i < length; i++)
                {
                    chars[i] = validChars[data[i] % validChars.Length];
                }
            }
            return new string(chars);
        }
    }
}
