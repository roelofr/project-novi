using System;
using System.Security.Cryptography;
using System.Text;

namespace News
{
    internal class CacheCrypto
    {
        /// <summary>
        ///     Create a Sha1 hasher for our URLs
        /// </summary>
        private static readonly SHA1 Sha1 = SHA1.Create();

        /// <summary>
        ///     Calculates the hash from a string
        /// </summary>
        /// <param name="toHash"></param>
        /// <returns></returns>
        protected static string GetHash(String toHash)
        {
            try
            {
                return BitConverter.ToString(Sha1.ComputeHash(Encoding.UTF8.GetBytes(toHash)));
            }
            catch
            {
                return null;
            }
        }
    }
}