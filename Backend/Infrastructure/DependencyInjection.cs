using Backend.Application;
using Backend.Application.Loaders;
using Backend.Application.Loaders.Journals;
using Backend.Application.Loaders.Parties;
using Backend.Application.Providers;
using Backend.Diagnostics;
using Backend.Events.Services;
using Backend.Events.States;
using Backend.Infrastructure.Duckstation;
using Backend.Infrastructure.Memory;
using Backend.Infrastructure.Processes;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Journals;
using Backend.Memory.Readers.Journals.Quests;
using Backend.Memory.Readers.Parties;
using Backend.Memory.Readers.Parties.Digimons;
using Backend.Memory.Repositories;

namespace Backend.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBackendServices(this IServiceCollection services, string basePath)
        {
            services.AddSingleton<IProcessService, WindowsProcessProvider>();
            services.AddSingleton<IMemoryProvider, WindowsMemoryProvider>();
            services.AddSingleton<IDuckstationConnector, DuckstationConnector>();
            services.AddSingleton<IMemoryReader, MemoryReader>();

            string memoryDefinitionsDirectory = Path.Combine(basePath, "Memory", "Definitions");
            services.AddSingleton<IAddressesRepository>(new AddressesRepository(memoryDefinitionsDirectory));

            services.AddSingleton<IDigimonReader, DigimonReader>();
            services.AddSingleton<IDigimonSlotReader, DigimonSlotReader>();
            services.AddSingleton<IDigievolutionReader, DigievolutionReader>();
            services.AddSingleton<IStoredDigievolutionReader, StoredDigievolutionReader>();
            services.AddSingleton<IDigievolutionSlotReader, DigievolutionSlotReader>();
            services.AddSingleton<IPartyReader, PartyReader>();
            services.AddSingleton<IRequisiteReader, RequisiteReader>();
            services.AddSingleton<IStepReader, StepReader>();
            services.AddSingleton<IPlayerReader, PlayerReader>();
            services.AddSingleton<IQuestReader, QuestReader>();
            services.AddSingleton<IAuctionReader, AuctionReader>();

            services.AddSingleton<IPlayerLoader, PlayerLoader>();
            services.AddSingleton<IAuctionLoader, AuctionLoader>();
            services.AddSingleton<QuestLoader>();
            services.AddSingleton<IJournalLoader, JournalLoader>();
            services.AddSingleton<IPartyLoader, PartyLoader>();
            services.AddSingleton<DigimonLoader>();

            services.AddSingleton<IPlayerProvider, PlayerProvider>();
            services.AddSingleton<IPartyProvider, PartyProvider>();
            services.AddSingleton<IJournalProvider, JournalProvider>();

            services.AddSingleton<StateComposer>();
            services.AddSingleton<DebugConsoleRenderer>();
            services.AddSingleton<IDuckstationConnectionCoordinator, DuckstationConnectionHandler>();

            services.AddSingleton<IGameStateStore, GameStateStore>();
            services.AddSingleton<IEventDispatcherService, EventDispatcherService>();

            services.AddHostedService<GameLoopService>();

            return services;
        }
    }
}
