namespace Tests.Unit.Application;

using Backend.Application;
using Backend.Infrastructure.Duckstation;
using Moq;
using Xunit;

public class DuckstationConnectorTests
{
    private readonly Mock<IDuckstationConnection> DuckstationConnectionMock;

    public DuckstationConnectorTests()
    {
        DuckstationConnectionMock = new Mock<IDuckstationConnection>();
    }

    private DuckstationConnector CreateDuckstationConnector()
    {
        return new DuckstationConnector(DuckstationConnectionMock.Object);
    }

    [Fact]
    public void EnsureConnection_ShouldReturnTrue_WhenConnectedAndAlive()
    {
        DuckstationConnectionMock.Setup(connection => connection.IsConnected).Returns(true);
        DuckstationConnectionMock.Setup(connection => connection.IsConnectionAlive()).Returns(true);

        var duckstationConnector = CreateDuckstationConnector();

        var isReadyToCompose = duckstationConnector.EnsureConnection();

        Assert.True(isReadyToCompose);
    }

    [Fact]
    public void EnsureConnection_ShouldReturnFalse_WhenConnectedButNotAlive()
    {
        DuckstationConnectionMock.Setup(connection => connection.IsConnected).Returns(true);
        DuckstationConnectionMock.Setup(connection => connection.IsConnectionAlive()).Returns(false);

        var duckstationConnector = CreateDuckstationConnector();

        var isReadyToCompose = duckstationConnector.EnsureConnection();

        Assert.False(isReadyToCompose);
        DuckstationConnectionMock.Verify(connection => connection.Disconnect(), Times.Once);
    }

    [Fact]
    public void EnsureConnection_ShouldReturnFalse_WhenTryConnectFails()
    {
        DuckstationConnectionMock.Setup(connection => connection.IsConnected).Returns(false);
        DuckstationConnectionMock.Setup(connection => connection.TryConnect()).Returns(false);

        var duckstationConnector = CreateDuckstationConnector();

        var isReadyToCompose = duckstationConnector.EnsureConnection();

        Assert.False(isReadyToCompose);
        DuckstationConnectionMock.Verify(connection => connection.Disconnect(), Times.Never);
    }

    [Fact]
    public void EnsureConnection_ShouldReturnTrue_WhenTryConnectSucceeds()
    {
        DuckstationConnectionMock.Setup(connection => connection.IsConnected).Returns(false);
        DuckstationConnectionMock.SetupSequence(connection => connection.TryConnect())
            .Returns(true)
            .Returns(true);
        DuckstationConnectionMock.Setup(connection => connection.IsConnectionAlive()).Returns(true);

        var duckstationConnector = CreateDuckstationConnector();

        var isReadyToCompose = duckstationConnector.EnsureConnection();

        Assert.True(isReadyToCompose);
    }

    [Fact]
    public void Disconnect_ShouldDisconnectConnection()
    {
        var duckstationConnector = CreateDuckstationConnector();

        duckstationConnector.Disconnect();

        DuckstationConnectionMock.Verify(connection => connection.Disconnect(), Times.Once);
    }
}
