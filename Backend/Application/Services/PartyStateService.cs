using Backend.Domain.Models;
using Backend.Memory.Repositories;
using Backend.Memory.Readers;

namespace Backend.Application.Services
{
    public class PartyStateService(
        IAddressesRepository addressesRepository,
        IAddressesReader addressesReader,
        DigimonStateService digimonStateService)
    {
        public Party GetParty()
        {
            var partyAddresses = addressesRepository.GetPartyAddresses();
            var resource = addressesReader.ReadPartyResource(partyAddresses);
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