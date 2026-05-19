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
            MapId = "00AF"
        };

        var dto = PlayerConverter.ToDTO(player);

        Assert.True(dto.Name.HasValue);
        Assert.Equal("Agumon", dto.Name.Value);
        Assert.True(dto.Bits.HasValue);
        Assert.Equal(12345, dto.Bits.Value);
        Assert.True(dto.Location.HasValue);
        Assert.Equal("00AF", dto.Location.Value);
    }
}
