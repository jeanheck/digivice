using Backend.Memory.Repositories;
using Backend.Memory.Readers.Party;
using Backend.Memory.Resources;

namespace Backend.Application.Loaders
{
    public class PartyLoader(
        IAddressesRepository addressesRepository,
        IPartyReader partyReader,
        DigimonLoader digimonLoader)
    {
        public PartyResource Load()
        {
            var partyAddresses = addressesRepository.GetPartyAddresses();
            var partyResource = partyReader.Read(partyAddresses);

            foreach (var slotResource in partyResource.SlotsResource)
            {
                if (slotResource.DigimonId is not null/*&& slotResource.DigimonId != partyAddresses.EmptySlotId*/)
                {
                    slotResource.DigimonResource = digimonLoader.Load(slotResource.DigimonId.Value);
                }
            }

            return partyResource;
        }
    }
}
