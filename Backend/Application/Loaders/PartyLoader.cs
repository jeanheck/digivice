using Backend.Application.Loaders.Interfaces;
using Backend.Memory.Repositories;
using Backend.Memory.Resources;
using Backend.Memory.Readers.Interfaces;

namespace Backend.Application.Loaders
{
    public class PartyLoader(
        IAddressesRepository addressesRepository,
        IPartyReader partyReader,
        IDigimonLoader digimonLoader) : IPartyLoader
    {
        public PartyResource Load()
        {
            var partyAddresses = addressesRepository.GetPartyAddresses();
            var partyResource = partyReader.Read(partyAddresses);

            foreach (var slotResource in partyResource.SlotsResource)
            {
                if (slotResource.DigimonId != partyAddresses.EmptySlotId)
                {
                    slotResource.DigimonResource = digimonLoader.Load(slotResource.DigimonId, slotResource.Index - 1);
                }
            }

            return partyResource;
        }
    }
}
