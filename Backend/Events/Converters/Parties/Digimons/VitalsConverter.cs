using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.DTO.Party.Digimon;

namespace Backend.Events.Converters.Parties.Digimons;

public static class VitalsConverter
{
    public static VitalsDTO ToDTO(Vitals vitals) => new()
    {
        MaxHP = vitals.MaxHP,
        MaxMP = vitals.MaxMP,
        CurrentHP = vitals.CurrentHP,
        CurrentMP = vitals.CurrentMP
    };
}
