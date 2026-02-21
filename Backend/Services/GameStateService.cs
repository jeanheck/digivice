using System.Collections.Generic;
using Backend.Interfaces;
using Backend.Models;
using Backend.Utils;

namespace Backend.Services
{
    public class GameStateService
    {
        private readonly IMemoryReader _memoryReader;

        private const int ProtagonistNameAddress = 0x00048D88;
        private const int PartySlot1Address = 0x00048DA4;

        private readonly Dictionary<int, (string Name, int Address)> _digimonDatabase = new()
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
            var bytes = _memoryReader.ReadBytes(ProtagonistNameAddress, 10);
            if (bytes == null) return null;

            return new Player
            {
                Name = TextDecoder.DecodeProtagonist(bytes)
            };
        }

        public Party GetParty()
        {
            var party = new Party();
            // Digimon IDs are likely stored as Int32 (4 bytes each)
            // Reading 3 slots * 4 bytes = 12 bytes
            var slots = _memoryReader.ReadBytes(PartySlot1Address, 12);

            if (slots == null) return party;

            for (int i = 0; i < 3; i++)
            {
                int idOffset = i * 4;
                byte id = slots[idOffset];

                // In DW3, if a slot is empty, it usually has a value like 0xFF
                if (_digimonDatabase.TryGetValue(id, out var data))
                {
                    // Level, Exp, HP and MP offsets relative to our database base addresses
                    int exp = _memoryReader.ReadInt32(data.Address + 0x18);
                    short lvl = _memoryReader.ReadInt16(data.Address + 0x1C);
                    short curHp = _memoryReader.ReadInt16(data.Address + 0x20);
                    short maxHp = _memoryReader.ReadInt16(data.Address + 0x22);
                    short curMp = _memoryReader.ReadInt16(data.Address + 0x24);
                    short maxMp = _memoryReader.ReadInt16(data.Address + 0x26);

                    party.Digimons.Add(new Digimon
                    {
                        Id = id,
                        Name = data.Name,
                        BaseAddress = data.Address,
                        Experience = exp,
                        Level = lvl,
                        CurrentHP = curHp,
                        MaxHP = maxHp,
                        CurrentMP = curMp,
                        MaxMP = maxMp
                    });
                }
            }

            return party;
        }
    }
}
