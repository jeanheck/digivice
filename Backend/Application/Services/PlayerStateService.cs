using Backend.Domain.Models;
using Backend.Memory.Repositories;
using Backend.Memory.Readers;
using Backend.Domain.Assemblers;

namespace Backend.Application.Services
{
    public class PlayerStateService(
        IAddressesRepository addressesRepository, 
        IResourceReader resourceReader)
    {
        public Player GetPlayer()
        {
            var addresses = addressesRepository.GetPlayerAddresses();
            var resource = resourceReader.ReadPlayer(addresses);
            return PlayerAssembler.Assemble(resource);
        }
    }
}