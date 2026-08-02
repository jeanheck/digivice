using Backend.Memory.Resources.Parties;

namespace Backend.Application.Loaders.Parties
{
    public interface IDigimonLoader
    {
        DigimonResource? Load(int digimonId);
    }
}
