using Backend.Application;
using Backend.Application.Loaders;
using Backend.Application.Loaders.Interfaces;
using Backend.Application.Providers;
using Backend.Application.Providers.Interfaces;
using Backend.Diagnostics;
using Backend.Events.Services;
using Backend.Events.States;
using Backend.Infrastructure.Duckstation;
using Backend.Infrastructure.Memory;
using Backend.Infrastructure.Processes;
using Backend.Memory.Readers;
using Backend.Memory.Repositories;
using Backend.Memory.Readers.Interfaces;

namespace Backend.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBackendServices(this IServiceCollection services, string basePath)
        {
            services.AddSingleton<IProcessService, WindowsProcessProvider>();
            services.AddSingleton<IMemoryProvider, WindowsMemoryProvider>();
            services.AddSingleton<DuckstationSession>();
            services.AddSingleton<IDuckstationSession>(provider => provider.GetRequiredService<DuckstationSession>());
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
            services.AddSingleton<IInBattleReader, InBattleReader>();
            services.AddSingleton<IEnemyReader, EnemyReader>();
            services.AddSingleton<IDigimonBattleReader, DigimonBattleReader>();
            services.AddSingleton<IRequisiteReader, RequisiteReader>();
            services.AddSingleton<IStepReader, StepReader>();
            services.AddSingleton<IPlayerReader, PlayerReader>();
            services.AddSingleton<ICardBattleReader, CardBattleReader>();
            services.AddSingleton<IImportantItemsReader, ImportantItemsReader>();
            services.AddSingleton<IAuctionsReader, AuctionsReader>();
            services.AddSingleton<IQuestReader, QuestReader>();
            services.AddSingleton<INpcsReader, NpcsReader>();

            services.AddSingleton<IPlayerLoader, PlayerLoader>();
            services.AddSingleton<IImportantItemsLoader, ImportantItemsLoader>();
            services.AddSingleton<IAuctionsLoader, AuctionsLoader>();
            services.AddSingleton<INpcsLoader, NpcsLoader>();
            services.AddSingleton<IQuestLoader, QuestLoader>();
            services.AddSingleton<IJournalLoader, JournalLoader>();
            services.AddSingleton<IPartyLoader, PartyLoader>();
            services.AddSingleton<IDigimonLoader, DigimonLoader>();
            services.AddSingleton<IDigimonBattleLoader, DigimonBattleLoader>();
            services.AddSingleton<ICardBattleLoader, CardBattleLoader>();

            services.AddSingleton<IPlayerProvider, PlayerProvider>();
            services.AddSingleton<IImportantItemsProvider, ImportantItemsProvider>();
            services.AddSingleton<IPartyProvider, PartyProvider>();
            services.AddSingleton<IDigimonBattleProvider, DigimonBattleProvider>();
            services.AddSingleton<ICardBattleProvider, CardBattleProvider>();
            services.AddSingleton<IAuctionsProvider, AuctionsProvider>();
            services.AddSingleton<INpcsProvider, NpcsProvider>();
            services.AddSingleton<IJournalProvider, JournalProvider>();

            services.AddSingleton<StateComposer>();
            services.AddSingleton<DebugConsoleRenderer>();

            services.AddSingleton<IGameStateStore, GameStateStore>();
            services.AddSingleton<IEventDispatcherService, EventDispatcherService>();

            services.AddHostedService<GameLoopService>();

            return services;
        }
    }
}
