using Backend.Events.DTO.Interfaces;

namespace Backend.Events.DTO;

public record StateDTO : IDTO
{
    public PlayerDTO? Player { get; init; }
    public ImportantItemsDTO? ImportantItems { get; init; }
    public PartyDTO? Party { get; init; }
    public BattleDTO? Battle { get; init; }
    public CardBattleDTO? CardBattle { get; init; }
    public JournalDTO? Journal { get; init; }
}
