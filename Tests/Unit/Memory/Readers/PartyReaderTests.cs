namespace Tests.Memory.Readers;

using Xunit;
using Moq;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Parties;
using Backend.Memory.Addresses;
using Backend.Memory.Addresses.Parties;
using Backend.Memory.Resources.Parties;

public class PartyReaderTests
{
    [Fact]
    public void Read_ShouldMapPartyResourceCorrectly()
    {
        // Arrange
        var slot1 = new SlotAddresses { Index = 0, Address = 0x1000 };
        var slot2 = new SlotAddresses { Index = 1, Address = 0x2000 };

        var addresses = new PartyAddresses
        {
            BytesPerSlot = 128,
            EmptySlotId = 255,
            Slots = [slot1, slot2]
        };

        var expectedResource1 = new DigimonSlotResource { Index = 0, DigimonId = 1 };
        var expectedResource2 = new DigimonSlotResource { Index = 1, DigimonId = 2 };

        var slotReaderMock = new Mock<IDigimonSlotReader>();
        slotReaderMock.Setup(s => s.Read(slot1, 128)).Returns(expectedResource1);
        slotReaderMock.Setup(s => s.Read(slot2, 128)).Returns(expectedResource2);

        var reader = new PartyReader(slotReaderMock.Object);

        // Act
        var result = reader.Read(addresses);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.SlotsResource.Count);
        Assert.Equal(expectedResource1, result.SlotsResource[0]);
        Assert.Equal(expectedResource2, result.SlotsResource[1]);
    }
}
