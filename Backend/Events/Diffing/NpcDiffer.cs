using Backend.Domain.Models;
using Backend.Events.Converters;
using Backend.Events.Diffing.Extensions;
using Backend.Events.Diffing.Npcs;
using Backend.Events.DTO.Npcs;

namespace Backend.Events.Diffing;

public static class NpcDiffer
{
    public static NpcDTO? Diff(Npc? previousNpc, Npc newNpc)
    {
        if (newNpc.HasNoChanges(previousNpc))
        {
            return null;
        }

        if (previousNpc == null)
        {
            return NpcConverter.ToDTO(newNpc);
        }

        List<NpcBattleDTO> digimonBattlesDelta = [];
        foreach (var newBattle in newNpc.DigimonBattles)
        {
            var previousBattle = previousNpc.DigimonBattles.FirstOrDefault(battle => battle.Id == newBattle.Id);
            var battleDelta = NpcBattleDiffer.Diff(previousBattle, newBattle);
            if (battleDelta != null)
            {
                digimonBattlesDelta.Add(battleDelta);
            }
        }

        if (digimonBattlesDelta.Count == 0)
        {
            return null;
        }

        return new NpcDTO
        {
            Id = newNpc.Id,
            DigimonBattles = digimonBattlesDelta,
        };
    }
}
