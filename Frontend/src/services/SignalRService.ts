import * as signalR from '@microsoft/signalr'
import { invoke } from '@tauri-apps/api/core'
import type { GameEventDTOMap } from '../dtos/events.dto'
import { signalRLogger } from '../utils/Logger'
import { APP_CONFIG } from '../config'

class SignalRService {
    private connection: signalR.HubConnection | null = null
    // Armazena handlers de forma tipada
    private handlers: Map<keyof GameEventDTOMap, ((data: any) => void)[]> = new Map()

    /**
     * Subscribe to a SignalR event with strong typing.
     */
    public on<K extends keyof GameEventDTOMap>(
        eventName: K,
        handler: (data: GameEventDTOMap[K]) => void
    ) {
        if (!this.handlers.has(eventName)) {
            this.handlers.set(eventName, [])
        }
        this.handlers.get(eventName)?.push(handler)
    }

    /**
     * Manually set the list of events to register with SignalR.
     */
    public setEventNames(names: (keyof GameEventDTOMap)[]) {
        names.forEach(name => {
            if (!this.handlers.has(name)) {
                this.handlers.set(name, []);
            }
        });
    }

    private async getHubUrl(): Promise<string> {
        // In development (Vite), we use the relative proxy.
        if (APP_CONFIG.IS_DEV) {
            return APP_CONFIG.BACKEND.HUB_PATH
        }

        // In production (Tauri), we are trying to obtain the dynamic port from the backend.
        try {
            const port = await invoke<number>('get_backend_port')
            return `http://localhost:${port}${APP_CONFIG.BACKEND.HUB_PATH}`
        } catch (err) {
            signalRLogger.warn(`Failed to get backend port via Tauri. Using fallback: ${APP_CONFIG.BACKEND.DEFAULT_PORT}`)
            return APP_CONFIG.BACKEND.FALLBACK_URL
        }
    }

    public async startConnection() {
        const hubUrl = await this.getHubUrl()

        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(hubUrl)
            .withAutomaticReconnect()
            .build()

        this.registerInternalStatusEvents()
        this.registerBackendEvents()

        try {
            await this.connection.start()
            signalRLogger.info(`Connected to GameHub at: ${hubUrl}`)
            this.emit('ConnectionStatusChanged', { isConnected: true })
        } catch (err) {
            signalRLogger.error(`Connection Error at ${hubUrl}`, err)
            this.emit('ConnectionStatusChanged', { isConnected: false })
        }
    }

    private registerInternalStatusEvents() {
        if (!this.connection) return

        this.connection.onreconnecting(() => {
            signalRLogger.warn('Reconnecting...')
            this.emit('ConnectionStatusChanged', { isConnected: false })
        })

        this.connection.onreconnected(() => {
            signalRLogger.info('Reconnected.')
            this.emit('ConnectionStatusChanged', { isConnected: true })
        })

        this.connection.onclose(() => {
            signalRLogger.error('Connection closed.')
            this.emit('ConnectionStatusChanged', { isConnected: false })
        })
    }

    private registerBackendEvents() {
        if (!this.connection) return

        // Only logs events that have handlers defined in the GameEventDTOMap.
        for (const eventName of this.handlers.keys()) {
            this.connection.on(eventName, (data: any) => {
                signalRLogger.debug(`Hub Event [${eventName}]`, data)
                this.emit(eventName, data)
            })
        }
    }

    private emit<K extends keyof GameEventDTOMap>(eventName: K, data: GameEventDTOMap[K]) {
        const eventHandlers = this.handlers.get(eventName)
        if (eventHandlers) {
            eventHandlers.forEach(handler => handler(data))
        }
    }

    public get isConnected() {
        return this.connection?.state === signalR.HubConnectionState.Connected
    }
}

export const signalRService = new SignalRService()
