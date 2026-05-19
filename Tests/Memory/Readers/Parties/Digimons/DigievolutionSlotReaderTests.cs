namespace Tests.Memory.Readers.Parties.Digimons;

using System;
using Xunit;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Parties.Digimons;
using Backend.Memory.Addresses.Parties;

public class DigievolutionSlotReaderTests
{
    [Fact]
    public void Read_ShouldReturnDigievolutionSlotWithCorrectId()
    {
        // Arrange
        var slotAddresses = new SlotAddresses
        {
            Index = 4,
            Address = 64
        };

        var block = new byte[256];
        var bytes = BitConverter.GetBytes((short)85); // DigievolutionId = 85
        Array.Copy(bytes, 0, block, 64, bytes.Length);

        var memoryBlockReader = new MemoryBlockReader(block);
        var reader = new DigievolutionSlotReader();

        // Act
        var result = reader.Read(memoryBlockReader, slotAddresses);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(4, result.Index);
        Assert.Equal(85, result.DigievolutionId);
    }
}
