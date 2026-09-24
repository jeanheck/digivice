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
                Bits = resource.Bits,
                MapId = resource.MapId.ToString("X4"),
                PreviousMapId = resource.PreviousMapId.ToString("X4"),
                SeabedRoute = resource.SeabedRoute,
                MapVariant = resource.MapVariant
            };
        }
    }
}
