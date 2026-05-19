namespace Tests.Memory.Readers;

using Backend.Memory.Readers;

public class MemoryBlockReaderTests
{
    [Fact]
    public void ReadInt16_ShouldReturnCorrectValue_WhenOffsetIsValid()
    {
        var buffer = new byte[] { 0x01, 0x02, 0x03 };
        var reader = new MemoryBlockReader(buffer);

        var result = reader.ReadInt16(0);

        Assert.Equal(513, result);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(-1)]
    public void ReadInt16_ShouldReturnZero_WhenOffsetIsOutOfBounds(int offset)
    {
        var buffer = new byte[] { 0x01, 0x02, 0x03 };
        var reader = new MemoryBlockReader(buffer);

        var result = reader.ReadInt16(offset);

        Assert.Equal(0, result);
    }

    [Fact]
    public void ReadInt16_ShouldReturnZero_WhenBufferIsEmpty()
    {
        var reader = new MemoryBlockReader([]);

        var result = reader.ReadInt16(0);

        Assert.Equal(0, result);
    }

    [Fact]
    public void ReadInt32_ShouldReturnCorrectValue_WhenOffsetIsValid()
    {
        var buffer = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05 };
        var reader = new MemoryBlockReader(buffer);

        var result = reader.ReadInt32(1);

        Assert.Equal(67305985, result);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(-1)]
    public void ReadInt32_ShouldReturnZero_WhenOffsetIsOutOfBounds(int offset)
    {
        var buffer = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
        var reader = new MemoryBlockReader(buffer);

        var result = reader.ReadInt32(offset);

        Assert.Equal(0, result);
    }

    [Fact]
    public void ReadInt32_ShouldReturnZero_WhenBufferIsEmpty()
    {
        var reader = new MemoryBlockReader([]);

        var result = reader.ReadInt32(0);

        Assert.Equal(0, result);
    }
}
