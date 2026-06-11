using Backend.Infrastructure.Memory;
using Backend.Infrastructure.Processes;
using Serilog;

namespace Backend.Infrastructure.Duckstation;

public sealed class DuckstationConnection(
    IProcessService processService,
    IMemoryProvider memoryProvider,
    IConfiguration configuration) : IDuckstationConnection
{
    private readonly string? EmulatorProcessName = configuration.GetValue<string>("EmulatorProcessName");

    public bool IsConnected { get; private set; }
    public IMemoryAccessor? Accessor { get; private set; }
    private int? ConnectedProcessId { get; set; }

    public bool IsConnectionAlive()
    {
        if (!IsConnected || Accessor == null || ConnectedProcessId is null)
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

    public bool TryConnect()
    {
        Disconnect();

        try
        {
            if (string.IsNullOrEmpty(EmulatorProcessName))
            {
                Log.Error("EmulatorProcessName not found in appsettings.json");
                IsConnected = false;
                return false;
            }

            int? processId = processService.GetProcessIdByName(EmulatorProcessName);

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
        catch (Exception ex)
        {
            Log.Error("Failed to connect to DuckStation: {Msg}", ex.Message);
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
