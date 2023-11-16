import { defineConfig, UserConfig } from 'vite';
import glob from 'glob';
import path from 'node:path';
import { fileURLToPath } from 'node:url';
import fs from 'fs';
import { setDefaultResultOrder } from 'dns';
import vue from '@vitejs/plugin-vue';
import VueI18nPlugin from '@intlify/unplugin-vue-i18n/vite';
import dsv from '@rollup/plugin-dsv';
import svgLoader from 'vite-svg-loader';
import { UMBRACO_PROJECT_NAME, PORT } from './config';
setDefaultResultOrder('verbatim');

const scriptGlob = Object.fromEntries(
    glob
        .sync('script/*{ts,js}')
        .map((file) => [
            path.relative('', file.slice(0, file.length - path.extname(file).length)),
            fileURLToPath(new URL(file, import.meta.url)),
        ])
);

const styleGlob = Object.fromEntries(
    glob
        .sync('style/*.scss')
        .map((file) => [
            path.relative('', file.slice(0, file.length - path.extname(file).length)),
            fileURLToPath(new URL(file, import.meta.url)),
        ])
);

const styleComponentsGlob = Object.fromEntries(
    glob
        .sync('style/components/*.scss')
        .map((file) => [
            path.relative('', file.slice(0, file.length - path.extname(file).length)),
            fileURLToPath(new URL(file, import.meta.url)),
        ])
);

export default defineConfig(({ command, mode }): UserConfig => {
    //const env = loadEnv(mode, process.cwd(), '');

    return {
        base: mode == 'production' ? '/dist' : '/',
        optimizeDeps: { exclude: ['swiper/vue', 'swiper/types'] },
        resolve: {
            alias: {
                '@': path.resolve(__dirname, './'),
                'vue': path.resolve(__dirname, './node_modules/vue/dist/vue.esm-bundler.js'),
                '~bootstrap': path.resolve(__dirname, './node_modules/bootstrap'),
                '@arla-lib': path.resolve(__dirname, './arlanet-library'),
                '@scss': path.resolve(__dirname, './style'),
            },
        },
        css: {
            devSourcemap: true,
        },
        publicDir: false,
        plugins: [
            vue(),
            dsv(),
            svgLoader(),
            VueI18nPlugin({
                include: path.resolve(
                    path.dirname(fileURLToPath(import.meta.url)),
                    `../../${UMBRACO_PROJECT_NAME}}/wwwroot/dist/lang/**`
                ),
            }),
        ],
        build: {
            outDir: `../../${UMBRACO_PROJECT_NAME}/wwwroot/dist/`, // Change this to correct folder inside the Umbraco project
            emptyOutDir: true, // needs to be explicitly set because it’s outside of ./
            sourcemap: mode === 'development',
            rollupOptions: {
                input: {
                    ...scriptGlob,
                    ...styleGlob,
                    ...styleComponentsGlob,
                },
                output: {
                    assetFileNames: (assetInfo: any) => {
                        let extType = assetInfo.name.substring(assetInfo.name.lastIndexOf('.') + 1);
                        if (/png|jpe?g|svg|gif|tiff|bmp|ico/i.test(extType)) {
                            extType = 'img';
                        }
                        return `${extType}/[name][extname]`;
                    },
                    chunkFileNames: 'script/chunkies/[name].js',
                    entryFileNames: '[name].js',
                },
            },
        },
        server: {
            port: PORT,
            https: {
                key: fs.readFileSync('certs/localhost-key.pem'),
                cert: fs.readFileSync('certs/localhost.pem'),
            },
            hmr: {
                port: PORT,
                protocol: 'wss',
            },
        },
    };
});
