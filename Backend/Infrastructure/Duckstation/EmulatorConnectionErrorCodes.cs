namespace Backend.Infrastructure.Duckstation;

public static class EmulatorConnectionErrorCodes
{
    public const string ConfigMissing = "config_missing";
    public const string ProcessNotFound = "process_not_found";
    public const string MappingNotFound = "mapping_not_found";
    public const string ConnectionFailed = "connection_failed";
    public const string MemoryReadFailed = "memory_read_failed";
    public const string StateComposeFailed = "state_compose_failed";
}
