using Backend.Domain.Models.Parties;
using Backend.Events.Converters.Parties;
using Backend.Events.Diffing.Extensions;
using Backend.Events.Diffing.Parties.Digimons;
using Backend.Events.DTO.Parties;
using Backend.Events.DTO.Parties.Digimons;

namespace Backend.Events.Diffing.Parties;

public static class DigimonDiffer
{
    public static DigimonDTO? Diff(Digimon? previousDigimon, Digimon? newDigimon)
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
        bool tpChanged = previousDigimon.TP != newDigimon.TP;
        bool blastChanged = previousDigimon.Blast != newDigimon.Blast;
        bool experienceChanged = previousDigimon.Experience != newDigimon.Experience;
        bool activeDigievolutionIdChanged = previousDigimon.ActiveDigievolutionId != newDigimon.ActiveDigievolutionId;
        var hpDelta = VitalDiffer.Diff(previousDigimon.HP, newDigimon.HP);
        var mpDelta = VitalDiffer.Diff(previousDigimon.MP, newDigimon.MP);
        var inBattleDelta = InBattleDiffer.Diff(previousDigimon.InBattle, newDigimon.InBattle);
        var attributesDelta = AttributesDiffer.Diff(previousDigimon.Attributes, newDigimon.Attributes);
        var resistancesDelta = ResistancesDiffer.Diff(previousDigimon.Resistances, newDigimon.Resistances);
        var equipmentsDelta = EquipmentsDiffer.Diff(previousDigimon.Equipments, newDigimon.Equipments);

        List<DigievolutionSlotDTO> digievolutionsDelta = [];
        foreach (var newDigievolutionSlot in newDigimon.Digievolutions)
        {
            var previousDigievolutionSlot = previousDigimon.Digievolutions
                .FirstOrDefault(slot => slot.Index == newDigievolutionSlot.Index);
            var digievolutionSlotDelta = DigievolutionSlotDiffer.Diff(previousDigievolutionSlot, newDigievolutionSlot);
            if (digievolutionSlotDelta != null)
            {
                digievolutionsDelta.Add(digievolutionSlotDelta);
            }
        }

        List<StoredDigievolutionDTO> storedDigievolutionsDelta = [];
        foreach (var newStoredDigievolution in newDigimon.StoredDigievolutions)
        {
            var previousStoredDigievolution = previousDigimon.StoredDigievolutions
                .FirstOrDefault(stored => stored.DigievolutionId == newStoredDigievolution.DigievolutionId);
            var storedDigievolutionDelta = StoredDigievolutionDiffer.Diff(
                previousStoredDigievolution,
                newStoredDigievolution);
            if (storedDigievolutionDelta != null)
            {
                storedDigievolutionsDelta.Add(storedDigievolutionDelta);
            }
        }

        bool hasAnyChanges = levelChanged ||
                             tpChanged ||
                             blastChanged ||
                             experienceChanged ||
                             activeDigievolutionIdChanged ||
                             hpDelta != null ||
                             mpDelta != null ||
                             inBattleDelta != null ||
                             attributesDelta != null ||
                             resistancesDelta != null ||
                             equipmentsDelta != null ||
                             digievolutionsDelta.Count > 0 ||
                             storedDigievolutionsDelta.Count > 0;

        if (!hasAnyChanges)
        {
            return null;
        }

        var dto = new DigimonDTO();
        if (levelChanged)
        {
            dto = dto with { Level = newDigimon.Level };
        }
        if (tpChanged)
        {
            dto = dto with { TP = newDigimon.TP };
        }
        if (blastChanged)
        {
            dto = dto with { Blast = newDigimon.Blast };
        }
        if (experienceChanged)
        {
            dto = dto with { Experience = newDigimon.Experience };
        }
        if (activeDigievolutionIdChanged)
        {
            dto = dto with { ActiveDigievolutionId = newDigimon.ActiveDigievolutionId };
        }
        if (hpDelta != null)
        {
            dto = dto with { HP = hpDelta };
        }
        if (mpDelta != null)
        {
            dto = dto with { MP = mpDelta };
        }
        if (inBattleDelta != null)
        {
            dto = dto with { InBattle = inBattleDelta };
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
        if (storedDigievolutionsDelta.Count > 0)
        {
            dto = dto with { StoredDigievolutions = storedDigievolutionsDelta };
        }

        return dto;
    }
}
