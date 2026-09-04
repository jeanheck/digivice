using Backend.Memory.Addresses;
using Backend.Memory.Readers.Helpers;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers
{
    public class AuctionsReader(IMemoryReader memoryReader) : IAuctionsReader
    {
        public AuctionsResource Read(AuctionsAddresses addresses)
        {
            return new AuctionsResource
            {
                DivineBarrier = FlagByteHelper.Read(memoryReader, addresses.DivineBarrier.Address, addresses.DivineBarrier.BitMask),
                HazardShield = FlagByteHelper.Read(memoryReader, addresses.HazardShield.Address, addresses.HazardShield.BitMask),
                SniperShield = FlagByteHelper.Read(memoryReader, addresses.SniperShield.Address, addresses.SniperShield.BitMask),
                DramonShield = FlagByteHelper.Read(memoryReader, addresses.DramonShield.Address, addresses.DramonShield.BitMask),
                YinYangWand = FlagByteHelper.Read(memoryReader, addresses.YinYangWand.Address, addresses.YinYangWand.BitMask),
            };
        }
    }
}
