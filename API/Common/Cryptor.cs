
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Linq;
using System.Text;

namespace API.Common
{
    public static class Cryptor
    {
        static public bool VerifyPasswordHash(string password, byte[] storedHash)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            var computedHash = CalculateHashOfPassword(password);
            return computedHash.SequenceEqual(storedHash);
        }
        static public byte[] CalculateHashOfPassword(string password)
        {
            var hashed = KeyDerivation.Pbkdf2(
                password,
                GenerateSalt(),
                KeyDerivationPrf.HMACSHA256,
                10000,
                256 / 8);

            var s = Convert.ToString(hashed);
            return hashed;
        }

        static public byte[] GenerateSalt()
        {
            return Encoding.UTF8.GetBytes("THIS_IS_SALT");
        }
    }
}
