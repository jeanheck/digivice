using Backend.Models;
using Backend.Interfaces;

namespace Backend.Services
{
    public class PartyStateService(
        IGameDatabase gameDatabase,
        IGameReader gameReader,
        DigimonStateService digimonStateService)
    {
        public Party GetParty()
        {
            var partyAddresses = gameDatabase.GetPartyAddresses();
            var resource = gameReader.ReadParty(partyAddresses);
            var party = new Party();

            for (int i = 0; i < resource.DigimonIds.Count; i++)
            {
                byte digimonId = (byte)resource.DigimonIds[i];
                if (digimonStateService.IsEmptySlot(digimonId))
                {
                    continue;
                }

                party.Slots[i] = digimonStateService.GetDigimon(i + 1, digimonId);
            }

            return party;
        }
    }
}

