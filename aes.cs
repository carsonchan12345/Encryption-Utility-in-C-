using System;
using System.Text;
using System.Security.Cryptography;

namespace RandomName
{
    public static class RandomName
    {
        public static string Encrypt(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes("pemgail9uzpgzl88");
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(256 / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            byte[] cipherTextBytes;
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    memoryStream.Close();
                    cryptoStream.Close();
                }
            }
            return Convert.ToBase64String(cipherTextBytes);
        }
        public static string Decrypt(string cipherText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes("pemgail9uzpgzl88");
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(256 / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int byteCount = 0;
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(cipherTextBytes))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                    memoryStream.Close();
                    cryptoStream.Close();
                }
            }
            return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
        }
        //if -d flag is set, decrypt the string, else encrypt the string
        static void Main(string[] args)
        {
            string plainText = "";
            string passPhrase = "";
            bool decrypt = false;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-d")
                {
                    decrypt = true;
                }
                else if (args[i] == "-p")
                {
                    plainText = args[i + 1];
                }
                else if (args[i] == "-k")
                {
                    passPhrase = args[i + 1];
                }
            }
            if (decrypt)
            {
                Console.WriteLine(Decrypt(plainText, passPhrase));
            }
            else
            {
                Console.WriteLine(Encrypt(plainText, passPhrase));
            }
        }
    }
}


