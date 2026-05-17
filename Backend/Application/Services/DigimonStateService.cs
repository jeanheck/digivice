using Backend.Domain.Models;
using Backend.Domain.Assemblers;
using Backend.Memory.Repositories;
using Backend.Memory.Readers.Digimon;
using Backend.Memory.Addresses.Digimon;

namespace Backend.Application.Services
{
    public class DigimonStateService(
        IAddressesRepository addressesRepository,
        IDigimonReader digimonReader)
    {
        private DigimonStatusAddresses Addresses => addressesRepository.GetDigimonStatusAddresses();

        public Digimon? GetDigimon(int slotIndex, byte digimonId)
        {
            var digimonEntry = addressesRepository.GetDigimonAddressById(digimonId);

            if (digimonEntry == null || digimonEntry.Address == 0)
            {
                Serilog.Log.Warning("Unknown Digimon ID: 0x{Id:X2}", digimonId);
                return null;
            }

            var digimonResource = digimonReader.Read(digimonEntry, Addresses);

            var digimon = DigimonAssembler.Assemble(digimonResource);
            digimon.SlotIndex = slotIndex;

            return digimon;
        }
    }
}
