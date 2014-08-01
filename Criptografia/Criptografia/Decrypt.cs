using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Criptografia
{
    public class Decrypt
    {
        Security security;
        public byte[] Decrypt512(byte[] array, string senha)
        {
            security = new Security();
            return security.Decrypt(array, senha);
        }

        public string Decrypt512(string texto, string senha)
        {
            security = new Security();
            return security.Decrypt(texto, senha);
        }
    }
}
