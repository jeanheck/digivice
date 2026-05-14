/**
 * Simple Logger utility to standardize console output and handle environments.
 */

type LogLevel = 'debug' | 'info' | 'warn' | 'error'

class Logger {
    private context: string
    private color: string

    constructor(context: string, color: string = '#888') {
        this.context = context
        this.color = color
    }

    private shouldLog(level: LogLevel): boolean {
        // Vite built-in: DEV is true during 'npm run dev'
        // If DEV is true, we want all logs.
        if (import.meta.env.DEV) {
            return true
        }

        // If not in DEV (Production), only log warnings and errors
        return level === 'warn' || level === 'error'
    }

    private formatMessage(level: LogLevel, message: string): [string, string, string] {
        const timestamp = new Date().toLocaleTimeString()
        return [
            `%c[${timestamp}] [${this.context}] [${level.toUpperCase()}] %c${message}`,
            `color: ${this.color}; font-weight: bold;`,
            'color: inherit; font-weight: normal;'
        ]
    }

    public debug(message: string, ...args: any[]) {
        if (this.shouldLog('debug')) {
            // Use console.log instead of console.debug to avoid browser default filters
            console.log(...this.formatMessage('debug', message), ...args)
        }
    }

    public info(message: string, ...args: any[]) {
        if (this.shouldLog('info')) {
            console.log(...this.formatMessage('info', message), ...args)
        }
    }

    public warn(message: string, ...args: any[]) {
        if (this.shouldLog('warn')) {
            console.warn(...this.formatMessage('warn', message), ...args)
        }
    }

    public error(message: string, ...args: any[]) {
        if (this.shouldLog('error')) {
            console.error(...this.formatMessage('error', message), ...args)
        }
    }
}

// Factory to create loggers with specific colors
export const createLogger = (context: string, color?: string) => new Logger(context, color)

// Default loggers
export const signalRLogger = createLogger('SignalR', '#3498db') // Blue

