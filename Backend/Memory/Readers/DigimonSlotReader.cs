using Backend.Memory.Addresses.Parties;
using Backend.Memory.Resources.Parties;
using Backend.Memory.Readers.Interfaces;

namespace Backend.Memory.Readers
{
    public class DigimonSlotReader(IMemoryReader memoryReader) : IDigimonSlotReader
    {
        public DigimonSlotResource Read(SlotAddresses addresses)
        {
            return new DigimonSlotResource
            {
                Index = addresses.Index,
                DigimonId = memoryReader.ReadByte(addresses.Address)
            };
        }
    }
}
