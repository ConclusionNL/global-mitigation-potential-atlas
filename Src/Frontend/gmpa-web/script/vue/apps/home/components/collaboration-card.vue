<template>
    <div>
        <div class="collab-card" v-if="showComposeCollaborationSet">
            <div class="card-top">
                <div class="title">Select multiple countries to view collaboration potentials</div>
                <closeIcon
                    class="close-btn"
                    alt="close-button"
                    height="24"
                    width="24"
                    @click="
                        useCountries.resetCountries();
                        useCountries.setCollabMode(false);
                    " />
            </div>
            <div class="flex-collab">
                <div class="countries-collab-list">
                    <div v-for="country in selectedCountries" :key="country">
                        <div class="small-card">
                            <div>{{ country.properties.name }}</div>
                            <closeIcon
                                width="24"
                                height="24"
                                style="cursor: pointer"
                                @click="useCountries.removeCountry(country)" />
                        </div>
                    </div>
                </div>
                <button
                    v-if="selectedCountries.length > 1"
                    class="benefits-btn"
                    @click="showComposeCollaborationSet = false">
                    View benefits
                </button>
            </div>
            <div class="suggestions-container">
                <div style="font-weight: 500">Our suggestions</div>
                <div @click="filterBox = !filterBox" class="filter-box">
                    <div>Filters</div>
                    <filterIcon width="24" height="24" />
                </div>
            </div>
            <div v-if="filterBox" class="filter-options-dropdown">
                <div v-for="filter in mitTypes">
                    <li
                        @click="
                            mitType = filter;
                            filterBox = !filterBox;
                        ">
                        MtCO2 at {{ filter.replaceAll(/[a-zA-Z]/g, '') }} $/tCO2e
                    </li>
                </div>
            </div>
            <div class="suggestion-country-container space-between">
                <div
                    class="suggestion-country-boxes"
                    v-for="collabCandidate in collaborationCandidatesList"
                    :key="collabCandidate.id">
                    <input
                        :id="collabCandidate.id"
                        type="checkbox"
                        :name="`checkbox-${n}`"
                        :value="collabCandidate"
                        class="checkbox"
                        @change="useCountries.addCountry(collabCandidate)" />
                    <div class="label-box">
                        <div class="label-title">
                            {{ collabCandidate.properties.name }}
                        </div>
                        <div class="label-details">
                            <div class="mit-amount">
                                {{ getMitPotCollabCountries(collabCandidate, mitType) ?? '' }}
                            </div>
                            <div>MtCO2 at {{ mitType.replaceAll(/[a-zA-Z]/g, '') }}</div>
                        </div>
                    </div>
                    <div class="other-info"></div>
                </div>
            </div>
        </div>
        <div class="benefits-card" v-if="!showComposeCollaborationSet">
            <div class="suggestion-country-container">
                <div class="back-box">
                    <div class="d-flex back-btn" @click="showComposeCollaborationSet = true">
                        <backIcon width="24" height="24"></backIcon>
                        <div>Back</div>
                    </div>
                    <div class="switch-box">
                        <div>Cost / tCO2e</div>
                        <label class="switch">
                            <input type="checkbox"  v-model="mitigationCosts" />
                            <span class="slider round"></span>
                        </label>
                        <div>Absolute tCO2e</div>
                    </div>
                </div>
                <div class="divider"></div>
                <div class="cost-of-achieving">
                    <div class="card-top bot-pad">
                        <div class="title">
                            {{ mitigationCosts ?  'Cost of achieving maximum' :'Maximum'  }} mitigation
                            potential in
                            <span class="selected-collaboration">{{
                                selectedCountries
                                    .map((country) => country.properties.name)
                                    .join(', ')
                            }}</span>
                            in autarky vs collaboration
                        </div>
                    </div>
                    <maximumMitigationPotentialGauge
                        :countriesList="selectedCountries"
                        :showMitigationCosts="mitigationCosts" />
                </div>
                <div class="divider"></div>
                <div class="coalition-potential">
                    <div class="card-top bot-pad">
                        <div class="title">
                            Coalition Maximum Mitigation Potential (Absolute with Collaboration)
                        </div>
                    </div>
                    <coalitionMaximumMitigationPotential :countriesList="selectedCountries">
                    </coalitionMaximumMitigationPotential>
                    <button class="benefits-btn" @click="emit('show-benefits')">
                        Show Mitigation Potential Diagram
                    </button>
                </div>
                <div class="divider"></div>
                <div class="country-statistics">
                    <class class="card-top bot-pad">
                        <div class="title">View Country Statistics</div>
                    </class>
                    <div class="country-navigations">
                        <div v-for="country in selectedCountries" :key="country">
                            <div
                                @click="emit('country-navigation', country)"
                                class="small-card statistics-btn">
                                <div>{{ country.properties.name }}</div>
                                <forwardIcon width="24" height="24" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted, watch, defineProps } from 'vue';
import { useSelectedCountries } from '../composables/useSelectedCountries';
import closeIcon from '../assets/cross.svg';
import filterIcon from '../assets/filter.svg';
import backIcon from '../assets/arrow-left.svg';
import forwardIcon from '../assets/arrow-right.svg';
import maximumMitigationPotentialGauge from './maximum-mitigation-potential-gauge.vue';
import coalitionMaximumMitigationPotential from './coalition-maximum-mitigation-potential.vue';

