using Backend.Domain.Models;
using Backend.Memory.Resources;

namespace Backend.Domain.Assemblers
{
    public static class AuctionsAssembler
    {
        public static Auctions Assemble(AuctionsResource resource)
        {
            return new Auctions
            {
                DivineBarrier = (resource.DivineBarrier ?? 0) != 0,
                HazardShield = (resource.HazardShield ?? 0) != 0,
                SniperShield = (resource.SniperShield ?? 0) != 0,
                DramonShield = (resource.DramonShield ?? 0) != 0,
                YinYangWand = (resource.YinYangWand ?? 0) != 0
            };
        }
    }
}
