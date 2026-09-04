using Backend.Memory.Addresses.Parties;
using Backend.Memory.Resources.Parties;
using Backend.Memory.Readers.Interfaces;

namespace Backend.Memory.Readers
{
    public class DigimonSlotReader(IMemoryReader memoryReader) : IDigimonSlotReader
    {
        public DigimonSlotResource Read(SlotAddresses addresses, int bytesPerSlot)
        {
            var bytes = memoryReader.ReadBytes(addresses.Address, bytesPerSlot);
            int? digimonId = bytes.Length > 0 ? bytes[0] : null;

            return new DigimonSlotResource
            {
                Index = addresses.Index,
                DigimonId = digimonId
            };
        }
    }
}
