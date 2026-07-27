using Backend.Memory.Addresses;
using Backend.Memory.Resources;

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
                SeabedRoute = memoryReader.ReadBytes(addresses.SeabedRoute, 1)[0],
                MapVariant = memoryReader.ReadBytes(addresses.MapVariant, 1)[0]
            };
        }
    }
}
