using Backend.Interfaces;
using System.Diagnostics;

namespace Backend.Infrastructure.Processes
{
    public class WindowsProcessProvider : IProcessService
    {
        public int? GetProcessIdByName(string processName)
        {
            var process = Process.GetProcessesByName(processName).FirstOrDefault();
            return process?.Id;
        }
    }
}
