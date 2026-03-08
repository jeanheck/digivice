using Backend.Models;
using Backend.Models.Digimons;
using Backend.Constants;
using Backend.Interfaces;
using System.Linq;

namespace Backend.Services
{
    public class PartyStateService
    {
        private readonly GameDatabase _database;
        private readonly GameReader _reader;
        private readonly DigimonStateService _digimonStateService;

        public PartyStateService(GameDatabase database, GameReader reader, DigimonStateService digimonStateService)
        {
            _database = database;
            _reader = reader;
            _digimonStateService = digimonStateService;
        }

        public Party GetParty()
        {
            var addresses = _database.GetPartyAddresses();
            var resource = _reader.ReadParty(addresses);

            var party = new Party();

            for (int i = 0; i < resource.ActiveDigimonIds.Count && i < 3; i++)
            {
                byte digimonId = (byte)resource.ActiveDigimonIds[i];

                if (digimonId == DigimonAddresses.EmptySlotId) continue;

                if (DigimonAddresses.Digimons.TryGetValue(digimonId, out var data))
                {
                    party.Slots[i] = _digimonStateService.BuildDigimon(i + 1, digimonId, data.Address);
                }
                else
                {
                    Serilog.Log.Warning("Unknown Digimon ID detected in party slot: 0x{Id:X2} at address 0x{Address:X8}", digimonId, addresses.PartySlot1);
                }
            }

            return party;
        }


    }
}
