using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.DTO.Parties.Digimons;

namespace Backend.Events.Converters.Parties.Digimons;

public static class EquipmentsConverter
{
    public static EquipmentsDTO ToDTO(Equipments equipments) => new()
    {
        Head = equipments.Head,
        Body = equipments.Body,
        RightHand = equipments.RightHand,
        LeftHand = equipments.LeftHand,
        Accessory1 = equipments.Accessory1,
        Accessory2 = equipments.Accessory2
    };
}
