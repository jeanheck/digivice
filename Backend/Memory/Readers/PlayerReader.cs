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
                NameInBytes = memoryReader.ReadBytes(addresses.Name, addresses.NameBufferSize),
                MapId = memoryReader.ReadInt16(addresses.MapId),
                SeabedRoute = memoryReader.ReadBytes(addresses.SeabedRoute, 1)[0],
                SeabedRouteType = memoryReader.ReadBytes(addresses.SeabedRouteType, 1)[0]
            };
        }
    }
}
