using System.Text;

namespace Backend.Utils
{
    public static class TextDecoder
    {
        /// <summary>
        /// Decodes a raw byte buffer from the game's custom character encoding into a string.
        /// Bytes 0x01-0x25 map to uppercase characters (offset +0x33).
        /// Bytes above 0x25 map to lowercase characters (offset +0x39).
        /// Decoding stops at 0x00 or 0xFF terminators.
        /// </summary>
        public static string Decode(byte[] buffer)
        {
            if (buffer == null) return string.Empty;

            StringBuilder sb = new StringBuilder();
            foreach (var b in buffer)
            {
                if (b == 0x00 || b == 0xFF) break;

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