import { useCollaborationStore } from '../stores/collaborationStore';

const collaborationStore = useCollaborationStore();

const useCountries = useSelectedCountries();
const selectedCountries = useCountries.selectedCountries;
const emit = defineEmits(['show-benefits', 'country-navigation']);
const showComposeCollaborationSet = ref(true);
const mitigationCosts = ref(true);

const props = defineProps({
    collaborationCandidatesList: [],
});

const filterBox = ref(false);

const mitTypes = [
    'mitigationPotentialAt50',
    'mitigationPotentialAt100',
    'mitigationPotentialAt200',
];

const mitType = ref(mitTypes[0]);

const getMitPotCollabCountries = (collabCandidate, mitType) => {
    return collaborationStore.getMitigationPotentialContributionsForCollaborationCandidate(
        selectedCountries.value,
        collabCandidate
    )[mitType];
};
</script>

<style scoped>
.collab-card {
    padding: 16px;
    height: fit-content;
    width: 876px;
    color: #214b63;
    background-color: white;
    border-radius: 8px;
    box-shadow: 0px 4px 8px 0px #214b6352;
}

.benefits-card {
    display: flex;
    flex-direction: row;
    padding: 16px;
    height: fit-content;
    width: 1376px;
    background-color: white;
    border-radius: 8px;
    box-shadow: 0px 4px 8px 0px #214b6352;
}

.title {
    font-weight: 600;
    font-size: 22px;
    line-height: 24px;
    color: #214b63;
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

.label-details {
    background-color: #bac5cb;
    display: flex;
    align-items: center;
    flex-direction: row;
    font-size: 13px;
    line-height: 18px;
    border-radius: 2px;
    padding: 2px 4px 2px 2px;
    gap: 8px;
}

.mit-amount {
    background-color: white;
    padding: 1px 2px;
    border-bottom-left-radius: 2px;
    border-top-left-radius: 2px;
}

.filter-box {
    display: flex;
    flex-direction: row;
    gap: 10px;
    font-weight: 500;
    cursor: pointer;
}

.flex-collab {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    padding: 12px 0 22px 0;
}

.benefits-btn {
    border: none;
    text-align: center;
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
    flex-flow: row wrap;
    gap: 16px;
}

.space-between {
    justify-content: space-between;
}

.suggestion-country-boxes {
    display: flex;
    flex: 1;
    align-items: center;
    border: 1px solid #f07004;
    border-radius: 4px;
    gap: 14px;
    max-width: calc(50% - 8px);
    height: fit-content;
    padding: 8px 8px 8px 12px;
}

.checkbox {
    width: 24px;
    height: 24px;
    border-radius: 4px;
}

.label-box {
    display: flex;
    flex-direction: column;
}

.label-title {
    color: #214b63;
    font-size: 16px;
    font-weight: 600;
}

.label-subtitle {
    font-size: 10px;
    border: 1px solid #f07004;
    border-radius: 2px;
    align-items: center;
    gap: 4px;
    height: 38px;
    width: 62px;
    padding: 2px 1px;
    margin: 4px;
}

.card-top {
    display: flex;
    justify-content: space-between;
    align-items: top;
}

.bot-pad {
    padding-bottom: 20px;
}

.country-navigations {
    display: flex;
    flex-direction: column;
    gap: 10px;
}

.close-btn,
.statistics-btn {
    cursor: pointer;
}

.selected-collaboration {
    color: #f07004;
}

.back-btn {
    color: #214b63;
    gap: 8px;
    cursor: pointer;
}

.back-box {
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    flex: 0.5;
    padding-right: 12px;
}

.cost-of-achieving {
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    flex: 1.5;
    padding-inline: 12px;
    padding-bottom: 20px;
}

.coalition-potential {
    display: flex;
    flex-direction: column;
    flex: 1.5;
    padding-inline: 12px;
}

.country-statistics {
    flex: 1;
    padding-inline: 12px;
}

.divider {
    width: 1px;
    background: #dae3e8;
}

.switch {
    position: relative;
    display: inline-block;
    width: 106px;
    height: 62px;
}

.switch input {
    opacity: 0;
    width: 0;
    height: 0;
}

.slider {
    position: absolute;
    cursor: pointer;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    border: 1px solid gray;
    -webkit-transition: 0.4s;
    transition: 0.4s;
}

.slider:before {
    position: absolute;
    content: '';
    height: 28px;
    width: 100px;
    left: 2px;
    bottom: 2px;
    background-color: white;
    -webkit-transition: 0.4s;
    transition: 0.4s;
}

input:checked + .slider:before {
    -webkit-transform: translateY(-28px);
    -ms-transform: translateY(-28px);
    transform: translateY(-28px);
}

.slider.round {
    border-radius: 16px;
}

.slider.round:before {
    background: #214b63;
    border-radius: 14px;
}

.switch-box {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    font-size: 14px;
    align-self: center;
    justify-self: center;
}

.filter-options-dropdown {
    position: absolute;
    right: 0;
    padding: 0;
    cursor: pointer;
    max-height: 200px;
    overflow-y: auto;
    background-color: white;
    font-size: 12px;
    box-shadow: 0px 4px 8px 0px #214b6352;
}

.filter-options-dropdown li {
    list-style: none;
    padding: 8px;
    background-color: white;
}

.filter-options-dropdown li:hover {
    background: lightgray;
}
</style>
