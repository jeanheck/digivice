using Backend.Domain.Models;
using Backend.Memory.Resources;
using Backend.Domain.Shared;

namespace Backend.Domain.Assemblers
{
    public static class PlayerAssembler
    {
        public static Player Assemble(PlayerResource resource)
        {
            var playerName = PlayerNameDecoder.Decode(resource.NameInBytes);

            return new Player
            {
                Name = playerName,
                Bits = resource.Bits ?? 0,
                MapId = resource.MapId?.ToString("X4") ?? string.Empty,
                PreviousMapId = resource.PreviousMapId?.ToString("X4") ?? string.Empty,
                SeabedRoute = resource.SeabedRoute ?? 0,
                SeabedRouteType = resource.SeabedRouteType ?? 0
            };
        }
    }
}
