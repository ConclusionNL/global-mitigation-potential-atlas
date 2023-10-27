<template>
    <h1><span class="selected-collaboration">{{selectedCollaboration}}</span>'s Mitigation Potential Diagram 2030/2050</h1>

    <label for="countrySelect">Select a Country:</label>
    <select id="countrySelect" v-model="selectedCountry" @change="handleCountryChange">
        <option v-for="option in selectListOptions.concat(countriesList.map(country => ({
            label: country.properties.name,
            value: country,
        })))" :key="n" :value="option.value">
            {{ option.label }}
        </option>
    </select>
</template>

<script setup>
import { ref, onMounted, watch, defineProps, computed } from 'vue';
import closeIcon from '../assets/cross.svg';
import filterIcon from '../assets/filter.svg';

const emit = defineEmits(['technology-selected']);

const props = defineProps({
    countriesList: []
});

const selectedCountry = ref({"value":"all"})
const selectListOptions = ref([{ "label": "Full Collaboration", "value": "all" }])

const selectedCollaboration = computed(() => {
    if (selectedCountry.value == 'all'||selectedCountry.value.value == 'all') {
        return props.countriesList.map(country => country.properties.name).join(', ')
    }
    else return selectedCountry.value.properties.name
});


// const handleCountryChange = () => {
//     // This method is invoked when the user selects a country
//     // You can access the selected country object as this.selectedCountry
//     if (selectedCountry) {
//         console.log('Selected Country:', selectedCountry.value);
//         console.log('Selected Country:', selectedCountry.label);
//         // Perform actions or invoke your handler function here
//     }
// }
</script>

<style>
.selected-collaboration {color: orange;}
</style>