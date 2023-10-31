<template>
    <h4 class="country-divider">Countries</h4>
    <ul class="countries-list">
        <!-- #region Pilot countries-->
        <li>
            <div class="country-divider">
                Pilot
            </div>
        <li v-for="country in props.countries">
            <div v-if="country.Active">
                <div @click="countryEligible(country.Name)" class="countries-link-active pointer">{{ country.Name }}</div>
            </div>
        </li>
        </li>
        <!-- #endregion -->
        <div v-for="(continent, i) in continents" :index="continent" style="padding-right: 10px">
        <li>
            <div class="country-divider" @click="continentsCollapsable[i] = !continentsCollapsable[i]">
                {{continent}}
            </div>
            <div v-for="country in props.countries" class="expander" :class="{ 'expanded': continentsCollapsable[i] }">
                <div class="expander-content">
                    <li v-if="country.Continent == continent">
                        <div class="countries-list-item-active">
                            <div v-if="country.Active">
                                <a class="countries-link-active" :href=country.Url>{{ country.Name }}</a>
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
        (country: any) => country.properties.iso_a2
    );
    const collaborationCandidateCountryCodes =
        collaborationStore.findCollaboratingCountries(selectedCountryCodes);

    const country = useCountries.getCountryByName(cName);

    if (
        inCollabMode.value &&
        (collaborationCandidateCountryCodes.length == 0 ||
            !collaborationCandidateCountryCodes.includes(country.properties.iso_a2))
    ) {
        return;
    }

    if (inCollabMode.value) {
        useCountries.addCountry(country);
    } else {
        useCountries.setCountry(country);
    }
}


</script>

<style lang="scss">
    .pointer {
        cursor: pointer;
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
</style>