using System.Text;

namespace Backend.Utils
{
    public static class TextDecoder
    {
        public static string DecodeProtagonist(byte[] buffer) => Decode(buffer);

        public static string DecodeDigimon(byte[] buffer) => Decode(buffer);

        private static string Decode(byte[] buffer)
        {
            if (buffer == null) return string.Empty;

            StringBuilder sb = new StringBuilder();
            foreach (var b in buffer)
            {
                if (b == 0x00 || b == 0xFF) break;

                // Game Rule:
                // Uppercase/Numbers/Symbols (0x01 to 0x25): ASCII + 0x33
                // Lowercase (above 0x25): ASCII + 0x39

                if (b >= 0x01 && b <= 0x25)
                {
                    sb.Append((char)(b + 0x33));
                }
                else
                {
                    sb.Append((char)(b + 0x39));
                }
            }
            return sb.ToString();
        }
    }
}
