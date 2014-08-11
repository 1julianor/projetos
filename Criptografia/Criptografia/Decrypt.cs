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
        public byte[] DecryptUsingKey(byte[] conteudo, string nomeChave, string pathChave)
        {
            security = new Security();
            return security.Decrypt(conteudo, pathChave, nomeChave);
        }

        
    }
}
