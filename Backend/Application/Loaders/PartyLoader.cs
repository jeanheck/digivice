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
                        var combatVitals = inCombatReader.ReadSlot(
                            inCombatAddresses,
                            slotResource.Index - 1);

                        if (combatVitals.Id != 0)
                        {
                            digimonResource.HP = combatVitals.HP;
                            digimonResource.MP = combatVitals.MP;
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
