using Backend.Domain.Models;
using Backend.Memory.Repositories;
using Backend.Memory.Readers;
using Backend.Domain.Assemblers;

namespace Backend.Application.Services
{
    public class PlayerStateService(
        IAddressesRepository addressesRepository, 
        IAddressesReader addressesReader)
    {
        public Player GetPlayer()
        {
            var addresses = addressesRepository.GetPlayerAddresses();
            var resource = addressesReader.ReadPlayerResource(addresses);
            return PlayerAssembler.Assemble(resource);
        }
    }
}