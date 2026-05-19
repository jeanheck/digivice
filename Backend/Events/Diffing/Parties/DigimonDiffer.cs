using Backend.Domain.Models.Parties;
using Backend.Events.Converters.Parties;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Party;
using Backend.Events.DTO.Party.Digimon;

namespace Backend.Events.Diffing.Parties;

public static class DigimonDiffer
{
    public static DigimonDTO? Diff(Digimon? previousDigimon, Digimon newDigimon)
    {
        if (newDigimon.HasNoChanges(previousDigimon))
        {
            return null;
        }

        if (previousDigimon == null)
        {
            return DigimonConverter.ToDTO(newDigimon);
        }

        bool levelChanged = previousDigimon.Level != newDigimon.Level;
        bool experienceChanged = previousDigimon.Experience != newDigimon.Experience;
        bool activeDigievolutionIdChanged = previousDigimon.ActiveDigievolutionId != newDigimon.ActiveDigievolutionId;

        var vitalsDelta = !previousDigimon.Vitals.Equals(newDigimon.Vitals)
            ? VitalsConverter.ToDTO(newDigimon.Vitals)
            : null;

        var attributesDelta = !previousDigimon.Attributes.Equals(newDigimon.Attributes)
            ? AttributesConverter.ToDTO(newDigimon.Attributes)
            : null;

        var resistancesDelta = !previousDigimon.Resistances.Equals(newDigimon.Resistances)
            ? ResistancesConverter.ToDTO(newDigimon.Resistances)
            : null;

        var equipmentsDelta = !previousDigimon.Equipments.Equals(newDigimon.Equipments)
            ? EquipmentsConverter.ToDTO(newDigimon.Equipments)
            : null;

        var digievolutionsDelta = new List<DigievolutionSlotDTO>();
        foreach (var newEvolution in newDigimon.Digievolutions)
        {
            var previousEvolution = previousDigimon.Digievolutions.FirstOrDefault(e => e.Index == newEvolution.Index);
            var evolutionDelta = DigievolutionSlotDiffer.Diff(previousEvolution, newEvolution);
            if (evolutionDelta != null)
            {
                digievolutionsDelta.Add(evolutionDelta);
            }
        }

        bool hasAnyChanges = levelChanged ||
                             experienceChanged ||
                             activeDigievolutionIdChanged ||
                             vitalsDelta != null ||
                             attributesDelta != null ||
                             resistancesDelta != null ||
                             equipmentsDelta != null ||
                             digievolutionsDelta.Count > 0;

        if (!hasAnyChanges)
        {
            return null;
        }

        var dto = new DigimonDTO();
        if (levelChanged)
        {
            dto = dto with { Level = newDigimon.Level };
        }
        if (experienceChanged)
        {
            dto = dto with { Experience = newDigimon.Experience };
        }
        if (activeDigievolutionIdChanged)
        {
            dto = dto with { ActiveDigievolutionId = newDigimon.ActiveDigievolutionId };
        }
        if (vitalsDelta != null)
        {
            dto = dto with { Vitals = vitalsDelta };
        }
        if (attributesDelta != null)
        {
            dto = dto with { Attributes = attributesDelta };
        }
        if (resistancesDelta != null)
        {
            dto = dto with { Resistances = resistancesDelta };
        }
        if (equipmentsDelta != null)
        {
            dto = dto with { Equipments = equipmentsDelta };
        }
        if (digievolutionsDelta.Count > 0)
        {
            dto = dto with { Digievolutions = digievolutionsDelta };
        }

        return dto;
    }
}
