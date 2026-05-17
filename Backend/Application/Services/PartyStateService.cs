using Backend.Domain.Models;
using Backend.Domain.Assemblers;
using Backend.Memory.Repositories;
using Backend.Memory.Readers.Party;
using Backend.Memory.Readers.Digimon;

namespace Backend.Application.Services
{
    public class PartyStateService(
        IAddressesRepository addressesRepository,
        IPartyReader partyReader,
        IDigimonReader digimonReader)
    {
        public Party GetParty()
        {
            var digimonStatusAddresses = addressesRepository.GetDigimonStatusAddresses();
            var partyAddresses = addressesRepository.GetPartyAddresses();
            var partyResource = partyReader.Read(partyAddresses);

            foreach (var slotResource in partyResource.SlotsResource)
            {
                if (slotResource.DigimonId != partyAddresses.EmptySlotId)
                {
                    var digimonAddress = addressesRepository.GetDigimonAddressById(slotResource.DigimonId);

                    slotResource.DigimonResource = digimonReader.Read(digimonAddress, digimonStatusAddresses);
                }
            }

            return PartyAssembler.Assemble(partyResource); ;
        }
    }
}