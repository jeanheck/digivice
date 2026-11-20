import { fileURLToPath, URL } from "node:url";

import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
import tailwindcss from "@tailwindcss/vite";

export default defineConfig({
  plugins: [vue(), tailwindcss()],
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    },
  },
  // Keeps Rust compile errors visible in the `tauri:dev` terminal.
  clearScreen: false,
  server: {
    // Must match `devUrl` in Tauri/tauri.conf.json.
    port: 5173,
    strictPort: true,
  },
  build: {
    // Static game data (JSON) ships in the main chunk; size is irrelevant for a desktop app.
    chunkSizeWarningLimit: 2000,
  },
});
