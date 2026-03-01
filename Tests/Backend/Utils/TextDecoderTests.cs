using Backend.Utils;
using Xunit;

namespace Tests.Backend.Utils
{
    public class TextDecoderTests
    {
        // --- Standard decoding ---

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
        public void Decode_ShouldHandleMixedCaseDigimonName_Kotemon()
        {
            // "Kotemon" -> K(18) o(36) t(3B) e(2C) m(34) o(36) n(35)
            byte[] input = [0x18, 0x36, 0x3B, 0x2C, 0x34, 0x36, 0x35, 0x00];
            Assert.Equal("Kotemon", TextDecoder.Decode(input));
        }

        // --- Edge cases ---

        [Fact]
        public void Decode_ShouldReturnEmpty_WhenBufferIsNull()
        {
            Assert.Equal(string.Empty, TextDecoder.Decode(null!));
        }

        [Fact]
        public void Decode_ShouldStopAtNullTerminator()
        {
            // Only 'T' before 0x00
            byte[] input = [0x21, 0x00, 0x12];
            Assert.Equal("T", TextDecoder.Decode(input));
        }

        [Fact]
        public void Decode_ShouldStopAtFFTerminator()
        {
            // Only 'T' before 0xFF
            byte[] input = [0x21, 0xFF, 0x12];
            Assert.Equal("T", TextDecoder.Decode(input));
        }

        // --- Boundary: uppercase/lowercase transition at 0x27/0x28 ---

        [Fact]
        public void Decode_ShouldHandleBoundaryCharacters()
        {
            // 0x27 = 'Z' (last uppercase), 0x28 = 'a' (first lowercase)
            byte[] input = [0x27, 0x28];
            Assert.Equal("Za", TextDecoder.Decode(input));
        }

        // --- DecodeName: supports digits and symbols ---

        [Fact]
        public void DecodeName_ShouldHandleLettersAndDigits()
        {
            // "Ta01" -> T(0x21) a(0x28) 0(0x42) 1(0x43)
            byte[] input = [0x21, 0x28, 0x42, 0x43, 0x00];
            Assert.Equal("Ta01", TextDecoder.DecodeName(input));
        }
    }
}
