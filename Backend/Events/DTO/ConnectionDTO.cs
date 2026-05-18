using Backend.Events.DTO.Interfaces;

namespace Backend.Events.DTO;

public record class ConnectionDTO(bool IsConnected) : IDTO;
