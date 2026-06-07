namespace Tests.Domain.Assemblers;

using Backend.Domain.Assemblers;
using Backend.Memory.Resources;
using Xunit;

public class AuctionAssemblerTests
{
    [Fact]
    public void Assemble_ShouldMapAuctionResourcesToDomainModel()
    {
        var auctionResources = new List<AuctionResource>
        {
            new() { Id = "divineBarrier", Value = 0x01 },
            new() { Id = "hazardShield", Value = 0x00 },
        };

        var result = AuctionAssembler.Assemble(auctionResources);

        Assert.Equal(2, result.Count);
        Assert.Equal("divineBarrier", result[0].Id);
        Assert.Equal(0x01, result[0].Value);
        Assert.Equal("hazardShield", result[1].Id);
        Assert.Equal(0x00, result[1].Value);
    }
}
