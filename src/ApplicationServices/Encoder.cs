using System;
using System.Text;

namespace ApplicationServices
{
    public class Encoder
    {
        public static string SafeEncodeToString(byte[] input)
        {
            var base64 = Convert.ToBase64String(input);
            var safeString = base64.Replace('/', '_');
            return safeString;
        }

        public static byte[] SafeDecodeToByteArray(string input)
        {
            var unsafeString = input.Replace('_', '/');
            var bytes = Convert.FromBase64String(unsafeString);
            return bytes;
        }
    }
}