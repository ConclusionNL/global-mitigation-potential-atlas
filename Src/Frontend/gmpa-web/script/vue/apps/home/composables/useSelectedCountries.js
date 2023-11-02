import { ref } from 'vue';

const selectedCountries = ref([]);
const dataSet = ref([]);
const inCollabMode = ref(false);

export function useSelectedCountries() {
    function getCountryById(countryId) {
        return dataSet.value.find((c) => c.id === countryId);
    }

    function getCountryByName(countryName) {
        return dataSet.value.find(
            (c) => c.properties.name.toLocaleLowerCase() === countryName.toLocaleLowerCase()
        );
    }

    function setCountry(country) {
        if (!country) return;
        resetCountries();
        addCountry(country);
    }

    function addCountry(country) {
        if (isCountryInList(country) || !country) return;
        selectedCountries.value.push(country);
    }

    function removeCountry(country) {
        selectedCountries.value = selectedCountries.value.filter((c) => c.id !== country.id);
    }

    function resetCountries() {
        selectedCountries.value = [];
    }

    function setCollabMode(boolean) {
        inCollabMode.value = boolean;
    }

    function isCountryInList(country) {
        return selectedCountries.value.map((c) => c.id).includes(country.id);
    }

    return {
        dataSet,
        selectedCountries,
        inCollabMode,
        setCollabMode,
        getCountryById,
        getCountryByName,
        setCountry,
        addCountry,
        removeCountry,
        resetCountries,
        isCountryInList,
    };
}
