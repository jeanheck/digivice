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
            RightHand = 3,
            LeftHand = 4,
            Accessory1 = 5,
            Accessory2 = 6
        });

        Assert.Equal(1, dto.Head.Value);
        Assert.Equal(2, dto.Body.Value);
        Assert.Equal(3, dto.RightHand.Value);
        Assert.Equal(4, dto.LeftHand.Value);
        Assert.Equal(5, dto.Accessory1.Value);
        Assert.Equal(6, dto.Accessory2.Value);
    }
}
