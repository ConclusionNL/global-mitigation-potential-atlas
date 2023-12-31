﻿<template>
    <div class="navigation-container">
        <div class="d-grid gap-2">
            <button type="button" class="btn btn-light" @click="goToPage('/cases')">Facilitative policies</button>
            <button type="button" class="btn btn-light" @click="goToPage('/policies')">National policies</button>
        </div>
        <h4 class="country-divider">Country / Region / Territory</h4>
        <ul class="countries-list">
            <!-- #region Pilot countries-->
            <li>
                <div class="continent-container">
                    <div class="continent-divider">
                        Pilot
                    </div>
                </div>
            <li v-for="country in props.countries">
                <div v-if="country.Active">
                    <div @click="countryEligible(country.CountryId)" class="countries-link-active pointer">{{ country.Name
                    }}</div>
                </div>
            </li>
            </li>
            <!-- #endregion -->
            <div v-for="(continent, i) in continents" :index="continent" style="padding-right: 10px">
                <li>
                    <div class="continent-container">
                        <div class="continent-divider" @click="continentsCollapsable[i] = !continentsCollapsable[i]">
                            {{ continent }}
                        </div>
                    </div>
                    <div v-for="country in props.countries" class="expander"
                        :class="{ 'expanded': continentsCollapsable[i] }">
                        <div class="expander-content">
                <li v-if="country.Continent == continent">
                    <div class="countries-list-item-active">
                        <div v-if="country.Active">
                            <div @click="countryEligible(country.CountryId)" class="countries-link-active pointer">{{
                                country.Name }}</div>
                        </div>
                        <div v-else class="countries-list-item-disabled">
                            {{ country.Name }}
                        </div>
                    </div>
                </li>
            </div>
    </div>
    </li>
    </div>
    </ul>
    </div>
</template>

<script lang="ts" setup>
import { defineProps, ref } from "vue";
import { useCollaborationStore } from '../stores/collaborationStore';
import { useSelectedCountries } from '../composables/useSelectedCountries';

const collaborationStore = useCollaborationStore();
const useCountries = useSelectedCountries();
const selectedCountries = useCountries.selectedCountries;
const inCollabMode = useCountries.inCollabMode;

const props = defineProps({
    countries: {},
})

const continents = ["Africa", "Asia", "North America", "South America", "Europe", "Oceania"]
const continentsCollapsable = ref([]);

continents.forEach(continent => {
    continentsCollapsable.value.push(true);
});

const countryEligible = (cName) => {
    const selectedCountryCodes = selectedCountries.value.map(
        (country: any) => country.properties.iso_n3
    );
    const collaborationCandidateCountryCodes =
        collaborationStore.findCollaboratingCountries(selectedCountryCodes);

    const country = useCountries.getCountryById(cName);

    if (
        inCollabMode.value &&
        (collaborationCandidateCountryCodes.length == 0 ||
            !collaborationCandidateCountryCodes.includes(country.properties.iso_n3))
    ) {
        return;
    }

    if (inCollabMode.value) {
        useCountries.addCountry(country);
    } else {
        useCountries.setCountry(country);
    }
}

const goToPage = (url) => {
    window.location.href = url;
};

</script>

<style lang="scss">
.pointer {
    cursor: pointer;
}

.navigation-container {
    margin-top: 15px;
}

.expander {
    display: grid;
    grid-template-rows: 0fr;
    overflow: hidden;
    transition: grid-template-rows 0.2s;
}

.expander-content {
    min-height: 0;
    transition: visibility 0.2s;
    visibility: hidden;
}

.expander.expanded {
    grid-template-rows: 1fr;
}

.expander.expanded .expander-content {
    visibility: visible;
}

.continent-container {
    width: 100%;
    text-align: center;
}

.continent-divider {
    display: inline-block;
    width: 90%;
    border-bottom: 1px solid #000;
    margin-top: 5px;
    text-align: center;
    font-weight: bold;
    cursor: pointer;
}
</style>