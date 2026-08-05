namespace Tests.Integration.Application.Loaders.Parties;

using Backend.Application.Loaders.Parties;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Parties;
using Backend.Memory.Resources.Parties;
using Moq;
using Tests.Integration.Application.Loaders;
using Xunit;

public class DigimonInCombatLoaderTests : LoaderIntegrationTestBase
{
    [Fact]
    public void Load_ShouldIntegrateAddressesAndReaderPipeline()
    {
        var addressesRepository = CreateAddressesRepository();
        var memoryReaderMock = new Mock<IMemoryReader>();

        memoryReaderMock.Setup(m => m.ReadInt16(0x0004B3F8)).Returns((short)DigimonInCombatResource.CombatMapId);
        memoryReaderMock.Setup(m => m.ReadInt16(0x000A4470)).Returns((short)386);
        memoryReaderMock.Setup(m => m.ReadInt16(0x000A4470 + 0x06)).Returns((short)1850);
        memoryReaderMock.Setup(m => m.ReadInt16(0x000A4470 + 0x08)).Returns((short)1400);
        memoryReaderMock.Setup(m => m.ReadInt16(0x000A4470 + 0x0A)).Returns((short)1140);
        memoryReaderMock.Setup(m => m.ReadInt16(0x000A4470 + 0x0C)).Returns((short)900);
        memoryReaderMock.Setup(m => m.ReadBytes(0x000A4470 + 0x1C, 1)).Returns([0x04]);

        var digimonInCombatLoader = new DigimonInCombatLoader(
            addressesRepository,
            new DigimonInCombatReader(memoryReaderMock.Object));

        var digimonInCombatResource = digimonInCombatLoader.Load(0);

        Assert.True(digimonInCombatResource.IsCombatMap);
        Assert.True(digimonInCombatResource.IsInCombat);
        Assert.Equal(DigimonInCombatResource.CombatMapId, digimonInCombatResource.MapId);
        Assert.Equal(386, digimonInCombatResource.Id);
        Assert.Equal(1400, digimonInCombatResource.HP.Current);
        Assert.Equal(1850, digimonInCombatResource.HP.Max);
        Assert.Equal(900, digimonInCombatResource.MP.Current);
        Assert.Equal(1140, digimonInCombatResource.MP.Max);
        Assert.Equal(0x04, digimonInCombatResource.Condition);
    }

    [Fact]
    public void Load_ShouldReportNotInCombat_WhenMapIdIsNotCombatMap()
    {
        var addressesRepository = CreateAddressesRepository();
        var memoryReaderMock = new Mock<IMemoryReader>();

        memoryReaderMock.Setup(m => m.ReadInt16(0x0004B3F8)).Returns((short)0x0100);
        memoryReaderMock.Setup(m => m.ReadInt16(0x000A4470)).Returns((short)386);
        memoryReaderMock.Setup(m => m.ReadInt16(0x000A4470 + 0x06)).Returns((short)1850);
        memoryReaderMock.Setup(m => m.ReadInt16(0x000A4470 + 0x08)).Returns((short)1400);
        memoryReaderMock.Setup(m => m.ReadInt16(0x000A4470 + 0x0A)).Returns((short)1140);
        memoryReaderMock.Setup(m => m.ReadInt16(0x000A4470 + 0x0C)).Returns((short)900);
        memoryReaderMock.Setup(m => m.ReadBytes(0x000A4470 + 0x1C, 1)).Returns([0x04]);

        var digimonInCombatLoader = new DigimonInCombatLoader(
            addressesRepository,
            new DigimonInCombatReader(memoryReaderMock.Object));

        var digimonInCombatResource = digimonInCombatLoader.Load(0);

        Assert.False(digimonInCombatResource.IsCombatMap);
        Assert.False(digimonInCombatResource.IsInCombat);
        Assert.Equal(386, digimonInCombatResource.Id);
        Assert.Equal(0x04, digimonInCombatResource.Condition);
    }

    [Fact]
    public void Load_ShouldReportNotInCombat_WhenCombatMapButHpMaxIsZero()
    {
        var addressesRepository = CreateAddressesRepository();
        var memoryReaderMock = new Mock<IMemoryReader>();

        memoryReaderMock.Setup(m => m.ReadInt16(0x0004B3F8)).Returns((short)DigimonInCombatResource.CombatMapId);
        memoryReaderMock.Setup(m => m.ReadInt16(0x000A4470)).Returns((short)386);
        memoryReaderMock.Setup(m => m.ReadInt16(0x000A4470 + 0x06)).Returns((short)0);
        memoryReaderMock.Setup(m => m.ReadInt16(0x000A4470 + 0x08)).Returns((short)0);
        memoryReaderMock.Setup(m => m.ReadInt16(0x000A4470 + 0x0A)).Returns((short)0);
        memoryReaderMock.Setup(m => m.ReadInt16(0x000A4470 + 0x0C)).Returns((short)0);
        memoryReaderMock.Setup(m => m.ReadBytes(0x000A4470 + 0x1C, 1)).Returns([0x00]);

        var digimonInCombatLoader = new DigimonInCombatLoader(
            addressesRepository,
            new DigimonInCombatReader(memoryReaderMock.Object));

        var digimonInCombatResource = digimonInCombatLoader.Load(0);

        Assert.True(digimonInCombatResource.IsCombatMap);
        Assert.False(digimonInCombatResource.IsInCombat);
    }
}
