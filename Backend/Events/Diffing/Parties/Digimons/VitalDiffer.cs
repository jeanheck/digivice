using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Parties.Digimons;

namespace Backend.Events.Diffing.Parties.Digimons;

public static class VitalDiffer
{
    public static VitalDTO? Diff(Vital? previousVital, Vital newVital)
    {
        if (newVital.HasNoChanges(previousVital))
        {
            return null;
        }

        if (previousVital == null)
        {
            return VitalConverter.ToDTO(newVital);
        }

        var dto = new VitalDTO();

        if (newVital.Current != previousVital.Current)
        {
            dto = dto with { Current = newVital.Current };
        }
        if (newVital.Max != previousVital.Max)
        {
            dto = dto with { Max = newVital.Max };
        }

        return dto;
    }
}
