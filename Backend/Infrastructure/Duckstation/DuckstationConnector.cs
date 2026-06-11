using Backend.Application;
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

    private bool IsConnected { get; set; }
    private int? ConnectedProcessId { get; set; }

    public bool EnsureConnection()
    {
        if (IsConnected && !IsConnectionAlive())
        {
            Disconnect();
            return false;
        }

        if (!IsConnected)
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
        duckstationSession.Accessor?.Dispose();
        duckstationSession.Accessor = null;
        ConnectedProcessId = null;
        IsConnected = false;
    }

    private bool IsConnectionAlive()
    {
        if (!IsConnected || duckstationSession.Accessor == null || ConnectedProcessId is null)
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

            IMemoryAccessor? accessor = memoryProvider.OpenExisting(duckstationMapName);

            if (accessor == null)
            {
                IsConnected = false;
                return false;
            }

            duckstationSession.Accessor = accessor;
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
}
