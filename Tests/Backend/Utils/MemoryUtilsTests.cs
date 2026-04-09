using Backend.Utils;

namespace Tests.Backend.Utils
{
    public class MemoryUtilsTests
    {
        [Theory]
        [InlineData(null, 50, 50)]
        [InlineData("", 50, 50)]
        [InlineData("100", 50, 100)] // Integer string
        [InlineData("0x1A", 50, 26)] // Hex string
        [InlineData("1A", 50, 26)] // Hex string without 0x
        [InlineData("invalid", 50, 50)]
        public void ReadInt32OffsetSafely_ShouldHandleVariousFormats_AndFallback(string? offset, int fallback, int expected)
        {
            var result = MemoryUtils.ReadInt32OffsetSafely(offset, fallback);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReadInt16FromBlock_ShouldReturnExpectedValue_WhenValid()
        {
            // Block representing 0x1234 at offset 2
            byte[] block = { 0x00, 0x00, 0x34, 0x12, 0x00 };
            string hexOffset = "2";

            var result = MemoryUtils.ReadInt16FromBlock(block, hexOffset);

            Assert.Equal(0x1234, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("invalid")]
        [InlineData("10")] // Out of bounds
        public void ReadInt16FromBlock_ShouldReturnZero_WhenInvalidOrOutOfBounds(string? hexOffset)
        {
            byte[] block = { 0x01, 0x02, 0x03 };
            var result = MemoryUtils.ReadInt16FromBlock(block, hexOffset);
            Assert.Equal(0, result);
        }

        [Fact]
        public void ReadInt32FromBlock_ShouldReturnExpectedValue_WhenValid()
        {
            // Block representing 0x11223344 at offset 1
            byte[] block = { 0x00, 0x44, 0x33, 0x22, 0x11, 0x00 };
            string hexOffset = "1";

            var result = MemoryUtils.ReadInt32FromBlock(block, hexOffset);

            Assert.Equal(0x11223344, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("invalid")]
        [InlineData("4")] // Out of bounds (needs 4 bytes starting at index 4, meaning length must be 8)
        public void ReadInt32FromBlock_ShouldReturnZero_WhenInvalidOrOutOfBounds(string? hexOffset)
        {
            byte[] block = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };
            var result = MemoryUtils.ReadInt32FromBlock(block, hexOffset);
            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData(null, 0)]
        [InlineData("", 0)]
        [InlineData("1A", 26)]
        [InlineData("0x1A", 26)]
        [InlineData("invalid", 0)]
        public void ParseHex_ShouldParseCorrectly_OrReturnZero(string? hex, int expected)
        {
            var result = MemoryUtils.ParseHex(hex);
            Assert.Equal(expected, result);
        }
    }
}
