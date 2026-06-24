using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Parties.Digimons;

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
        if (newEquipaments.Right != previousEquipaments.Right)
        {
            dto = dto with { Right = newEquipaments.Right };
        }
        if (newEquipaments.Left != previousEquipaments.Left)
        {
            dto = dto with { Left = newEquipaments.Left };
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
