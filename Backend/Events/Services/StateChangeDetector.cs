using Backend.Events.Data;
using Backend.Events.Data.Digimon;
using Backend.Models;
using Backend.Models.Quests;
using Backend.Models.Digimons;

namespace Backend.Events.Services;

public class StateChangeDetector
{
    public IEnumerable<BaseEvent> DetectChanges(State? previousState, State newState)
    {
        var events = new List<BaseEvent>();

        if (previousState == null)
        {
            events.Add(new InitialStateChangedEvent(newState));
            return events;
        }

        // 1. Player Comparison
        if (newState.Player != null)
        {
            if (previousState.Player != null)
            {
                if (newState.Player.MapId != previousState.Player.MapId)
                {
                    events.Add(new PlayerLocationChangedEvent(newState.Player.MapId));
                }

                if (newState.Player.Name != previousState.Player.Name)
                {
                    events.Add(new PlayerNameChangedEvent(newState.Player.Name));
                }

                if (newState.Player.Bits != previousState.Player.Bits)
                {
                    events.Add(new PlayerBitsChangedEvent(newState.Player.Bits));
                }
            }
            else
            {
                events.Add(new PlayerNameChangedEvent(newState.Player.Name));
                events.Add(new PlayerBitsChangedEvent(newState.Player.Bits));
            }
        }

        // 2. Party Comparison
        DetectPartyChanges(previousState.Party, newState.Party, events);

        // 3. Important Items Comparison
        if (HasChanged(previousState.ImportantItems, newState.ImportantItems))
        {
            events.Add(new ImportantItemsChangedEvent(newState.ImportantItems));
        }

        // 4. Journal Comparison
        if (HasChanged(previousState.Journal, newState.Journal))
        {
            events.Add(new JournalChangedEvent(newState.Journal));
        }

        return events;
    }

    private void DetectPartyChanges(Party? oldParty, Party? newParty, List<BaseEvent> events)
    {
        if (newParty == null) return;

        if (oldParty == null)
        {
            var activeDigimons = newParty.Slots.Where(d => d != null).Select(d => d!).ToList();
            events.Add(new PartySlotsChangedEvent(activeDigimons));
            return;
        }

        var newSlots = newParty.Slots;
        var oldSlots = oldParty.Slots;

        bool partyRosterChanged = false;
        if (newSlots.Count != oldSlots.Count)
        {
            partyRosterChanged = true;
        }
        else
        {
            for (int i = 0; i < newSlots.Count; i++)
            {
                var newDigi = newSlots[i];
                var oldDigi = oldSlots[i];

                if ((newDigi == null && oldDigi != null) || (newDigi != null && oldDigi == null) ||
                    (newDigi != null && oldDigi != null && newDigi.BasicInfo.Name != oldDigi.BasicInfo.Name))
                {
                    partyRosterChanged = true;
                    break;
                }
            }
        }

        if (partyRosterChanged)
        {
            var activeDigimons = newSlots.Where(d => d != null).Select(d => d!).ToList();
            events.Add(new PartySlotsChangedEvent(activeDigimons));
        }
        else
        {
            for (int i = 0; i < newSlots.Count; i++)
            {
                if (newSlots[i] != null && oldSlots[i] != null)
                {
                    if (!newSlots[i]!.Equals(oldSlots[i]))
                    {
                        DetectDigimonChanges(i, oldSlots[i]!, newSlots[i]!, events);
                    }
                }
            }
        }
    }

    private void DetectDigimonChanges(int index, Digimon oldDigi, Digimon newDigi, List<BaseEvent> events)
    {
        // Compare Vitals
        if (oldDigi.BasicInfo.CurrentHP != newDigi.BasicInfo.CurrentHP ||
            oldDigi.BasicInfo.MaxHP != newDigi.BasicInfo.MaxHP ||
            oldDigi.BasicInfo.CurrentMP != newDigi.BasicInfo.CurrentMP ||
            oldDigi.BasicInfo.MaxMP != newDigi.BasicInfo.MaxMP)
        {
            events.Add(new DigimonVitalsChangedEvent(index, newDigi.BasicInfo.CurrentHP, newDigi.BasicInfo.MaxHP, newDigi.BasicInfo.CurrentMP, newDigi.BasicInfo.MaxMP));
        }

        // Compare XP (Experience)
        if (oldDigi.BasicInfo.Experience != newDigi.BasicInfo.Experience)
        {
            events.Add(new DigimonExperienceChangedEvent(index, newDigi.BasicInfo.Level, newDigi.BasicInfo.Experience));
        }

        // Compare Level
        if (newDigi.BasicInfo.Level > oldDigi.BasicInfo.Level)
        {
            events.Add(new DigimonLevelChangedEvent(index, oldDigi.BasicInfo.Level, newDigi.BasicInfo.Level));
        }

        // Compare Attributes
        if (!oldDigi.Attributes.Equals(newDigi.Attributes))
        {
            events.Add(new DigimonAttributesChangedEvent(index, newDigi.Attributes.Strength, newDigi.Attributes.Defense, newDigi.Attributes.Spirit, newDigi.Attributes.Wisdom, newDigi.Attributes.Speed, newDigi.Attributes.Charisma));
        }

        // Compare Resistances
        if (!oldDigi.Resistances.Equals(newDigi.Resistances))
        {
            events.Add(new DigimonResistancesChangedEvent(index, newDigi.Resistances.Fire, newDigi.Resistances.Water, newDigi.Resistances.Ice, newDigi.Resistances.Wind, newDigi.Resistances.Thunder, newDigi.Resistances.Machine, newDigi.Resistances.Dark));
        }

        // Compare Equipments
        if (!oldDigi.Equipments.Equals(newDigi.Equipments))
        {
            events.Add(new DigimonEquipmentsChangedEvent(index, newDigi.Equipments));
        }

        // Compare Equipped Digievolutions
        if (!Enumerable.SequenceEqual(oldDigi.EquippedDigievolutions, newDigi.EquippedDigievolutions))
        {
            events.Add(new DigimonDigievolutionsChangedEvent(index, newDigi.EquippedDigievolutions));
        }

        // Compare Active Digievolution
        if (oldDigi.ActiveDigievolutionId != newDigi.ActiveDigievolutionId)
        {
            events.Add(new DigimonActiveDigievolutionChangedEvent(index, newDigi.ActiveDigievolutionId));
        }
    }

    private static bool HasChanged<T>(T? oldVal, T? newVal) where T : class
    {
        if (oldVal == null && newVal == null) return false;
        if (oldVal == null || newVal == null) return true;
        return !oldVal.Equals(newVal);
    }
}
