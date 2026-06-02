export function parseConnectionStatus(data: unknown): { isConnected: boolean } {
    if (data !== null && typeof data === "object") {
        const record = data as Record<string, unknown>;

        if (typeof record.isConnected === "boolean") {
            return { isConnected: record.isConnected };
        }

        if (typeof record.IsConnected === "boolean") {
            return { isConnected: record.IsConnected };
        }
    }

    return { isConnected: false };
}
