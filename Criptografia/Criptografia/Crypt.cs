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
           security = new Security();
           return security.Encrypt(array, senha);
       }

       public string Encrypt(string texto, string senha)
       {
           security = new Security();
           return security.Encrypt(texto, senha);
       }

        

       
       
    }
}
