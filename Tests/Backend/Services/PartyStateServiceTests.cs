using Backend.Interfaces;
using Backend.Models.Addresses;
using Backend.Models.Resources;
using Backend.Services;
using Moq;

namespace Tests.Backend.Services
{
    public class PartyStateServiceTests
    {
        private readonly Mock<IGameDatabase> _mockDatabase;
        private readonly Mock<IGameReader> _mockReader;
        private readonly DigimonStateService _digimonService;
        private readonly PartyStateService _partyService;

        public PartyStateServiceTests()
        {
            _mockDatabase = new Mock<IGameDatabase>();
            _mockReader = new Mock<IGameReader>();

            _digimonService = new DigimonStateService(_mockDatabase.Object, _mockReader.Object);
            _partyService = new PartyStateService(_mockDatabase.Object, _mockReader.Object, _digimonService);
        }

        [Fact]
        public void GetParty_ShouldIgnoreEmptySlots_AndMapAddresses()
        {
            _mockDatabase.Setup(db => db.GetPartyAddresses()).Returns(new PartyAddresses { PartySlot1 = "A" });
            _mockDatabase.Setup(db => db.GetDigimonAddresses()).Returns(new DigimonAddresses
            {
                EmptySlotId = "0xFF",
                Digimons = new List<DigimonBaseAddress>
                {
                    new DigimonBaseAddress { Id = 1, Address = "0x2000", Name = "Agumon" }
                }
            });

            _mockReader.Setup(r => r.ReadParty(It.IsAny<PartyAddresses>()))
                       .Returns(new PartyResource { ActiveDigimonIds = { 0x01, 0xFF } });

            _mockReader.Setup(r => r.ReadDigimonResource(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DigimonAddresses>()))
                       .Returns(new DigimonResource
                       {
                           LogicBlock = new byte[1000]
                       });

            var result = _partyService.GetParty();

            Assert.NotNull(result);
            Assert.NotNull(result.Slots[0]);
            Assert.Equal("Agumon", result.Slots[0]?.BasicInfo?.Name);

            Assert.Null(result.Slots[1]);
            Assert.Null(result.Slots[2]);
        }
    }
}
