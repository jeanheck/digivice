using Backend.Domain.Models;
using Backend.Memory.Repositories;
using Backend.Memory.Readers.Party;

namespace Backend.Application.Services
{
    public class PartyStateService(
        IAddressesRepository addressesRepository,
        IPartyReader partyReader,
        DigimonStateService digimonStateService)
    {
        public Party GetParty()
        {
            var partyAddresses = addressesRepository.GetPartyAddresses();
            var resource = partyReader.Read(partyAddresses);
            var party = new Party();

            foreach (var slot in resource.SlotsResource)
            {
                if (digimonStateService.IsEmptySlot((byte)slot.DigimonId)) continue;

                party.Digimons[slot.Index - 1] = digimonStateService.GetDigimon(slot.Index, (byte)slot.DigimonId);
            }

            return party;
        }
    }
}