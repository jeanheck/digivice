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
            Party = state.Party != null ? PartyConverter.ToDTO(state.Party) : null,
            Battle = state.Battle != null ? BattleConverter.ToDTO(state.Battle) : null,
            Journal = state.Journal != null ? JournalConverter.ToDTO(state.Journal) : null,
        };
    }
}
