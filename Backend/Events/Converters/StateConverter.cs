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
            Journal = state.Journal != null ? JournalConverter.ToDTO(state.Journal) : null,
            Party = state.Party
        };
    }
}
