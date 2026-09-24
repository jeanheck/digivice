namespace Tests.Integration.Memory.Repositories;

using Backend.Memory.Addresses;
using Tests.Integration.Application.Loaders;

public class AddressesRepositoryDefinitionsTests : LoaderIntegrationTestBase
{
    [Fact]
    public void RealDefinitions_ShouldLoadEveryAddressesFile()
    {
        var repository = CreateAddressesRepository();

        Assert.Equal(0x00048DA0, repository.GetPlayerAddresses().Bits);
        Assert.NotEqual(0, repository.GetImportantItemsAddresses().TreeBoots);
        Assert.NotEqual(0, repository.GetCardBattleAddresses().Id);
        Assert.NotEqual(0, repository.GetDigimonBattleAddresses().Field);
        Assert.NotEqual(0, repository.GetEnemyAddresses().EnemySlotBase);
        Assert.Equal(3, repository.GetEnemyAddresses().SlotCount);
        Assert.NotEqual(0, repository.GetInBattleAddresses().AllySlotBase);
        Assert.NotEqual(0, repository.GetAuctionsAddresses().DivineBarrier.Address);

        var partyAddresses = repository.GetPartyAddresses();
        Assert.Equal(0xFF, partyAddresses.EmptySlotId);
        Assert.Equal([1, 2, 3], partyAddresses.Slots.Select(slot => slot.Index));
        Assert.All(partyAddresses.Slots, slot => Assert.NotEqual(0, slot.Address));

        var digimonStatusAddresses = repository.GetDigimonStatusAddresses();
        Assert.NotEqual(0, digimonStatusAddresses.Level);
        Assert.NotEmpty(digimonStatusAddresses.Digievolutions.Slots);

        var digimonsAddresses = repository.GetDigimonsAddresses();
        Assert.Contains(0, digimonsAddresses.Keys);
        Assert.All(digimonsAddresses.Values, digimon => Assert.NotEqual(0, digimon.MemoryBlockAddress));
    }

    [Fact]
    public void RealDefinitions_ShouldLoadEveryNpcWithBattles()
    {
        var npcsAddresses = CreateAddressesRepository().GetNpcsAddresses();

        foreach (var property in typeof(NpcsAddresses).GetProperties())
        {
            var npcAddresses = (NpcAddresses)property.GetValue(npcsAddresses)!;
            Assert.True(npcAddresses.Battles.Count > 0, $"{property.Name} has no battles");
        }
    }

    [Fact]
    public void RealDefinitions_ShouldLoadEveryQuestFolder()
    {
        var repository = CreateAddressesRepository();

        Assert.NotEmpty(repository.GetMainQuest().Steps);
        Assert.Equal(3, repository.GetAllSideQuests().Count);
        Assert.Equal(5, repository.GetAllLegendaryWeapons().Count);
        Assert.Equal(8, repository.GetAllDriAgents().Count);
        Assert.Equal(2, repository.GetAllDuelIsland().Count);
        Assert.All(
            repository.GetAllSideQuests()
                .Concat(repository.GetAllLegendaryWeapons())
                .Concat(repository.GetAllDriAgents())
                .Concat(repository.GetAllDuelIsland()),
            quest =>
            {
                Assert.False(string.IsNullOrEmpty(quest.Id));
                Assert.NotEmpty(quest.Steps);
            });
    }
}
