using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCG.Crypto
{
    public class PasswordManager
    {
        public static string encrypt(string password)
        {
           return SimpleHash.ComputeHash(password, SimpleHashAlgorithm.SHA512, null);
        }

        public static bool verify(string password, string passwordHashed)
        {
            return SimpleHash.VerifyHash(password, SimpleHashAlgorithm.SHA512, passwordHashed);
        }
    }
}
