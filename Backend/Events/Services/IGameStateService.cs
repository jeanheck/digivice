using Backend.Domain.Models;

namespace Backend.Events.Services;

public interface IGameStateService
{
    void ProcessNewState(State newState);
}
