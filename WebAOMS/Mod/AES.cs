using System;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace WebAOMS.AES
{
    public class AES
    {
        private readonly Encoding encoding;

        private SicBlockCipher mode;


        public AES(Encoding encoding)
        {
            this.encoding = encoding;
            this.mode = new SicBlockCipher(new AesFastEngine());
        }

        public static string ByteArrayToString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", string.Empty);
        }

        public static byte[] StringToByteArray(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];

            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }


        public string Encrypt(string plain, byte[] key, byte[] iv)
        {
            byte[] input = this.encoding.GetBytes(plain);

            byte[] bytes = this.BouncyCastleCrypto(true, input, key, iv);

            string result = ByteArrayToString(bytes);

            return result;
        }


        public string Decrypt(string cipher, byte[] key, byte[] iv)
        {
            byte[] bytes = this.BouncyCastleCrypto(false, StringToByteArray(cipher), key, iv);

            string result = this.encoding.GetString(bytes);

            return result;
        }


        private byte[] BouncyCastleCrypto(bool forEncrypt, byte[] input, byte[] key, byte[] iv)
        {
            try
            {
                this.mode.Init(forEncrypt, new ParametersWithIV(new KeyParameter(key), iv));

                BufferedBlockCipher cipher = new BufferedBlockCipher(this.mode);

                return cipher.DoFinal(input);
            }
            catch (CryptoException)
            {
                throw;
            }
        }
    }
    public static class ShortEncryptDecrypt
    {
        // This is the extension method.
        // The first parameter takes the "this" modifier
        // and specifies the type for which the method is defined.
        private static byte[] key = new byte[16] { 5, 9, 3, 3, 5, 4, 7, 2, 9, 0, 1, 6, 6, 4, 9, 6 };
        private static byte[] iv = new byte[16] { 7, 2, 6, 4, 9, 6, 7, 8, 9, 0, 5, 2, 3, 3, 5, 7 };
        public static string SEncrypt(this string str)
        {
            AES aes = new AES(System.Text.Encoding.UTF8);
            return aes.Encrypt(str,key,iv);
        }

        public static string SDecrypt(this string str)
        {
            AES aes = new AES(System.Text.Encoding.UTF8);
            return aes.Decrypt(str, key, iv);
        }
    }


}