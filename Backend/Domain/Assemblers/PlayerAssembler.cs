using Backend.Domain.Models;
using Backend.Memory.Resources;

namespace Backend.Domain.Assemblers
{
    public static class PlayerAssembler
    {
        public static Player Assemble(PlayerResource resource)
        {
            return new Player
            {
                Bits = resource.Bits ?? 0,
                MapId = resource.MapId?.ToString("X4") ?? string.Empty,
                PreviousMapId = resource.PreviousMapId?.ToString("X4") ?? string.Empty,
                SeabedRoute = resource.SeabedRoute ?? 0,
                MapVariant = resource.MapVariant ?? 0
            };
        }
    }
}
