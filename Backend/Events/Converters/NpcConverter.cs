using Backend.Domain.Models;
using Backend.Domain.Models.Npcs;
using Backend.Events.DTO.Npcs;

namespace Backend.Events.Converters;

public static class NpcConverter
{
    public static NpcDTO ToDTO(Npc npc)
    {
        return new NpcDTO
        {
            Id = npc.Id,
            DigimonBattles = npc.DigimonBattles.Select(ToBattleDTO).ToList(),
            CardBattles = npc.CardBattles.Select(ToBattleDTO).ToList(),
        };
    }

    public static NpcBattleDTO ToBattleDTO(NpcBattle battle)
    {
        return new NpcBattleDTO
        {
            Id = battle.Id,
            Value = battle.Value,
        };
    }
}
