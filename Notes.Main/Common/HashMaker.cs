using Org.BouncyCastle.Tls;
using System.Security.Cryptography;
using System.Text;

namespace Notes.Main.Common
{
    public class HashMaker
    {
        public static string GetSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                var sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }

        public static bool VerifySHA256Hash(string input, string compareTo)
        {
            var hashOfInput = GetSHA256Hash(input);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, compareTo) == 0;
        }
    }
}
