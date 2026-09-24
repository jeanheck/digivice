namespace Tests.Memory.Readers;

using Backend.Memory.Addresses.Parties;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Interfaces;
using Moq;
using Xunit;

public class InBattleReaderTests
{
    private const long AllySlotBase = 0x000A4470;
    private const int SlotStride = 0x20;
    private const int StrengthOffset = 0x10;

    [Theory]
    [InlineData(1, 0x00)]
    [InlineData(2, 0x20)]
    [InlineData(3, 0x40)]
    public void Read_ShouldResolveAllySlotFromPartySlotIndex(int partySlotIndex, int expectedSlotOffset)
    {
        var addresses = new InBattleAddresses
        {
            AllySlotBase = AllySlotBase,
            SlotStride = SlotStride,
            Strength = StrengthOffset
        };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock
            .Setup(m => m.ReadInt16(AllySlotBase + expectedSlotOffset + StrengthOffset))
            .Returns((short)252);

        var reader = new InBattleReader(memoryReaderMock.Object);

        var result = reader.Read(addresses, partySlotIndex);

        Assert.Equal(252, result.Strength);
    }
}
