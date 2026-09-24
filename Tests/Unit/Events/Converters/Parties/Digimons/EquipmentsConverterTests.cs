namespace Tests.Events.Converters.Parties.Digimons;

using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;

public class EquipmentsConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapAllEquipmentFields()
    {
        var dto = EquipmentsConverter.ToDTO(new Equipments
        {
            Head = 1,
            Body = 2,
            Right = 3,
            Left = 4,
            Accessory1 = 5,
            Accessory2 = 6
        });

        Assert.Equal(1, dto.Head.Value);
        Assert.Equal(2, dto.Body.Value);
        Assert.Equal(3, dto.Right.Value);
        Assert.Equal(4, dto.Left.Value);
        Assert.Equal(5, dto.Accessory1.Value);
        Assert.Equal(6, dto.Accessory2.Value);
    }

    [Fact]
    public void ToDTO_ShouldPreserveNullEquipmentSlots()
    {
        var dto = EquipmentsConverter.ToDTO(new Equipments { Head = 101 });

        Assert.Equal(101, dto.Head.Value);
        Assert.True(dto.Body.HasValue);
        Assert.Null(dto.Body.Value);
        Assert.True(dto.Accessory2.HasValue);
        Assert.Null(dto.Accessory2.Value);
    }
}
