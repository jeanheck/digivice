namespace Tests.Domain.Assemblers;

using Backend.Domain.Assemblers;
using Backend.Memory.Resources;

public class ImportantItemsAssemblerTests
{
    [Fact]
    public void Assemble_ShouldMapNonZeroBytesToTrue()
    {
        var resource = new ImportantItemsResource
        {
            TreeBoots = 0x01,
            FishingPole = 0x02,
            AsukaTrophy = 0xFF,
            SunTrophy = 0x01
        };

        var result = ImportantItemsAssembler.Assemble(resource);

        Assert.True(result.TreeBoots);
        Assert.True(result.FishingPole);
        Assert.True(result.AsukaTrophy);
        Assert.True(result.SunTrophy);
    }

    [Fact]
    public void Assemble_ShouldMapZeroAndNullToFalse()
    {
        var resource = new ImportantItemsResource
        {
            TreeBoots = 0x00,
            FishingPole = null,
            AsukaTrophy = 0x00,
            SunTrophy = null
        };

        var result = ImportantItemsAssembler.Assemble(resource);

        Assert.False(result.TreeBoots);
        Assert.False(result.FishingPole);
        Assert.False(result.AsukaTrophy);
        Assert.False(result.SunTrophy);
    }
}
