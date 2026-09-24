using Backend.Memory.Addresses;
using Backend.Memory.Resources;
using Backend.Memory.Readers.Interfaces;

namespace Backend.Memory.Readers
{
    public class PlayerReader(IMemoryReader memoryReader) : IPlayerReader
    {
        public PlayerResource Read(PlayerAddresses addresses)
        {
            return new PlayerResource
            {
                Bits = memoryReader.ReadInt32(addresses.Bits),
                MapId = memoryReader.ReadInt16(addresses.MapId),
                PreviousMapId = memoryReader.ReadInt16(addresses.PreviousMapId),
                SeabedRoute = memoryReader.ReadByte(addresses.SeabedRoute),
                MapVariant = memoryReader.ReadByte(addresses.MapVariant)
            };
        }
    }
}
