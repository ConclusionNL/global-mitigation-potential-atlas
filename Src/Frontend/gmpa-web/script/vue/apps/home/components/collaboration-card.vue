<template>
    <div class="collab-card">
        <div class="title">Select multiple countries to view collaboration potentials</div>
        <div class="flex-collab">
            <div class="countries-collab-list">
                <div v-for="country in props.countriesList" :key="country">
                    <div class="small-card">
                        <div>{{ country.properties.name }}</div>
                        <closeIcon width="24" height="24" style="cursor: pointer"
                            @click="emit('country-closed', country)" />
                    </div>
                </div>
            </div>
            <button v-if="countriesList.length > 1" class="benefits-btn" @click="emit('show-benefits')">View benefits</button>
        </div>
        <div class="suggestions-container">
            <div style="font-weight: 500">Our suggestions</div>
            <div class="filter-box">
                <div>Filters</div>
                <filterIcon width="24" height="24" />
            </div>
        </div>
        <div class="suggestion-country-container">
            <div v-for="collaborationCandidate in collaborationCandidatesList" :key="n">
                <div class="suggestion-country-boxes">
                    <input :id="collaborationCandidate.id" type="checkbox" :name="`checkbox-${n}`"
                        :value="collaborationCandidate" class="checkbox" @change="emit('country-added', collaborationCandidate)" />
                    <div class="label-box">
                        <div class="label-title">{{ collaborationCandidate.properties.name }}</div>
<!--                        <div class="label-subtitle">Data with filters from country {{ collaborationCandidate.properties.iso_a2 }}</div>
-->
                    </div>
                    <div class="other-info"></div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted, watch, defineProps, computed } from 'vue';
import closeIcon from '../assets/cross.svg';
import filterIcon from '../assets/filter.svg';

const emit = defineEmits(['country-closed', 'country-added','show-benefits']);

const props = defineProps({
    countriesList: [], collaborationCandidatesList: []
});

</script>

<style scoped>
.collab-card {
    padding: 16px;
    height: fit-content;
    width: 776px;
    background-color: white;
    border-radius: 8px;
    box-shadow: 0px 4px 8px 0px #214b6352;
}

.title {
    font-weight: 600;
    font-size: 22px;
}

.small-card {
    display: flex;
    flex-direction: row;
    justify-content: center;
    gap: 10px;
    color: white;
    background-color: #f07004;
    padding: 12px 16px;
    width: fit-content;
    height: 48px;
    border-radius: 4px;
    font-weight: 600;
}

.countries-collab-list {
    display: flex;
    flex-flow: row wrap;
    gap: 10px;
}

.suggestions-container {
    display: flex;
    justify-content: space-between;
    border-top: 1px solid #214b63;
    padding: 8px;
}

.filter-box {
    display: flex;
    flex-direction: row;
    gap: 10px;
    font-weight: 500;
}

.flex-collab {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    padding: 12px 0 22px 0;
}

.benefits-btn {
    overflow: hidden;
    border: none;
    text-align: center;
    text-decoration: none;
    border-radius: 4px;
    cursor: pointer;
    font-weight: 600;
    height: 48px;
    font-size: 16px;
    width: fit-content;
    color: #f07004;
    border: 1px solid #f07004;
    background-color: white;
    padding: 12px 16px;
    flex-shrink: 0;
}

.suggestion-country-container {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    padding: 8px;
}

.suggestion-country-boxes {
    flex: 1;
    display: flex;
    flex: auto;
    align-items: center;
    border: 1px solid #f07004;
    border-radius: 4px;
    gap: 16px;
    height: 60px;
    width: 180px;
    padding: 8px 12px;
}

.checkbox {
    width: 24px;
    height: 24px;
    border-radius: 4px;
}

.label-box {
    display: flex;
    flex-direction: column;

    .label-title {
        color: #214b63;
        font-size: 16px;
        font-weight: 600;
    }

    .label-subtitle {
        font-size: 14px;
    }
}
</style>
