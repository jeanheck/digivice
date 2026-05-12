using Backend.Interfaces;
using Backend.Models.Addresses;
using Backend.Models.Resources;
using Backend.Services;
using Moq;

namespace Tests.Backend.Services
{
    public class DigimonStateServiceTests
    {
        private readonly Mock<IGameDatabase> _mockDatabase;
        private readonly Mock<IGameReader> _mockReader;
        private readonly DigievolutionStateService _digiEvoService;
        private readonly DigimonStateService _digimonService;

        public DigimonStateServiceTests()
        {
            _mockDatabase = new Mock<IGameDatabase>();
            _mockReader = new Mock<IGameReader>();
            _digiEvoService = new DigievolutionStateService();
            _digimonService = new DigimonStateService(_mockDatabase.Object, _mockReader.Object, _digiEvoService);
        }

        [Fact]
        public void GetDigimon_ShouldMapAttributesCorrectly_FromLogicBlock()
        {
            var logicBlock = new byte[100];
            logicBlock[0] = 0xE8; logicBlock[1] = 0x03; // 1000 CurrentHP
            logicBlock[2] = 0x32; logicBlock[3] = 0x00; // 50 Str

            _mockDatabase.Setup(db => db.GetDigimonAddresses()).Returns(new DigimonAddresses
            {
                BasicInfo = new BasicInfoAddresses { CurrentHP = "0x00" },
                Attributes = new AttributesAddresses { Strength = "0x02" },
                Equipaments = new EquipmentsAddresses { Head = "0x0A" }
            });

            _mockReader.Setup(r => r.ReadDigimon(1, 0, It.IsAny<DigimonAddresses>()))
                       .Returns(new DigimonResource { LogicBlock = logicBlock });

            var result = _digimonService.GetDigimon(1, 0x02, 0);

            Assert.Equal(1000, result.BasicInfo?.CurrentHP);
            Assert.Equal(50, result.Attributes?.Strength);
            Assert.Equal("Unknown", result.BasicInfo?.Name);
        }
    }
}
