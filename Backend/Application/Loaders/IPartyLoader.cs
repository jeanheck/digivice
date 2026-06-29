using Backend.Memory.Resources;

namespace Backend.Application.Loaders
{
    public interface IPartyLoader
    {
        PartyResource Load();
    }
}
