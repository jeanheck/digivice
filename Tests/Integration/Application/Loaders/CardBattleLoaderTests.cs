namespace Tests.Integration.Application.Loaders;

using Backend.Application.Loaders;
using Backend.Memory;
using Backend.Memory.Readers;
using Moq;
using Xunit;

public class CardBattleLoaderTests : LoaderIntegrationTestBase
{
    [Fact]
    public void Load_ShouldIntegrateCardBattleAddressesAndReader()
    {
        var addressesRepository = CreateAddressesRepository();

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadInt32(0x0004B404)).Returns(11);

        var cardBattleReader = new CardBattleReader(memoryReaderMock.Object);
        var cardBattleLoader = new CardBattleLoader(addressesRepository, cardBattleReader);

        var cardBattleResource = cardBattleLoader.Load();

        Assert.NotNull(cardBattleResource);
        Assert.Equal(11, cardBattleResource.OpponentId);
    }

    [Fact]
    public void Load_ShouldThrowMemoryReadException_WhenMemoryReaderCannotReadCardBattleData()
    {
        var addressesRepository = CreateAddressesRepository();

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadInt32(0x0004B404))
            .Throws(new MemoryReadException(0x0004B404, "Memory session is not connected."));

        var cardBattleReader = new CardBattleReader(memoryReaderMock.Object);
        var cardBattleLoader = new CardBattleLoader(addressesRepository, cardBattleReader);

        Assert.Throws<MemoryReadException>(() => cardBattleLoader.Load());
    }
}
