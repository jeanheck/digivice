namespace Backend.Events.Models;

public enum EventType
{
    InitialState,
    EmulatorConnectionStatusChanged,
    PlayerChanged,
    ImportantItemsChanged,
    PartyChanged,
    DigimonBattleChanged,
    CardBattleChanged,
    JournalChanged
}
