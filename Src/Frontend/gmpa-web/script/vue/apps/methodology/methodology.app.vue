<template>
    <h1 v-html="title"></h1>
    <methodology-aside></methodology-aside>
</template>

<script setup lang="ts">
    import methodologyAside from './components/methodology-aside.vue';

    const props = defineProps<{
        title: string;
    }>();
</script>

<style lang="scss">
    // Do nothing
</style>