using Backend.Memory.Addresses.Parties;
using Backend.Memory.Resources.Party;

namespace Backend.Memory.Readers.Parties.Digimons
{
    public interface IDigimonReader
    {
        DigimonResource Read(DigimonAddress digimonAddress, DigimonStatusAddresses digimonStatusAddresses);
    }
}
