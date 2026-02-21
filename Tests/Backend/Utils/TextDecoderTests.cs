using Backend.Utils;

namespace Tests.Backend.Utils
{
    public class TextDecoderTests
    {
        [Fact]
        public void DecodeProtagonist_ShouldHandleSimpleText()
        {
            // "TESTE" (ASCII 54 45 53 54 45) -> ROM (21 12 20 21 12)
            byte[] input = new byte[] { 0x21, 0x12, 0x20, 0x21, 0x12, 0x00 };
            string expected = "TESTE";

            string result = TextDecoder.DecodeProtagonist(input);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void DecodeDigimon_ShouldHandleMixedCase()
        {
            // "Kotemon"
            // 'K' (4B) -> 18 (4B - 33)
            // 'o' (6F) -> 36 (6F - 39)
            // 't' (74) -> 3B (74 - 39)
            // 'e' (65) -> 2C (65 - 39)
            // 'm' (6D) -> 34 (6D - 39)
            // 'o' (6F) -> 36 (6F - 39)
            // 'n' (6E) -> 35 (6E - 39)
            byte[] input = new byte[] { 0x18, 0x36, 0x3B, 0x2C, 0x34, 0x36, 0x35, 0x00 };
            string expected = "Kotemon";

            string result = TextDecoder.DecodeDigimon(input);

            Assert.Equal(expected, result);
        }
    }
}
