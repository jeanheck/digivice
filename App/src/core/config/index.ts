/**
 * Centralized Application Configuration
 */

export const AppConfig = {
  memory: {
    pollingIntervalMs: 500,
  },
  // Simple check to see if we are running inside Tauri
  isTauri: !!(window as any).__TAURI_INTERNALS__,
};
