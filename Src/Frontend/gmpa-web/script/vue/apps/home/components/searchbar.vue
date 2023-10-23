<template>
    <div class="search-input">
        <input
            v-model="input"
            type="text"
            placeholder="Search for region or country"
            class="search-bar"
            :class="{ active: !input }"
            @input="handleInput" />
        <searchIcon width="24" height="24" class="search-icon" />
        <div v-if="handleInput">asdasd</div>
    </div>
</template>

<script setup>
import { ref, onMounted, watch, defineEmits, computed } from 'vue';
import searchIcon from '../assets/search.svg';

const input = ref('');
const emit = defineEmits(['search-input']);

watch(input, (newValue) => {
    emit('search-input', newValue);
});

let test = ['oppa', 'keke', 'iop'];

const handleInput = computed(() => {
    if (!input.value) return;
    const filteredInput = test.filter((e) =>
        e.toLocaleLowerCase().startsWith(input.value.toLocaleLowerCase())
    );
    return filteredInput.length > 0 ? filteredInput : false;
});
</script>

<style scoped>
.search-bar {
    width: 395px;
    padding: 12px 38px 12px 16px;
    box-shadow: 0px 4px 4px 0px #00000040 inset;
    border-radius: 64px;
    background: #ffffff;
    border: none;
    outline: none;
}

.search-icon {
    position: absolute;
    right: 13px;
    top: 13px;
    color: #214b63;
    cursor: pointer;
}

::selection {
    color: #ffffff;
    background: #664aff;
}

.search-input.active input {
    border-radius: 5px 5px 0 0;
}

.search-input .results {
    padding: 0;
    opacity: 0;
    pointer-events: none;
    max-height: 280px;
    overflow-y: auto;
}

.search-input.active .results {
    padding: 10px 8px;
    opacity: 1;
    pointer-events: auto;
}

.results li {
    list-style: none;
    padding: 8px 12px;
    display: none;
    width: 100%;
    cursor: default;
    border-radius: 3px;
}

.search-input.active .results li {
    display: block;
}

.results li:hover {
    background: #efefef;
}
</style>
