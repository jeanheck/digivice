using Backend.Domain.Models.Parties;
using Backend.Events.Converters.Parties;
using Backend.Events.Diffing.Extensions;
using Backend.Events.Diffing.Parties.Digimons;
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
        var vitalsDelta = VitalsDiffer.Diff(previousDigimon.Vitals, newDigimon.Vitals);
        var attributesDelta = AttributesDiffer.Diff(previousDigimon.Attributes, newDigimon.Attributes);
        var resistancesDelta = ResistancesDiffer.Diff(previousDigimon.Resistances, newDigimon.Resistances);
        var equipmentsDelta = EquipmentsDiffer.Diff(previousDigimon.Equipments, newDigimon.Equipments);

        var digievolutionsDelta = new List<DigievolutionSlotDTO>();
        foreach (var newDigievolution in newDigimon.Digievolutions)
        {
            var previousDigievolution = previousDigimon.Digievolutions.FirstOrDefault(e => e.Index == newDigievolution.Index);
            var digievolutionDelta = DigievolutionSlotDiffer.Diff(previousDigievolution, newDigievolution);
            if (digievolutionDelta != null)
            {
                digievolutionsDelta.Add(digievolutionDelta);
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
