namespace Backend.Events.Models;

public enum EventType
{
    InitialState,
    EmulatorConnectionStatusChanged,
    PlayerChanged,
    ImportantItemsChanged,
    PartyChanged,
    BattleChanged,
    CardBattleChanged,
    JournalChanged
}
