import { defineStore } from 'pinia';

import countryDataRecords from './countries-data/country-details.csv';

export const useCountriesDataStore = defineStore('countriesData', () => {
    const countryData = countryDataRecords.map((country) => {
        // Create a new object by spreading the original item
        // and adding the derived property
        if (country.iso_n3.length<2) country.iso_n3= `00${country.iso_n3}`
        if (country.iso_n3.length<3) country.iso_n3= `0${country.iso_n3}`
        return {
          ...country
          , name: country.name_long
        }
    }) 

    return {
        countryData,
    };
});
