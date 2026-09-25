using Backend.Memory.Resources.Parties;

namespace Backend.Application.Loaders.Interfaces
{
    public interface IDigimonLoader
    {
        DigimonResource? Load(int digimonId, int zeroBasedPartySlotIndex);
    }
}
