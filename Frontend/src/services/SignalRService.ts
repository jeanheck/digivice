import * as signalR from '@microsoft/signalr'
import { invoke } from '@tauri-apps/api/core'
import type { GameEventMap } from '../types/events'

class SignalRService {
    private connection: signalR.HubConnection | null = null
    // Armazena handlers de forma tipada
    private handlers: Map<keyof GameEventMap, ((data: any) => void)[]> = new Map()

    /**
     * Subscribe to a SignalR event with strong typing.
     * @param eventName Name of the event from the backend (must exist in GameEventMap).
     * @param handler Callback function with typed data.
     */
    public on<K extends keyof GameEventMap>(
        eventName: K, 
        handler: (data: GameEventMap[K]) => void
    ) {
        if (!this.handlers.has(eventName)) {
            this.handlers.set(eventName, [])
        }
        this.handlers.get(eventName)?.push(handler)
    }

    /**
     * Manually set the list of events to register with SignalR.
     * Useful for automatic discovery of store actions.
     */
    public setEventNames(names: (keyof GameEventMap)[]) {
        names.forEach(name => {
            if (!this.handlers.has(name)) {
                this.handlers.set(name, []);
            }
        });
    }

    public async startConnection() {
        const isProd = import.meta.env.PROD
        let hubUrl = '/gamehub'
        
        if (isProd) {
            try {
                const port = await invoke<number>('get_backend_port')
                hubUrl = `http://localhost:${port}/gamehub`
            } catch (err) {
                console.error('Failed to get backend port from Tauri:', err)
                hubUrl = 'http://localhost:5000/gamehub'
            }
        }

        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(hubUrl)
            .withAutomaticReconnect()
            .build()

        this.registerInternalStatusEvents()
        this.registerBackendEvents()

        try {
            await this.connection.start()
            console.log('SignalR Connected to GameHub.')
            this.emit('ConnectionStatusChanged', { isConnected: true })
        } catch (err) {
            console.error('SignalR Connection Error: ', err)
            this.emit('ConnectionStatusChanged', { isConnected: false })
        }
    }

    private registerInternalStatusEvents() {
        if (!this.connection) return

        this.connection.onreconnecting(() => {
            console.warn('SignalR reconnecting...')
            this.emit('ConnectionStatusChanged', { isConnected: false })
        })

        this.connection.onreconnected(() => {
            console.log('SignalR reconnected.')
            this.emit('ConnectionStatusChanged', { isConnected: true })
        })

        this.connection.onclose(() => {
            console.error('SignalR connection closed.')
            this.emit('ConnectionStatusChanged', { isConnected: false })
        })
    }

    private registerBackendEvents() {
        if (!this.connection) return

        // Registra apenas os eventos que têm handlers definidos na GameEventMap
        for (const eventName of this.handlers.keys()) {
            this.connection.on(eventName, (data: any) => {
                this.emit(eventName, data)
            })
        }
    }

    private emit<K extends keyof GameEventMap>(eventName: K, data: GameEventMap[K]) {
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
