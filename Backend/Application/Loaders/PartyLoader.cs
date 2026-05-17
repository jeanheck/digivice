using Backend.Memory.Repositories;
using Backend.Memory.Readers.Party;
using Backend.Memory.Readers.Digimon;
using Backend.Memory.Resources;

namespace Backend.Application.Loaders
{
    public class PartyLoader(
        IAddressesRepository addressesRepository,
        IPartyReader partyReader,
        IDigimonReader digimonReader)
    {
        public PartyResource Load()
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

            return partyResource;
        }
    }
}
