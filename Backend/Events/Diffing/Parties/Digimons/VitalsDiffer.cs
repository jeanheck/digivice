using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Parties.Digimons;

namespace Backend.Events.Diffing.Parties.Digimons;

public static class VitalsDiffer
{
    public static VitalsDTO? Diff(Vitals? previousVitals, Vitals newVitals)
    {
        if (newVitals.HasNoChanges(previousVitals))
        {
            return null;
        }

        if (previousVitals == null)
        {
            return VitalsConverter.ToDTO(newVitals);
        }

        var dto = new VitalsDTO();

        if (newVitals.MaxHP != previousVitals.MaxHP)
        {
            dto = dto with { MaxHP = newVitals.MaxHP };
        }
        if (newVitals.MaxMP != previousVitals.MaxMP)
        {
            dto = dto with { MaxMP = newVitals.MaxMP };
        }
        if (newVitals.CurrentHP != previousVitals.CurrentHP)
        {
            dto = dto with { CurrentHP = newVitals.CurrentHP };
        }
        if (newVitals.CurrentMP != previousVitals.CurrentMP)
        {
            dto = dto with { CurrentMP = newVitals.CurrentMP };
        }

        return dto;
    }
}
