using Mono.Math;
using System.Text;

namespace _7dsgcKeyFinder
{
    public class Decrypt
    {
        public Decrypt(byte[] key)
        {
            publicKey = key;
            byte[] array = new byte[128];
            exponentField = (int)publicKey[16] | (int)publicKey[17] << 8 | (int)publicKey[18] << 16;
            Buffer.BlockCopy(publicKey, 20, array, 0, array.Length);
            Array.Reverse(array);
            nField = new BigInteger(array);
        }

        public string DecryptLiteral(byte[] bytes)
        {
            BigInteger bigInteger = new BigInteger(bytes);
            byte[] array = bigInteger.ModPow(exponentField, nField).GetBytes();
            return Translate(array);
        }

        private string Translate(byte[] b)
        {
            int i;
            for (i = 0; i < b.Length; i++)
            {
                if (b[i] != 0)
                {
                    break;
                }
            }
            if (i != b.Length)
            {
                byte[] array = new byte[b.Length - i];
                Buffer.BlockCopy(b, i, array, 0, b.Length - i);
                return Encoding.UTF8.GetString(array);
            }
            return string.Empty;
        }

        private readonly byte[] publicKey;

        private readonly int exponentField;

        private readonly BigInteger nField;
    }
}