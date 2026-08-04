using Backend.Application.Loaders.Parties;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Parties;
using Backend.Memory.Repositories;
using Backend.Memory.Resources;

namespace Backend.Application.Loaders
{
    public class PartyLoader(
        IAddressesRepository addressesRepository,
        IPartyReader partyReader,
        IDigimonLoader digimonLoader,
        IInCombatReader inCombatReader) : IPartyLoader
    {
        public PartyResource Load()
        {
            var partyAddresses = addressesRepository.GetPartyAddresses();
            var partyResource = partyReader.Read(partyAddresses);
            var inCombatAddresses = addressesRepository.GetInCombatAddresses();

            foreach (var slotResource in partyResource.SlotsResource)
            {
                if (slotResource.DigimonId is not null && slotResource.DigimonId != partyAddresses.EmptySlotId)
                {
                    var digimonResource = digimonLoader.Load(slotResource.DigimonId.Value);
                    if (digimonResource is null)
                    {
                        slotResource.DigimonId = null;
                        slotResource.DigimonResource = null;
                    }
                    else
                    {
                        // Override with In Combat data when a combat is happening
                        var inCombatData = inCombatReader.ReadSlot(inCombatAddresses, slotResource.Index - 1);
                        if (inCombatData.IsInCombat)
                        {
                            digimonResource.HP = inCombatData.HP;
                            digimonResource.MP = inCombatData.MP;
                        }

                        slotResource.DigimonResource = digimonResource;
                    }
                }
                else
                {
                    slotResource.DigimonId = null;
                    slotResource.DigimonResource = null;
                }
            }

            return partyResource;
        }
    }
}
