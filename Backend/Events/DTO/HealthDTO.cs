using Backend.Events.DTO.Interfaces;

namespace Backend.Events.DTO;

public record class HealthDTO(
    bool IsHealthy,
    string? ErrorCode = null,
    string? ErrorDetail = null) : IDTO;
