namespace Tests.Domain.Assemblers;

using Backend.Domain.Assemblers;
using Backend.Memory.Resources;
using Xunit;

public class AuctionsAssemblerTests
{
    [Fact]
    public void Assemble_ShouldMapNonZeroFlagsToTrue()
    {
        var resource = new AuctionsResource
        {
            DivineBarrier = 0x01,
            HazardShield = 0x00,
            SniperShield = 0x04,
            DramonShield = null,
            YinYangWand = 0x10,
        };

        var result = AuctionsAssembler.Assemble(resource);

        Assert.True(result.DivineBarrier);
        Assert.False(result.HazardShield);
        Assert.True(result.SniperShield);
        Assert.False(result.DramonShield);
        Assert.True(result.YinYangWand);
    }
}
