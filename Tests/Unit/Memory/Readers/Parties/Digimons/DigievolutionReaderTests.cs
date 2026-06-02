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

    [Fact]
    public void Read_ShouldReturnUnlockedLevel_WhenIdIsFound()
    {
        // Arrange
        var digievolutionsAddresses = new DigievolutionsAddresses
        {
            UnlockedDigievolutionsStart = 10,
            UnlockedDigievolutionEntryStride = 4,
            MaxUnlockedDigievolutions = 3,
            Id = 0,
            Level = 2,
            Dvxp = 4
        };

        var block = new byte[256];
        // Entrada 0 (offset 10): ID 3, Level 15
        WriteInt16(block, 10, 3);
        WriteInt16(block, 12, 15);

        // Entrada 1 (offset 14): ID 5, Level 40
        WriteInt16(block, 14, 5);
        WriteInt16(block, 16, 40);

        // Entrada 2 (offset 18): ID 8, Level 20
        WriteInt16(block, 18, 8);
        WriteInt16(block, 20, 20);

        var memoryBlockReader = new MemoryBlockReader(block);
        var reader = new DigievolutionReader();

        // Act
        var result = reader.Read(memoryBlockReader, 5, digievolutionsAddresses);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(40, result.Level);
    }

    [Fact]
    public void Read_ShouldReturnDefaultLevelOne_WhenIdIsNotFound()
    {
        // Arrange
        var digievolutionsAddresses = new DigievolutionsAddresses
        {
            UnlockedDigievolutionsStart = 10,
            UnlockedDigievolutionEntryStride = 4,
            MaxUnlockedDigievolutions = 2,
            Id = 0,
            Level = 2,
            Dvxp = 4
        };

        var block = new byte[256];
        // Entrada 0 (offset 10): ID 3, Level 15
        WriteInt16(block, 10, 3);
        WriteInt16(block, 12, 15);

        var memoryBlockReader = new MemoryBlockReader(block);
        var reader = new DigievolutionReader();

        // Act
        var result = reader.Read(memoryBlockReader, 99, digievolutionsAddresses);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Level); // Fallback padrão
    }
}
