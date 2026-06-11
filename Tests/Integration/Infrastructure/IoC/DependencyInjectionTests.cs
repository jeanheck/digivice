namespace Tests.Integration.Infrastructure.IoC;

using System;
using System.Linq;
using Backend.Application;
using Backend.Application.Loaders;
using Backend.Application.Loaders.Journals;
using Backend.Application.Loaders.Parties;
using Backend.Application.Providers;
using Backend.Diagnostics;
using Backend.Events.Services;
using Backend.Events.States;
using Backend.Infrastructure;
using Backend.Infrastructure.Duckstation;
using Backend.Infrastructure.Memory;
using Backend.Infrastructure.Processes;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Journals;
using Backend.Memory.Readers.Journals.Quests;
using Backend.Memory.Readers.Parties;
using Backend.Memory.Readers.Parties.Digimons;
using Backend.Memory.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

public class DependencyInjectionTests
{
    [Fact]
    public void AddBackendServices_ShouldResolveAllRegisteredServicesWithoutExceptions()
    {
        ServiceCollection services = new ServiceCollection();

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new System.Collections.Generic.KeyValuePair<string, string?>("EmulatorProcessName", "duckstation")
            })
            .Build();

        services.AddSingleton(configuration);
        services.AddLogging();
        services.AddSignalR();

        services.AddBackendServices(AppContext.BaseDirectory);

        ServiceProvider provider = services.BuildServiceProvider();

        Assert.NotNull(provider.GetRequiredService<IProcessService>());
        Assert.NotNull(provider.GetRequiredService<IMemoryProvider>());
        Assert.NotNull(provider.GetRequiredService<IDuckstationSession>());
        Assert.NotNull(provider.GetRequiredService<IMemoryReader>());
        Assert.NotNull(provider.GetRequiredService<IDuckstationConnector>());
        Assert.NotNull(provider.GetRequiredService<IAddressesRepository>());


        Assert.NotNull(provider.GetRequiredService<IDigimonReader>());
        Assert.NotNull(provider.GetRequiredService<IDigimonSlotReader>());
        Assert.NotNull(provider.GetRequiredService<IDigievolutionReader>());
        Assert.NotNull(provider.GetRequiredService<IDigievolutionSlotReader>());
        Assert.NotNull(provider.GetRequiredService<IPartyReader>());
        Assert.NotNull(provider.GetRequiredService<IRequisiteReader>());
        Assert.NotNull(provider.GetRequiredService<IStepReader>());
        Assert.NotNull(provider.GetRequiredService<IPlayerReader>());
        Assert.NotNull(provider.GetRequiredService<IQuestReader>());

        Assert.NotNull(provider.GetRequiredService<IPlayerLoader>());
        Assert.NotNull(provider.GetRequiredService<QuestLoader>());
        Assert.NotNull(provider.GetRequiredService<IJournalLoader>());
        Assert.NotNull(provider.GetRequiredService<IPartyLoader>());
        Assert.NotNull(provider.GetRequiredService<DigimonLoader>());

        Assert.NotNull(provider.GetRequiredService<IPlayerProvider>());
        Assert.NotNull(provider.GetRequiredService<IPartyProvider>());
        Assert.NotNull(provider.GetRequiredService<IJournalProvider>());

        Assert.NotNull(provider.GetRequiredService<StateComposer>());
        Assert.NotNull(provider.GetRequiredService<DebugConsoleRenderer>());

        Assert.NotNull(provider.GetRequiredService<IGameStateStore>());
        Assert.NotNull(provider.GetRequiredService<IEventDispatcherService>());

        var hostedServices = provider.GetServices<IHostedService>().ToList();
        Assert.Single(hostedServices);
        Assert.IsType<GameLoopService>(hostedServices[0]);
    }
}
