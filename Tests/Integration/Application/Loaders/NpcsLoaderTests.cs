namespace Tests.Integration.Application.Loaders;

using Backend.Application.Loaders;
using Backend.Memory.Readers;
using Moq;
using Xunit;

public class NpcsLoaderTests : LoaderIntegrationTestBase
{
    [Fact]
    public void Load_ShouldIntegrateNpcsAddressesAndReader()
    {
        var addressesRepository = CreateAddressesRepository();
        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(It.IsAny<long>(), 1)).Returns([0]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B3DF, 1)).Returns([(byte)0x20]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B39A, 1)).Returns([(byte)0xFF]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B39B, 1)).Returns([(byte)0xFF]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B39C, 1)).Returns([(byte)0x01]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x00048DB9, 1)).Returns([(byte)0x01]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x00048DBA, 1)).Returns([(byte)0x01]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x00048DBB, 1)).Returns([(byte)0x01]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B3E0, 1)).Returns([(byte)0x40]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B3E1, 1)).Returns([(byte)0x40]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B3E5, 1)).Returns([(byte)0x08]);

        var npcsReader = new NpcsReader(memoryReaderMock.Object);
        var npcsLoader = new NpcsLoader(addressesRepository, npcsReader);

        var resource = npcsLoader.Load();

        Assert.Equal(0x20, resource.Genji.Battles.Single(battle => battle.Id == "first").Value);
        Assert.Equal(0x01, resource.Genji.Battles.Single(battle => battle.Id == "second").Value);
        Assert.Equal(0x02, Assert.Single(resource.Natsumi.Battles).Value);
        Assert.Equal(0x08, Assert.Single(resource.Catherine.Battles).Value);
        Assert.Equal(0x20, Assert.Single(resource.Robert.Battles).Value);
        Assert.Equal(0x02, Assert.Single(resource.Chris.Battles).Value);
        Assert.Equal(0x01, Assert.Single(resource.Tomomi.Battles).Value);
        Assert.Equal(0x04, Assert.Single(resource.Mitch.Battles).Value);
        Assert.Equal(0x80, Assert.Single(resource.Bob.Battles).Value);
        Assert.Equal(0x04, Assert.Single(resource.Andy.Battles).Value);
        Assert.Equal(0x08, Assert.Single(resource.George.Battles).Value);
        Assert.Equal(0x10, Assert.Single(resource.MeiLin.Battles).Value);
        Assert.Equal(0x20, Assert.Single(resource.Jessica.Battles).Value);
        Assert.Equal(0x40, Assert.Single(resource.Gordon.Battles).Value);
        Assert.Equal(0x80, Assert.Single(resource.Alice.Battles).Value);
        Assert.Equal(0x01, Assert.Single(resource.Nakano.Battles).Value);
        Assert.Equal(0x01, Assert.Single(resource.SeiryuLeader.Battles).Value);
        Assert.Equal(0x40, Assert.Single(resource.Keith.Battles).Value);
        Assert.Equal(0x01, Assert.Single(resource.SuzakuLeader.Battles).Value);
        Assert.Equal(0x40, Assert.Single(resource.FakeByakkoLeader.Battles).Value);
        Assert.Equal(0x01, Assert.Single(resource.ByakkoLeader.Battles).Value);
        Assert.Equal(0x08, Assert.Single(resource.AoaAttacker.Battles).Value);
    }
}
