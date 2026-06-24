using Backend.Events.DTO.Interfaces;

namespace Backend.Events.DTO;

public record class ConnectionDTO(
    bool IsConnected,
    string? ErrorCode = null,
    string? ErrorDetail = null) : IDTO;
