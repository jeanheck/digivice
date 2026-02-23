import * as signalR from '@microsoft/signalr';

const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://127.0.0.1:5000/gamehub")
    .build();

connection.on("InitialStateSync", (data) => {
    console.log("RECEIVED INITIAL STATE:");
    console.log(JSON.stringify(data, null, 2));
    process.exit(0);
});

connection.start().then(() => {
    console.log("Connected. Waiting for InitialStateSync...");
}).catch(err => {
    console.error("Connection failed: ", err);
    process.exit(1);
});

// Force exist after 5 seconds if no event
setTimeout(() => {
    console.log("Timeout waiting for event.");
    process.exit(1);
}, 5000);
