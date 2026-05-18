using Backend.Events.DTO.Interfaces;

namespace Backend.Events.DTO;

public record StateDTO : IDTO
{
    public PlayerDTO? Player { get; init; }
    public JournalDTO? Journal { get; init; }
    public object? Party { get; init; } // Mantido como object para migração progressiva do Party
}
