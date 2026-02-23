import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'
import { signalRService } from './services/SignalRService'
import './style.css'

const app = createApp(App)

app.use(createPinia())
app.use(router)

// Inicia a comunicação em tempo real com o Backend
signalRService.startConnection()

app.mount('#app')
