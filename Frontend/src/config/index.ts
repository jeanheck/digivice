/**
 * Centralized Application Configuration
 */

export const APP_CONFIG = {
    BACKEND: {
        DEFAULT_PORT: 5000,
        HUB_PATH: '/gamehub',
        // Helper to get the absolute fallback URL
        get FALLBACK_URL() {
            return `http://localhost:${this.DEFAULT_PORT}${this.HUB_PATH}`
        }
    },
    // Simple check to see if we are running inside Tauri
    IS_TAURI: !!(window as any).__TAURI_INTERNALS__,
    IS_DEV: import.meta.env.DEV,
    IS_PROD: import.meta.env.PROD
}
