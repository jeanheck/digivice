using Backend.Memory.Addresses;
using Backend.Memory.Readers.Helpers;
using Backend.Memory.Resources;
using Backend.Memory.Readers.Interfaces;

namespace Backend.Memory.Readers
{
    public class NpcsReader(IMemoryReader memoryReader) : INpcsReader
    {
        public NpcsResource Read(NpcsAddresses addresses)
        {
            return new NpcsResource
            {
                Genji = ReadNpc(addresses.Genji),
                Natsumi = ReadNpc(addresses.Natsumi),
                Catherine = ReadNpc(addresses.Catherine),
                Lucia = ReadNpc(addresses.Lucia),
                Robert = ReadNpc(addresses.Robert),
                Akiba = ReadNpc(addresses.Akiba),
                Chris = ReadNpc(addresses.Chris),
                Tomomi = ReadNpc(addresses.Tomomi),
                Mitch = ReadNpc(addresses.Mitch),
                Bob = ReadNpc(addresses.Bob),
                Andy = ReadNpc(addresses.Andy),
                George = ReadNpc(addresses.George),
                MeiLin = ReadNpc(addresses.MeiLin),
                Jessica = ReadNpc(addresses.Jessica),
                Gordon = ReadNpc(addresses.Gordon),
                Alice = ReadNpc(addresses.Alice),
                Nakano = ReadNpc(addresses.Nakano),
                SeiryuLeader = ReadNpc(addresses.SeiryuLeader),
                Keith = ReadNpc(addresses.Keith),
                SuzakuLeader = ReadNpc(addresses.SuzakuLeader),
                FakeByakkoLeader = ReadNpc(addresses.FakeByakkoLeader),
                ByakkoLeader = ReadNpc(addresses.ByakkoLeader),
                AoaAttacker = ReadNpc(addresses.AoaAttacker),
            };
        }

        private NpcResource ReadNpc(NpcAddresses npcAddresses)
        {
            return new NpcResource
            {
                Battles = ReadBattles(npcAddresses.Battles),
            };
        }

        private List<NpcBattleResource> ReadBattles(Dictionary<string, NpcBattleAddresses> battles)
        {
            return [.. battles.Select(battle => new NpcBattleResource
            {
                Id = battle.Key,
                Value = FlagByteHelper.Read(memoryReader, battle.Value.Address, battle.Value.BitMask),
            })];
        }
    }
}
