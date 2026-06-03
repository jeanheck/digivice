import { listen } from "@tauri-apps/api/event";
import { useGameStore } from "@/stores/use-game-store";

export async function initializeTauriBackendProcessListener(): Promise<void> {
    try {
        await listen<number | null>("backend-crashed", () => {
            const store = useGameStore();
            store.setBackendProcessFailed(true);
        });
    } catch {
        // Not running inside Tauri (e.g. Vite-only dev in browser).
    }
}
