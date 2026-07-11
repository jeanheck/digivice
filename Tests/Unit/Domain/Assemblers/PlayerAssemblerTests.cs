namespace Tests.Domain.Assemblers;

using Backend.Domain.Assemblers;
using Backend.Memory.Resources;

public class PlayerAssemblerTests
{
    [Fact]
    public void Assemble_ShouldMapAllFieldsCorrectly_WhenResourceIsValid()
    {
        var resource = new PlayerResource
        {
            NameInBytes = [0x0E, 0x28],
            Bits = 1500,
            MapId = 255,
            SeabedRoute = 0x08,
            IsSubmerged = 0x01
        };

        var result = PlayerAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal("Aa", result.Name);
        Assert.Equal(1500, result.Bits);
        Assert.Equal("00FF", result.MapId);
        Assert.Equal((byte)0x08, result.SeabedRoute);
        Assert.True(result.IsSubmerged);
    }

    [Fact]
    public void Assemble_ShouldFallBackToSafeDefaults_WhenFieldsAreNull()
    {
        var resource = new PlayerResource
        {
            NameInBytes = null,
            Bits = null,
            MapId = null,
            SeabedRoute = null,
            IsSubmerged = null
        };

        var result = PlayerAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal(string.Empty, result.Name);
        Assert.Equal(0, result.Bits);
        Assert.Equal(string.Empty, result.MapId);
        Assert.Equal((byte)0, result.SeabedRoute);
        Assert.False(result.IsSubmerged);
    }

    [Fact]
    public void Assemble_ShouldSetIsSubmergedFalse_WhenByteIsNotOne()
    {
        var resource = new PlayerResource
        {
            NameInBytes = [0x0E, 0x28],
            Bits = 0,
            MapId = 0,
            SeabedRoute = 0x07,
            IsSubmerged = 0x00
        };

        var result = PlayerAssembler.Assemble(resource);

        Assert.Equal((byte)0x07, result.SeabedRoute);
        Assert.False(result.IsSubmerged);
    }
}
