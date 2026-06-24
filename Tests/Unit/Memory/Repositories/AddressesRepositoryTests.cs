namespace Tests.Memory.Repositories;

using System;
using System.IO;
using System.Text.Json;
using Xunit;
using Backend.Memory.Repositories;
using Backend.Memory.Addresses;
using Backend.Memory.Addresses.Parties;
using Backend.Memory.Addresses.Journals;

public class AddressesRepositoryTests : IDisposable
{
    private readonly string tempDirectoryPath;
    private readonly AddressesRepository repository;

    public AddressesRepositoryTests()
    {
        tempDirectoryPath = Path.Combine(Path.GetTempPath(), "DigiviceTests_" + Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDirectoryPath);

        // Criar estrutura de subdiretórios esperada
        Directory.CreateDirectory(Path.Combine(tempDirectoryPath, "Parties"));
        Directory.CreateDirectory(Path.Combine(tempDirectoryPath, "Quests"));
        Directory.CreateDirectory(Path.Combine(tempDirectoryPath, "Quests", "SideQuests"));

        repository = new AddressesRepository(tempDirectoryPath);
    }

    [Fact]
    public void GetPlayerAddresses_ShouldLoadAndDeserializeCorrectly()
    {
        // Arrange
        var fakePlayer = new PlayerAddresses
        {
            NameBufferSize = 10,
            Name = 0x00048D88,
            Bits = 0x00048DA0,
            MapId = 0x0004B3F8
        };
        var json = JsonSerializer.Serialize(fakePlayer);
        File.WriteAllText(Path.Combine(tempDirectoryPath, "PlayerAddresses.json"), json);

        // Act
        var result = repository.GetPlayerAddresses();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10, result.NameBufferSize);
        Assert.Equal(0x00048D88, result.Name);
        Assert.Equal(0x00048DA0, result.Bits);
        Assert.Equal(0x0004B3F8, result.MapId);
    }

    [Fact]
    public void GetPartyAddresses_ShouldLoadAndDeserializeCorrectly()
    {
        // Arrange
        var fakeParty = new PartyAddresses
        {
            BytesPerSlot = 256,
            EmptySlotId = 99,
            Slots = []
        };
        var json = JsonSerializer.Serialize(fakeParty);
        File.WriteAllText(Path.Combine(tempDirectoryPath, "PartyAddresses.json"), json);

        // Act
        var result = repository.GetPartyAddresses();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(256, result.BytesPerSlot);
        Assert.Equal(99, result.EmptySlotId);
    }

    [Fact]
    public void GetDigimonStatusAddresses_ShouldLoadAndDeserializeCorrectly()
    {
        // Arrange
        var fakeStatus = new DigimonStatusAddresses();
        var json = JsonSerializer.Serialize(fakeStatus);
        File.WriteAllText(Path.Combine(tempDirectoryPath, "Parties", "DigimonStatusAddresses.json"), json);

        // Act
        var result = repository.GetDigimonStatusAddresses();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetDigimonsAddresses_ShouldLoadAndDeserializeCorrectly()
    {
        // Arrange
        var fakeDigimons = new DigimonsAddresses
        {
            Digimons = [
                new DigimonAddress { Id = 1, Name = "Agumon", Address = 0x800100 },
                new DigimonAddress { Id = 2, Name = "Gabumon", Address = 0x800200 }
            ]
        };
        var json = JsonSerializer.Serialize(fakeDigimons);
        File.WriteAllText(Path.Combine(tempDirectoryPath, "Parties", "DigimonsAddresses.json"), json);

        // Act
        var result = repository.GetDigimonsAddresses();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Digimons.Count);
        Assert.Equal("Agumon", result.Digimons[0].Name);
        Assert.Equal(0x800100, result.Digimons[0].Address);
    }

    [Fact]
    public void GetDigimonAddressById_ShouldReturnCorrectAddress_WhenIdExists()
    {
        // Arrange
        var fakeDigimons = new DigimonsAddresses
        {
            Digimons = [
                new DigimonAddress { Id = 3, Name = "Patamon", Address = 0x800300 }
            ]
        };
        var json = JsonSerializer.Serialize(fakeDigimons);
        File.WriteAllText(Path.Combine(tempDirectoryPath, "Parties", "DigimonsAddresses.json"), json);

        // Act
        var result = repository.GetDigimonAddressById(3);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Patamon", result!.Name);
        Assert.Equal(0x800300, result.Address);
    }

    [Fact]
    public void GetDigimonAddressById_ShouldReturnNull_WhenIdDoesNotExist()
    {
        // Arrange
        var fakeDigimons = new DigimonsAddresses { Digimons = [] };
        var json = JsonSerializer.Serialize(fakeDigimons);
        File.WriteAllText(Path.Combine(tempDirectoryPath, "Parties", "DigimonsAddresses.json"), json);

        // Act
        var result = repository.GetDigimonAddressById(99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetMainQuest_ShouldLoadAndDeserializeCorrectly()
    {
        // Arrange
        var fakeMainQuest = new QuestAddresses { Id = "Main1" };
        var json = JsonSerializer.Serialize(fakeMainQuest);
        File.WriteAllText(Path.Combine(tempDirectoryPath, "Quests", "MainQuestAddresses.json"), json);

        // Act
        var result = repository.GetMainQuest();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Main1", result.Id);
    }

    [Fact]
    public void GetAllSideQuests_ShouldLoadAllSideQuestsCorrectly()
    {
        // Arrange
        var side1 = new QuestAddresses { Id = "FolderBag" };
        var side2 = new QuestAddresses { Id = "TreeBoots" };
        var side3 = new QuestAddresses { Id = "FishingPole" };

        File.WriteAllText(Path.Combine(tempDirectoryPath, "Quests", "SideQuests", "FolderBagAddresses.json"), JsonSerializer.Serialize(side1));
        File.WriteAllText(Path.Combine(tempDirectoryPath, "Quests", "SideQuests", "TreeBootsAddresses.json"), JsonSerializer.Serialize(side2));
        File.WriteAllText(Path.Combine(tempDirectoryPath, "Quests", "SideQuests", "FishingPoleAddresses.json"), JsonSerializer.Serialize(side3));

        // Act
        var result = repository.GetAllSideQuests();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Equal("FolderBag", result[0].Id);
        Assert.Equal("TreeBoots", result[1].Id);
        Assert.Equal("FishingPole", result[2].Id);
    }

    [Fact]
    public void GetPlayerAddresses_ShouldCacheLoadedInstance()
    {
        // Arrange
        var fakePlayer = new PlayerAddresses { NameBufferSize = 10 };
        var json = JsonSerializer.Serialize(fakePlayer);
        File.WriteAllText(Path.Combine(tempDirectoryPath, "PlayerAddresses.json"), json);

        // Act
        var firstResult = repository.GetPlayerAddresses();
        var secondResult = repository.GetPlayerAddresses();

        // Assert
        Assert.NotNull(firstResult);
        Assert.Same(firstResult, secondResult); // Valida que aponta para o mesmo objeto em memória (Cache)
    }

    [Fact]
    public void GetPlayerAddresses_ShouldThrowFileNotFoundException_WhenFileIsMissing()
    {
        // Act & Assert
        Assert.Throws<FileNotFoundException>(() => repository.GetPlayerAddresses());
    }

    [Fact]
    public void GetPlayerAddresses_ShouldReturnEmptyInstance_WhenJsonIsNull()
    {
        // Arrange (Escrevemos "null" no arquivo JSON)
        File.WriteAllText(Path.Combine(tempDirectoryPath, "PlayerAddresses.json"), "null");

        // Act
        var result = repository.GetPlayerAddresses();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.NameBufferSize);
        Assert.Equal(0, result.Name);
    }

    [Fact]
    public void GetAllSideQuests_ShouldThrowFileNotFoundException_WhenAnySideQuestFileIsMissing()
    {
        // Arrange (Escrevemos apenas duas das três side quests esperadas)
        var side1 = new QuestAddresses { Id = "FolderBag" };
        var side2 = new QuestAddresses { Id = "TreeBoots" };

        File.WriteAllText(Path.Combine(tempDirectoryPath, "Quests", "SideQuests", "FolderBagAddresses.json"), JsonSerializer.Serialize(side1));
        File.WriteAllText(Path.Combine(tempDirectoryPath, "Quests", "SideQuests", "TreeBootsAddresses.json"), JsonSerializer.Serialize(side2));
        // Omitimos FishingPoleAddresses.json de propósito

        // Act & Assert
        Assert.Throws<FileNotFoundException>(() => repository.GetAllSideQuests());
    }

    public void Dispose()
    {
        if (Directory.Exists(tempDirectoryPath))
        {
            Directory.Delete(tempDirectoryPath, recursive: true);
        }
    }
}
