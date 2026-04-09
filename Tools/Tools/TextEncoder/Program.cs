using System.Text;

namespace TextEncoderTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Digimon World 3 (2003) Text Encoder for Cheat Engine ===");
            Console.WriteLine("Type the name of the map or text you want to search (e.g., Asuka, Central, Digimon):");
            Console.WriteLine("Type 'exit' to quit.\n");

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) continue;
                if (input.Trim().ToLower() == "exit") break;

                Console.WriteLine("\n[Cheat Engine Array of Bytes]:");
                Console.WriteLine(EncodeToHexShell(input));
                Console.WriteLine();
            }
        }

        static string EncodeToHexShell(string text)
        {
            StringBuilder hexString = new StringBuilder();

            foreach (char c in text)
            {
                int charValue = (int)c;
                byte encodedByte = 0x00;

                // Space in DW3 usually maps to 0x00 (terminator) or a specific char, 
                // but searches are safer without spaces. We'll skip them or warn.
                if (c == ' ')
                {
                    // DW3 spaces can be complex, better to search words individually.
                    hexString.Append("?? ");
                    continue;
                }

                // Reversed logic from TextDecoder.cs
                if (charValue >= 65 && charValue <= 90) // A-Z
                {
                    encodedByte = (byte)(charValue - 0x33);
                }
                else if (charValue >= 97 && charValue <= 122) // a-z
                {
                    encodedByte = (byte)(charValue - 0x39);
                }

                if (encodedByte != 0x00)
                {
                    hexString.Append(encodedByte.ToString("X2") + " ");
                }
            }

            return hexString.ToString().TrimEnd();
        }
    }
}
