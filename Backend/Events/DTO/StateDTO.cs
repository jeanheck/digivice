using Backend.Events.DTO.Interfaces;

namespace Backend.Events.DTO;

public record StateDTO : IDTO
{
    public PlayerDTO? Player { get; init; }
    public JournalDTO? Journal { get; init; }
    public PartyDTO? Party { get; init; }
}
