using Backend.Domain.Models;
using Backend.Memory.Readers;
using Backend.Interfaces;

namespace Backend.Application.Services
{
    public class PartyStateService(
        IAddressesRepository addressesRepository,
        IResourceReader resourceReader,
        DigimonStateService digimonStateService)
    {
        public Party GetParty()
        {
            var partyAddresses = addressesRepository.GetPartyAddresses();
            var resource = resourceReader.ReadParty(partyAddresses);
            var party = new Party();

            for (int i = 0; i < resource.DigimonIds.Count; i++)
            {
                byte digimonId = resource.DigimonIds[i];
                if (digimonStateService.IsEmptySlot(digimonId)) continue;

                party.Slots[i] = digimonStateService.GetDigimon(i + 1, digimonId);
            }

            return party;
        }
    }
}