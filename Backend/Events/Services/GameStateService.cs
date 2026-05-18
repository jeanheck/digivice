using Backend.Domain.Models;
using Backend.Events.Generation;
using Backend.Events.States;

namespace Backend.Events.Services;

public class GameStateService(
    IGameStateStore gameStateStore,
    IEventDispatcherService eventDispatcherService) : IGameStateService
{
    public void ProcessNewState(State newState)
    {
        var previousState = gameStateStore.CurrentState;
        
        var events = StateEventGenerator.Generate(previousState, newState);
        
        eventDispatcherService.DispatchEvents(events);
        
        gameStateStore.UpdateState(newState);
    }
}
