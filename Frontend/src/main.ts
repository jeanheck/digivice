import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import { signalRService } from './events/signalr.service'
import './style.css'

import i18n from './i18n'
import { initializeSignalRHandlers } from './events/signalr.handlers';


const app = createApp(App)

app.use(createPinia())
app.use(i18n)

// Initializes the handlers (bridge between SignalR and Pinia)
initializeSignalRHandlers();

// Initiates real-time communication with the backend.
signalRService.startConnection()

app.mount('#app')

