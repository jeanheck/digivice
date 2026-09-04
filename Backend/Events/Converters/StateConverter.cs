using Backend.Domain.Models;
using Backend.Events.DTO;

namespace Backend.Events.Converters;

public static class StateConverter
{
    public static StateDTO ToDTO(State state)
    {
        return new StateDTO
        {
            Player = state.Player != null ? PlayerConverter.ToDTO(state.Player) : null,
            ImportantItems = state.ImportantItems != null ? ImportantItemsConverter.ToDTO(state.ImportantItems) : null,
            Party = state.Party != null ? PartyConverter.ToDTO(state.Party) : null,
            DigimonBattle = state.DigimonBattle != null ? DigimonBattleConverter.ToDTO(state.DigimonBattle) : null,
            CardBattle = state.CardBattle != null ? CardBattleConverter.ToDTO(state.CardBattle) : null,
            Journal = state.Journal != null ? JournalConverter.ToDTO(state.Journal) : null,
        };
    }
}
