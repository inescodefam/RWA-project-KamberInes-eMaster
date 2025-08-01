﻿using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace eProfessional.BLL.Auth
{
    public class HashPwd
    {
        public static string GetSalt()
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            string b64Salt = Convert.ToBase64String(salt);

            return b64Salt;
        }

        public static string GetHash(string password, string b64salt)
        {
            byte[] salt = Convert.FromBase64String(b64salt);

            byte[] hash =
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8);
            string b64Hash = Convert.ToBase64String(hash);

            return b64Hash;
        }
    }
}
