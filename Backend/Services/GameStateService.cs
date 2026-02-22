using System.Collections.Generic;
using Backend.Interfaces;
using Backend.Models;
using Backend.Models.Digimons;
using Backend.Utils;
using Backend.Constants;
using static Backend.Constants.MemoryAddresses;

namespace Backend.Services
{
    public class GameStateService
    {
        private readonly IMemoryReader _memoryReader;

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
            var bytes = _memoryReader.ReadBytes(Offsets.PlayerName, MemoryConstants.ProtagonistNameLength);
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

            for (int i = 0; i < Offsets.PartySlots.Length; i++)
            {
                var slotAddress = Offsets.PartySlots[i];
                // Each slot ID is an Int32, but we only need the first byte
                var idBytes = _memoryReader.ReadBytes(slotAddress, MemoryConstants.SlotIdSize);
                if (idBytes == null) continue;

                byte id = idBytes[0];

                if (id == 0xFF) continue; // Skip empty slots (0xFF)

                if (_digimonDatabase.TryGetValue(id, out var data))
                {
                    party.Digimons.Add(new Digimon
                    {
                        Id = id,
                        SlotIndex = i + 1,
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
                else
                {
                    Serilog.Log.Warning("Unknown Digimon ID detected in party slot: 0x{Id:X2} at address 0x{Address:X8}", id, slotAddress);
                }
            }

            return party;
        }
    }
}
