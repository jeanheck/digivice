using Backend.Memory.Addresses;
using Backend.Memory.Resources;
using Backend.Memory.Readers.Interfaces;

namespace Backend.Memory.Readers
{
    public class AuctionsReader(IMemoryReader memoryReader) : IAuctionsReader
    {
        public AuctionsResource Read(AuctionsAddresses addresses)
        {
            return new AuctionsResource
            {
                DivineBarrier = memoryReader.ReadByte(addresses.DivineBarrier.Address, addresses.DivineBarrier.BitMask),
                HazardShield = memoryReader.ReadByte(addresses.HazardShield.Address, addresses.HazardShield.BitMask),
                SniperShield = memoryReader.ReadByte(addresses.SniperShield.Address, addresses.SniperShield.BitMask),
                DramonShield = memoryReader.ReadByte(addresses.DramonShield.Address, addresses.DramonShield.BitMask),
                YinYangWand = memoryReader.ReadByte(addresses.YinYangWand.Address, addresses.YinYangWand.BitMask),
            };
        }
    }
}
