using Backend.Memory.Resources.Parties;

namespace Backend.Application.Loaders.Parties
{
    public interface IDigimonInCombatLoader
    {
        DigimonInCombatResource Load(int zeroBasedPartySlotIndex);
    }
}
