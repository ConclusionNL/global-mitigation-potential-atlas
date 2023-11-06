import { defineStore } from 'pinia';

import countryDataRecords from './countries-data/country-details.csv';
import countryGeoJSON from './countries-data/50m-world-atlas-geojson.json';
import heatmapRecords from './heatmaps.csv';
import * as topojson from 'topojson';

let countries

export const useCountriesDataStore = defineStore('countriesData', () => {
    const countryData = countryDataRecords.map((country) => {
        // Create a new object by spreading the original item
        // and adding the derived property
        if (country.iso_n3.length < 2) country.iso_n3 = `00${country.iso_n3}`
        if (country.iso_n3.length < 3) country.iso_n3 = `0${country.iso_n3}`
        return {
            ...country
            , name: country.name_long
        }
    })

    const fetchData = async () => {
        if (countries) return countries

        const countriesById = countryData.reduce((accumulator, d) => {
            accumulator[d.iso_n3] = d;
            return accumulator;
        }, {});
        countries = topojson.feature(countryGeoJSON, countryGeoJSON.objects.countries);
        countries.features.forEach((d) => {
            // add all country properties from the TSV file to the features of the countries
            Object.assign(d.properties, countriesById[d.id]);

            // using the ISo2 country code (iso_a2), check heatmapData array for an object with the right COUnTRY property value
            const countryCode = d.properties.iso_a2;
            heatmapRecords
                .filter((h) => h['Country'] == countryCode)
                .forEach((c) => {
                    // copy properties from c to d.properties
                    for (let key in c) {
                        if (key !== 'Country') {
                            d.properties[key] = c[key];
                        }
                    }
                    // set the property in_heatmap to true to indicate that there is heatmap data for this country
                    d.properties['in_heatmap'] = true;
                });

            // todo - these properties are added in a not very efficient way
            // "Mitigation_Potential(GtCO2e)":"234","Mitigation_Cost($/GtCO2e)":"5","Mitigation_Potential(GtCO2e)_at_50":"234","Mitigation_Potential(GtCO2e)_at_100":"250","Mitigation_Potential(GtCO2e)_at_200":"300"}
        });
        return countries
    }

    return {
        countryData,
        countryGeoJSON,
        fetchData,
    };
});
