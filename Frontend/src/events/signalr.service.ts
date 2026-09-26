import * as signalR from "@microsoft/signalr";
import { invoke } from "@tauri-apps/api/core";
import type { EventsMap } from "./events.map";
import type { EventDTO } from "./dto/event.dto";
import { signalRLogger } from "./logger";
import { AppConfig } from "@/config";
import { formatHubConnectionError } from "./hub-connection-error";

const ConnectionMaxAttempts = 20;
const ConnectionRetryDelayMs = 250;

type HandlersMap = { [K in keyof EventsMap]?: ((data: EventsMap[K]) => void)[] };

class SignalRService {
  private connection: signalR.HubConnection | null = null;
  private handlers: HandlersMap = {};

  /**
   * Subscribe to a SignalR event with strong typing.
   * Can be called before or after startConnection.
   */
  public on<K extends keyof EventsMap>(eventName: K, handler: (data: EventsMap[K]) => void) {
    const eventHandlers: ((data: EventsMap[K]) => void)[] = this.handlers[eventName] ?? [];
    const isFirstHandler = eventHandlers.length === 0;

    eventHandlers.push(handler);
    // TS cannot correlate the generic K on indexed writes to a mapped type.
    this.handlers[eventName] = eventHandlers as HandlersMap[K];

    if (isFirstHandler) {
      this.bindBackendEvent(eventName);
    }
  }

  private async getHubUrl(): Promise<string> {
    // In development (Vite), we use the relative proxy.
    if (AppConfig.isDev) {
      return AppConfig.backend.hubPath;
    }

    // In production (Tauri), we are trying to obtain the dynamic port from the backend.
    try {
      const port = await invoke<number>("get_backend_port");
      return `http://localhost:${port}${AppConfig.backend.hubPath}`;
    } catch (err) {
      signalRLogger.warn(
        `Failed to get backend port via Tauri. Using fallback: ${AppConfig.backend.defaultPort}`,
      );
      return AppConfig.backend.fallbackUrl;
    }
  }

  private createConnection(hubUrl: string): signalR.HubConnection {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl)
      .withAutomaticReconnect()
      .build();

    this.connection = connection;
    this.registerInternalStatusEvents();
    this.registerBackendEvents();
    return connection;
  }

  public async startConnection() {
    const hubUrl = await this.getHubUrl();
    const connection = this.createConnection(hubUrl);
    let lastError: unknown = null;

    for (let attempt = 1; attempt <= ConnectionMaxAttempts; attempt++) {
      try {
        await connection.start();
        signalRLogger.info(`Connected to GameHub at: ${hubUrl}`);
        this.emit("HubConnectionStatusChanged", { isConnected: true });
        return;
      } catch (err) {
        lastError = err;
        signalRLogger.warn(
          `Connection attempt ${attempt}/${ConnectionMaxAttempts} failed at ${hubUrl}`,
          err,
        );

        if (attempt < ConnectionMaxAttempts) {
          await new Promise((resolve) => setTimeout(resolve, ConnectionRetryDelayMs));
        }
      }
    }

    signalRLogger.error(`Connection Error at ${hubUrl}`, lastError);
    this.emit("HubConnectionStatusChanged", {
      isConnected: false,
      errorMessage: formatHubConnectionError(lastError),
    });
  }

  private registerInternalStatusEvents() {
    if (!this.connection) {
      return;
    }

    this.connection.onreconnecting(() => {
      signalRLogger.warn("Reconnecting...");
      this.emit("HubConnectionStatusChanged", {
        isConnected: false,
        errorMessage: "Reconnecting to backend...",
        preserveGameState: true,
      });
    });

    this.connection.onreconnected(() => {
      signalRLogger.info("Reconnected.");
      this.emit("HubConnectionStatusChanged", { isConnected: true });
    });

    this.connection.onclose((err) => {
      signalRLogger.error("Connection closed.", err);
      this.emit("HubConnectionStatusChanged", {
        isConnected: false,
        errorMessage: err ? formatHubConnectionError(err) : "Connection closed.",
      });
    });
  }

  private registerBackendEvents() {
    const eventNames = Object.keys(this.handlers) as (keyof EventsMap)[];
    for (const eventName of eventNames) {
      this.bindBackendEvent(eventName);
    }
  }

  private bindBackendEvent<K extends keyof EventsMap>(eventName: K): void {
    // HubConnectionStatusChanged is emitted locally, never by the hub.
    if (!this.connection || eventName === "HubConnectionStatusChanged") {
      return;
    }

    this.connection.on(eventName, (eventDto: EventDTO<EventsMap[K]>) => {
      signalRLogger.debug(`Hub Event [${eventName}]`, eventDto);
      this.emit(eventName, eventDto.payload);
    });
  }

  private emit<K extends keyof EventsMap>(eventName: K, data: EventsMap[K]) {
    const eventHandlers: ((data: EventsMap[K]) => void)[] | undefined = this.handlers[eventName];
    eventHandlers?.forEach((handler) => handler(data));
  }
}

export const signalRService = new SignalRService();
