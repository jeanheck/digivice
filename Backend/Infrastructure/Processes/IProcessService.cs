namespace Backend.Infrastructure.Processes
{
    public interface IProcessService
    {
        int? GetProcessIdByName(string processName);
    }
}
