using Backend.Events.DTO.Interfaces;

namespace Backend.Events.DTO;

public record StateDTO : IDTO
{
    public required PlayerDTO Player { get; init; }
    public required ImportantItemsDTO ImportantItems { get; init; }
    public required PartyDTO Party { get; init; }
    public required DigimonBattleDTO DigimonBattle { get; init; }
    public required CardBattleDTO CardBattle { get; init; }
    public required AuctionsDTO Auctions { get; init; }
    public required NpcsDTO Npcs { get; init; }
    public required JournalDTO Journal { get; init; }
}
