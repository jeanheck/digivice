namespace Tests.Memory.Readers.Parties.Digimons;

using System;
using Xunit;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Parties.Digimons;
using Backend.Memory.Addresses.Parties.Digimons;

public class DigievolutionReaderTests
{
    private static void WriteInt16(byte[] block, int offset, short value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, block, offset, bytes.Length);
    }

    private static void WriteInt32(byte[] block, int offset, int value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, block, offset, bytes.Length);
    }

    [Fact]
    public void Read_ShouldReturnUnlockedLevel_WhenIdIsFound()
    {
        // Arrange
        var digievolutionsAddresses = new DigievolutionsAddresses
        {
            UnlockedDigievolutionsStart = 10,
            UnlockedDigievolutionEntryStride = 8,
            MaxUnlockedDigievolutions = 3,
            Id = 0,
            Level = 2,
            Dvxp = 4
        };

        var block = new byte[256];
        // Entrada 0 (offset 10): ID 3, Level 15, Dvxp 100
        WriteInt16(block, 10, 3);
        WriteInt16(block, 12, 15);
        WriteInt32(block, 14, 100);

        // Entrada 1 (offset 18): ID 5, Level 40, Dvxp 250
        WriteInt16(block, 18, 5);
        WriteInt16(block, 20, 40);
        WriteInt32(block, 22, 250);

        // Entrada 2 (offset 26): ID 8, Level 20, Dvxp 50
        WriteInt16(block, 26, 8);
        WriteInt16(block, 28, 20);
        WriteInt32(block, 30, 50);

        var memoryBlockReader = new MemoryBlockReader(block);
        var reader = new DigievolutionReader();

        // Act
        var result = reader.Read(memoryBlockReader, 5, digievolutionsAddresses);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(40, result.Level);
        Assert.Equal(250, result.Dvxp);
    }

    [Fact]
    public void Read_ShouldReturnDefaultDvxpZero_WhenIdIsNotFound()
    {
        var digievolutionsAddresses = new DigievolutionsAddresses
        {
            UnlockedDigievolutionsStart = 10,
            UnlockedDigievolutionEntryStride = 8,
            MaxUnlockedDigievolutions = 1,
            Id = 0,
            Level = 2,
            Dvxp = 4
        };

        var block = new byte[256];
        WriteInt16(block, 10, 3);
        WriteInt16(block, 12, 15);
        WriteInt32(block, 14, 100);

        var memoryBlockReader = new MemoryBlockReader(block);
        var reader = new DigievolutionReader();

        var result = reader.Read(memoryBlockReader, 99, digievolutionsAddresses);

        Assert.NotNull(result);
        Assert.Equal(1, result.Level);
        Assert.Equal(0, result.Dvxp);
    }

}
