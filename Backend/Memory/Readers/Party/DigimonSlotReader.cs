using Backend.Memory.Addresses.Party;
using Backend.Memory.Resources.Party;

namespace Backend.Memory.Readers.Party
{
    public class DigimonSlotReader(IMemoryReader memoryReader) : IDigimonSlotReader
    {
        public DigimonSlotResource Read(SlotAddresses addresses, int bytesPerSlot)
        {
            var bytes = memoryReader.ReadBytes(addresses.Address, bytesPerSlot);
            var digimonId = (bytes != null && bytes.Length > 0) ? (int)bytes[0] : 0;

            return new DigimonSlotResource
            {
                Index = addresses.Index,
                DigimonId = digimonId
            };
        }
    }
}
