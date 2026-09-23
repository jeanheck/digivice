using Backend.Events.DTO.Interfaces;
using Backend.Events.Models;

namespace Backend.Events.DTO;

public record class HealthDTO(
    HealthStatus Status,
    string? ErrorCode = null,
    string? ErrorDetail = null) : IDTO;
