<template>
    <h1>
        <span class="selected-collaboration">{{ selectedCollaboration }}</span
        >'s Mitigation Potential Diagram for 2050
    </h1>
    
    <span class="model-explanation"  v-if="!(collaborationCountriesList[0]['DNM'])">
    This uses a generalized STEVFNs model that better captures the challenges and benefits of international collaboration. This is translated from potentially different detailed national-level models used by respective countries. Emissions and costs are annualized for a 30 year greenfield project built in 2050. System meets 2050 demand for power and high temperature industrial heating sectors.
    </span>
    <span class="model-explanation"  v-else>
        This uses a detailed national-level model that represents the current model and data being used by the country. This is used to calibrate the STEVFNs model used to model international collaboration. The system meets 2015-2070 demand for power, industry and transport sectors. The costs are the NPV costs for a brownfield project from 2015-2070. The emissions are 2050 emissions assuming a linear reduction from 2025-2050, and constant emissions from 2050-2070.
    </span>
    <div v-if="countriesList.length > 0">
        <label for="countryDataSetSelect" v-if="countriesList.length > 1">Select one Country, Full collaboration or Autarky:</label>
        <label for="countryDataSetSelect" v-else>Select Overall or Detailed National Modelling:</label>
        <select
            id="countryDataSetSelect"
            v-model="selectedCountryDataSet"
            @change="handleCountryDataSetChange"
            class="select-element">
            <option
                v-for="option in (countriesList.length> 1? selectListOptions:[])
                .concat(
                    countriesList.map((country) => ({
                        label: country.properties.name,
                        value: country,
                    })))
                .concat(
                    countriesList.map((country) => ({
                        label: `${country.properties.name } - Detailed National Modelling`,
                        value: {...country,'DNM':true}
                    }))
                )"
                :value="option.value">
                {{ option.label }}
            </option>
        </select>
    </div>
    <collaborationStackedAreaChart
        :collaborationCountriesList="collaborationCountriesList" :dataSetType="dataSetTypeToShow"
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

const selectedCountryDataSet = ref('all' );
const selectListOptions = ref([{ label: 'Full Collaboration', value: 'all' }, { label: 'Autarky', value: 'all-autarky' }]);

const selectedCollaboration = computed(() => {
    if (selectedCountryDataSet.value == 'all' || selectedCountryDataSet.value.value == 'all'
    || selectedCountryDataSet.value == 'all-autarky' || selectedCountryDataSet.value.value == 'all-autarky') {
        return props.countriesList.map((country) => country.properties.name).join(', ');
    } else return selectedCountryDataSet.value.properties.name;
});

const collaborationCountriesList = computed(() => {
    if (selectedCountryDataSet.value == 'all' || selectedCountryDataSet.value.value == 'all'  || selectedCountryDataSet.value == 'all-autarky' || selectedCountryDataSet.value.value == 'all-autarky')  {
        return props.countriesList;
    } else return [selectedCountryDataSet.value];
});

const dataSetTypeToShow = computed(() => {
    let dataSetType ="collaboration"
    if (selectedCountryDataSet.value == 'all-autarky' || selectedCountryDataSet.value.value == 'all-autarky')  dataSetType = "autarky"
    if (selectedCountryDataSet.value['DNM'] ){dataSetType ="detailedNationalModelling"}
    console.log(`data set type ${dataSetType}`)
    return dataSetType;
    
});
</script>

<style>
.selected-collaboration {
    color: orange;
}

.select-element {
    width: auto; /* Allow the select element to adjust its width based on content */
}

.model-explanation {
font-size:14px;
font-style:italic
}
</style>
