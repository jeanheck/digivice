using Backend.Domain.Models;
using Backend.Domain.Assemblers;
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
            var party = PartyAssembler.Assemble(resource);

            foreach (var slot in party.Slots)
            {
                if (slot.DigimonId == partyAddresses.EmptySlotId)
                {
                    continue;
                }

                slot.Digimon = digimonStateService.GetDigimon(slot.Index, (byte)slot.DigimonId);
            }

            return party;
        }
    }
}