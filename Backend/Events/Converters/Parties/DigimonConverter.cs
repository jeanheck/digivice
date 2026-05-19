using Backend.Domain.Models.Parties;
using Backend.Events.DTO.Party;

namespace Backend.Events.Converters.Parties;

public static class DigimonConverter
{
    public static DigimonDTO ToDTO(Digimon digimon) => new()
    {
        Level = digimon.Level,
        Experience = digimon.Experience,
        Vitals = VitalsConverter.ToDTO(digimon.Vitals),
        Attributes = AttributesConverter.ToDTO(digimon.Attributes),
        Resistances = ResistancesConverter.ToDTO(digimon.Resistances),
        Equipments = EquipmentsConverter.ToDTO(digimon.Equipments),
        Digievolutions = digimon.Digievolutions.Select(DigievolutionSlotConverter.ToDTO).ToList(),
        ActiveDigievolutionId = digimon.ActiveDigievolutionId
    };
}
