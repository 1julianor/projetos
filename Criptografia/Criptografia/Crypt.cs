using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Criptografia
{
    public class Crypt
    {
       public Security security;
       public byte[] Encrypt(byte[] array, string senha) 
       {
           security = new Security(Security.HashProvider.SHA512);
           return null;
       }

        /// <summary>
        /// Creates a random salt to add to a password.
        /// </summary>
        /// <returns></returns>
        public static string CreateSalt()
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] number = new byte[32];
            rng.GetBytes(number);
            return Convert.ToBase64String(number);
        }

       
       
    }
}
