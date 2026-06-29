namespace Tests.Domain.Shared;

using Backend.Domain.Shared;

public class PlayerNameDecoderTests
{
    [Fact]
    public void Decode_ShouldReturnEmptyString_WhenBufferIsNull()
    {
        var result = PlayerNameDecoder.Decode(null);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Decode_ShouldDecodeAlphabetAndNumbers()
    {
        // 0x0E = 'A', 0x28 = 'a', 0x04 = '0'
        var buffer = new byte[] { 0x0E, 0x28, 0x04 };

        var result = PlayerNameDecoder.Decode(buffer);

        Assert.Equal("Aa0", result);
    }

    [Fact]
    public void Decode_ShouldStopAtZeroOrFF()
    {
        // 0x0E = 'A', 0x00 = terminator, 0x28 = 'a'
        var bufferWithZero = new byte[] { 0x0E, 0x00, 0x28 };
        // 0x0E = 'A', 0xFF = terminator, 0x28 = 'a'
        var bufferWithFF = new byte[] { 0x0E, 0xFF, 0x28 };

        var resultZero = PlayerNameDecoder.Decode(bufferWithZero);
        var resultFF = PlayerNameDecoder.Decode(bufferWithFF);

        Assert.Equal("A", resultZero);
        Assert.Equal("A", resultFF);
    }

    [Fact]
    public void Decode_ShouldCollapseMultipleSpacesAndTrim()
    {
        // 0x01 = space, 0x0E = 'A'
        var buffer = new byte[] { 0x01, 0x0E, 0x01, 0x01, 0x0E, 0x01 };

        var result = PlayerNameDecoder.Decode(buffer);

        Assert.Equal("A A", result);
    }

    [Theory]
    [InlineData(0x02, '!')]
    [InlineData(0x03, '?')]
    [InlineData(0xE5, '.')]
    [InlineData(0xE6, '?')]
    [InlineData(0xE7, '!')]
    [InlineData(0xE8, '-')]
    [InlineData(0xE9, '~')]
    public void Decode_ShouldCorrectlyMapSpecialSymbols(byte specialByte, char expectedChar)
    {
        var buffer = new byte[] { specialByte };

        var result = PlayerNameDecoder.Decode(buffer);

        Assert.Equal(expectedChar.ToString(), result);
    }

    [Fact]
    public void Decode_ShouldReturnEmptyString_WhenBufferIsEmpty()
    {
        var result = PlayerNameDecoder.Decode([]);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Decode_ShouldReturnEmptyString_WhenBufferContainsOnlySpaces()
    {
        var result = PlayerNameDecoder.Decode([0x01, 0x01]);

        Assert.Equal(string.Empty, result);
    }
}
