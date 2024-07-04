using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;

namespace Maew123.Api.Utilities
{
    public class KeyAlgorithm
    {
        //เมิน class นี้ไป ใช้ generate key เฉยๆ
        public static void whatever()
        {
            //byte[] Key = GenerateAes256KWKey();

            //// Save the key to a file
            //string directoryPath = @"C:\Users\WIN10\source\repos\Maew123\Maew123.api\ZKey";
            //string filePath = Path.Combine(directoryPath, "keyfile.bin");

            //SaveKeyToFile(Key, filePath);

            //Console.WriteLine("Key generated and saved to file successfully.");
        }

        public static byte[] GenerateAes256KWKey()
        {
            const int KeySizeInBits = 256;
            using (var aes = new AesCryptoServiceProvider { KeySize = KeySizeInBits })
            {
                aes.GenerateKey();
                return aes.Key;
            }
        }

        public static void SaveKeyToFile(byte[] key, string filePath)
        {
            File.WriteAllBytes(filePath, key);
        }

        public static byte[] GenerateHmacSha512Key()
        {
            const int KeySizeInBits = 512;
            const int KeySizeInBytes = KeySizeInBits / 8;

            using (var rng = new RNGCryptoServiceProvider())
            {
                var key = new byte[KeySizeInBytes];
                rng.GetBytes(key);
                return key;
            }
        }

        public static string GenerateHmacSha512KeyAsString()
        {
            const int KeySizeInBits = 512;
            const int KeySizeInBytes = KeySizeInBits / 8;

            using (var rng = new RNGCryptoServiceProvider())
            {
                var key = new byte[KeySizeInBytes];
                rng.GetBytes(key);
                return Convert.ToBase64String(key);
            }
        }

        //generate 512keyAsString without using any library
        public static string Generate512BitKey()
        {
            var keyBytes = new byte[64];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(keyBytes);
            }

            return Convert.ToBase64String(keyBytes);
        }
    }
}
