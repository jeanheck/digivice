using Backend.Domain.Models;
using Backend.Domain.Assemblers;
using Backend.Memory.Repositories;
using Backend.Memory.Readers.Party;
using Backend.Application.Resolvers;

namespace Backend.Application.Services
{
    public class PartyStateService(
        IAddressesRepository addressesRepository,
        IPartyReader partyReader,
        DigimonAddressResolver digimonAddressResolver)
    {
        public Party GetParty()
        {
            var partyAddresses = addressesRepository.GetPartyAddresses();
            var partyResource = partyReader.Read(partyAddresses);
            var party = PartyAssembler.Assemble(partyResource);

            foreach (var slot in party.Slots)
            {
                if (slot.DigimonId == partyAddresses.EmptySlotId)
                {
                    continue;
                }

                var digimonAddress = digimonAddressResolver.Resolve(slot.DigimonId);
            }

            return party;
        }
    }
}