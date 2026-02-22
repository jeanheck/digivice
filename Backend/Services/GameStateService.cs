using System.Collections.Generic;
using Backend.Interfaces;
using Backend.Models;
using Backend.Utils;

namespace Backend.Services
{
    public class GameStateService
    {
        private readonly IMemoryReader _memoryReader;

        private static class Offsets
        {
            public const int ProtagonistName = 0x00048D88;
            public const int Bits = 0x00048DA0;

            public const int PartySlot1 = 0x00048DA4;
            public const int PartySlot2 = 0x00048DA8;
            public const int PartySlot3 = 0x00048DAC;

            public static readonly int[] PartySlots = { PartySlot1, PartySlot2, PartySlot3 };

            // Offsets relative to Digimon base address
            public const int Experience = 0x18;
            public const int Level = 0x1C;
            public const int CurrentHP = 0x20;
            public const int MaxHP = 0x22;
            public const int CurrentMP = 0x24;
            public const int MaxMP = 0x26;

            public const int Attack = 0x28;
            public const int Defense = 0x2A;
            public const int Spirit = 0x2C;
            public const int Wisdom = 0x2E;
            public const int Speed = 0x30;
            public const int Charisma = 0x32;

            public const int FireResistance = 0x34;
            public const int WaterResistance = 0x36;
            public const int IceResistance = 0x38;
            public const int WindResistance = 0x3A;
            public const int ThunderResistance = 0x3C;
            public const int MetalResistance = 0x3E;
            public const int DarkResistance = 0x40;
        }

        private static class MemoryConstants
        {
            public const int ProtagonistNameLength = 10;
            public const int SlotIdSize = 4;
        }


        private static readonly Dictionary<int, (string Name, int Address)> _digimonDatabase = new()
        {
            { 0, ("Kotemon", 0x0004949C) },
            { 1, ("Kumamon", 0x00049878) },
            { 2, ("Monmon",  0x00049C54) },
            { 3, ("Agumon",  0x0004A030) },
            { 4, ("Veemon",  0x0004A40C) },
            { 5, ("Guilmon", 0x0004A7E8) },
            { 6, ("Renamon", 0x0004ABC4) },
            { 7, ("Patamon", 0x0004AFA0) }
        };

        public GameStateService(IMemoryReader memoryReader)
        {
            _memoryReader = memoryReader;
        }

        public Player? GetPlayer()
        {
            var bytes = _memoryReader.ReadBytes(Offsets.ProtagonistName, MemoryConstants.ProtagonistNameLength);
            if (bytes == null) return null;

            return new Player
            {
                Name = TextDecoder.DecodeProtagonist(bytes),
                Bits = _memoryReader.ReadInt32(Offsets.Bits)
            };
        }

        public Party GetParty()
        {
            var party = new Party();

            foreach (var slotAddress in Offsets.PartySlots)
            {
                // Each slot ID is an Int32, but we only need the first byte
                var idBytes = _memoryReader.ReadBytes(slotAddress, MemoryConstants.SlotIdSize);
                if (idBytes == null) continue;

                byte id = idBytes[0];

                if (_digimonDatabase.TryGetValue(id, out var data))
                {
                    party.Digimons.Add(new Digimon
                    {
                        Id = id,
                        Name = data.Name,
                        BaseAddress = data.Address,
                        Experience = _memoryReader.ReadInt32(data.Address + Offsets.Experience),
                        Level = _memoryReader.ReadInt16(data.Address + Offsets.Level),
                        CurrentHP = _memoryReader.ReadInt16(data.Address + Offsets.CurrentHP),
                        MaxHP = _memoryReader.ReadInt16(data.Address + Offsets.MaxHP),
                        CurrentMP = _memoryReader.ReadInt16(data.Address + Offsets.CurrentMP),
                        MaxMP = _memoryReader.ReadInt16(data.Address + Offsets.MaxMP),
                        Attributes = new Attributes
                        {
                            Attack = _memoryReader.ReadInt16(data.Address + Offsets.Attack),
                            Defense = _memoryReader.ReadInt16(data.Address + Offsets.Defense),
                            Spirit = _memoryReader.ReadInt16(data.Address + Offsets.Spirit),
                            Wisdom = _memoryReader.ReadInt16(data.Address + Offsets.Wisdom),
                            Speed = _memoryReader.ReadInt16(data.Address + Offsets.Speed),
                            Charisma = _memoryReader.ReadInt16(data.Address + Offsets.Charisma)
                        },
                        Resistances = new Resistances
                        {
                            Fire = _memoryReader.ReadInt16(data.Address + Offsets.FireResistance),
                            Water = _memoryReader.ReadInt16(data.Address + Offsets.WaterResistance),
                            Ice = _memoryReader.ReadInt16(data.Address + Offsets.IceResistance),
                            Wind = _memoryReader.ReadInt16(data.Address + Offsets.WindResistance),
                            Thunder = _memoryReader.ReadInt16(data.Address + Offsets.ThunderResistance),
                            Metal = _memoryReader.ReadInt16(data.Address + Offsets.MetalResistance),
                            Dark = _memoryReader.ReadInt16(data.Address + Offsets.DarkResistance)
                        }
                    });
                }
            }

            return party;
        }
    }
}
