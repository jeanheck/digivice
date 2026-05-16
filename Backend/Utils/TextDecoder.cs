using System.Text;
using System.Text.RegularExpressions;

namespace Backend.Utils
{
    public static class TextDecoder
    {
        // Complete character map derived from game data:
        // Uppercase A-Z: bytes 0x0E - 0x27
        // Lowercase a-z: bytes 0x28 - 0x41
        // Digits 0-9:    bytes 0x42 - 0x4B
        // Symbols:       bytes 0x02 - 0x0D and 0x4C+
        private static readonly Dictionary<byte, char> CharMap = BuildCharMap();

        private static Dictionary<byte, char> BuildCharMap()
        {
            var map = new Dictionary<byte, char>();

            // Byte 0x01 = space (handled separately)

            // Bytes 0x02 - 0x03: symbols before digits
            map[0x02] = '!';
            map[0x03] = '?';

            // Digits 0-9: bytes 0x04 - 0x0D
            for (int i = 0; i < 10; i++)
            {
                map[(byte)(0x04 + i)] = (char)('0' + i);
            }

            // Uppercase A-Z: bytes 0x0E - 0x27
            for (int i = 0; i < 26; i++)
            {
                map[(byte)(0x0E + i)] = (char)('A' + i);
            }

            // Lowercase a-z: bytes 0x28 - 0x41
            for (int i = 0; i < 26; i++)
            {
                map[(byte)(0x28 + i)] = (char)('a' + i);
            }

            // Symbols after lowercase: bytes 0x4C+
            char[] postSymbols = { '.', '-', '/', '_', '~', '#', '$', '%', '&', '(', ')', '*', '+', ',', ':', ';', '<', '=', '>', '@', '"', '\'' };
            for (int i = 0; i < postSymbols.Length; i++)
            {
                map[(byte)(0x4C + i)] = postSymbols[i];
            }

            // Special Symbols discovered via RAM dump ("!?.-~")
            map[0xE5] = '.';
            map[0xE6] = '?';
            map[0xE7] = '!';
            map[0xE8] = '-';
            map[0xE9] = '~';

            return map;
        }

        /// <summary>
        /// Decodes a player/character name from the game's custom encoding.
        /// Collapses consecutive spaces, trims the result, and stops at 0x00 or 0xFF.
        /// Allowing digits and special characters (e.g., "Ta01.?-~").
        /// </summary>
        public static string DecodeName(byte[]? buffer)
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
                else if (CharMap.TryGetValue(b, out char c))
                {
                    sb.Append(c);
                }
            }

            string raw = sb.ToString();
            string normalized = Regex.Replace(raw, @" {2,}", " ").Trim();

            if (string.IsNullOrWhiteSpace(normalized)) return string.Empty;
            foreach (char c in normalized)
            {
                if (char.IsControl(c)) return string.Empty;
            }

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
