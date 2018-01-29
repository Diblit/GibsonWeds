using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCG.Crypto
{
    public class HashCreator
    {
        public static string genHashFromGuid(string prefix, int? limit = null)
        {
            string plainText = prefix + Guid.NewGuid().ToString();
            string hash = SimpleHash.ComputeHash(plainText, SimpleHashAlgorithm.MD5, null);

            if (limit.HasValue && limit > 0)
                if (hash.Length > 50)
                    hash = hash.Remove(49);
            return hash;
        }

        /// <summary>
        /// Generates URL Safe Hash From GUID
        /// </summary>
        /// <param name="prefix">Salt Value</param>
        /// <param name="limit">The maximum amount of characters permitted</param>
        /// <returns>A URL Safe Hash Value</returns>
        /// <author>MI Laher 2014-06-25</author>
        public static string genUrlSafeHashFromGuid(string prefix, int? limit = null)
        {
            //get hash as per normal
            string plainText = prefix + Guid.NewGuid().ToString();
            string hash = SimpleHash.ComputeHash(plainText, SimpleHashAlgorithm.MD5, null);

            if (limit.HasValue && limit > 0)
                if (hash.Length > 50)
                    hash = hash.Remove(49);

            //list of whitelisted characters
            var allowedCharsLetters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var allowedCharsNumbers = "0123456789";
            var allowedCharsSpecial = @"-_.,()[]{}#@";
            var allowedChars = allowedCharsLetters + allowedCharsNumbers + allowedCharsSpecial;

            Random r = new Random(); //a random number generator
            var hashCharArray = hash.ToCharArray(); //get char array version of hash (non read only)

            //loop through characters to identify and replace illegal characters
            for (int i = 0; i < hashCharArray.Length; i++)
            {
                var character = hashCharArray[i];

                if (!allowedChars.Contains(character)) //if illegal character found replace with any whitelisted character
                {
                    int rIdx = r.Next(0, allowedChars.Length - 1);
                    character = allowedChars[rIdx];
                }

                if (i == hashCharArray.Length - 1)
                {
                    if (allowedCharsSpecial.Contains(character))
                    {
                        int rIdx = r.Next(0, allowedCharsLetters.Length - 1);
                        character = allowedChars[rIdx];
                    }
                }

                hashCharArray[i] = character;
            }

            
            hash = new string(hashCharArray);
            hash.Trim();            

            return hash;
        }

        public static string genHash(string UniqueValue)
        {
            string plainText = UniqueValue;
            string hash = SimpleHash.ComputeHash(plainText, SimpleHashAlgorithm.MD5, null);
            return hash;
        }

        public static string getUserVerifyEmailHash()
        {
            string plainText = "ve" + Guid.NewGuid().ToString();
            string hash = SimpleHash.ComputeHash(plainText, SimpleHashAlgorithm.MD5, null);

            if (hash.Length > 50)
                hash = hash.Remove(49);
            return hash;
        }

        public static string getCustomerHash()
        {
            string plainText = "cust" + Guid.NewGuid().ToString();
            string hash = SimpleHash.ComputeHash(plainText, SimpleHashAlgorithm.MD5, null);

            if (hash.Length > 50)
                hash = hash.Remove(49);
            return hash;
        }

        /// <summary>
        /// Gets a 50 Character hash value
        /// </summary>
        /// <returns>a unique string</returns>
        /// <author>MI Laher</author>
        /// <created>2013-12-08</created>
        public static string getHashForURL()
        {
            return genUrlSafeHashFromGuid("ts", 50);
        }

        /// <summary>
        /// Gets a 20 Character hash2 value 
        /// </summary>
        /// <returns>a unique string</returns>
        /// <author>MI Laher</author>
        /// <created>2013-12-08</created>
        public static string getGetHash2ForURL()
        {
            return genUrlSafeHashFromGuid("ts2", 20);
        }

        public static bool verifyHash(string UniqueValue, string UniqueValueHashed)
        {
            return SimpleHash.VerifyHash(UniqueValue, SimpleHashAlgorithm.MD5, UniqueValueHashed);
        }
    }
}
