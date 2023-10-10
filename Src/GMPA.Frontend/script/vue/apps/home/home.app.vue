<template>
    <homeSidenav :textOrStuff="stuff"></homeSidenav>
</template>

<script setup lang="ts">
    import homeSidenav from './components/home-sidenav.vue';

    const props = defineProps<{
        countries: [];
        stuff: string;
    }>();
</script>

<style lang="scss">
    body {
        background: white;
    }
</style>