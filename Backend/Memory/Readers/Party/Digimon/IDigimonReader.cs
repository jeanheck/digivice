using Backend.Memory.Addresses.Parties;
using Backend.Memory.Resources.Party;

namespace Backend.Memory.Readers.Party.Digimon
{
    public interface IDigimonReader
    {
        DigimonResource Read(DigimonAddress digimonAddress, DigimonStatusAddresses digimonStatusAddresses);
    }
}
