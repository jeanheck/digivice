using Backend.Application.Loaders.Interfaces;
using Backend.Memory.Readers;
using Backend.Memory.Repositories;
using Backend.Memory.Resources;

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
