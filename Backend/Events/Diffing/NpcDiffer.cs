using Backend.Domain.Models;
using Backend.Events.Converters;
using Backend.Events.Diffing.Extensions;
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
            return NpcsConverter.ToNpcDTO(newNpc);
        }

        List<NpcBattleDTO> battlesDelta = [];
        foreach (var newBattle in newNpc.Battles)
        {
            var previousBattle = previousNpc.Battles.FirstOrDefault(battle => battle.Id == newBattle.Id);
            var battleDelta = NpcBattleDiffer.Diff(previousBattle, newBattle);
            if (battleDelta != null)
            {
                battlesDelta.Add(battleDelta);
            }
        }

        if (battlesDelta.Count == 0)
        {
            return null;
        }

        return new NpcDTO
        {
            Battles = battlesDelta,
        };
    }
}
