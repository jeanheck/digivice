using Backend.Events.Hubs;
using Backend.Events.Services;
using Backend.Models;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Backend.Models.Digimons;
using Backend.Models.Quests;

namespace Tests.Backend.Services
{
    public class EventDispatcherServiceTests
    {
        private readonly Mock<IHubContext<GameHub>> _hubContextMock;
        private readonly Mock<IHubClients> _clientsMock;
        private readonly Mock<IClientProxy> _clientProxyMock;
        private readonly Mock<ISingleClientProxy> _singleClientProxyMock;
        private readonly EventDispatcherService _dispatcher;

        public EventDispatcherServiceTests()
        {
            _hubContextMock = new Mock<IHubContext<GameHub>>();
            _clientsMock = new Mock<IHubClients>();
            _clientProxyMock = new Mock<IClientProxy>();
            _singleClientProxyMock = new Mock<ISingleClientProxy>();

            _clientsMock.Setup(c => c.All).Returns(_clientProxyMock.Object);
            _clientsMock.Setup(c => c.Client(It.IsAny<string>())).Returns(_singleClientProxyMock.Object);
            _hubContextMock.Setup(h => h.Clients).Returns(_clientsMock.Object);

            _dispatcher = new EventDispatcherService(_hubContextMock.Object);
        }

        private Digimon CreateValidDigimon(int hp = 100, int level = 1, int xp = 0, Digievolution? evo1 = null)
        {
            return new Digimon
            {
                SlotIndex = 1,
                BasicInfo = new BasicInfo { Name = "Agumon", CurrentHP = hp, MaxHP = 100, CurrentMP = 100, MaxMP = 100, Level = level, Experience = xp },
                Attributes = new Attributes { Strength = 10, Defense = 10, Spirit = 10, Wisdom = 10, Speed = 10, Charisma = 10 },
                Resistances = new Resistances { Fire = 1, Water = 1, Ice = 1, Wind = 1, Thunder = 1, Machine = 1, Dark = 1 },
                Equipments = new Equipments { Head = 0, Body = 0, RightHand = 0, LeftHand = 0, Accessory1 = 0, Accessory2 = 0 },
                EquippedDigievolutions = new Digievolution?[3] { evo1, null, null }
            };
        }

