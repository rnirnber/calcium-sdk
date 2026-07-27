using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CalciumSDK
{
    public static partial class Helpers
    {
        public static string GetDigest(List<byte> bytes)
        {
            var ret = "";
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] sig_bytes = sha512.ComputeHash(bytes.ToArray());
                string hash_str = BitConverter.ToString(sig_bytes).Replace("-", "").ToLowerInvariant();
                ret = hash_str;
            }
            return ret;
        }
        public static string GetDigest(byte[] bytes)
        {
            return GetDigest(bytes.ToList());
        }
    }
    internal class GetDigest
    {
    }
}
