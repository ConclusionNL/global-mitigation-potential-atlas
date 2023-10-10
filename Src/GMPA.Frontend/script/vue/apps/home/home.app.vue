<template>
    <h1 v-html="stuff"></h1>
    <homeSidenav :textOrStuff="stuff"></homeSidenav>
</template>

<script setup lang="ts">
    import homeSidenav from './components/home-sidenav.vue';

    const props = defineProps<{
        stuff: string;
    }>();
</script>

<style lang="scss">
    // Do Nothing
</style>