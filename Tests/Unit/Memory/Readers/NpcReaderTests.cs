namespace Tests.Memory.Readers;

using Backend.Memory.Addresses;
using Backend.Memory.Readers;
using Moq;
using Xunit;

public class NpcReaderTests
{
    [Fact]
    public void Read_ShouldExtractDigimonBattleValue_FromAddressAndBitMask()
    {
        var npcEntry = new KeyValuePair<string, NpcAddresses>(
            "catherine",
            new NpcAddresses
            {
                DigimonBattles = new Dictionary<string, NpcBattleAddresses>
                {
                    ["first"] = new NpcBattleAddresses { Address = 0x0004B39A, BitMask = 0x08 },
                },
            }
        );

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B39A, 1)).Returns([(byte)0x7B]);

        var reader = new NpcReader(memoryReaderMock.Object);

        var result = reader.Read(npcEntry);

        Assert.Equal("catherine", result.Id);
        var battle = Assert.Single(result.DigimonBattles);
        Assert.Equal("first", battle.Id);
        Assert.Equal(0x08, battle.Value);
        memoryReaderMock.Verify(memoryReader => memoryReader.ReadBytes(0x0004B39A, 1), Times.Once);
    }

    [Fact]
    public void Read_ShouldReturnZeroValue_WhenBitMaskIsNotSet()
    {
        var npcEntry = new KeyValuePair<string, NpcAddresses>(
            "lucia",
            new NpcAddresses
            {
                DigimonBattles = new Dictionary<string, NpcBattleAddresses>
                {
                    ["first"] = new NpcBattleAddresses { Address = 0x0004B39A, BitMask = 0x10 },
                },
            }
        );

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B39A, 1)).Returns([(byte)0x0B]);

        var reader = new NpcReader(memoryReaderMock.Object);

        var result = reader.Read(npcEntry);

        var battle = Assert.Single(result.DigimonBattles);
        Assert.Equal(0x00, battle.Value);
    }

    [Fact]
    public void Read_ShouldReadSecondByte_WhenBattleUses0x4B39B()
    {
        var npcEntry = new KeyValuePair<string, NpcAddresses>(
            "chris",
            new NpcAddresses
            {
                DigimonBattles = new Dictionary<string, NpcBattleAddresses>
                {
                    ["first"] = new NpcBattleAddresses { Address = 0x0004B39B, BitMask = 0x02 },
                },
            }
        );

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B39B, 1)).Returns([(byte)0x03]);

        var reader = new NpcReader(memoryReaderMock.Object);

        var result = reader.Read(npcEntry);

        var battle = Assert.Single(result.DigimonBattles);
        Assert.Equal(0x02, battle.Value);
        memoryReaderMock.Verify(memoryReader => memoryReader.ReadBytes(0x0004B39B, 1), Times.Once);
    }

    [Fact]
    public void Read_ShouldReadThirdByte_WhenBattleUses0x4B39C()
    {
        var npcEntry = new KeyValuePair<string, NpcAddresses>(
            "nakano",
            new NpcAddresses
            {
                DigimonBattles = new Dictionary<string, NpcBattleAddresses>
                {
                    ["first"] = new NpcBattleAddresses { Address = 0x0004B39C, BitMask = 0x01 },
                },
            }
        );

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B39C, 1)).Returns([(byte)0x01]);

        var reader = new NpcReader(memoryReaderMock.Object);

        var result = reader.Read(npcEntry);

        var battle = Assert.Single(result.DigimonBattles);
        Assert.Equal(0x01, battle.Value);
        memoryReaderMock.Verify(memoryReader => memoryReader.ReadBytes(0x0004B39C, 1), Times.Once);
    }

    [Fact]
    public void Read_ShouldReadMultipleBattles_ForSameNpc()
    {
        var npcEntry = new KeyValuePair<string, NpcAddresses>(
            "genji",
            new NpcAddresses
            {
                DigimonBattles = new Dictionary<string, NpcBattleAddresses>
                {
                    ["first"] = new NpcBattleAddresses { Address = 0x0004B3DF, BitMask = 0x20 },
                    ["second"] = new NpcBattleAddresses { Address = 0x0004B39A, BitMask = 0x01 },
                },
            }
        );

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B3DF, 1)).Returns([(byte)0xA0]);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B39A, 1)).Returns([(byte)0x03]);

        var reader = new NpcReader(memoryReaderMock.Object);

        var result = reader.Read(npcEntry);

        Assert.Equal(2, result.DigimonBattles.Count);
        Assert.Equal(0x20, result.DigimonBattles.Single(battle => battle.Id == "first").Value);
        Assert.Equal(0x01, result.DigimonBattles.Single(battle => battle.Id == "second").Value);
    }
}
