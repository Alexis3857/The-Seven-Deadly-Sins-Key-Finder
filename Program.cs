using System.Globalization;
using System.Text.RegularExpressions;

namespace _7dsgcKeyFinder
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                byte[] metadata = File.ReadAllBytes(args[0]);
                byte[] publicKey = new byte[151];
                int intOffset;
                Console.Write("Enter the key offset : ");
                string stringHexOffset = Console.ReadLine();
                while (!HexStringToInt(stringHexOffset, metadata.Length - publicKey.Length, out intOffset))
                {
                    Console.Write("Enter the key offset : ");
                    stringHexOffset = Console.ReadLine();
                }
                Array.Copy(metadata, intOffset, publicKey, 0, publicKey.Length);
                s_decryptor = new Decrypt(publicKey);
                Console.WriteLine("Key set.\n");
                while (true)
                {
                    byte[] key = new byte[128];
                    Console.Write("Enter the offset to decrypt : ");
                    stringHexOffset = Console.ReadLine();
                    while (!HexStringToInt(stringHexOffset, metadata.Length - key.Length, out intOffset))
                    {
                        Console.Write("Enter the offset to decrypt : ");
                        stringHexOffset = Console.ReadLine();
                    }
                    Array.Copy(metadata, intOffset, key, 0, key.Length);
                    Console.WriteLine($"Key : {s_decryptor.DecryptLiteral(key)}\n");
                }
            }
            else
            {
                Console.WriteLine("You have to pass the path of global-metadata.dat as an argument.");
            }
        }

        public static bool HexStringToInt(string hexString, int maxLen, out int hexInt)
        {
            hexInt = 0;
            if (!Regex.IsMatch(hexString, "^0x[0-9a-fA-F]+$") || !int.TryParse(hexString.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out hexInt))
            {
                Console.WriteLine("Please enter a valid string.\n");
                return false;
            }
            if (hexInt > maxLen)
            {
                Console.WriteLine("The offset is too big.\n");
                return false;
            }
            return true;
        }

        private static Decrypt s_decryptor;
    }
}