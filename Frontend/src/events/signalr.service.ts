import * as signalR from '@microsoft/signalr'
import { invoke } from '@tauri-apps/api/core'
import type { EventsMap } from './events.map'
import { signalRLogger } from './logger'
import { APP_CONFIG } from '../config'
import { formatHubConnectionError } from './hub-connection-error'

class SignalRService {
    private connection: signalR.HubConnection | null = null
    // Stores handlers in a typed manner
    private handlers: Map<keyof EventsMap, ((data: any) => void)[]> = new Map()

    /**
     * Subscribe to a SignalR event with strong typing.
     */
    public on<K extends keyof EventsMap>(
        eventName: K,
        handler: (data: EventsMap[K]) => void
    ) {
        if (!this.handlers.has(eventName)) {
            this.handlers.set(eventName, [])
        }
        this.handlers.get(eventName)?.push(handler)
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
            this.emit('HubConnectionStatusChanged', { isConnected: true })
        } catch (err) {
            signalRLogger.error(`Connection Error at ${hubUrl}`, err)
            this.emit('HubConnectionStatusChanged', {
                isConnected: false,
                errorMessage: formatHubConnectionError(err),
            })
        }
    }

    private registerInternalStatusEvents() {
        if (!this.connection) return

        this.connection.onreconnecting(() => {
            signalRLogger.warn('Reconnecting...')
            this.emit('HubConnectionStatusChanged', {
                isConnected: false,
                errorMessage: 'Reconnecting to backend...',
            })
        })

        this.connection.onreconnected(() => {
            signalRLogger.info('Reconnected.')
            this.emit('HubConnectionStatusChanged', { isConnected: true })
        })

        this.connection.onclose((err) => {
            signalRLogger.error('Connection closed.', err)
            this.emit('HubConnectionStatusChanged', {
                isConnected: false,
                errorMessage: err ? formatHubConnectionError(err) : 'Connection closed.',
            })
        })
    }

    private registerBackendEvents() {
        if (!this.connection) return

        // Filter out internal client-only events from remote hub registrations
        const backendEventNames = Array.from(this.handlers.keys())
            .filter(name => name !== 'HubConnectionStatusChanged')

        for (const eventName of backendEventNames) {
            this.connection.on(eventName, (eventWrapper: any) => {
                signalRLogger.debug(`Hub Event [${eventName}]`, eventWrapper);

                // If we receive the wrapped structure from the backend, extract only the 'payload' property
                const payload = eventWrapper && typeof eventWrapper === 'object' && 'payload' in eventWrapper
                    ? eventWrapper.payload
                    : eventWrapper

                this.emit(eventName, payload)
            })
        }
    }

    private emit<K extends keyof EventsMap>(eventName: K, data: EventsMap[K]) {
        const eventHandlers = this.handlers.get(eventName)
        if (eventHandlers) {
            eventHandlers.forEach(handler => handler(data))
        }
    }
}

export const signalRService = new SignalRService()
