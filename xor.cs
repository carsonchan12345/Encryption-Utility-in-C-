using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace RandomName
{
        public static class RandomName
    {
        //xor encryption and decryption function
        public static byte[] xor(byte[] plainTextBytes, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes("pemgail9uzpgzl88");
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(256 / 8);
            for (int i = 0; i < plainTextBytes.Length; i++)
            {
                plainTextBytes[i] = (byte)(plainTextBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }
            return plainTextBytes;
        }
        //read binary file and return byte array
        public static byte[] ReadLocalFilePath(string filePath)
        {
            byte[] buffer = null;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
            }
            return buffer;
        }
        //output byte to binary file
        public static void WriteLocalFilePath(string filePath, byte[] buffer)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            fs.Write(buffer, 0, buffer.Length);
            fs.Close();
        }
        //take -k as xor key, -o as output file path and -i as input file path. Print help message if missing these arguments
        public static void Main(string[] args)
        {
            string key = null;
            string output = null;
            string input = null;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-k")
                {
                    key = args[i + 1];
                }
                if (args[i] == "-o")
                {
                    output = args[i + 1];
                }
                if (args[i] == "-i")
                {
                    input = args[i + 1];
                }
            }
            if (key == null || output == null || input == null)
            {
                Console.WriteLine("Usage: xor -k key -o output -i input");
                return;
            }
            byte[] buffer = ReadLocalFilePath(input);
            byte[] result = xor(buffer, key);
            WriteLocalFilePath(output, result);
        }
    }
}
