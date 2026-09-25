namespace Tests.Memory.Readers;

using Backend.Memory.Addresses;
using Backend.Memory.Readers;
using Moq;
using Xunit;
using Backend.Memory.Readers.Interfaces;

public class NpcsReaderTests
{
    [Fact]
    public void Read_ShouldExtractBattleValue_FromAddressAndBitMask()
    {
        var addresses = new NpcsAddresses
        {
            Catherine = new NpcAddresses
            {
                Battles = new Dictionary<string, NpcBattleAddresses>
                {
                    ["first"] = new NpcBattleAddresses { Address = 0x0004B39A, BitMask = 0x08 },
                },
            },
        };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B39A, 1)).Returns([(byte)0x7B]);

        var reader = new NpcsReader(memoryReaderMock.Object);

        var result = reader.Read(addresses);

        var battle = Assert.Single(result.Catherine.Battles);
        Assert.Equal("first", battle.Id);
        Assert.Equal(0x08, battle.Value);
        memoryReaderMock.Verify(memoryReader => memoryReader.ReadBytes(0x0004B39A, 1), Times.Once);
    }

    [Fact]
    public void Read_ShouldReturnZeroValue_WhenBitMaskIsNotSet()
    {
        var addresses = new NpcsAddresses
        {
            Lucia = new NpcAddresses
            {
                Battles = new Dictionary<string, NpcBattleAddresses>
                {
                    ["first"] = new NpcBattleAddresses { Address = 0x0004B39A, BitMask = 0x10 },
                },
            },
        };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B39A, 1)).Returns([(byte)0x0B]);

        var reader = new NpcsReader(memoryReaderMock.Object);

        var result = reader.Read(addresses);

        var battle = Assert.Single(result.Lucia.Battles);
        Assert.Equal(0x00, battle.Value);
    }

    [Fact]
    public void Read_ShouldReadSecondByte_WhenBattleUses0x4B39B()
    {
        var addresses = new NpcsAddresses
        {
            Chris = new NpcAddresses
            {
                Battles = new Dictionary<string, NpcBattleAddresses>
                {
                    ["first"] = new NpcBattleAddresses { Address = 0x0004B39B, BitMask = 0x02 },
                },
            },
        };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B39B, 1)).Returns([(byte)0x03]);

        var reader = new NpcsReader(memoryReaderMock.Object);

        var result = reader.Read(addresses);

        var battle = Assert.Single(result.Chris.Battles);
        Assert.Equal(0x02, battle.Value);
        memoryReaderMock.Verify(memoryReader => memoryReader.ReadBytes(0x0004B39B, 1), Times.Once);
    }

    [Fact]
    public void Read_ShouldReadThirdByte_WhenBattleUses0x4B39C()
    {
        var addresses = new NpcsAddresses
        {
            Nakano = new NpcAddresses
            {
                Battles = new Dictionary<string, NpcBattleAddresses>
                {
                    ["first"] = new NpcBattleAddresses { Address = 0x0004B39C, BitMask = 0x01 },
                },
            },
        };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B39C, 1)).Returns([(byte)0x01]);

        var reader = new NpcsReader(memoryReaderMock.Object);

        var result = reader.Read(addresses);

        var battle = Assert.Single(result.Nakano.Battles);
        Assert.Equal(0x01, battle.Value);
        memoryReaderMock.Verify(memoryReader => memoryReader.ReadBytes(0x0004B39C, 1), Times.Once);
    }

    [Fact]
    public void Read_ShouldRead48DbxByte_WhenBattleUses0x48DB9()
    {
        var addresses = new NpcsAddresses
        {
            SeiryuLeader = new NpcAddresses
            {
                Battles = new Dictionary<string, NpcBattleAddresses>
                {
                    ["first"] = new NpcBattleAddresses { Address = 0x00048DB9, BitMask = 0x01 },
                },
            },
        };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x00048DB9, 1)).Returns([(byte)0x01]);

        var reader = new NpcsReader(memoryReaderMock.Object);

        var result = reader.Read(addresses);

        var battle = Assert.Single(result.SeiryuLeader.Battles);
        Assert.Equal(0x01, battle.Value);
        memoryReaderMock.Verify(memoryReader => memoryReader.ReadBytes(0x00048DB9, 1), Times.Once);
    }

    [Fact]
    public void Read_ShouldReadMultipleBattles_ForSameNpc()
    {
        var addresses = new NpcsAddresses
        {
            Genji = new NpcAddresses
            {
                Battles = new Dictionary<string, NpcBattleAddresses>
                {
                    ["first"] = new NpcBattleAddresses { Address = 0x0004B3DF, BitMask = 0x20 },
                    ["second"] = new NpcBattleAddresses { Address = 0x0004B39A, BitMask = 0x01 },
                },
            },
        };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B3DF, 1)).Returns([(byte)0xA0]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B39A, 1)).Returns([(byte)0x03]);

        var reader = new NpcsReader(memoryReaderMock.Object);

        var result = reader.Read(addresses);

        Assert.Equal(2, result.Genji.Battles.Count);
        Assert.Equal(0x20, result.Genji.Battles.Single(battle => battle.Id == "first").Value);
        Assert.Equal(0x01, result.Genji.Battles.Single(battle => battle.Id == "second").Value);
    }
}
