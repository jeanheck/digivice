using Backend.Utils;

namespace Tests.Backend.Utils
{
    public class TextDecoderTests
    {


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
