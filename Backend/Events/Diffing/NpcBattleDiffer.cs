using Backend.Domain.Models;
using Backend.Events.Converters;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Npcs;

namespace Backend.Events.Diffing;

public static class NpcBattleDiffer
{
    public static NpcBattleDTO? Diff(NpcBattle? previousBattle, NpcBattle newBattle)
    {
        if (newBattle.HasNoChanges(previousBattle))
        {
            return null;
        }

        if (previousBattle == null)
        {
            return NpcsConverter.ToBattleDTO(newBattle);
        }

        if (previousBattle.Value == newBattle.Value)
        {
            return null;
        }

        return new NpcBattleDTO
        {
            Id = newBattle.Id,
            Value = newBattle.Value,
        };
    }
}
