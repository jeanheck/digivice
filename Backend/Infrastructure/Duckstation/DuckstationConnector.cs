using Backend.Infrastructure.Memory;
using Backend.Infrastructure.Processes;
using Serilog;

namespace Backend.Infrastructure.Duckstation;

public sealed class DuckstationConnector(
    DuckstationSession duckstationSession,
    IProcessService processService,
    IMemoryProvider memoryProvider,
    IConfiguration configuration) : IDuckstationConnector
{
    private readonly string? EmulatorProcessName = configuration.GetValue<string>("EmulatorProcessName");
    private int? ConnectedProcessId { get; set; }

    private bool HasActiveConnection =>
        ConnectedProcessId is not null && duckstationSession.Accessor is not null;
    private bool ProcessIdChanged => processService.GetProcessIdByName(EmulatorProcessName!) != ConnectedProcessId;

    public bool EnsureConnection()
    {
        if (HasActiveConnection && !ProcessIdChanged)
        {
            return true;
        }

        ClearSession();

        try
        {
            if (string.IsNullOrEmpty(EmulatorProcessName))
            {
                Log.Error("EmulatorProcessName not found in appsettings.json");
                return false;
            }

            int? processId = processService.GetProcessIdByName(EmulatorProcessName);

            if (processId == null)
            {
                Log.Error("Duckstation Process ID not found!");
                return false;
            }

            string duckstationMapName = $"duckstation_{processId}";
            IMemoryAccessor? memoryAcessor = memoryProvider.OpenExisting(duckstationMapName);

            if (memoryAcessor == null)
            {
                Log.Error("Duckstation Memory Acessor not found!");
                return false;
            }

            Log.Information("Connected to DuckStation! Mapping found: {MapName}", duckstationMapName);
            duckstationSession.SetAccessor(memoryAcessor);
            ConnectedProcessId = processId;
            return true;
        }
        catch (Exception ex)
        {
            Log.Error("Failed to connect to DuckStation: {Msg}", ex.Message);
            return false;
        }
    }

    public void ClearSession()
    {
        duckstationSession.ClearAccessor();
        ConnectedProcessId = null;
    }
}
