namespace Tests.Memory.Readers.Parties.Digimons;

using System;
using Backend.Memory.Addresses.Parties;
using Backend.Memory.Addresses.Parties.Digimons;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Parties.Digimons;
using Backend.Memory.Resources.Parties.Digimons;
using Moq;
using Xunit;

public class DigimonReaderTests
{
    private static void WriteInt16(byte[] block, int offset, short value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, block, offset, bytes.Length);
    }

    private static void WriteInt32(byte[] block, int offset, int value)
    {
        var bytes = BitConverter.GetBytes(value);
        Array.Copy(bytes, 0, block, offset, bytes.Length);
    }

    [Fact]
    public void Read_ShouldReturnNull_WhenMemoryBlockIsTooShort()
    {
        var address = new DigimonAddress { Id = 1, Address = 0x800100 };
        var statusAddresses = new DigimonStatusAddresses();

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x800100, 1500)).Returns(new byte[1000]);

        var slotReaderMock = new Mock<IDigievolutionSlotReader>();
        var evolutionReaderMock = new Mock<IDigievolutionReader>();
        var storedDigievolutionReaderMock = new Mock<IStoredDigievolutionReader>();

        var reader = new DigimonReader(
            memoryReaderMock.Object,
            slotReaderMock.Object,
            evolutionReaderMock.Object,
            storedDigievolutionReaderMock.Object
        );

        var result = reader.Read(address, statusAddresses);

        Assert.Null(result);
    }

    [Fact]
    public void Read_ShouldCorrectlyMapDigimonFields_WhenBlockIsFullyLoaded()
    {
        // Arrange
        var address = new DigimonAddress { Id = 1, Address = 0x800100 };

        var statusAddresses = new DigimonStatusAddresses
        {
            Experience = 10,
            Level = 14,
            Vitals = new VitalsAddresses { CurrentHP = 16, MaxHP = 18, CurrentMP = 20, MaxMP = 22 },
            Attributes = new AttributesAddresses { Strength = 24, Defense = 26, Spirit = 28, Wisdow = 30, Speed = 32, Charisma = 34 },
            Resistances = new ResistancesAddresses { Fire = 36, Water = 38, Ice = 40, Wind = 42, Thunder = 44, Machine = 46, Dark = 48 },
            Equipaments = new EquipmentsAddresses { Head = 50, Body = 52, Right = 54, Left = 56, Accessory1 = 58, Accessory2 = 60 },
            Digievolutions = new DigievolutionsAddresses
            {
                ActiveDigievolution = 200,
                Slots = []
            }
        };

        var block = new byte[1500];
        WriteInt32(block, 10, 85000); // Experience
        WriteInt16(block, 14, 45);    // Level

        // Vitals
        WriteInt16(block, 16, 500);   // CurrentHP
        WriteInt16(block, 18, 1000);  // MaxHP
        WriteInt16(block, 20, 200);   // CurrentMP
        WriteInt16(block, 22, 400);   // MaxMP

        // Attributes
        WriteInt16(block, 24, 80);    // Strength
        WriteInt16(block, 26, 75);    // Defense
        WriteInt16(block, 28, 60);    // Spirit
        WriteInt16(block, 30, 95);    // Wisdom
        WriteInt16(block, 32, 110);   // Speed
        WriteInt16(block, 34, 40);    // Charisma

        // Resistances
        WriteInt16(block, 36, 10);    // Fire
        WriteInt16(block, 38, 20);    // Water
        WriteInt16(block, 40, 30);    // Ice
        WriteInt16(block, 42, 40);    // Wind
        WriteInt16(block, 44, 50);    // Thunder
        WriteInt16(block, 46, 60);    // Machine
        WriteInt16(block, 48, 70);    // Dark

        // Equipments
        WriteInt16(block, 50, 101);   // Head
        WriteInt16(block, 52, 102);   // Body
        WriteInt16(block, 54, 103);   // Right
        WriteInt16(block, 56, 104);   // Left
        WriteInt16(block, 58, 105);   // Accessory1
        WriteInt16(block, 60, 106);   // Accessory2

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x800100, 1500)).Returns(block);
        memoryReaderMock.Setup(m => m.ReadInt16(0x800100 + 200)).Returns((short)3); // ActiveDigievolutionId

        var slotReaderMock = new Mock<IDigievolutionSlotReader>();
        var evolutionReaderMock = new Mock<IDigievolutionReader>();
        var storedDigievolutionReaderMock = new Mock<IStoredDigievolutionReader>();
        storedDigievolutionReaderMock
            .Setup(s => s.Read(It.IsAny<MemoryBlockReader>(), statusAddresses.Digievolutions))
            .Returns([]);

        var reader = new DigimonReader(
            memoryReaderMock.Object,
            slotReaderMock.Object,
            evolutionReaderMock.Object,
            storedDigievolutionReaderMock.Object
        );

        // Act
        var result = reader.Read(address, statusAddresses);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.ActiveDigievolutionId);
        Assert.Equal(85000, result.Experience);
        Assert.Equal(45, result.Level);

        // Vitals
        Assert.Equal(500, result.Vitals.CurrentHP);
        Assert.Equal(1000, result.Vitals.MaxHP);
        Assert.Equal(200, result.Vitals.CurrentMP);
        Assert.Equal(400, result.Vitals.MaxMP);

        // Attributes
        Assert.Equal(80, result.Attributes.Strength);
        Assert.Equal(75, result.Attributes.Defense);
        Assert.Equal(60, result.Attributes.Spirit);
        Assert.Equal(95, result.Attributes.Wisdow);
        Assert.Equal(110, result.Attributes.Speed);
        Assert.Equal(40, result.Attributes.Charisma);

        // Resistances
        Assert.Equal(10, result.Resistances.Fire);
        Assert.Equal(20, result.Resistances.Water);
        Assert.Equal(30, result.Resistances.Ice);
        Assert.Equal(40, result.Resistances.Wind);
        Assert.Equal(50, result.Resistances.Thunder);
        Assert.Equal(60, result.Resistances.Machine);
        Assert.Equal(70, result.Resistances.Dark);

        // Equipments
        Assert.Equal(101, result.Equipments.Head);
        Assert.Equal(102, result.Equipments.Body);
        Assert.Equal(103, result.Equipments.Right);
        Assert.Equal(104, result.Equipments.Left);
        Assert.Equal(105, result.Equipments.Accessory1);
        Assert.Equal(106, result.Equipments.Accessory2);
    }

    [Fact]
    public void Read_ShouldFallbackToZeroActiveDigievolutionId_WhenMemoryReaderReturnsNull()
    {
        // Arrange
        var address = new DigimonAddress { Id = 1, Address = 0x800100 };
        var statusAddresses = new DigimonStatusAddresses
        {
            Experience = 10,
            Level = 14,
            Vitals = new VitalsAddresses(),
            Attributes = new AttributesAddresses(),
            Resistances = new ResistancesAddresses(),
            Equipaments = new EquipmentsAddresses(),
            Digievolutions = new DigievolutionsAddresses
            {
                ActiveDigievolution = 200,
                Slots = []
            }
        };

        var block = new byte[1500];
        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x800100, 1500)).Returns(block);
        memoryReaderMock.Setup(m => m.ReadInt16(0x800100 + 200)).Returns((short?)null); // Retorna nulo

        var slotReaderMock = new Mock<IDigievolutionSlotReader>();
        var evolutionReaderMock = new Mock<IDigievolutionReader>();
        var storedDigievolutionReaderMock = new Mock<IStoredDigievolutionReader>();
        storedDigievolutionReaderMock
            .Setup(s => s.Read(It.IsAny<MemoryBlockReader>(), statusAddresses.Digievolutions))
            .Returns([]);

        var reader = new DigimonReader(
            memoryReaderMock.Object,
            slotReaderMock.Object,
            evolutionReaderMock.Object,
            storedDigievolutionReaderMock.Object
        );

        // Act
        var result = reader.Read(address, statusAddresses);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.ActiveDigievolutionId); // Deve fazer fallback para 0
    }

    [Fact]
    public void Read_ShouldMapEvolutionSlotsAndResources_WhenSlotsArePresent()
    {
        // Arrange
        var address = new DigimonAddress { Id = 1, Address = 0x800100 };
        var slotAddress1 = new SlotAddresses { Index = 0, Address = 300 };
        var slotAddress2 = new SlotAddresses { Index = 1, Address = 304 };

        var statusAddresses = new DigimonStatusAddresses
        {
            Experience = 10,
            Level = 14,
            Vitals = new VitalsAddresses(),
            Attributes = new AttributesAddresses(),
            Resistances = new ResistancesAddresses(),
            Equipaments = new EquipmentsAddresses(),
            Digievolutions = new DigievolutionsAddresses
            {
                ActiveDigievolution = 200,
                Slots = [slotAddress1, slotAddress2]
            }
        };

        var block = new byte[1500];
        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x800100, 1500)).Returns(block);
        memoryReaderMock.Setup(m => m.ReadInt16(0x800100 + 200)).Returns((short)5);

        var slotResource1 = new DigievolutionSlotResource { Index = 0, DigievolutionId = 12 };
        var slotResource2 = new DigievolutionSlotResource { Index = 1, DigievolutionId = 15 };

        var slotReaderMock = new Mock<IDigievolutionSlotReader>();
        slotReaderMock.Setup(s => s.Read(It.IsAny<MemoryBlockReader>(), slotAddress1)).Returns(slotResource1);
        slotReaderMock.Setup(s => s.Read(It.IsAny<MemoryBlockReader>(), slotAddress2)).Returns(slotResource2);

        var evolutionResource1 = new DigievolutionResource { Level = 10, Dvxp = 500 };
        var evolutionResource2 = new DigievolutionResource { Level = 25, Dvxp = 1200 };

        var evolutionReaderMock = new Mock<IDigievolutionReader>();
        evolutionReaderMock.Setup(e => e.Read(It.IsAny<MemoryBlockReader>(), 12, statusAddresses.Digievolutions)).Returns(evolutionResource1);
        evolutionReaderMock.Setup(e => e.Read(It.IsAny<MemoryBlockReader>(), 15, statusAddresses.Digievolutions)).Returns(evolutionResource2);

        var storedDigievolutions = new List<StoredDigievolutionResource>
        {
            new() { DigievolutionId = 12, Level = 10 },
            new() { DigievolutionId = 15, Level = 25 },
            new() { DigievolutionId = 99, Level = 5 }
        };

        var storedDigievolutionReaderMock = new Mock<IStoredDigievolutionReader>();
        storedDigievolutionReaderMock
            .Setup(s => s.Read(It.IsAny<MemoryBlockReader>(), statusAddresses.Digievolutions))
            .Returns(storedDigievolutions);

        var reader = new DigimonReader(
            memoryReaderMock.Object,
            slotReaderMock.Object,
            evolutionReaderMock.Object,
            storedDigievolutionReaderMock.Object
        );

        // Act
        var result = reader.Read(address, statusAddresses);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Digievolutions.Count);
        Assert.Equal(slotResource1, result.Digievolutions[0]);
        Assert.Equal(evolutionResource1, result.Digievolutions[0].DigievolutionResource);
        Assert.Equal(slotResource2, result.Digievolutions[1]);
        Assert.Equal(evolutionResource2, result.Digievolutions[1].DigievolutionResource);
        Assert.Equal(3, result.StoredDigievolutions.Count);
        Assert.Equal(99, result.StoredDigievolutions[2].DigievolutionId);
        Assert.Equal(5, result.StoredDigievolutions[2].Level);
    }
}
