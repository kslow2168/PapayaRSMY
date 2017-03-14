using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using OpenNETCF.Security.Cryptography;
using System.Text.RegularExpressions;

namespace PapayaX2.Helpers
{
    public class UtilitiesHelper
    {
        public static string appendToLength(string baseString, int length, string toBeAppended)
        {
            while (baseString.Length < length)
            {
                baseString += toBeAppended;
            }
            return baseString;
        }

        public static string ToAlphaNumericOnly(string input)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            return rgx.Replace(input, "");
        }

        public static string ToAlphaOnly(string input)
        {
            Regex rgx = new Regex("[^a-zA-Z]");
            return rgx.Replace(input, "");
        }

        public static string ToNumericOnly(string input)
        {
            Regex rgx = new Regex("[^0-9]");
            return rgx.Replace(input, "");
        }

        #region Crypto

        private const string passwordPhrase = "papaya"; // can be any string
        private const string salt = "p@p@y@"; // can be any string but 8 bytes long
        private const int passwordIterations = 2; // can be any number but 2 is enough
        private const string initializationVector = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
        private const int keySize = 256; // can be 192 or 128 or 256

        public static string Encrypt(string stringToEncrypt)
        {
            try
            {
                // Convert strings into byte arrays.
                // Let us assume that strings only contain ASCII codes.
                // If strings include Unicode characters, use Unicode, UTF7, or UTF8 encoding.
                Byte[] initializationVectorBytes = Encoding.ASCII.GetBytes(initializationVector);
                Byte[] saltBytes = Encoding.ASCII.GetBytes(salt);

                // Convert our plaintext into a byte array.
                // Let us assume that plaintext contains UTF8-encoded characters.
                Byte[] stringToEncryptBytes = Encoding.UTF8.GetBytes(stringToEncrypt);

                // First, we must create a password, from which the key will be derived.
                // This password will be generated from the specified passphrase and 
                // salt value. The password will be created using the specified hash 
                // algorithm. Password creation can be done in several iterations.
                OpenNETCF.Security.Cryptography.PasswordDeriveBytes keyGenerator = new OpenNETCF.Security.Cryptography.PasswordDeriveBytes(passwordPhrase, saltBytes, "SHA1", passwordIterations);
                //PasswordDeriveBytes keyGenerator = new PasswordDeriveBytes(passwordPhrase, saltBytes, "SHA1", passwordIterations);

                // Use the password to generate pseudo-random bytes for the encryption
                // key. Specify the size of the key in bytes (instead of bits).
                Byte[] keyBytes = keyGenerator.GetBytes(keySize / 8);

                // Create initialized Rijndael encryption object.
                RijndaelManaged symmetricKey = new RijndaelManaged();

                // It is reasonable to set encryption mode to Cipher Block Chaining
                // (CBC). Use default options for other symmetric key parameters.
                symmetricKey.Mode = CipherMode.CBC;

                // Generate encryptor from the existing key bytes and initialization 
                // vector. Key size will be defined based on the number of the key bytes.
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initializationVectorBytes);

                // Define memory stream which will be used to hold encrypted data.
                MemoryStream memoryStream = new MemoryStream();

                // Define cryptographic stream (always use Write mode for encryption).
                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

                // Start encrypting.
                cryptoStream.Write(stringToEncryptBytes, 0, stringToEncryptBytes.Length);

                // Finish encrypting.
                cryptoStream.FlushFinalBlock();

                // Convert our encrypted data from a memory stream into a byte array.

                Byte[] cipherTextBytes = memoryStream.ToArray();

                // Close both streams.
                memoryStream.Close();
                cryptoStream.Close();

                // Convert encrypted data into a base64-encoded string.
                string cipherText = Convert.ToBase64String(cipherTextBytes);

                // Return encrypted string.
                return cipherText;
            }
            catch (Exception)
            {
                //global::System.Windows.Forms.MessageBox.Show("Error on Functions.Encrypt(): " + ex.Message);
                return null;
            }
        }

        public static string Decrypt(string stringToDecrypt)
        {
            try
            {
                // Convert strings defining encryption key characteristics into byte
                // arrays. Let us assume that strings only contain ASCII codes.
                // If strings include Unicode characters, use Unicode, UTF7, or UTF8 encoding.
                Byte[] initializationVectorBytes = Encoding.ASCII.GetBytes(initializationVector);
                Byte[] saltBytes = Encoding.ASCII.GetBytes(salt);

                // Convert our ciphertext into a byte array.
                Byte[] cipherTextBytes = Convert.FromBase64String(stringToDecrypt);

                // First, we must create a password, from which the key will be 
                // derived. This password will be generated from the specified 
                // passphrase and salt value. The password will be created using
                // the specified hash algorithm. Password creation can be done in several iterations.
                OpenNETCF.Security.Cryptography.PasswordDeriveBytes keyGenerator = new OpenNETCF.Security.Cryptography.PasswordDeriveBytes(passwordPhrase, saltBytes, "SHA1", passwordIterations);
                //PasswordDeriveBytes keyGenerator = new PasswordDeriveBytes(passwordPhrase, saltBytes, "SHA1", passwordIterations);

                // Use the password to generate pseudo-random bytes for the encryption
                // key. Specify the size of the key in bytes (instead of bits).
                Byte[] keyBytes = keyGenerator.GetBytes(keySize / 8);

                // Create initialized Rijndael encryption object.
                RijndaelManaged symmetricKey = new RijndaelManaged();

                // It is reasonable to set encryption mode to Cipher Block Chaining
                // (CBC). Use default options for other symmetric key parameters.
                symmetricKey.Mode = CipherMode.CBC;

                // Generate decryptor from the existing key bytes and initialization 
                // vector. Key size will be defined based on the number of the key bytes.
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initializationVectorBytes);

                // Define memory stream which will be used to hold encrypted data.
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

                // Define memory stream which will be used to hold encrypted data.
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                // Since at this point we don't know what the size of decrypted data
                // will be, allocate the buffer long enough to hold ciphertext;
                // plaintext is never longer than ciphertext.
                Byte[] plainTextBytes = new Byte[cipherTextBytes.Length];

                // Start decrypting.
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                // Close both streams.
                memoryStream.Close();
                cryptoStream.Close();

                // Convert decrypted data into a string. 
                // Let us assume that the original plaintext string was UTF8-encoded.
                string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

                // Return decrypted string.
                return plainText;
            }
            catch (Exception)
            {
                //System.Windows.Forms.MessageBox.Show("Error on Functions.Decrypt(): " + ex.Message);
                return null;
            }
        }

        #endregion
    }
}