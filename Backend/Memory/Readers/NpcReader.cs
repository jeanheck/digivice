using Backend.Memory.Addresses;
using Backend.Memory.Readers.Helpers;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers
{
    public class NpcReader(IMemoryReader memoryReader) : INpcReader
    {
        public NpcResource Read(KeyValuePair<string, NpcAddresses> npcAddresses)
        {
            return new NpcResource
            {
                Id = npcAddresses.Key,
                DigimonBattles = ReadBattles(npcAddresses.Value.DigimonBattles),
                CardBattles = ReadBattles(npcAddresses.Value.CardBattles),
            };
        }

        private List<NpcBattleResource> ReadBattles(Dictionary<string, NpcBattleAddresses> battles)
        {
            return [.. battles.Select(battle => new NpcBattleResource
            {
                Id = battle.Key,
                Value = FlagByteHelper.Read(memoryReader, battle.Value.Address, battle.Value.BitMask),
            })];
        }
    }
}
