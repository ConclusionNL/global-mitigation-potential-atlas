import { createApp, pinia } from './vue';

import HomeApp from './vue/apps/home';

const homeAppMountingContainer = document.getElementById('home');

if (homeAppMountingContainer) {
    createApp(HomeApp).use(pinia).mount(homeAppMountingContainer);
}