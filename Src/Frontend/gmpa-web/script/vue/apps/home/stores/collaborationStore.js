/* eslint-disable @typescript-eslint/explicit-function-return-type */
import { defineStore } from 'pinia';
import * as d3 from 'd3';

import collabRecords from './collaborationdata.csv';
import fakeRecords from './fakecollaborationdata.json';
import heatmapRecords from './heatmaps.csv';
import { ref } from 'vue';

export const useCollaborationStore = defineStore('collaboration', () => {
    const collaborationData = ref(collabRecords);
    const fakeDataSet = ref(fakeRecords);
    const heatmapData = ref(heatmapRecords);
    let dataSet = ref([]);

    const prepareData = async (countries) => {
        console.log(`prepare data ${countries}`);

        if (countries.includes('FAKE')) {
            const data = JSON.parse(JSON.stringify(fakeDataSet.value));
            prepareFakeData(data);
            dataSet.value = data;
        } else {
            dataSet.value = prepareCollaborationData(collaborationData.value, countries);
        }
        console.log(`this data set ${dataSet.value}`);
        // this is how you can add data in store's state this.myData = {prop:42}
    };

    return {
        collaborationData,
        fakeDataSet,
        heatmapData,
        dataSet,
        prepareData,
    };
});

const deriveCountryKey = (countryArray) => {
    return countryArray.sort().join('');
};

const prepareCollaborationData = (raw, countries) => {
    // format:
    // [{x: , techA: , techB: }]
    // all technologies should be present in every data object  - 0 if they have no value
    //

    const data = [];

    const countryKey = deriveCountryKey(countries);
    console.log(`countryKey= ${countryKey}`);

    // determine the country key for each record: alphabetical concatenation of country_1..country_4
    // filter on records with the right country key
    // determine all values for technology_name

    const consolidation = {};
    for (const rec of raw) {
        const collaboratingCountries = [rec.country_1, rec.country_2, rec.country_3, rec.country_4];
        const collabKey = deriveCountryKey(collaboratingCountries);

        // console.log(`collabkey ${collabKey}  emissions ${rec.collaboration_emissions} ${rec.technology_cost}`)
        if (collabKey == countryKey) {
            const x = parseFloat(rec.collaboration_emissions);
            if (!consolidation[x]) consolidation[x] = {};
            consolidation[x][rec['technology_name']] = parseFloat(rec.technology_cost);
        }
    }
    const uniquePropertyNames = new Set();

    // find the unique technologyNames
    for (let prop in consolidation) {
        if (consolidation.hasOwnProperty(prop)) {
            // Iterate over properties of each property of consolidation
            Object.keys(consolidation[prop]).forEach((innerProp) => {
                uniquePropertyNames.add(innerProp);
            });
        }
    }
    const technologyNames = [...uniquePropertyNames];
    // make sure that all entries in consolidation have entries for all technologies
    for (let prop in consolidation) {
        // loop over all X values
        // loop over technologyNames
        for (let technology of technologyNames)
            if (!consolidation[prop].hasOwnProperty(technology)) {
                consolidation[prop][technology] = 0;
            }
    }

    //data = []
    for (let prop in consolidation) {
        // loop over all X values
        consolidation[prop].x = parseFloat(prop);
        data.push(consolidation[prop]);
    }
    prepareFakeData(data);
    return data;
};

const prepareFakeData = (data) => {
    data.sort((a, b) => d3.ascending(a.x, b.x));
    // Iterate through the array of objects
    for (const obj of data) {
        let sum = Object.keys(obj).reduce(function (sum, key) {
            if (key !== 'x') {
                return sum + obj[key];
            }
            return sum;
        }, 0);
        obj['sum'] = sum;
    }
};
