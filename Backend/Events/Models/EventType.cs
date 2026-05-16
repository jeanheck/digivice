namespace Backend.Events.Models;

public enum EventType
{
    ConnectionStatusChanged,
    InitialState,
    PlayerBitsChanged,
    PlayerNameChanged,
    PlayerLocationChanged,
    PartySlotsChanged,
    DigimonVitalsChanged,
    DigimonExperienceChanged,
    DigimonAttributesChanged,
    DigimonResistancesChanged,
    DigimonLevelChanged,
    DigimonEquipmentsChanged,
    DigimonDigievolutionsChanged,
    DigimonActiveDigievolutionChanged,
    JournalChanged
}
