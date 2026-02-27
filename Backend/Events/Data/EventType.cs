namespace Backend.Events.Data;

public enum EventType
{
    ConnectionStatusChanged,
    InitialStateSync,
    PlayerBitsChanged,
    PartySlotsChanged,
    DigimonVitalsChanged,
    DigimonXpGained,
    DigimonAttributesChanged,
    DigimonResistancesChanged,
    DigimonLevelUp,
    DigimonEquipmentsChanged,
    DigimonDigievolutionsChanged,
    DigimonDigievolutionLevelUp,
    ImportantItemsChanged
}
