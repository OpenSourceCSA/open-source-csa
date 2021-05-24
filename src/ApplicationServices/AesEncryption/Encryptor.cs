using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace ApplicationServices.AesEncryption
{
    public static class   Encryptor
    {
        private static int _aesSize = 128;

        /// <summary>
        /// Encrypt and store IV in first 128 bits (16 bytes) of encrypted bytes.
        /// </summary>
        /// <param name="input">Plain text to encrypt.</param>
        /// <param name="key">Aes Key</param>
        /// <param name="iv">Aes IV</param>
        /// <returns></returns>
        public static string EncryptToBase64(string input, byte[] key, byte[] iv)
        {
            // encrypt plain text
            var bytes = EncryptStringToBytes_Aes(input, key, iv);

            // prepend cipher text with the IV.
            IEnumerable<byte> withIv = iv.Concat(bytes);
            
            // convert to base64 string for output
            var encrypted = Convert.ToBase64String(withIv.ToArray());
            return encrypted;
        }

        /// <summary>
        /// Decrypt text that has IV stored in the cipher text
        /// </summary>
        /// <param name="input">Cipher text.</param>
        /// <param name="key">Aes Key used to encrypt data.</param>
        /// <returns></returns>
        public static string DecryptFromBase64(string input, byte[] key)
        {
            
            var bytes = Convert.FromBase64String(input);
            var bits = _aesSize / 8;
            
            // get IV from first n bits.
            byte[] iv = bytes.Take(bits).ToArray();

            // get cipher text from the remainder
            byte[] cipherText = bytes.Skip(bits).Take(bytes.Length - bits).ToArray();

            // decrypt it
            var plainText = DecryptStringFromBytes_Aes(cipherText, key, iv);
            
            return plainText;
        }

        private static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }

        private static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}