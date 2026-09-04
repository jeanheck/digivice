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
    AuctionsChanged,
    NpcsChanged,
    JournalChanged
}
