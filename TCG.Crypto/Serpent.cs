using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Paddings;

namespace TCG.Crypto
{
    public class Serpent
    {
        public static void test()
        {
            string plain = "4242-4242-4242-4242";
            string key = KeyGen.NewKey();

            string data_encrypted = encrypt(plain, key);

            string data_decrypted = decrypt(data_encrypted, key);
        }

        public static string encrypt(string plain, string key)
        {

            BCEngine bcEngine = new BCEngine(new SerpentEngine(), Encoding.ASCII);
            bcEngine.SetPadding(new Pkcs7Padding());
            return bcEngine.Encrypt(plain, key);
        }

        public static string decrypt(string cipher, string key)
        {
            BCEngine bcEngine = new BCEngine(new SerpentEngine(), Encoding.ASCII);
            bcEngine.SetPadding(new Pkcs7Padding());
            return bcEngine.Decrypt(cipher, key);
        }
    }
}
