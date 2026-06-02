namespace Tests.Memory.Readers.Parties.Digimons;

using System;
using Xunit;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Parties.Digimons;
using Backend.Memory.Addresses.Parties.Digimons;

public class StoredDigievolutionReaderTests
{
    private static void WriteInt16(byte[] block, int offset, short value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, block, offset, bytes.Length);
    }

    [Fact]
    public void Read_ShouldReturnUnlockedEntries_WhenIdsAreGreaterThanZero()
    {
        var digievolutionsAddresses = new DigievolutionsAddresses
        {
            UnlockedDigievolutionsStart = 10,
            UnlockedDigievolutionEntryStride = 4,
            MaxUnlockedDigievolutions = 4,
            Id = 0,
            Level = 2,
            Dvxp = 4
        };

        var block = new byte[256];
        WriteInt16(block, 10, 3);
        WriteInt16(block, 12, 15);
        WriteInt16(block, 14, 5);
        WriteInt16(block, 16, 40);
        WriteInt16(block, 18, 0);
        WriteInt16(block, 20, 99);
        WriteInt16(block, 22, 8);
        WriteInt16(block, 24, 20);

        var memoryBlockReader = new MemoryBlockReader(block);
        var reader = new StoredDigievolutionReader();

        var result = reader.Read(memoryBlockReader, digievolutionsAddresses);

        Assert.Equal(3, result.Count);
        Assert.Equal(3, result[0].DigievolutionId);
        Assert.Equal(15, result[0].Level);
        Assert.Equal(5, result[1].DigievolutionId);
        Assert.Equal(40, result[1].Level);
        Assert.Equal(8, result[2].DigievolutionId);
        Assert.Equal(20, result[2].Level);
    }

    [Fact]
    public void Read_ShouldReturnEmptyList_WhenNoUnlockedEntriesExist()
    {
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
        WriteInt16(block, 10, 0);
        WriteInt16(block, 12, 15);
        WriteInt16(block, 14, 0);
        WriteInt16(block, 16, 40);

        var memoryBlockReader = new MemoryBlockReader(block);
        var reader = new StoredDigievolutionReader();

        var result = reader.Read(memoryBlockReader, digievolutionsAddresses);

        Assert.Empty(result);
    }
}
