using System.Text;
using System.Text.RegularExpressions;

namespace Backend.Utils
{
    public static class TextDecoder
    {
        /// <summary>
        /// Decodes a raw byte buffer from the game's custom character encoding into a string.
        /// Byte 0x01 is treated as a space character (the game uses 0x01 0x01 for one space).
        /// Bytes 0x02-0x25 map to uppercase characters (offset +0x33).
        /// Bytes above 0x25 map to lowercase characters (offset +0x39).
        /// Decoding stops at 0x00 or 0xFF terminators.
        /// After decoding, consecutive spaces are collapsed and the result is trimmed.
        /// Returns empty string if the result contains non-printable or invalid characters
        /// (e.g. during battle when the buffer is repurposed for other UI text).
        /// </summary>
        public static string Decode(byte[] buffer)
        {
            if (buffer == null) return string.Empty;

            StringBuilder sb = new StringBuilder();
            foreach (var b in buffer)
            {
                if (b == 0x00 || b == 0xFF) break;

                if (b == 0x01)
                {
                    sb.Append(' ');
                }
                else if (b >= 0x02 && b <= 0x25)
                {
                    sb.Append((char)(b + 0x33));
                }
                else
                {
                    sb.Append((char)(b + 0x39));
                }
            }

            // Collapse double-spaces (game encodes a single space as 0x01 0x01)
            // and trim leading/trailing whitespace
            string raw = sb.ToString();
            string normalized = Regex.Replace(raw, @" {2,}", " ").Trim();

            // Validate: a real area name contains only letters and single spaces.
            // If the buffer holds battle/UI text (garbage), skip it.
            if (!IsValidAreaName(normalized))
                return string.Empty;

            return normalized;
        }

        /// <summary>
        /// Returns true if the string looks like a valid map area name
        /// (only contains ASCII letters, spaces, hyphens, and apostrophes).
        /// </summary>
        private static bool IsValidAreaName(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            foreach (char c in s)
            {
                if (!char.IsLetter(c) && c != ' ' && c != '-' && c != '\'')
                    return false;
            }
            return true;
        }
    }
}
