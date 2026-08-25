using Backend.Domain.Models.Npcs;
using Backend.Events.Converters;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Npcs;

namespace Backend.Events.Diffing.Npcs;

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
            return NpcConverter.ToBattleDTO(newBattle);
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
