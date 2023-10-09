import { createApp, pinia } from './vue';

import MethodologyApp from './vue/apps/methodology';

const methodologyAppMountingContainer = document.getElementById('main');

if (methodologyAppMountingContainer) {
    createApp(MethodologyApp).use(pinia).mount(methodologyAppMountingContainer);
}
