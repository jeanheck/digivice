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
    private bool HasLoggedSuccessfulConnection { get; set; }

    private bool HasActiveConnection =>
        ConnectedProcessId is not null && duckstationSession.Accessor is not null;

    private bool IsConnectionAlive()
    {
        if (!HasActiveConnection)
        {
            return false;
        }

        if (string.IsNullOrEmpty(EmulatorProcessName))
        {
            return false;
        }

        var currentProcessId = processService.GetProcessIdByName(EmulatorProcessName);
        return currentProcessId == ConnectedProcessId;
    }

    private bool TryConnect()
    {
        Disconnect();

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
                return false;
            }

            string duckstationMapName = $"duckstation_{processId}";

            IMemoryAccessor? accessor = memoryProvider.OpenExisting(duckstationMapName);

            if (accessor == null)
            {
                return false;
            }

            duckstationSession.SetAccessor(accessor);
            ConnectedProcessId = processId;
            LogSuccessfulConnectionIfFirstTime(duckstationMapName);
            return true;
        }
        catch (Exception ex)
        {
            Log.Error("Failed to connect to DuckStation: {Msg}", ex.Message);
            return false;
        }
    }

    public bool EnsureConnection()
    {
        if (HasActiveConnection && !IsConnectionAlive())
        {
            Disconnect();
            return false;
        }

        if (!HasActiveConnection)
        {
            if (!TryConnect())
            {
                return false;
            }
        }

        return true;
    }

    public void Disconnect()
    {
        duckstationSession.ClearAccessor();
        ConnectedProcessId = null;
    }

    private void LogSuccessfulConnectionIfFirstTime(string duckstationMapName)
    {
        if (HasLoggedSuccessfulConnection)
        {
            return;
        }

        HasLoggedSuccessfulConnection = true;
        Log.Information("Connected to DuckStation! Mapping found: {MapName}", duckstationMapName);
    }
}
