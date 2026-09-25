using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.DTO.Parties.Digimons;

namespace Backend.Events.Converters.Parties.Digimons;

public static class VitalConverter
{
    public static VitalDTO ToDTO(Vital vital) => new()
    {
        Current = vital.Current,
        Max = vital.Max
    };
}