        [Fact]
        public void ProcessGameState_ShouldSendAllEvents_WhenEverythingChanges()
        {
            var initialState = new State
            {
                Player = new Player { MapId = "0001", Bits = 10, Name = "Me" },
                Party = new Party { Slots = new List<Digimon?> { CreateValidDigimon(100, 1, 10, new Digievolution { Id = 5, Level = 1 }) } },
                ImportantItems = new ImportantItems { FolderBag = new ImportantItem { Has = false }, TreeBoots = null, FishingPole = null, RedSnapper = null },
                Journal = new MainQuest { Steps = new List<QuestStep>(), Prerequisites = new List<Requisite>() }
                          != null ? new Journal { MainQuest = new MainQuest { Steps = new List<QuestStep>(), Prerequisites = new List<Requisite>() }, SideQuests = new List<SideQuest>() } : null
            };

            _dispatcher.ProcessGameState(initialState);

            _clientProxyMock.Invocations.Clear();

            var newState = new State
            {
                Player = new Player { MapId = "0002", Bits = 20, Name = "Me" },
                Party = new Party
                {
                    Slots = new List<Digimon?>
                    {
                        new Digimon
                        {
                            SlotIndex = 1,
                            BasicInfo = new BasicInfo { Name = "Agumon", CurrentHP = 50, MaxHP = 100, CurrentMP = 100, MaxMP = 100, Level = 2, Experience = 50 },
                            Attributes = new Attributes { Strength = 15, Defense = 10, Spirit = 10, Wisdom = 10, Speed = 10, Charisma = 10 },
                            Resistances = new Resistances { Fire = 2, Water = 1, Ice = 1, Wind = 1, Thunder = 1, Machine = 1, Dark = 1 },
                            Equipments = new Equipments { Head = 10, Body = 0, RightHand = 0, LeftHand = 0, Accessory1 = 0, Accessory2 = 0 },
                            EquippedDigievolutions = new Digievolution?[3] { new Digievolution { Id = 5, Level = 2 }, null, null }
                        }
                    }
                },
                ImportantItems = new ImportantItems { FolderBag = new ImportantItem { Has = true }, TreeBoots = null, FishingPole = null, RedSnapper = null },
                Journal = new Journal { MainQuest = new MainQuest { Title = "New Task", Steps = new List<QuestStep>(), Prerequisites = new List<Requisite>() }, SideQuests = new List<SideQuest>() }
            };

            _dispatcher.ProcessGameState(newState);

            _clientProxyMock.Verify(c => c.SendCoreAsync("LocationChanged", It.IsAny<object[]>(), default), Times.Once);
            _clientProxyMock.Verify(c => c.SendCoreAsync("PlayerBitsChanged", It.IsAny<object[]>(), default), Times.Once);

            _clientProxyMock.Verify(c => c.SendCoreAsync("DigimonVitalsChanged", It.IsAny<object[]>(), default), Times.Once);
            _clientProxyMock.Verify(c => c.SendCoreAsync("DigimonXpGained", It.IsAny<object[]>(), default), Times.Once);
            _clientProxyMock.Verify(c => c.SendCoreAsync("DigimonLevelUp", It.IsAny<object[]>(), default), Times.Once);
            _clientProxyMock.Verify(c => c.SendCoreAsync("DigimonAttributesChanged", It.IsAny<object[]>(), default), Times.Once);
            _clientProxyMock.Verify(c => c.SendCoreAsync("DigimonResistancesChanged", It.IsAny<object[]>(), default), Times.Once);
            _clientProxyMock.Verify(c => c.SendCoreAsync("DigimonEquipmentsChanged", It.IsAny<object[]>(), default), Times.Once);
            _clientProxyMock.Verify(c => c.SendCoreAsync("DigimonDigievolutionsChanged", It.IsAny<object[]>(), default), Times.Once);
            _clientProxyMock.Verify(c => c.SendCoreAsync("DigimonDigievolutionLevelUp", It.IsAny<object[]>(), default), Times.Once);

            _clientProxyMock.Verify(c => c.SendCoreAsync("ImportantItemsChanged", It.IsAny<object[]>(), default), Times.Once);
            _clientProxyMock.Verify(c => c.SendCoreAsync("JournalChanged", It.IsAny<object[]>(), default), Times.Once);
        }

        [Fact]
        public void ProcessGameState_ShouldSendPartySlotsChanged_WhenRosterChanges()
        {
            var initialState = new State { Party = new Party { Slots = new List<Digimon?> { CreateValidDigimon() } } };
            var digi2 = CreateValidDigimon();
            digi2.BasicInfo.Name = "Paildramon";
            var newState = new State { Party = new Party { Slots = new List<Digimon?> { CreateValidDigimon(), digi2 } } };

            _dispatcher.ProcessGameState(initialState);
            _clientProxyMock.Invocations.Clear();
            _dispatcher.ProcessGameState(newState);

            _clientProxyMock.Verify(c => c.SendCoreAsync("PartySlotsChanged", It.IsAny<object[]>(), default), Times.Once);
        }

        [Fact]
        public void DispatchConnectionStatus_ShouldSendToAllClients()
        {
            _dispatcher.DispatchConnectionStatus(true);
            _clientProxyMock.Verify(c => c.SendCoreAsync("ConnectionStatusChanged", It.IsAny<object[]>(), default), Times.Once);
        }

        [Fact]
        public void ProcessGameState_ShouldSendInitialState_OnFirstRun()
        {
            var fakeState = new State();
            _dispatcher.ProcessGameState(fakeState);
            _clientProxyMock.Verify(c => c.SendCoreAsync("InitialStateSync", It.IsAny<object[]>(), default), Times.Once);
        }
    }
}
