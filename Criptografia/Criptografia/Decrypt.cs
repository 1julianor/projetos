using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Criptografia
{
    class Decrypt
    {
        public Security security;
        public byte[] DecryptArrayBite(byte[] array, string senha)
        {
            security = new Security(Security.HashProvider.SHA512);
            return null;
        }
    }
}
