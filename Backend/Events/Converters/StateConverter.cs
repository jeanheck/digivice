using Backend.Domain.Models;
using Backend.Events.DTO;

namespace Backend.Events.Converters;

public static class StateConverter
{
    public static StateDTO ToDTO(State state)
    {
        return new StateDTO
        {
            Player = PlayerConverter.ToDTO(state.Player),
            ImportantItems = ImportantItemsConverter.ToDTO(state.ImportantItems),
            Party = PartyConverter.ToDTO(state.Party),
            DigimonBattle = DigimonBattleConverter.ToDTO(state.DigimonBattle),
            CardBattle = CardBattleConverter.ToDTO(state.CardBattle),
            Auctions = AuctionsConverter.ToDTO(state.Auctions),
            Npcs = NpcsConverter.ToDTO(state.Npcs),
            Journal = JournalConverter.ToDTO(state.Journal),
        };
    }
}
