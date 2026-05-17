using Backend.Domain.Models;
using Backend.Memory.Repositories;
using Backend.Memory.Readers;
using Backend.Domain.Assemblers;
using Backend.Memory.Resources;

namespace Backend.Application.Services
{
    public class PlayerStateService(
        IAddressesRepository addressesRepository,
        IPlayerReader playerReader)
    {
        private PlayerResource GetResource()
        {
            var addresses = addressesRepository.GetPlayerAddresses();
            return playerReader.Read(addresses);
        }

        public Player GetPlayer()
        {
            return PlayerAssembler.Assemble(GetResource());
        }
    }
}