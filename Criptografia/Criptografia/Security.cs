using RSACryptoProvider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Criptografia
{
    class Security
    {
        #region Private
        private static int _iterations = 2;
        private static int _keySize = 256;
        

        private static string _hash;
        private static string _salt = "4f45566b7377e1a4fc";
        private static string _vector = "8947az34awl34kjq";
        #endregion

        #region Construtor
        public Security(Hasher.HashProvider hashProvider = Hasher.HashProvider.SHA512)
        {
            
            switch (hashProvider) {
                case Hasher.HashProvider.MD5:
                    _hash = Hasher.HashProvider.MD5.ToString();
                    break;
                case Hasher.HashProvider.SHA1:
                    _hash = Hasher.HashProvider.SHA1.ToString();
                    break;
                case Hasher.HashProvider.SHA256:
                    _hash = Hasher.HashProvider.SHA256.ToString();
                    break;
                case Hasher.HashProvider.SHA384:
                    _hash = Hasher.HashProvider.SHA384.ToString();
                    break;
                case Hasher.HashProvider.SHA512:
                    _hash = Hasher.HashProvider.SHA512.ToString();
                    break;
            }
        }
        #endregion

        #region Public
        public byte[] Encrypt(byte[] valueBytes, string password)
        {
            return ASCIIEncoding.ASCII.GetBytes(Encrypt<AesManaged>(valueBytes, password));

        }

        public byte[] Encrypt(byte[] conteudo, string pathChave, string nomeChave)
        {
            return EncryptUsingKey(conteudo, pathChave, nomeChave);
        }

        public byte[] Decrypt(byte[] conteudo, string pathChave, string nomeChave) 
        {
            return DecryptUsingKey(conteudo, pathChave, nomeChave);
        }

        public byte[] Decrypt(byte[] valueBytes, string password)
        {
            return ASCIIEncoding.ASCII.GetBytes(Decrypt<AesManaged>(valueBytes, password));
        }
        #endregion

        #region Methods Private
         private byte[] EncryptUsingKey(byte[] conteudo, string pathChave, string nomeChave) 
         {
             KeyGenerator key = new KeyGenerator();
             string keyInfo = key.LoadKey(pathChave, nomeChave);
             RSAx rsa = new RSAx(keyInfo, GetSizeKey(nomeChave));
             byte[] CTX = rsa.Encrypt(Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(conteudo)), true);
             string CipherText = Convert.ToBase64String(CTX);
             return Encoding.UTF8.GetBytes(CipherText);
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

        private byte[] DecryptUsingKey(byte[] conteudo, string pathChave, string nomeChave) 
        {
            KeyGenerator key = new KeyGenerator();
            string keyInfo = key.LoadKey(pathChave, nomeChave);
            RSAx rsa = new RSAx(keyInfo, GetSizeKey(nomeChave));
            byte[] ETX = Convert.FromBase64String(Encoding.UTF8.GetString(conteudo));
            byte[] PTX = rsa.Decrypt(ETX, true);
            string DecryptedString = Encoding.UTF8.GetString(PTX);
            return Encoding.UTF8.GetBytes(DecryptedString);
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
        #endregion

        #region Utilitarios
        private int GetSizeKey(string nameKey) 
        {
            string[] split = nameKey.Split('#');
            return int.Parse(split[split.Length - 2]);
        }
        #endregion
    }
}
