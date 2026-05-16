using Backend.Domain.Models;
using Backend.Events.Models;
using Backend.Events.Models.Player;

namespace Backend.Events.Diffing;

public static class PlayerDiffer
{
    public static IEnumerable<BaseEvent> Diff(Player? previousPlayer, Player? newPlayer)
    {
        var events = new List<BaseEvent>();

        if (newPlayer != null)
        {
            if (previousPlayer != null)
            {
                if (newPlayer.MapId != previousPlayer.MapId)
                {
                    events.Add(new PlayerLocationChangedEvent(newPlayer.MapId));
                }

                if (newPlayer.Name != previousPlayer.Name)
                {
                    events.Add(new PlayerNameChangedEvent(newPlayer.Name));
                }

                if (newPlayer.Bits != previousPlayer.Bits)
                {
                    events.Add(new PlayerBitsChangedEvent(newPlayer.Bits));
                }
            }
            else
            {
                events.Add(new PlayerNameChangedEvent(newPlayer.Name));
                events.Add(new PlayerBitsChangedEvent(newPlayer.Bits));
                events.Add(new PlayerLocationChangedEvent(newPlayer.MapId));
            }
        }

        return events;
    }
}
