import { createApp } from "vue";
import { createPinia } from "pinia";

import App from "./App.vue";
import "@/core/extensions/math.extensions";
import "@fontsource/press-start-2p/latin-400.css";
import "@fontsource/exo-2/latin-400.css";
import "@fontsource/exo-2/latin-700.css";
import "@fontsource/chakra-petch/latin-400.css";
import "@fontsource/chakra-petch/latin-700.css";
import "@fontsource/noto-emoji/400.css";
import "@fontsource/noto-color-emoji/400.css";
import "./style.css";

import i18n from "@/ui/i18n";
import { initializeMemoryLoop } from "@/memory/loop/memory-loop.initializer";

const app = createApp(App);

app.use(createPinia());
app.use(i18n);

initializeMemoryLoop();

app.mount("#app");
