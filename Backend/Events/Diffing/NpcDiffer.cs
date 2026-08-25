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

        List<NpcBattleDTO> cardBattlesDelta = [];
        foreach (var newBattle in newNpc.CardBattles)
        {
            var previousBattle = previousNpc.CardBattles.FirstOrDefault(battle => battle.Id == newBattle.Id);
            var battleDelta = NpcBattleDiffer.Diff(previousBattle, newBattle);
            if (battleDelta != null)
            {
                cardBattlesDelta.Add(battleDelta);
            }
        }

        if (digimonBattlesDelta.Count == 0 && cardBattlesDelta.Count == 0)
        {
            return null;
        }

        var dto = new NpcDTO { Id = newNpc.Id };
        if (digimonBattlesDelta.Count > 0)
        {
            dto = dto with { DigimonBattles = digimonBattlesDelta };
        }
        if (cardBattlesDelta.Count > 0)
        {
            dto = dto with { CardBattles = cardBattlesDelta };
        }
        return dto;
    }
}
