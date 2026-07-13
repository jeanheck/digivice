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
            PreviousMapId = 0x023E,
            SeabedRoute = 0x08,
            SeabedRouteType = 0x01
        };

        var result = PlayerAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal("Aa", result.Name);
        Assert.Equal(1500, result.Bits);
        Assert.Equal("00FF", result.MapId);
        Assert.Equal("023E", result.PreviousMapId);
        Assert.Equal((byte)0x08, result.SeabedRoute);
        Assert.Equal((byte)0x01, result.SeabedRouteType);
    }

    [Fact]
    public void Assemble_ShouldFallBackToSafeDefaults_WhenFieldsAreNull()
    {
        var resource = new PlayerResource
        {
            NameInBytes = null,
            Bits = null,
            MapId = null,
            PreviousMapId = null,
            SeabedRoute = null,
            SeabedRouteType = null
        };

        var result = PlayerAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal(string.Empty, result.Name);
        Assert.Equal(0, result.Bits);
        Assert.Equal(string.Empty, result.MapId);
        Assert.Equal(string.Empty, result.PreviousMapId);
        Assert.Equal((byte)0, result.SeabedRoute);
        Assert.Equal((byte)0, result.SeabedRouteType);
    }

    [Fact]
    public void Assemble_ShouldPassThroughSeabedRouteType_WhenByteIsZero()
    {
        var resource = new PlayerResource
        {
            NameInBytes = [0x0E, 0x28],
            Bits = 0,
            MapId = 0,
            SeabedRoute = 0x07,
            SeabedRouteType = 0x00
        };

        var result = PlayerAssembler.Assemble(resource);

        Assert.Equal((byte)0x07, result.SeabedRoute);
        Assert.Equal((byte)0x00, result.SeabedRouteType);
    }
}
