using Backend.Utils;
using Xunit;

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
        public void DecodeProtagonist_ShouldHandleMixedCase_Jean()
        {
            // "Jean"
            // 'J' (4A) -> 17 (4A - 33)
            // 'e' (65) -> 2C (65 - 39)
            // 'a' (61) -> 28 (61 - 39)
            // 'n' (6E) -> 35 (6E - 39)
            byte[] input = new byte[] { 0x17, 0x2C, 0x28, 0x35, 0x00 };
            string expected = "Jean";

            string result = TextDecoder.DecodeProtagonist(input);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void DecodeProtagonist_ShouldReturnEmpty_WhenBufferIsNull()
        {
            string result = TextDecoder.DecodeProtagonist(null!);
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void DecodeProtagonist_ShouldStopAtTerminators()
        {
            byte[] inputWithNull = new byte[] { 0x21, 0x00, 0x12 };
            byte[] inputWithFF = new byte[] { 0x21, 0xFF, 0x12 };

            Assert.Equal("T", TextDecoder.DecodeProtagonist(inputWithNull));
            Assert.Equal("T", TextDecoder.DecodeProtagonist(inputWithFF));
        }

        [Fact]
        public void DecodeDigimon_ShouldHandleMixedCase()
        {
            // "Kotemon"
            byte[] input = new byte[] { 0x18, 0x36, 0x3B, 0x2C, 0x34, 0x36, 0x35, 0x00 };
            string expected = "Kotemon";

            string result = TextDecoder.DecodeDigimon(input);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void DecodeDigimon_ShouldHandleBoundaryCharacters()
        {
            // Boundary: Uppercase is <= 0x25, Lowercase is > 0x25
            // 0x25 + 0x33 = 0x58 ('X')
            // 0x26 + 0x39 = 0x5F ('_') - following the current logic

            byte[] input = new byte[] { 0x25, 0x26 };
            string result = TextDecoder.DecodeDigimon(input);

            Assert.Equal("X_", result);
        }

        [Fact]
        public void DecodeDigimon_ShouldReturnEmpty_WhenBufferIsNull()
        {
            string result = TextDecoder.DecodeDigimon(null!);
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void DecodeDigimon_ShouldStopAtTerminators()
        {
            byte[] inputWithNull = new byte[] { 0x18, 0x00, 0x36 };
            byte[] inputWithFF = new byte[] { 0x18, 0xFF, 0x36 };

            Assert.Equal("K", TextDecoder.DecodeDigimon(inputWithNull));
            Assert.Equal("K", TextDecoder.DecodeDigimon(inputWithFF));
        }
    }
}
