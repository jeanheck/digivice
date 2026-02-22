import * as signalR from '@microsoft/signalr';

export const connection = new signalR.HubConnectionBuilder()
    .withUrl('http://localhost:5000/gamehub') // Note: SignalR uses http/https for the initial negotiation, then upgrades to ws/wss
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Information)
    .build();

export const startConnection = async () => {
    try {
        if (connection.state === signalR.HubConnectionState.Disconnected) {
            await connection.start();
            console.log('SignalR Connected.');
        }
    } catch (err) {
        console.error('SignalR Connection Error: ', err);
        setTimeout(startConnection, 5000);
    }
};

// Lifecycle hooks
connection.onreconnecting(error => {
    console.warn('SignalR reconnecting...', error);
});

connection.onreconnected(connectionId => {
    console.log('SignalR reconnected. Connection ID:', connectionId);
});

connection.onclose(error => {
    console.error('SignalR connection closed. Error:', error);
});
