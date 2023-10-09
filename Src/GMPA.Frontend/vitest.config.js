import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [vue()],
    test: {
        globals: true,
        environment: 'happy-dom',
        coverage: {
            enabled: true,
            provider: 'istanbul', // or 'v8'
            reporter: ['text', 'html', 'clover', 'json'], // default
            include: ['script/', 'arlanet-library/'],
        },
    },
});
