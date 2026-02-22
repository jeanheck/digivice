using Backend.Interfaces;
using Backend.Models;
using Backend.Models.Digimons;
using Backend.Utils;
using Backend.Constants;

namespace Backend.Services
{
    public class GameStateService
    {
        private readonly IMemoryReaderService _memoryReader;

        public GameStateService(IMemoryReaderService memoryReader)
        {
            _memoryReader = memoryReader;
        }

        public Player? GetPlayer()
        {
            var bytes = _memoryReader.ReadBytes(PlayerAddresses.Name, PlayerAddresses.NameBufferSize);
            if (bytes == null) return null;

            var player = new Player
            {
                Name = TextDecoder.Decode(bytes),
                Bits = _memoryReader.ReadInt32(PlayerAddresses.Bits),
                Party = GetParty()
            };

            return player;
        }

        public Party GetParty()
        {
            var party = new Party();

            for (int i = 0; i < PlayerAddresses.PartySlots.Length; i++)
            {
                var slotAddress = PlayerAddresses.PartySlots[i];
                // Each slot ID is an Int32, but we only need the first byte
                var idBytes = _memoryReader.ReadBytes(slotAddress, PlayerAddresses.PartySlotStride);
                if (idBytes == null) continue;

                byte digimonId = idBytes[0];

                if (digimonId == DigimonAddresses.EmptySlotId) continue; // Skip empty slots

                if (DigimonAddresses.Digimons.TryGetValue(digimonId, out var data))
                {
                    party.Digimons.Add(new Digimon
                    {
                        SlotIndex = i + 1,
                        BasicInfo = new BasicInfo
                        {
                            Name = data.Name,
                            Experience = _memoryReader.ReadInt32(data.Address + DigimonAddresses.BasicInfo.Experience),
                            Level = _memoryReader.ReadInt16(data.Address + DigimonAddresses.BasicInfo.Level),
                            CurrentHP = _memoryReader.ReadInt16(data.Address + DigimonAddresses.BasicInfo.CurrentHP),
                            MaxHP = _memoryReader.ReadInt16(data.Address + DigimonAddresses.BasicInfo.MaxHP),
                            CurrentMP = _memoryReader.ReadInt16(data.Address + DigimonAddresses.BasicInfo.CurrentMP),
                            MaxMP = _memoryReader.ReadInt16(data.Address + DigimonAddresses.BasicInfo.MaxMP)
                        },
                        Attributes = new Attributes
                        {
                            Attack = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Attributes.Attack),
                            Defense = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Attributes.Defense),
                            Spirit = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Attributes.Spirit),
                            Wisdom = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Attributes.Wisdom),
                            Speed = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Attributes.Speed),
                            Charisma = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Attributes.Charisma)
                        },
                        Resistances = new Resistances
                        {
                            Fire = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Resistances.Fire),
                            Water = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Resistances.Water),
                            Ice = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Resistances.Ice),
                            Wind = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Resistances.Wind),
                            Thunder = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Resistances.Thunder),
                            Metal = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Resistances.Metal),
                            Dark = _memoryReader.ReadInt16(data.Address + DigimonAddresses.Resistances.Dark)
                        }
                    });
                }
                else
                {
                    Serilog.Log.Warning("Unknown Digimon ID detected in party slot: 0x{Id:X2} at address 0x{Address:X8}", digimonId, slotAddress);
                }
            }

            return party;
        }
    }
}
