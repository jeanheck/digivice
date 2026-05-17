using Backend.Memory.Addresses.Party;
using Backend.Memory.Resources.Party;

namespace Backend.Memory.Readers.Digimon
{
    public interface IDigimonReader
    {
        DigimonResource Read(DigimonAddress digimonAddress, DigimonStatusAddresses digimonStatusAddresses);
    }
}
