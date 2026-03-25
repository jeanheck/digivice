import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'
import { signalRService } from './services/SignalRService'
import './style.css'

import i18n from './i18n'

const app = createApp(App)

app.use(createPinia())
app.use(router)
app.use(i18n)

// Inicia a comunicação em tempo real com o Backend
signalRService.startConnection()

app.mount('#app')
