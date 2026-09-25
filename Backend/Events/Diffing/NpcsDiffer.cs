using Backend.Domain.Models;
using Backend.Events.Converters;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO;
using Backend.Events.DTO.Npcs;

namespace Backend.Events.Diffing;

public static class NpcsDiffer
{
    public static NpcsDTO Diff(Npcs? previousNpcs, Npcs newNpcs)
    {
        if (newNpcs.HasNoChanges(previousNpcs))
        {
            return new NpcsDTO();
        }

        if (previousNpcs == null)
        {
            return NpcsConverter.ToDTO(newNpcs);
        }

        var dto = new NpcsDTO();

        dto = ApplyNpcDelta(dto, previousNpcs.Genji, newNpcs.Genji, (current, npcDto) => current with { Genji = npcDto });
        dto = ApplyNpcDelta(dto, previousNpcs.Natsumi, newNpcs.Natsumi, (current, npcDto) => current with { Natsumi = npcDto });
        dto = ApplyNpcDelta(dto, previousNpcs.Catherine, newNpcs.Catherine, (current, npcDto) => current with { Catherine = npcDto });
        dto = ApplyNpcDelta(dto, previousNpcs.Lucia, newNpcs.Lucia, (current, npcDto) => current with { Lucia = npcDto });
        dto = ApplyNpcDelta(dto, previousNpcs.Robert, newNpcs.Robert, (current, npcDto) => current with { Robert = npcDto });
        dto = ApplyNpcDelta(dto, previousNpcs.Akiba, newNpcs.Akiba, (current, npcDto) => current with { Akiba = npcDto });
        dto = ApplyNpcDelta(dto, previousNpcs.Chris, newNpcs.Chris, (current, npcDto) => current with { Chris = npcDto });
        dto = ApplyNpcDelta(dto, previousNpcs.Tomomi, newNpcs.Tomomi, (current, npcDto) => current with { Tomomi = npcDto });
        dto = ApplyNpcDelta(dto, previousNpcs.Mitch, newNpcs.Mitch, (current, npcDto) => current with { Mitch = npcDto });
        dto = ApplyNpcDelta(dto, previousNpcs.Bob, newNpcs.Bob, (current, npcDto) => current with { Bob = npcDto });
        dto = ApplyNpcDelta(dto, previousNpcs.Andy, newNpcs.Andy, (current, npcDto) => current with { Andy = npcDto });
        dto = ApplyNpcDelta(dto, previousNpcs.George, newNpcs.George, (current, npcDto) => current with { George = npcDto });
        dto = ApplyNpcDelta(dto, previousNpcs.MeiLin, newNpcs.MeiLin, (current, npcDto) => current with { MeiLin = npcDto });
        dto = ApplyNpcDelta(dto, previousNpcs.Jessica, newNpcs.Jessica, (current, npcDto) => current with { Jessica = npcDto });
        dto = ApplyNpcDelta(dto, previousNpcs.Gordon, newNpcs.Gordon, (current, npcDto) => current with { Gordon = npcDto });
        dto = ApplyNpcDelta(dto, previousNpcs.Alice, newNpcs.Alice, (current, npcDto) => current with { Alice = npcDto });
        dto = ApplyNpcDelta(dto, previousNpcs.Nakano, newNpcs.Nakano, (current, npcDto) => current with { Nakano = npcDto });
        dto = ApplyNpcDelta(dto, previousNpcs.SeiryuLeader, newNpcs.SeiryuLeader, (current, npcDto) => current with { SeiryuLeader = npcDto });
        dto = ApplyNpcDelta(dto, previousNpcs.Keith, newNpcs.Keith, (current, npcDto) => current with { Keith = npcDto });
        dto = ApplyNpcDelta(dto, previousNpcs.SuzakuLeader, newNpcs.SuzakuLeader, (current, npcDto) => current with { SuzakuLeader = npcDto });
        dto = ApplyNpcDelta(dto, previousNpcs.FakeByakkoLeader, newNpcs.FakeByakkoLeader, (current, npcDto) => current with { FakeByakkoLeader = npcDto });
        dto = ApplyNpcDelta(dto, previousNpcs.ByakkoLeader, newNpcs.ByakkoLeader, (current, npcDto) => current with { ByakkoLeader = npcDto });
        dto = ApplyNpcDelta(dto, previousNpcs.AoaAttacker, newNpcs.AoaAttacker, (current, npcDto) => current with { AoaAttacker = npcDto });

        return dto;
    }

    private static NpcsDTO ApplyNpcDelta(
        NpcsDTO dto,
        Npc previousNpc,
        Npc newNpc,
        Func<NpcsDTO, NpcDTO, NpcsDTO> apply)
    {
        var npcDelta = NpcDiffer.Diff(previousNpc, newNpc);
        if (npcDelta == null)
        {
            return dto;
        }

        return apply(dto, npcDelta);
    }
}
