namespace Backend.Events.Models.State;

public class InitialStateEvent(Domain.Models.State state) : BaseEvent(EventType.InitialState)
{
    public Domain.Models.State State { get; } = state;
}
