namespace Backend.Interfaces
{
    public interface IProcessService
    {
        int? GetProcessIdByName(string processName);
    }
}
