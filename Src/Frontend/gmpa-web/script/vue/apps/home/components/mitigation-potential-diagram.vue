<template>
    <h1>
        <span class="selected-collaboration">{{ selectedCollaboration }}</span
        >'s Mitigation Potential Diagram 2030/2050
    </h1>
    <div v-if="countriesList.length > 1">
        <label for="countrySelect">Select one Country or Full collaboration:</label>
        <select
            id="countrySelect"
            v-model="selectedCountry"
            @change="handleCountryChange"
            class="select-element">
            <option
                v-for="option in selectListOptions.concat(
                    countriesList.map((country) => ({
                        label: country.properties.name,
                        value: country,
                    }))
                )"
                :value="option.value">
                {{ option.label }}
            </option>
        </select>
    </div>
    <collaborationStackedAreaChart
        :collaborationCountriesList="collaborationCountriesList"
        @technologySelected="handleTechnologySelected" />
</template>

<script setup>
import { ref, onMounted, watch, defineProps, computed } from 'vue';
import closeIcon from '../assets/cross.svg';
import filterIcon from '../assets/filter.svg';
import collaborationStackedAreaChart from './collaboration-stacked-area-chart.vue';

const emit = defineEmits(['technology-selected']);

const props = defineProps({
    countriesList: [],
});
const handleTechnologySelected = (payload) => {
    emit('technology-selected', payload);
};

const selectedCountry = ref({ value: 'all' });
const selectListOptions = ref([{ label: 'Full Collaboration', value: 'all' }]);

const selectedCollaboration = computed(() => {
    if (selectedCountry.value == 'all' || selectedCountry.value.value == 'all') {
        return props.countriesList.map((country) => country.properties.name).join(', ');
    } else return selectedCountry.value.properties.name;
});

const collaborationCountriesList = computed(() => {
    if (selectedCountry.value == 'all' || selectedCountry.value.value == 'all') {
        return props.countriesList;
    } else return [selectedCountry.value];
});
</script>

<style>
.selected-collaboration {
    color: orange;
}

.select-element {
    width: auto; /* Allow the select element to adjust its width based on content */
}
</style>
