using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;
using System.IO;

using Asmodat.Extensions.Objects;
using Asmodat.Debugging;

namespace Asmodat.Cryptography
{
    public class AES256
    {
        public byte[] AES_Encrypt(byte[] bytes, byte[] password)
        {
           
            byte[] encrypted = null;
            byte[] salt = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            using (MemoryStream stream = new MemoryStream())
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    
                    //aes.KeySize = 256;
                    //aes.BlockSize = 128;
                    

                    aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                    aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;

                    var key = new Rfc2898DeriveBytes(password, salt, 1024);
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = key.GetBytes(aes.BlockSize / 8);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.Zeros;

                    using (var crypto = new CryptoStream(stream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        crypto.Write(bytes, 0, bytes.Length);
                        crypto.Close();
                    }

                    encrypted = stream.ToArray();
                }
            }
            
            return encrypted;
        }

        public byte[] AES_Decrypt(byte[] bytes, byte[] password)
        {
            byte[] decrypted = null;
            byte[] salt = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            using (MemoryStream stream = new MemoryStream())
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    //aes.KeySize = 256;
                    //aes.BlockSize = 128;
                    aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                    aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;

                    var key = new Rfc2898DeriveBytes(password, salt, 1024);
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = key.GetBytes(aes.BlockSize / 8);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.Zeros;

                    using (var crypto = new CryptoStream(stream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        crypto.Write(bytes, 0, bytes.Length);
                        crypto.Close();
                    }

                    decrypted = stream.ToArray();
                }
            }

            return decrypted;
        }

        public string Encrypt(string str, string pwd)
        {
            str = str.LengthEncode();

            byte[] bytes = Encoding.UTF8.GetBytes(str);
            byte[] encrypted = null;
            byte[] password = Encoding.UTF8.GetBytes(pwd);

            password = SHA256.Create().ComputeHash(password);
            byte[] saltBytes = GetRandomBytes();

            byte[] result = new byte[saltBytes.Length + bytes.Length];
            for (int i = 0; i < saltBytes.Length; i++) result[i] = saltBytes[i];
            for (int i = 0; i < bytes.Length; i++) result[i + saltBytes.Length] = bytes[i];

            try
            {
                encrypted = AES_Encrypt(result, password);
            }
            catch(Exception ex)
            {
                ex.ToOutput();
                return null;
            }

            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string str, string pwd)
        {
            

            byte[] bytes = Convert.FromBase64String(str);
            byte[] password = Encoding.UTF8.GetBytes(pwd);
            byte[] decrypted;

            try
            {
                password = SHA256.Create().ComputeHash(password);
                decrypted = AES_Decrypt(bytes, password);
            }
            catch (Exception ex)
            {
                ex.ToOutput();
                return null;
            }

            byte[] result = new byte[bytes.Length - SaltSize];
            for (int i = SaltSize; i < decrypted.Length; i++)
                result[i - SaltSize] = decrypted[i];

            string output = Encoding.UTF8.GetString(result);

            output = output.LengthDecode();

            return output;

        }

        public int SaltSize { get; private set; } = 4;

        public byte[] GetRandomBytes()
        {
            byte[] bytes = new byte[SaltSize];
            RNGCryptoServiceProvider.Create().GetBytes(bytes);
            return bytes;
        }

    }
}
