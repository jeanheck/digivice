namespace Tests.Integration.Application.Loaders;

using Backend.Application.Loaders;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Battles;
using Moq;
using Xunit;

public class BattleLoaderTests : LoaderIntegrationTestBase
{
    [Fact]
    public void Load_ShouldReadEnemyFromEnemyAddresses()
    {
        var addressesRepository = CreateAddressesRepository();
        var memoryReaderMock = new Mock<IMemoryReader>();

        const long enemySlotBase = 0x000A44D0;
        memoryReaderMock.Setup(m => m.ReadInt16(enemySlotBase + 0x00)).Returns((short)122);
        memoryReaderMock.Setup(m => m.ReadInt16(enemySlotBase + 0x06)).Returns((short)672);
        memoryReaderMock.Setup(m => m.ReadInt16(enemySlotBase + 0x08)).Returns((short)600);
        memoryReaderMock.Setup(m => m.ReadInt16(enemySlotBase + 0x10)).Returns((short)0);
        memoryReaderMock.Setup(m => m.ReadInt16(enemySlotBase + 0x12)).Returns((short)0);
        memoryReaderMock.Setup(m => m.ReadInt16(enemySlotBase + 0x14)).Returns((short)84);
        memoryReaderMock.Setup(m => m.ReadBytes(enemySlotBase + 0x1C, 1)).Returns([0x01]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x000A4530, 1)).Returns([0x02]);

        var loader = new BattleLoader(addressesRepository, new EnemyReader(memoryReaderMock.Object), memoryReaderMock.Object);
        var resource = loader.Load();

        Assert.Equal(0x02, resource.Field);
        Assert.Equal(122, resource.Enemy.Id);
        Assert.Equal(0x01, resource.Enemy.Condition);
        Assert.Equal(84, resource.Enemy.Speed);
        Assert.Equal(600, resource.Enemy.HP.Current);
        Assert.Equal(672, resource.Enemy.HP.Max);
    }
}
