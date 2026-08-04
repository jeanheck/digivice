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
        IDigimonInCombatReader digimonInCombatReader) : IPartyLoader
    {
        public PartyResource Load()
        {
            var partyAddresses = addressesRepository.GetPartyAddresses();
            var partyResource = partyReader.Read(partyAddresses);
            var digimonInCombatAddresses = addressesRepository.GetDigimonInCombatAddresses();

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
                        var digimonInCombatResource = digimonInCombatReader.Read(digimonInCombatAddresses, slotResource.Index - 1);
                        if (digimonInCombatResource.IsInCombat)
                        {
                            digimonResource.HP = digimonInCombatResource.HP;
                            digimonResource.MP = digimonInCombatResource.MP;
                            digimonResource.Condition = digimonInCombatResource.Condition;
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
