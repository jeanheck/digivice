using Backend.Domain.Models;
using Backend.Events.Models;
using Backend.Events.Models.Digimon;

namespace Backend.Events.Diffing;

public static class DigimonDiffer
{
    public static IEnumerable<BaseEvent> Diff(int index, Digimon? previous, Digimon current)
    {
        var events = new List<BaseEvent>();

        if (previous == null)
        {
            // Note: If previous is null, we might want to trigger a full update event, 
            // but in the current logic, the caller (StateDiffer) handles party roster changes separately.
            return events;
        }

        // Compare Vitals
        if (previous.BasicInfo.CurrentHP != current.BasicInfo.CurrentHP ||
            previous.BasicInfo.MaxHP != current.BasicInfo.MaxHP ||
            previous.BasicInfo.CurrentMP != current.BasicInfo.CurrentMP ||
            previous.BasicInfo.MaxMP != current.BasicInfo.MaxMP)
        {
            events.Add(new DigimonVitalsChangedEvent(index, current.BasicInfo.CurrentHP, current.BasicInfo.MaxHP, current.BasicInfo.CurrentMP, current.BasicInfo.MaxMP));
        }

        // Compare XP (Experience)
        if (previous.BasicInfo.Experience != current.BasicInfo.Experience)
        {
            events.Add(new DigimonExperienceChangedEvent(index, current.BasicInfo.Experience));
        }

        // Compare Level
        if (current.BasicInfo.Level > previous.BasicInfo.Level)
        {
            events.Add(new DigimonLevelChangedEvent(index, current.BasicInfo.Level));
        }

        // Compare Attributes
        if (!previous.Attributes.Equals(current.Attributes))
        {
            events.Add(new DigimonAttributesChangedEvent(index, current.Attributes.Strength, current.Attributes.Defense, current.Attributes.Spirit, current.Attributes.Wisdom, current.Attributes.Speed, current.Attributes.Charisma));
        }

        // Compare Resistances
        if (!previous.Resistances.Equals(current.Resistances))
        {
            events.Add(new DigimonResistancesChangedEvent(index, current.Resistances.Fire, current.Resistances.Water, current.Resistances.Ice, current.Resistances.Wind, current.Resistances.Thunder, current.Resistances.Machine, current.Resistances.Dark));
        }

        // Compare Equipments
        if (!previous.Equipments.Equals(current.Equipments))
        {
            events.Add(new DigimonEquipmentsChangedEvent(index, current.Equipments));
        }

        // Compare Digievolutions
        if (!Enumerable.SequenceEqual(previous.Digievolutions, current.Digievolutions))
        {
            events.Add(new DigimonDigievolutionsChangedEvent(index, current.Digievolutions));
        }

        // Compare Active Digievolution
        if (previous.ActiveDigievolutionId != current.ActiveDigievolutionId)
        {
            events.Add(new DigimonActiveDigievolutionChangedEvent(index, current.ActiveDigievolutionId));
        }

        return events;
    }
}
