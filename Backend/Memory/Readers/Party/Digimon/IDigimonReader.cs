using Backend.Memory.Addresses.Party;
using Backend.Memory.Resources.Party;

namespace Backend.Memory.Readers.Party.Digimon
{
    public interface IDigimonReader
    {
        DigimonResource Read(DigimonAddress digimonAddress, DigimonStatusAddresses digimonStatusAddresses);
    }
}
