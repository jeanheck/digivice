namespace Tests.Memory.Readers.Battles;

using Backend.Memory.Addresses;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Battles;
using Backend.Memory.Resources.Battles;
using Moq;
using Xunit;

public class DigimonBattleReaderTests
{
    [Fact]
    public void Read_ShouldMapFieldAndDelegateEnemy()
    {
        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x000A4530, 1)).Returns([0x02]);

        var enemyResource = new EnemyResource { Id = 122, GroupId = 201 };
        var enemyReaderMock = new Mock<IEnemyReader>();
        var enemyAddresses = new EnemyAddresses();
        enemyReaderMock.Setup(r => r.Read(enemyAddresses)).Returns(enemyResource);

        var digimonBattleAddresses = new DigimonBattleAddresses { Field = 0x000A4530 };
        var reader = new DigimonBattleReader(memoryReaderMock.Object, enemyReaderMock.Object);

        var result = reader.Read(digimonBattleAddresses, enemyAddresses);

        Assert.Equal(0x02, result.Field);
        Assert.Same(enemyResource, result.Enemy);
        enemyReaderMock.Verify(r => r.Read(enemyAddresses), Times.Once);
    }

    [Fact]
    public void Read_ShouldDefaultFieldToZero_WhenBytesEmpty()
    {
        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x000A4530, 1)).Returns([]);

        var enemyReaderMock = new Mock<IEnemyReader>();
        enemyReaderMock.Setup(r => r.Read(It.IsAny<EnemyAddresses>())).Returns(new EnemyResource());

        var reader = new DigimonBattleReader(memoryReaderMock.Object, enemyReaderMock.Object);

        var result = reader.Read(
            new DigimonBattleAddresses { Field = 0x000A4530 },
            new EnemyAddresses());

        Assert.Equal(0, result.Field);
    }
}
