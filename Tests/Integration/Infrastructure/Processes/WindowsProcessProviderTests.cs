namespace Tests.Integration.Infrastructure.Processes;

using System;
using System.Diagnostics;
using Backend.Infrastructure.Processes;
using Xunit;

public class WindowsProcessProviderTests
{
    [Fact]
    public void GetProcessIdByName_ShouldReturnActivePid_WhenProcessExists()
    {
        WindowsProcessProvider provider = new WindowsProcessProvider();
        string currentProcessName = Process.GetCurrentProcess().ProcessName;

        int? processId = provider.GetProcessIdByName(currentProcessName);

        Assert.NotNull(processId);
        Assert.True(processId > 0);
    }

    [Fact]
    public void GetProcessIdByName_ShouldReturnNull_WhenProcessDoesNotExist()
    {
        WindowsProcessProvider provider = new WindowsProcessProvider();
        string nonExistentProcessName = $"NonExistentProcess_{Guid.NewGuid()}";

        int? processId = provider.GetProcessIdByName(nonExistentProcessName);

        Assert.Null(processId);
    }
}
