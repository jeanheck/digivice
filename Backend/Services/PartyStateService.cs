using Backend.Models;

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

            var digimonAddresses = _database.GetDigimonAddresses();
            byte emptySlotId = (byte)(string.IsNullOrEmpty(digimonAddresses.EmptySlotId) ? 0xFF : Convert.ToInt32(digimonAddresses.EmptySlotId, 16));

            for (int i = 0; i < resource.ActiveDigimonIds.Count && i < 3; i++)
            {
                byte digimonId = (byte)resource.ActiveDigimonIds[i];

                if (digimonId == emptySlotId) continue;

                int baseAddress = _digimonStateService.GetDigimonBaseAddressById(digimonAddresses, digimonId);
                if (baseAddress != 0)
                {
                    party.Slots[i] = _digimonStateService.GetDigimon(i + 1, digimonId, baseAddress);
                }
                else
                {
                    Serilog.Log.Warning("Unknown Digimon ID detected in party slot: 0x{Id:X2} at address 0x{AddressStr}", digimonId, addresses.PartySlot1);
                }
            }

            return party;
        }


    }
}
