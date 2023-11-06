<template>
    <div class="country-card">
        <div class="card-top">
            <div class="title">{{ selectedCountries[0].properties.name }}</div>
            <closeIcon
                class="close-btn"
                alt="close-button"
                height="24"
                width="24"
                @click="useCountries.resetCountries()" />
        </div>
        <div class="card-data-spacing">
            <div key="1" class="data">
                <div>Mitigation Potential</div>
                <div>
                    {{ selectedCountries[0].properties['Mitigation_Potential(GtCO2e)'] }} GtCO2e
                </div>
            </div>
            <div key="2" class="data">
                <div>Mitigation Cost</div>
                <div>
                    {{ selectedCountries[0].properties['Mitigation_Cost($/GtCO2e)'] }} $/GtCO2e
                </div>
            </div>
            <div key="3" class="data">
                <div>Mitigation Potential at 50</div>
                <div>
                    {{ selectedCountries[0].properties['Mitigation_Potential(GtCO2e)_at_50'] }}
                    GtCO2e
                </div>
            </div>
            <div key="4" class="data">
                <div>Mitigation Potential at 100</div>
                <div>
                    {{ selectedCountries[0].properties['Mitigation_Potential(GtCO2e)_at_100'] }}
                    GtCO2e
                </div>
            </div>
            <div key="5" class="data">
                <div>Mitigation Potential at 200</div>
                <div>
                    {{ selectedCountries[0].properties['Mitigation_Potential(GtCO2e)_at_200'] }}
                    GtCO2e
                </div>
            </div>
        </div>
        <div class="coalition-potential">
                    <coalitionMaximumMitigationPotential :countriesList="selectedCountries">
                    </coalitionMaximumMitigationPotential>
                </div>
        <div class="card-buttons">
            <button class="btn btn-alt-1" @click="useCountries.setCollabMode(true)">
                Select for collaboration
            </button>
            <button class="btn btn-alt-2" @click="emit('country-navigation', selectedCountries[0])">
                View country details
            </button>
            <button
                class="btn btn-alt-3"
                style="width: 100%"
                @click="emit('country-mitigation-potential-diagram', selectedCountries[0])">
                Mitigation Potential Diagram
            </button>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted, watch, defineProps } from 'vue';
import { useSelectedCountries } from '../composables/useSelectedCountries';
import closeIcon from '../assets/cross.svg';
import coalitionMaximumMitigationPotential from './coalition-maximum-mitigation-potential.vue';

const useCountries = useSelectedCountries();
const selectedCountries = useCountries.selectedCountries;

const emit = defineEmits(['country-navigation', 'country-mitigation-potential-diagram']);
</script>

<style scoped>
.country-card {
    padding: 24px 32px;
    height: fit-content;
    width: 480px;
    background-color: white;
    border-radius: 8px;
    box-shadow: 0px 4px 8px 0px #214b6352;
}

.card-top {
    display: flex;
    justify-content: space-between;
    align-items: center;
    color: #214b63;
}

.title {
    font-weight: 600;
    font-size: 22px;
}

.close-btn {
    cursor: pointer;
    color: #214b63;
}

.card-buttons {
    display: flex;
    flex-flow: row wrap;
    justify-content: space-between;
    gap: 10px;
}

.btn {
    border: none;
    padding: 12px 16px;
    text-align: center;
    text-decoration: none;
    display: inline-block;
    font-size: 16px;
    border-radius: 4px;
    cursor: pointer;
    font-weight: 600;
}

.btn-alt-1 {
    color: #f07004;
    border: 1px solid #f07004;
}

.btn-alt-2 {
    color: white;
    border: 1px solid #214b63;
    background-color: #214b63;
}

.btn-alt-3 {
    color: white;
    border: 1px solid #f07004;
    background-color: #f07004;
}

.data {
    display: flex;
    justify-content: space-between;
    flex-direction: row;
    padding-block: 2px;
}

.card-data-spacing {
    padding-block: 14px;
}
</style>
