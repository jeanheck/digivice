namespace Backend.Events.Models;

public enum EventType
{
    InitialState,
    HealthChanged,
    PlayerChanged,
    ImportantItemsChanged,
    PartyChanged,
    DigimonBattleChanged,
    CardBattleChanged,
    AuctionsChanged,
    NpcsChanged,
    JournalChanged
}
