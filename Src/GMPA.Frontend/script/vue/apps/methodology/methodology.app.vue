<template>
    <h1 v-html="title"></h1>
    {{countries}}
    <ul>
        <li v-for="(country, index) in countries" :key="`country-${index}`">{{country}}</li>
    </ul>
    <methodology-aside></methodology-aside>
</template>

<script setup lang="ts">
    import methodologyAside from './components/methodology-aside.vue';

    const props = defineProps<{
        title: string;
        countries: [];
    }>();
</script>

<style lang="scss">
    body{
        background: red;
    }
</style>