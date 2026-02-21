using Backend.Interfaces;
using System.Diagnostics;

namespace Backend.Services
{
    public class WindowsProcessService : IProcessService
    {
        public int? GetProcessIdByName(string processName)
        {
            var process = Process.GetProcessesByName(processName).FirstOrDefault();
            return process?.Id;
        }
    }
}
