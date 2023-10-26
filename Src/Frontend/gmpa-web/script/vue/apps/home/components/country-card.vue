<template>
    <div class="country-card">
        <div class="card-top">
            <div class="title">{{ selectedCountry.properties.name }}</div>
            <closeIcon
                class="close-btn"
                :src="closeIcon"
                alt="close-button"
                height="24"
                width="24"
                @click="emit('country-closed')" />
        </div>
        <div class="card-data-spacing">
            <div key="1" class="data">
                <div>Mitigation Potential(GtCO2e)</div>
                <div>{{ selectedCountry.properties['Mitigation_Potential(GtCO2e)'] }}</div>
            </div>
            <div key="2" class="data">
                <div>Mitigation Cost($/GtCO2e)</div>
                <div>{{ selectedCountry.properties['Mitigation_Cost($/GtCO2e)'] }}</div>
            </div>
            <div key="3" class="data">
                <div>Mitigation Potential(GtCO2e) at 50</div>
                <div>{{ selectedCountry.properties['Mitigation_Potential(GtCO2e)_at_50'] }}</div>
            </div>
            <div key="4" class="data">
                <div>Mitigation Potential(GtCO2e) at 100</div>
                <div>{{ selectedCountry.properties['Mitigation_Potential(GtCO2e)_at_100'] }}</div>
            </div>
            <div key="5" class="data">
                <div>Mitigation Potential(GtCO2e) at 200</div>
                <div>{{ selectedCountry.properties['Mitigation_Potential(GtCO2e)_at_200'] }}</div>
            </div>
            
        </div>
        <div class="card-buttons">
            <button class="btn btn-alt-1" @click="emit('collab-mode')">
                Select for collaboration
            </button>
            <button class="btn btn-alt-2" @click="emit('country-details')">
                View country details
            </button>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted, watch, defineProps } from 'vue';
import closeIcon from '../assets/cross.svg';

const emit = defineEmits(['country-closed', 'collab-mode', 'country-details']);

const props = defineProps({
    selectedCountry: {},
});
</script>

<style scoped>
.country-card {
    padding: 24px 32px;
    height: 289px;
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
    flex-direction: row;
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
