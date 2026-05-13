import * as signalR from '@microsoft/signalr'
import { invoke } from '@tauri-apps/api/core'

class SignalRService {
    private connection: signalR.HubConnection | null = null
    private handlers: Map<string, ((data: any) => void)[]> = new Map()

    /**
     * Subscribe to a SignalR event.
     * @param eventName Name of the event from the backend.
     * @param handler Callback function to handle the event data.
     */
    public on(eventName: string, handler: (data: any) => void) {
        if (!this.handlers.has(eventName)) {
            this.handlers.set(eventName, [])
        }
        this.handlers.get(eventName)?.push(handler)
    }

    /**
     * Manually set the list of events to register with SignalR.
     * Useful for automatic discovery of store actions.
     */
    public setEventNames(names: string[]) {
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

        // Register all events that have at least one handler subscribed
        for (const eventName of this.handlers.keys()) {
            this.connection.on(eventName, (data: any) => {
                this.emit(eventName, data)
            })
        }
    }

    private emit(eventName: string, data: any) {
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
