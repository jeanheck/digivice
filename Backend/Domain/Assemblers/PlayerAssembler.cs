using Backend.Domain.Models;
using Backend.Memory.Resources;
using Backend.Application.Shared;

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
                Bits = resource.Bits,
                MapId = resource.MapId?.ToString("X4")
            };
        }
    }
}
