using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Criptografia
{
    public class Security
    {
        private static int _iterations = 2;
        private static int _keySize = 256;

        private static string _hash = "SHA1";
        private static string _salt = ""; // Random
        private static string _vector = "8947az34awl34kjq"; // Random

        public enum HashProvider
        {
            MD5,
            SHA1,
            SHA256,
            SHA384,
            SHA512
        }

        #region Privado
        private HashAlgorithm _algorithm;
        #endregion
        /// <summary>
        /// Cria hash dos dados inseridos.
        /// </summary>
        #region Construtor
        public Security(HashProvider hashProvider = HashProvider.SHA512)
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

        #region Settings

        

        #endregion

        public byte[] Encrypt(byte[] valueBytes, string password)
        {
            _salt = CreateSalt();
            return ASCIIEncoding.ASCII.GetBytes(Encrypt<AesManaged>(valueBytes, password));
        }

        public string Encrypt(string value, string password)
        {
            _salt = CreateSalt();
            byte[] valueBytes = ASCIIEncoding.ASCII.GetBytes(value);
            return Encrypt<AesManaged>(valueBytes, password);
        }

        private string Encrypt<T>(byte[] valueBytes, string password)
                where T : SymmetricAlgorithm, new()
        {
            byte[] vectorBytes = ASCIIEncoding.ASCII.GetBytes(_vector);
            byte[] saltBytes = ASCIIEncoding.ASCII.GetBytes(_salt);
            

            byte[] encrypted;
            using (T cipher = new T())
            {
                PasswordDeriveBytes _passwordBytes =
                    new PasswordDeriveBytes(password, saltBytes, _hash, _iterations);
                byte[] keyBytes = _passwordBytes.GetBytes(_keySize / 8);

                cipher.Mode = CipherMode.CBC;

                using (ICryptoTransform encryptor = cipher.CreateEncryptor(keyBytes, vectorBytes))
                {
                    using (MemoryStream to = new MemoryStream())
                    {
                        using (CryptoStream writer = new CryptoStream(to, encryptor, CryptoStreamMode.Write))
                        {
                            writer.Write(valueBytes, 0, valueBytes.Length);
                            writer.FlushFinalBlock();
                            encrypted = to.ToArray();
                        }
                    }
                }
                cipher.Clear();
            }
            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(byte[] valueBytes, string password)
        {
            _salt = CreateSalt();
            return Decrypt<AesManaged>(valueBytes, password);
        }

        public string Decrypt(string value, string password)
        {
            _salt = CreateSalt();
            byte[] valueBytes = Convert.FromBase64String(value);
            return Decrypt<AesManaged>(valueBytes, password);
        }

        private string Decrypt<T>(byte[] valueBytes, string password) where T : SymmetricAlgorithm, new()
        {
            byte[] vectorBytes = ASCIIEncoding.ASCII.GetBytes(_vector);
            byte[] saltBytes = ASCIIEncoding.ASCII.GetBytes(_salt);
            

            byte[] decrypted;
            int decryptedByteCount = 0;

            using (T cipher = new T())
            {
                PasswordDeriveBytes _passwordBytes = new PasswordDeriveBytes(password, saltBytes, _hash, _iterations);
                byte[] keyBytes = _passwordBytes.GetBytes(_keySize / 8);

                cipher.Mode = CipherMode.CBC;

                try
                {
                    using (ICryptoTransform decryptor = cipher.CreateDecryptor(keyBytes, vectorBytes))
                    {
                        using (MemoryStream from = new MemoryStream(valueBytes))
                        {
                            using (CryptoStream reader = new CryptoStream(from, decryptor, CryptoStreamMode.Read))
                            {
                                decrypted = new byte[valueBytes.Length];
                                decryptedByteCount = reader.Read(decrypted, 0, decrypted.Length);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return String.Empty;
                }

                cipher.Clear();
            }
            return Encoding.UTF8.GetString(decrypted, 0, decryptedByteCount);
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
