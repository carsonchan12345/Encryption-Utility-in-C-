using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace RandomName
{
    
    public static class RandomName
    {

        //AES CBC encryption and decryption. Decryption with byte input and byte return. Encryption with byte input return output in byte.
        public static byte[] Encrypt(byte[] plainTextBytes, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes("pemgail9uzpgzl88");
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
            return cipherTextBytes;
        }
        public static byte[] Decrypt(byte[] cipherTextBytes, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes("pemgail9uzpgzl88");
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
            return plainTextBytes;
        }
        //take byte array and write to file
        public static void writeLocalFilePath(string filePath, byte[] buffer, FileMode fileMode)
        {
            using (FileStream fs = new FileStream(filePath, fileMode, FileAccess.Write))
            {
                fs.Write(buffer, 0, buffer.Length);
            }
        }
        //read file in byte array and return it
        public static byte[] readLocalFilePath(string filePath, FileMode fileMode)
        {
            byte[] buffer = null;
            using (FileStream fs = new FileStream(filePath, fileMode, FileAccess.Read))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
            }
            return buffer;
        }
        public static void help()
        {
            Console.WriteLine("Example: aesbyte.exe -k passPhrase -p inputFilePath -o outputFilePath");

        }
        static void Main(string[] args)
        {
            string passPhrase = "";
            string inputfilePath = "";
            string outputfilePath = "";
            bool decrypt = false;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-k")
                {
                    passPhrase = args[i + 1];
                }
                if (args[i] == "-p")
                {
                    inputfilePath = args[i + 1];
                }
                if (args[i] == "-d")
                {
                    decrypt = true;
                }
                if (args[i] == "-o")
                {
                    outputfilePath = args[i + 1];
                }

            }
            if (passPhrase == "" || inputfilePath == "" || outputfilePath == "")
            {
                help();
                return;
            }
            if (decrypt)
            {
                byte[] decrypted = Decrypt(readLocalFilePath(inputfilePath, FileMode.Open), passPhrase);
                Console.WriteLine(Encoding.UTF8.GetString(decrypted));
            }
            else
            {   byte[] buf=Encrypt(readLocalFilePath(inputfilePath, FileMode.Open), passPhrase);
                writeLocalFilePath(outputfilePath,buf,FileMode.Create);
            }
        }
    }
}



