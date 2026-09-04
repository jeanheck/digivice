namespace Tests.Memory.Readers;

using Backend.Memory.Addresses;
using Backend.Memory.Readers;
using Moq;
using Xunit;
using Backend.Memory.Readers.Interfaces;

public class CardBattleReaderTests
{
    [Fact]
    public void Read_ShouldMapCardBattleResourceCorrectly()
    {
        var addresses = new CardBattleAddresses
        {
            OpponentId = 0x0004B404,
        };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadInt32(0x0004B404)).Returns(11);

        var reader = new CardBattleReader(memoryReaderMock.Object);

        var result = reader.Read(addresses);

        Assert.NotNull(result);
        Assert.Equal(11, result.OpponentId);
    }
}
