<template>
    <homeSidenav :countries="props.countries"></homeSidenav>
</template>

<script setup lang="ts">
    import homeSidenav from './components/home-sidenav.vue';
    import { defineProps, reactive } from "vue";

    const props = defineProps<{
        countries: {};
    }>();

</script>

<style lang="scss">
    // Do Nothing
</style>