using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace RandomName
{
        public static class RandomName
    {
        //xor encryption and decryption function
    private static byte[] xorEncDec(byte[] inputData, string keyPhrase)
    {
        //byte[] keyBytes = Encoding.UTF8.GetBytes(keyPhrase);
        byte[] bufferBytes = new byte[inputData.Length];
        for (int i = 0; i < inputData.Length; i++)
        {
            bufferBytes[i] = (byte)(inputData[i] ^ Encoding.UTF8.GetBytes(keyPhrase)[i % Encoding.UTF8.GetBytes(keyPhrase).Length]);
        }
        return bufferBytes;
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
            byte[] result = xorEncDec(buffer, key);
            WriteLocalFilePath(output, result);
        }
    }
}
