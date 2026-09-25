import { createApp } from "vue";
import { createPinia } from "pinia";

import App from "./App.vue";
import { signalRService } from "./events/signalr.service";
import "@/extensions/math.extensions";
import "./style.css";

import i18n from "./i18n";
import { initializeSignalRHandlers } from "./events/signalr.handlers";
import { initializeTauriBackendProcessListener } from "./events/tauri-backend-process.listener";

const app = createApp(App);

app.use(createPinia());
app.use(i18n);

initializeTauriBackendProcessListener();

initializeSignalRHandlers();

signalRService.startConnection();

app.mount("#app");
