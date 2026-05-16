using Backend.Events.Models;

namespace Backend.Events.Diffing;

public interface IDiffer<T>
{
    IEnumerable<BaseEvent> Diff(T? previous, T current);
}