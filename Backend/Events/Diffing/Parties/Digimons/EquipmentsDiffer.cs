using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Parties.Digimons;

namespace Backend.Events.Diffing.Parties.Digimons;

public static class EquipmentsDiffer
{
    public static EquipmentsDTO? Diff(Equipments? previousEquipments, Equipments newEquipments)
    {
        if (newEquipments.HasNoChanges(previousEquipments))
        {
            return null;
        }

        if (previousEquipments == null)
        {
            return EquipmentsConverter.ToDTO(newEquipments);
        }

        var dto = new EquipmentsDTO();

        if (newEquipments.Head != previousEquipments.Head)
        {
            dto = dto with { Head = newEquipments.Head };
        }
        if (newEquipments.Body != previousEquipments.Body)
        {
            dto = dto with { Body = newEquipments.Body };
        }
        if (newEquipments.Right != previousEquipments.Right)
        {
            dto = dto with { Right = newEquipments.Right };
        }
        if (newEquipments.Left != previousEquipments.Left)
        {
            dto = dto with { Left = newEquipments.Left };
        }
        if (newEquipments.Accessory1 != previousEquipments.Accessory1)
        {
            dto = dto with { Accessory1 = newEquipments.Accessory1 };
        }
        if (newEquipments.Accessory2 != previousEquipments.Accessory2)
        {
            dto = dto with { Accessory2 = newEquipments.Accessory2 };
        }

        return dto;
    }
}
