using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Party.Digimon;

namespace Backend.Events.Diffing.Parties.Digimons;

public static class EquipmentsDiffer
{
    public static EquipmentsDTO? Diff(Equipments? previousEquipaments, Equipments newEquipaments)
    {
        if (newEquipaments.HasNoChanges(previousEquipaments))
        {
            return null;
        }

        if (previousEquipaments == null)
        {
            return EquipmentsConverter.ToDTO(newEquipaments);
        }

        var dto = new EquipmentsDTO();

        if (newEquipaments.Head != previousEquipaments.Head)
        {
            dto = dto with { Head = newEquipaments.Head };
        }
        if (newEquipaments.Body != previousEquipaments.Body)
        {
            dto = dto with { Body = newEquipaments.Body };
        }
        if (newEquipaments.RightHand != previousEquipaments.RightHand)
        {
            dto = dto with { RightHand = newEquipaments.RightHand };
        }
        if (newEquipaments.LeftHand != previousEquipaments.LeftHand)
        {
            dto = dto with { LeftHand = newEquipaments.LeftHand };
        }
        if (newEquipaments.Accessory1 != previousEquipaments.Accessory1)
        {
            dto = dto with { Accessory1 = newEquipaments.Accessory1 };
        }
        if (newEquipaments.Accessory2 != previousEquipaments.Accessory2)
        {
            dto = dto with { Accessory2 = newEquipaments.Accessory2 };
        }

        return dto;
    }
}
