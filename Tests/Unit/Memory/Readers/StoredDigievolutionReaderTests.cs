namespace Tests.Memory.Readers;

using Backend.Memory.Readers;
using Backend.Memory.Addresses.Parties.Digimons;

public class StoredDigievolutionReaderTests
{
    private const int UnlockedDigievolutionsStart = 0x50;
    private const int EntryStride = 20;

    private static DigievolutionsAddresses CreateAddresses(int maxUnlockedDigievolutions)
    {
        return new DigievolutionsAddresses
        {
            UnlockedDigievolutionsStart = UnlockedDigievolutionsStart,
            UnlockedDigievolutionEntryStride = EntryStride,
            MaxUnlockedDigievolutions = maxUnlockedDigievolutions,
            Id = 0x00,
            Level = 0x02,
            Dvxp = 0x04
        };
    }

    private static void WriteEntry(byte[] block, int entryIndex, short digievolutionId, short level, int dvxp)
    {
        var entryOffset = UnlockedDigievolutionsStart + (entryIndex * EntryStride);
        Array.Copy(BitConverter.GetBytes(digievolutionId), 0, block, entryOffset, 2);
        Array.Copy(BitConverter.GetBytes(level), 0, block, entryOffset + 2, 2);
        Array.Copy(BitConverter.GetBytes(dvxp), 0, block, entryOffset + 4, 4);
    }

    [Fact]
    public void Read_ShouldReturnUnlockedEntries_WhenIdsAreGreaterThanZero()
    {
        var block = new byte[1500];
        WriteEntry(block, 0, digievolutionId: 3, level: 15, dvxp: 70000);
        WriteEntry(block, 1, digievolutionId: 5, level: 40, dvxp: 250);
        WriteEntry(block, 2, digievolutionId: 8, level: 20, dvxp: 0);

        var result = new StoredDigievolutionReader().Read(new MemoryBlockReader(block), CreateAddresses(4));

        Assert.Equal(3, result.Count);
        Assert.Equal(3, result[0].DigievolutionId);
        Assert.Equal(15, result[0].Level);
        Assert.Equal(70000, result[0].Dvxp);
        Assert.Equal(5, result[1].DigievolutionId);
        Assert.Equal(40, result[1].Level);
        Assert.Equal(250, result[1].Dvxp);
        Assert.Equal(8, result[2].DigievolutionId);
        Assert.Equal(20, result[2].Level);
        Assert.Equal(0, result[2].Dvxp);
    }

    [Fact]
    public void Read_ShouldReturnEmptyList_WhenFirstEntryIsEmpty()
    {
        var block = new byte[1500];
        WriteEntry(block, 0, digievolutionId: 0, level: 15, dvxp: 100);
        WriteEntry(block, 1, digievolutionId: 99, level: 40, dvxp: 100);

        var result = new StoredDigievolutionReader().Read(new MemoryBlockReader(block), CreateAddresses(2));

        Assert.Empty(result);
    }

    [Fact]
    public void Read_ShouldStopAtFirstEmptyEntry_AndIgnoreGarbageAfterIt()
    {
        var block = new byte[1500];
        WriteEntry(block, 0, digievolutionId: 3, level: 15, dvxp: 100);
        WriteEntry(block, 1, digievolutionId: 0, level: 0, dvxp: 0);
        WriteEntry(block, 2, digievolutionId: 240, level: 180, dvxp: 999);

        var result = new StoredDigievolutionReader().Read(new MemoryBlockReader(block), CreateAddresses(4));

        var stored = Assert.Single(result);
        Assert.Equal(3, stored.DigievolutionId);
        Assert.Equal(15, stored.Level);
    }

    [Fact]
    public void Read_ShouldRespectMaxUnlockedDigievolutions()
    {
        var block = new byte[1500];
        WriteEntry(block, 0, digievolutionId: 3, level: 15, dvxp: 100);
        WriteEntry(block, 1, digievolutionId: 5, level: 40, dvxp: 200);
        WriteEntry(block, 2, digievolutionId: 8, level: 20, dvxp: 300);

        var result = new StoredDigievolutionReader().Read(new MemoryBlockReader(block), CreateAddresses(2));

        Assert.Equal(2, result.Count);
        Assert.Equal(5, result[1].DigievolutionId);
    }
}
