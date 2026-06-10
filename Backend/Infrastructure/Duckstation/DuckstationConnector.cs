using Backend.Infrastructure.Memory;
using Backend.Infrastructure.Processes;
using Serilog;

namespace Backend.Infrastructure.Duckstation;

public sealed class DuckstationConnector(
    IProcessService processService,
    IMemoryProvider memoryProvider,
    IConfiguration configuration) : IDuckstationConnector
{
    public bool IsConnected { get; private set; }
    public IMemoryAccessor? Accessor { get; private set; }
    private int? ConnectedProcessId { get; set; }

    public bool IsConnectionAlive()
    {
        if (!IsConnected || Accessor == null || ConnectedProcessId is null)
        {
            return false;
        }

        var emulatorName = configuration.GetValue<string>("EmulatorProcessName");
        if (string.IsNullOrEmpty(emulatorName))
        {
            return false;
        }

        var currentProcessId = processService.GetProcessIdByName(emulatorName);
        return currentProcessId == ConnectedProcessId;
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

            Accessor = memoryProvider.OpenExisting(duckstationMapName);

            if (Accessor == null)
            {
                IsConnected = false;
                return false;
            }

            ConnectedProcessId = processId;
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
        Accessor?.Dispose();
        Accessor = null;
        ConnectedProcessId = null;
        IsConnected = false;
    }

    public void Dispose()
    {
        Disconnect();
        Log.Information("Memory resources released.");
        GC.SuppressFinalize(this);
    }
}
