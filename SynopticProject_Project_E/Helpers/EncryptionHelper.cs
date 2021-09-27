﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SynopticProject_Project_E.Helpers
{
    // https://www.c-sharpcorner.com/article/how-to-encrypt-and-decrypt-in-c-sharp-using-simple-aes-keys/
    public static class EncryptionHelper
    {
        public static string Encrypt(string input, string publicKey, string privateKey)
        {
            string output = "";
            try
            {
                byte[] publicKeyByte = Encoding.UTF8.GetBytes(ValidateKey(publicKey));
                byte[] privateKeyByte = Encoding.UTF8.GetBytes(ValidateKey(privateKey));

                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    byte[] inputByteArray = Encoding.UTF8.GetBytes(input);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(publicKeyByte, privateKeyByte), CryptoStreamMode.Write);

                        cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                        cryptoStream.FlushFinalBlock();

                        output = Convert.ToBase64String(memoryStream.ToArray());
                    }
                }

                return output;
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message, exc.InnerException);
            }
        }

        public static string Decrypt(string input, string publicKey, string privateKey)
        {
            string output = "";
            try
            {
                byte[] publicKeyByte = Encoding.UTF8.GetBytes(ValidateKey(publicKey));
                byte[] privateKeyByte = Encoding.UTF8.GetBytes(ValidateKey(privateKey));

                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    byte[] inputByteArray = new byte[input.Replace(" ", "+").Length];
                    inputByteArray = Convert.FromBase64String(input.Replace(" ", "+"));

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(publicKeyByte, privateKeyByte), CryptoStreamMode.Write);

                        cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                        cryptoStream.FlushFinalBlock();

                        output = Encoding.UTF8.GetString(memoryStream.ToArray());

                    }
                }

                return output;
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message, exc.InnerException);
            }
        }

        private static string ValidateKey(string key)
        {
            key = key.Replace(" ", "+");

            // Force to be 8 characters
            if (key.Length > 8)
            {
                key = key.Substring(0, 8);
            }
            else if (key.Length < 8)
            {
                for (int i = key.Length; i < 8; i++)
                {
                    key += "=";
                }
            }

            return key;
        }
    }
}
