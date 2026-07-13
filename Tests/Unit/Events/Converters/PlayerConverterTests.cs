namespace Tests.Events.Converters;

using Backend.Domain.Models;
using Backend.Events.Converters;

public class PlayerConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapAllPlayerFields()
    {
        var player = new Player
        {
            Name = "Agumon",
            Bits = 12345,
            MapId = "00AF",
            PreviousMapId = "023E",
            SeabedRoute = 0x08,
            SeabedRouteType = 0x01
        };

        var dto = PlayerConverter.ToDTO(player);

        Assert.True(dto.Name.HasValue);
        Assert.Equal("Agumon", dto.Name.Value);
        Assert.True(dto.Bits.HasValue);
        Assert.Equal(12345, dto.Bits.Value);
        Assert.True(dto.Location.HasValue);
        Assert.Equal("00AF", dto.Location.Value);
        Assert.True(dto.PreviousMapId.HasValue);
        Assert.Equal("023E", dto.PreviousMapId.Value);
        Assert.True(dto.SeabedRoute.HasValue);
        Assert.Equal((byte)0x08, dto.SeabedRoute.Value);
        Assert.True(dto.SeabedRouteType.HasValue);
        Assert.Equal((byte)0x01, dto.SeabedRouteType.Value);
    }
}
