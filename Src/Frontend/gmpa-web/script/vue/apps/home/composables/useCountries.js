import { ref } from 'vue';

export function useCountries() {
    const selectedCountry = ref('');
    const selectedCountries = ref([]);

    function setSelectedCountry(country) {
        selectedCountry.value = country;
    }

    function addToSelectedCountries(country) {
        selectedCountries.value.push(country);
    }

    function removeFromSelectedCountries(country) {
        // selectedCountries.value.splice
    }

    return {
        selectedCountry,
        setSelectedCountry,
    };
}
