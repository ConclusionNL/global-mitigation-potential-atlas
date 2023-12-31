<template>
    <div class="search-input">
        <input
            v-model="input"
            type="text"
            placeholder="Search for region or country"
            class="search-bar"
            @keydown.enter="onEnter" />
        <searchIcon @click="onEnter" width="24" height="24" class="search-icon" />
        <div class="justify-center">
            <div
                v-if="filterInput && filterInput !== input && !inputContainsCountry"
                class="results"
                :class="{ active: !filterInput }">
                <div v-for="country in filterInput">
                    <li @click="onSuggestionClick(country)">
                        {{ country.Name }}
                    </li>
                </div>
            </div>
        </div>
        <div v-if="errorMsg" class="error justify-center">{{ errorMsg }}</div>
    </div>
</template>

<script setup>
import { ref, onMounted, watch, defineProps, computed } from 'vue';
import { useCollaborationStore } from '../stores/collaborationStore';
import { useSelectedCountries } from '../composables/useSelectedCountries';
import searchIcon from '../assets/search.svg';

const collaborationStore = useCollaborationStore();
const useCountries = useSelectedCountries();
const selectedCountries = useCountries.selectedCountries;
const inCollabMode = useCountries.inCollabMode;

const input = ref('');
const errorMsg = ref('');

const props = defineProps({
    countries: {},
});

watch(input, (newValue, oldValue) => {
    if (newValue.length < oldValue.length) {
        errorMsg.value = '';
    }
});

const filterInput = computed(() => {
    if (!input.value) return;
    const filteredInput = props.countries.filter((c) =>
        c.Name.toLocaleLowerCase().startsWith(input.value.toLocaleLowerCase())
    );
    return filteredInput.length > 0 ? filteredInput : false;
});

const onSuggestionClick = (country) => {
    input.value = country.Name;
    onEnter();
};

const inputContainsCountry = computed(() => {
    for (const country in props.countries) {
        console.log(props.countries[country].Name);
        if (props.countries[country].Name.toLocaleLowerCase() === input.value.toLocaleLowerCase()) {
            return props.countries[country];
        }
    }
    return false;
});

const onEnter = () => {
    const selectedCountryCodes = selectedCountries.value.map(
        (country) => country.properties.iso_a2
    );
    const collaborationCandidateCountryCodes =
        collaborationStore.findCollaboratingCountries(selectedCountryCodes);

    const country = useCountries.getCountryByName(inputContainsCountry.value.Name);

    if (!inputContainsCountry.value) {
        errorMsg.value = 'Country could not be found';
        return;
    } else if (!inputContainsCountry.value.Active) {
        errorMsg.value = 'Country is not available in the pilot';
        return;
    } else if (
        inCollabMode.value &&
        (collaborationCandidateCountryCodes.length == 0 ||
            !collaborationCandidateCountryCodes.includes(country.properties.iso_a2))
    ) {
        errorMsg.value = 'This country could not be collaborated with';
        return;
    }

    if (inCollabMode.value) {
        useCountries.addCountry(useCountries.getCountryByName(inputContainsCountry.value.Name));
    } else {
        useCountries.setCountry(useCountries.getCountryByName(inputContainsCountry.value.Name));
    }
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
    background: lightgray;
}

.justify-center {
    display: flex;
    justify-content: center;
}

.error {
    color: red;
    font-weight: 500;
}

@media screen and (max-width: 1199px) {
    .search-bar {
    width: 235px;
}

.search-input .results {
    width: 220px;
}
}
</style>
