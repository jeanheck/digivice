using Backend.Utils;
using Xunit;

namespace Tests.Backend.Utils
{
    public class TextDecoderTests
    {
        [Fact]
        public void Decode_ShouldHandleUppercaseAsciiName()
        {
            // "TESTE" -> ROM bytes: (21 12 20 21 12)
            byte[] input = [0x21, 0x12, 0x20, 0x21, 0x12, 0x00];
            Assert.Equal("TESTE", TextDecoder.Decode(input));
        }

        [Fact]
        public void Decode_ShouldHandleMixedCaseName_Jean()
        {
            // "Jean" -> J(17) e(2C) a(28) n(35)
            byte[] input = [0x17, 0x2C, 0x28, 0x35, 0x00];
            Assert.Equal("Jean", TextDecoder.Decode(input));
        }

        [Fact]
        public void Decode_ShouldReturnEmpty_WhenBufferIsNull()
        {
            Assert.Equal(string.Empty, TextDecoder.Decode(null!));
        }

        [Fact]
        public void Decode_ShouldStopAtNullTerminator()
        {
            byte[] input = [0x21, 0x00, 0x12];
            Assert.Equal("T", TextDecoder.Decode(input));
        }

        [Fact]
        public void Decode_ShouldStopAtFFTerminator()
        {
            byte[] input = [0x21, 0xFF, 0x12];
            Assert.Equal("T", TextDecoder.Decode(input));
        }

        [Fact]
        public void DecodeName_ShouldHandleLettersAndDigits()
        {
            // "Ta01" -> T(0x21) a(0x28) 0(0x04) 1(0x05)
            byte[] input = [0x21, 0x28, 0x04, 0x05, 0x00];
            Assert.Equal("Ta01", TextDecoder.DecodeName(input));
        }

        [Fact]
        public void DecodeName_ShouldHandleSymbols()
        {
            // Hyphen('-') is 0x4D based on the PostSymbols map offset 0x4C
            // ". -" -> .(0x4C) space(0x01) -(0x4D)
            byte[] input = [0x4C, 0x01, 0x4D, 0x00];
            Assert.Equal(". -", TextDecoder.DecodeName(input));
        }
    }
}
