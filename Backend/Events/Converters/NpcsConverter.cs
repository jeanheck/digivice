using Backend.Domain.Models;
using Backend.Events.DTO;
using Backend.Events.DTO.Npcs;

namespace Backend.Events.Converters;

public static class NpcsConverter
{
    public static NpcsDTO ToDTO(Npcs npcs)
    {
        return new NpcsDTO
        {
            Genji = ToNpcDTO(npcs.Genji),
            Natsumi = ToNpcDTO(npcs.Natsumi),
            Catherine = ToNpcDTO(npcs.Catherine),
            Lucia = ToNpcDTO(npcs.Lucia),
            Robert = ToNpcDTO(npcs.Robert),
            Akiba = ToNpcDTO(npcs.Akiba),
            Chris = ToNpcDTO(npcs.Chris),
            Tomomi = ToNpcDTO(npcs.Tomomi),
            Mitch = ToNpcDTO(npcs.Mitch),
            Bob = ToNpcDTO(npcs.Bob),
            Andy = ToNpcDTO(npcs.Andy),
            George = ToNpcDTO(npcs.George),
            MeiLin = ToNpcDTO(npcs.MeiLin),
            Jessica = ToNpcDTO(npcs.Jessica),
            Gordon = ToNpcDTO(npcs.Gordon),
            Alice = ToNpcDTO(npcs.Alice),
            Nakano = ToNpcDTO(npcs.Nakano),
            SeiryuLeader = ToNpcDTO(npcs.SeiryuLeader),
            Keith = ToNpcDTO(npcs.Keith),
            SuzakuLeader = ToNpcDTO(npcs.SuzakuLeader),
            FakeByakkoLeader = ToNpcDTO(npcs.FakeByakkoLeader),
            ByakkoLeader = ToNpcDTO(npcs.ByakkoLeader),
            AoaAttacker = ToNpcDTO(npcs.AoaAttacker),
        };
    }

    public static NpcDTO ToNpcDTO(Npc npc)
    {
        return new NpcDTO
        {
            Battles = npc.Battles.Select(ToBattleDTO).ToList(),
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
