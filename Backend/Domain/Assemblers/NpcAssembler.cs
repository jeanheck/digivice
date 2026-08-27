using Backend.Domain.Models;
using Backend.Domain.Models.Npcs;
using Backend.Memory.Resources;

namespace Backend.Domain.Assemblers
{
    public static class NpcAssembler
    {
        public static List<Npc> Assemble(IEnumerable<NpcResource> npcResources)
        {
            return [.. npcResources.Select(npcResource => new Npc
            {
                Id = npcResource.Id,
                DigimonBattles = AssembleBattles(npcResource.DigimonBattles),
            })];
        }

        private static List<NpcBattle> AssembleBattles(IEnumerable<NpcBattleResource> battleResources)
        {
            return [.. battleResources.Select(battleResource => new NpcBattle
            {
                Id = battleResource.Id,
                Value = battleResource.Value,
            })];
        }
    }
}
