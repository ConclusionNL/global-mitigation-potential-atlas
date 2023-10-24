<template>
    <div class="search-input">
        <input
            v-model="input"
            type="text"
            placeholder="Search for region or country"
            class="search-bar"
            @input="filterInput"
            @keydown.enter="onEnter" />
        <searchIcon @click="onEnter" width="24" height="24" class="search-icon" />
        <div class="justify-center">
            <div
                v-if="filterInput && filterInput !== input && !inputContainsCountry"
                class="results"
                :class="{ active: !filterInput }">
                <li @click="onSuggestionClick(country.Name)" v-for="country in filterInput">
                    {{ country.Name }}
                </li>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted, watch, defineEmits, defineProps, computed } from 'vue';
import searchIcon from '../assets/search.svg';

const input = ref('');
const emit = defineEmits(['search-input', 'country-searched']);

const props = defineProps({
    countries: {},
});

// watch(input, (newValue) => {
//     emit('search-input', newValue);
// });

const filterInput = computed(() => {
    if (!input.value) return;
    const filteredInput = props.countries.filter((c) =>
        c.Name.toLocaleLowerCase().startsWith(input.value.toLocaleLowerCase())
    );
    return filteredInput.length > 0 ? filteredInput : false;
});

const onSuggestionClick = (country) => {
    input.value = country;
    emit('country-searched', country);
};

const inputContainsCountry = computed(() => {
    for (const country in props.countries) {
        if (props.countries[country].Name.toLocaleLowerCase() === input.value.toLocaleLowerCase()) {
            return props.countries[country];
        }
    }
    return false;
});

const onEnter = () => {
    if (!inputContainsCountry.value || !inputContainsCountry.value.Active) return;

    console.log(inputContainsCountry.value.Active);

    console.log('it works!');
};
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

.search-input .results {
    padding: 0;
    cursor: pointer;
    max-height: 200px;
    width: 350px;
    overflow-y: auto;
    background-color: white;
    margin-top: 2px;
}

.search-input.active .results {
    padding: 10px 8px;
    background-color: white;
}

.results li {
    list-style: none;
    padding: 8px 12px;
    width: 100%;
    background-color: white;
}

.results li:hover {
    background: #efefef;
}

.justify-center {
    display: flex;
    justify-content: center;
}
</style>
