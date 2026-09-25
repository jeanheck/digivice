namespace Tests.Memory.Repositories;

using System;
using System.IO;
using System.Text.Json;
using Backend.Memory.Addresses;
using Backend.Memory.Addresses.Journals;
using Backend.Memory.Addresses.Parties;
using Backend.Memory.Repositories;
using Xunit;

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
            Bits = 0x00048DA0,
            MapId = 0x0004B3F8,
            PreviousMapId = 0x0004B400,
            SeabedRoute = 0x00048D78,
            MapVariant = 0x00048D7A
        };
        var json = JsonSerializer.Serialize(fakePlayer);
        File.WriteAllText(Path.Combine(tempDirectoryPath, "PlayerAddresses.json"), json);

        // Act
        var result = repository.GetPlayerAddresses();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0x00048DA0, result.Bits);
        Assert.Equal(0x0004B3F8, result.MapId);
        Assert.Equal(0x0004B400, result.PreviousMapId);
        Assert.Equal(0x00048D78, result.SeabedRoute);
        Assert.Equal(0x00048D7A, result.MapVariant);
    }

    [Fact]
    public void GetImportantItemsAddresses_ShouldLoadAndDeserializeCorrectly()
    {
        var fakeImportantItems = new ImportantItemsAddresses
        {
            TreeBoots = 0x00048DB4,
            FishingPole = 0x00048DB5,
            AsukaTrophy = 0x00048DC2,
            SunTrophy = 0x00048DC4
        };
        var json = JsonSerializer.Serialize(fakeImportantItems);
        File.WriteAllText(Path.Combine(tempDirectoryPath, "ImportantItemsAddresses.json"), json);

        var result = repository.GetImportantItemsAddresses();

        Assert.NotNull(result);
        Assert.Equal(0x00048DB4, result.TreeBoots);
        Assert.Equal(0x00048DB5, result.FishingPole);
        Assert.Equal(0x00048DC2, result.AsukaTrophy);
        Assert.Equal(0x00048DC4, result.SunTrophy);
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
        var fakeDigimons = new Dictionary<int, DigimonAddress>
        {
            [1] = new DigimonAddress { Name = "Agumon", MemoryBlockAddress = 0x800100, BlastAddress = 0x00042B76 },
            [2] = new DigimonAddress { Name = "Gabumon", MemoryBlockAddress = 0x800200, BlastAddress = 0x00042B78 }
        };
        var json = JsonSerializer.Serialize(fakeDigimons);
        File.WriteAllText(Path.Combine(tempDirectoryPath, "Parties", "DigimonsAddresses.json"), json);

        // Act
        var result = repository.GetDigimonsAddresses();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Agumon", result[1].Name);
        Assert.Equal(0x800100, result[1].MemoryBlockAddress);
        Assert.Equal(0x00042B76, result[1].BlastAddress);
        Assert.Equal(0x00042B78, result[2].BlastAddress);
    }

    [Fact]
    public void GetDigimonAddressById_ShouldReturnCorrectAddress_WhenIdExists()
    {
        // Arrange
        var fakeDigimons = new Dictionary<int, DigimonAddress>
        {
            [3] = new DigimonAddress { Name = "Patamon", MemoryBlockAddress = 0x800300 }
        };
        var json = JsonSerializer.Serialize(fakeDigimons);
        File.WriteAllText(Path.Combine(tempDirectoryPath, "Parties", "DigimonsAddresses.json"), json);

        // Act
        var result = repository.GetDigimonAddressById(3);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Patamon", result!.Name);
        Assert.Equal(0x800300, result.MemoryBlockAddress);
    }

    [Fact]
    public void GetDigimonAddressById_ShouldReturnNull_WhenIdDoesNotExist()
    {
        // Arrange
        var fakeDigimons = new Dictionary<int, DigimonAddress>();
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
        Assert.Contains(result, quest => quest.Id == "FolderBag");
        Assert.Contains(result, quest => quest.Id == "TreeBoots");
        Assert.Contains(result, quest => quest.Id == "FishingPole");
    }

    [Fact]
    public void GetAllSideQuests_ShouldCacheLoadedList()
    {
        File.WriteAllText(
            Path.Combine(tempDirectoryPath, "Quests", "SideQuests", "FolderBagAddresses.json"),
            JsonSerializer.Serialize(new QuestAddresses { Id = "FolderBag" }));

        var firstResult = repository.GetAllSideQuests();
        var secondResult = repository.GetAllSideQuests();

        Assert.Same(firstResult, secondResult);
    }

    [Fact]
    public void GetAllSideQuests_ShouldLoadOnlyExistingFiles()
    {
        File.WriteAllText(
            Path.Combine(tempDirectoryPath, "Quests", "SideQuests", "FolderBagAddresses.json"),
            JsonSerializer.Serialize(new QuestAddresses { Id = "FolderBag" }));
        File.WriteAllText(
            Path.Combine(tempDirectoryPath, "Quests", "SideQuests", "TreeBootsAddresses.json"),
            JsonSerializer.Serialize(new QuestAddresses { Id = "TreeBoots" }));

        var result = repository.GetAllSideQuests();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, quest => quest.Id == "FolderBag");
        Assert.Contains(result, quest => quest.Id == "TreeBoots");
    }

    [Fact]
    public void GetAllSideQuests_ShouldThrowDirectoryNotFoundException_WhenFolderIsMissing()
    {
        Directory.Delete(Path.Combine(tempDirectoryPath, "Quests", "SideQuests"), recursive: true);

        Assert.Throws<DirectoryNotFoundException>(() => repository.GetAllSideQuests());
    }

    [Fact]
    public void GetPlayerAddresses_ShouldCacheLoadedInstance()
    {
        // Arrange
        var fakePlayer = new PlayerAddresses { Bits = 0x00048DA0 };
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
        Assert.Equal(0, result.Bits);
        Assert.Equal(0, result.MapId);
    }

    public void Dispose()
    {
        if (Directory.Exists(tempDirectoryPath))
        {
            Directory.Delete(tempDirectoryPath, recursive: true);
        }
    }
}
