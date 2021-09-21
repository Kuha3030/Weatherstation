using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MVC_Sasema_test.Controllers
{
    public class IPHashing
    {
        public static byte[] GetHash(string ipAsString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(ipAsString));
        }

        public static string GetHashString(string userIP)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(userIP))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}