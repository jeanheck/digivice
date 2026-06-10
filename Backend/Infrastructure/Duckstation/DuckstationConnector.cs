using Backend.Infrastructure.Memory;
using Backend.Infrastructure.Processes;
using Serilog;

namespace Backend.Infrastructure.Duckstation;

public sealed class DuckstationConnector(
    IProcessService processService,
    IMemoryProvider memoryProvider,
    IConfiguration configuration) : IDuckstationConnector
{
    private IMemoryAccessor? accessor;
    private int? connectedProcessId;

    public bool IsConnected { get; private set; }
    public IMemoryAccessor? Accessor => accessor;

    public bool IsConnectionAlive()
    {
        if (!IsConnected || accessor == null || connectedProcessId is null)
        {
            return false;
        }

        var emulatorName = configuration.GetValue<string>("EmulatorProcessName");
        if (string.IsNullOrEmpty(emulatorName))
        {
            return false;
        }

        var currentProcessId = processService.GetProcessIdByName(emulatorName);
        return currentProcessId == connectedProcessId;
    }

    public bool TryConnect()
    {
        Disconnect();

        try
        {
            var emulatorName = configuration.GetValue<string>("EmulatorProcessName");
            if (string.IsNullOrEmpty(emulatorName))
            {
                Log.Error("EmulatorProcessName not found in appsettings.json");
                IsConnected = false;
                return false;
            }

            int? processId = processService.GetProcessIdByName(emulatorName);

            if (processId == null)
            {
                IsConnected = false;
                return false;
            }

            string duckstationMapName = $"duckstation_{processId}";

            accessor = memoryProvider.OpenExisting(duckstationMapName);

            if (accessor == null)
            {
                IsConnected = false;
                return false;
            }

            connectedProcessId = processId;
            IsConnected = true;
            Log.Information("Connected to DuckStation! Mapping found: {MapName}", duckstationMapName);
            return true;
        }
        catch (Exception)
        {
            IsConnected = false;
            return false;
        }
    }

    public void Disconnect()
    {
        accessor?.Dispose();
        accessor = null;
        connectedProcessId = null;
        IsConnected = false;
    }

    public void InvalidateConnection()
    {
        IsConnected = false;
    }

    public void Dispose()
    {
        Disconnect();
        Log.Information("Memory resources released.");
        GC.SuppressFinalize(this);
    }
}
