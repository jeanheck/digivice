using Backend.Domain.Models;
using Backend.Memory.Resources;

namespace Backend.Domain.Assemblers
{
    public static class NpcsAssembler
    {
        public static Npcs Assemble(NpcsResource resource)
        {
            return new Npcs
            {
                Genji = AssembleNpc(resource.Genji),
                Natsumi = AssembleNpc(resource.Natsumi),
                Catherine = AssembleNpc(resource.Catherine),
                Lucia = AssembleNpc(resource.Lucia),
                Robert = AssembleNpc(resource.Robert),
                Akiba = AssembleNpc(resource.Akiba),
                Chris = AssembleNpc(resource.Chris),
                Tomomi = AssembleNpc(resource.Tomomi),
                Mitch = AssembleNpc(resource.Mitch),
                Bob = AssembleNpc(resource.Bob),
                Andy = AssembleNpc(resource.Andy),
                George = AssembleNpc(resource.George),
                MeiLin = AssembleNpc(resource.MeiLin),
                Jessica = AssembleNpc(resource.Jessica),
                Gordon = AssembleNpc(resource.Gordon),
                Alice = AssembleNpc(resource.Alice),
                Nakano = AssembleNpc(resource.Nakano),
                SeiryuLeader = AssembleNpc(resource.SeiryuLeader),
                Keith = AssembleNpc(resource.Keith),
                SuzakuLeader = AssembleNpc(resource.SuzakuLeader),
                FakeByakkoLeader = AssembleNpc(resource.FakeByakkoLeader),
                ByakkoLeader = AssembleNpc(resource.ByakkoLeader),
                AoaAttacker = AssembleNpc(resource.AoaAttacker),
            };
        }

        private static Npc AssembleNpc(NpcResource npcResource)
        {
            return new Npc
            {
                Battles = AssembleBattles(npcResource.Battles),
            };
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
