using Backend.Application.Loaders.Interfaces;
using Backend.Memory.Repositories;
using Backend.Memory.Resources;
using Backend.Memory.Readers.Interfaces;

namespace Backend.Application.Loaders
{
    public class CardBattleLoader(
        IAddressesRepository addressesRepository,
        ICardBattleReader cardBattleReader) : ICardBattleLoader
    {
        public CardBattleResource Load()
        {
            return cardBattleReader.Read(addressesRepository.GetCardBattleAddresses());
        }
    }
}
