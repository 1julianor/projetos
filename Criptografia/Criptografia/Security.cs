using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Criptografia
{
    public class Security
    {
        HashAlgorithm hasher = null;

        public enum HashProvider
        {
            SHA1,
            SHA256,
            SHA384,
            SHA512,
            MD5
        }

        #region Privado
        private HashAlgorithm _algorithm;
        #endregion
        /// <summary>
        /// Cria hash dos dados inseridos.
        /// </summary>
        #region Construtor
        public Security(HashProvider hashProvider = HashProvider.SHA1)
        {
            switch (hashProvider) {
                case HashProvider.MD5:
                    _algorithm = new MD5CryptoServiceProvider();
                    break;
                case HashProvider.SHA1:
                    _algorithm = new SHA1Managed();
                    break;
                case HashProvider.SHA256:
                    _algorithm = new SHA256Managed();
                    break;
                case HashProvider.SHA384:
                    _algorithm = new SHA384Managed();
                    break;
                case HashProvider.SHA512:
                    _algorithm = new SHA512Managed();
                    break;
            }
        }
        #endregion

        #region Publico
        /// <summary>
        /// Monta hash para algum dado texto.
        /// </summary>
        /// <param name="plainText">Texto a ser criado o hash.</param>
        /// <returns>Hash do texto inserido.</returns>
        public string GetHash(string plainText)
        {
            byte[] cryptoByte = _algorithm.ComputeHash(ASCIIEncoding.ASCII.GetBytes(plainText));

            return Convert.ToBase64String(cryptoByte, 0, cryptoByte.Length);
        }
        /// <summary>
        /// Cria hash para array de bytes.
        /// </summary>
        /// <param name="byte">Array a ser criado o hash.</param>
        /// <returns>Hash do Array de byte inserido.</returns>
        public string GetHash(byte[] byteArray)
        {
            byte[] cryptoByte;
            cryptoByte = _algorithm.ComputeHash(byteArray);
            return Convert.ToBase64String(cryptoByte, 0, cryptoByte.Length);
        }
        #endregion
    }
}
