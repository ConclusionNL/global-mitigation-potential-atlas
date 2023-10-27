import { ref } from 'vue';

const selectedCountries = ref([]);
const dataSet = ref([]);
const inCollabMode = ref(false);

export function useSelectedCountries() {
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
        if (selectedCountries.value.map((c) => c.id).includes(country.id) || !country) return;
        selectedCountries.value.push(country);
    }

    function removeCountry(country) {
        selectedCountries.value = selectedCountries.value.filter((c) => c !== country);
    }

    function resetCountries() {
        selectedCountries.value = [];
    }

    function setCollabMode(boolean) {
        inCollabMode.value = boolean;
    }

    return {
        dataSet,
        selectedCountries,
        inCollabMode,
        setCollabMode,
        getCountryByName,
        setCountry,
        addCountry,
        removeCountry,
        resetCountries,
    };
}
