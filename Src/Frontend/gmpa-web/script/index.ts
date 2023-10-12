import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'
import HomePage from './vue/apps/home/home.app.vue'

const pinia = createPinia()
const app = createApp(App)

app.component("home-page", HomePage)

app.use(pinia)
app.mount('#app')