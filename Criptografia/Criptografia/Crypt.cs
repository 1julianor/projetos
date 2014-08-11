using RSACryptoProvider;
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
       Security security;
       public byte[] EncryptUsingKey(byte[] conteudo, string nomeChave, string pathChave)
       {
           if(conteudo == null || nomeChave.Trim() == "" || pathChave.Trim() == "")
           {
               return Encoding.UTF8.GetBytes("Parametros incompletos");
           }
           return security.Encrypt(conteudo, nomeChave, pathChave);
       }       
    }
}
