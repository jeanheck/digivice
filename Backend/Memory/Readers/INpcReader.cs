using Backend.Memory.Addresses;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers
{
    public interface INpcReader
    {
        NpcResource Read(KeyValuePair<string, NpcAddresses> npcAddresses);
    }
}
