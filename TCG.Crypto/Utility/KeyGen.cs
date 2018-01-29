using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TCG.Crypto
{
    /// <summary>
    /// M. Cassim
    /// Generates random key for crypto use
    /// </summary>
    public class KeyGen
    {
        private static RNGCryptoServiceProvider rng = null;

        public static string NewKey()
        {
            byte[] bytes = new byte[16];

            if (rng == null)
                rng = new RNGCryptoServiceProvider();

            rng.GetBytes(bytes);

            var key = Convert.ToBase64String(bytes);

            return key;
        }
    }
}
