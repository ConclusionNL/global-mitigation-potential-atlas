/* eslint-disable @typescript-eslint/explicit-function-return-type */
import { defineStore } from 'pinia';
import * as d3 from 'd3';

import collabRecords from './collaborationdata.csv';
import fakeRecords from './fakecollaborationdata.json';
import combinedAutarkyAndCollabRecords from './combined_data_autarky.csv';
import combinedAutarkyRecords from './autarky-and-collaboration-data/combined_data_autarky.csv';
import combinedCollaborationRecords from './autarky-and-collaboration-data/combined_data_collaboration.csv';

import heatmapRecords from './heatmaps.csv';
import heatmapAutarkyRecords from './autarky-and-collaboration-data/heatmap_autarky.csv';
import { ref } from 'vue';

export const useCollaborationStore = defineStore('collaboration', () => {
    const collaborationData = ref(collabRecords); // details per technology and per country collaboration set
    const combinedCollaborationData = ref(combinedAutarkyAndCollabRecords); // aggregates per country collaboration set - both autarky and collaboration, both costs and emission reduction

    const fakeDataSet = ref(fakeRecords);
    const heatmapData = ref(heatmapRecords);
    const heatmapAutarkyData = ref(heatmapAutarkyRecords);
    let dataSet = ref([]);
    let collaborations; // an array that contains string arrays with country codes for collaborating countries. each entry looks like [countryA], [countryB], ... (with up to four countries)
    let processedCombinedData = false
    let combinedData

    // combined data is an object with country keys as property names (keys) and objects with array properties 
    // collaboration and autarky. Each entry in these arrays has two properties: emissions, cost 
    // { 'ID': {collaboration: [{emissions: 34, cost:164}], autarky: [{emissions:53, cost:172 },{emissions:13, cost:72 }]}, 'SG': {} }
    const getCombinedData = () => {
        if (!processedCombinedData) {
            prepareCombinedDataFromFiles()
            processedCombinedData = true
        }
        return combinedData
    }

    const prepareCombinedDataFromFiles = () => {
        const data = {}

        // determine the country key for each record: alphabetical concatenation of country_1..country_4
        const collaborationProperties = ['emissions', 'cost']
        for (const rec of combinedCollaborationRecords) {
            const collaboratingCountries = [rec.country_1, rec.country_2, rec.country_3, rec.country_4]
            const collabKey = deriveCountryKey(collaboratingCountries)
            if (!data[collabKey]) { data[collabKey] = {"collaboration":[],"autarky":[]} }

            const consolidatedRecord = {}
            collaborationProperties.forEach((property) => {
                const x = parseFloat(rec[property])
                consolidatedRecord[property] = x
            })
            data[collabKey]["collaboration"].push(consolidatedRecord)
        }

        // add autarky
        for (const rec of combinedAutarkyRecords) {
            const combinedCountries = [rec.country_1, rec.country_2, rec.country_3, rec.country_4]
            const combinedKey = deriveCountryKey(combinedCountries)
            if (!data[combinedKey]) { data[combinedKey] = {"collaboration":[],"autarky":[]} }

            const consolidatedRecord = {}

            collaborationProperties.forEach((property) => {
                const x = parseFloat(rec[property])
                consolidatedRecord[property] = x
            })
            data[combinedKey]["autarky"].push(consolidatedRecord)
        }

        combinedData = data
    }



    const prepareData = async (countries) => {
        if (countries.includes('FAKE')) {
            const data = JSON.parse(JSON.stringify(fakeDataSet.value));
            prepareFakeData(data);
            dataSet.value = data;
        } else {
            dataSet.value = prepareCollaborationData(collaborationData.value, countries);
        }
        return dataSet.value;
    };

    const getCostOfAchievingMaximumMitigationPotentialInAutarkyvsCollaboration = (collaboratingCountries) => {
        // given an array list of two two letter country codes (for example ['ID','SG']) return an object with four values: 
        // { mitigationPotentialAutarky: 210, mitigationPotentialCollaboration: 300, mitigationCostAutarky: 50, mitigationCostCollaboration: 30 }
        return { mitigationPotentialAutarky: 40, mitigationPotentialCollaboration: 70, mitigationCostAutarky: 150, mitigationCostCollaboration: 110 }
    }

    // given the currently selected countries - give the collaboration candidate what it can contribute to the global mitigation (at 50, 100 and 200 $/MtCO2e)
    // this can be calculated by taking the currently selected countries plus the collaboration candidate ; find the mitigation potential values for the combination. then take the potential for the selected countries without the collaboration candidate. the delta is the contribution from the candidate
    // return an object with  values for mitigationPotentialAt50, mitigationPotentialAt100, mitigationPotentialAt200 
    const getMitigationPotentialContributionsForCollaborationCandidate = (selectedCountries, collaborationCandidate) => {
        const data = { mitigationPotentialAt50: '', mitigationPotentialAt100: '', mitigationPotentialAt200: '' }


        data.mitigationPotentialAt50 = -12.3
        data.mitigationPotentialAt100 = (Math.random() * -21.8).toFixed(1);
        data.mitigationPotentialAt200 = (Math.random() * -34.8).toFixed(1);

        // TODO - wait for Aniq to provide data
        /* The current file Example Data\Heatmap\Heatmaps.csv only has heatmap values as a function of country. I shall give you similar values as a function of collaboration.
    
     
    
    For example,
    
     
    
    Country 1             Country 2             Country 3             Country 4             Mitigation_Potential(GtCO2e)_at_50
    
    SG                                                                                                                          10
    
    ID                                                                                                                            50
    
    MY                                                                                                                         60
    
    SG                          ID                                                                                            70
    
    SG                          MY                                                                                         90
    
    ID                            MY                                                                                         120
    
    SG                          ID                            MY                                                         150
    
     
    
    So, if only SG is selected, ID and MY will pop up as potential collaboration options. The values will be the difference between working alone, and working together.
    
     
    
    ID = [SG_ID] – (SG + ID) =  70 – (10 + 50) = 10 GtCO2e
    
    My = [SG_MY] – (SG + MY) = 90 – (10 + 60) = 20 GtCO2e
    
     
    
    If SG and ID are selected, only MY will pop up as potential collaboration option. The value will be:
    
     
    
    MY = [SG_ID_MY] – (SG_ID + MY) = 150 – (70 + 60) = 20 GtCO2e
    */
        return data
    }



    // create an array that contains entries for each individual supported collaboration country set (each combination of countries for which we have a collaboration dat)
    const prepareCountryCollaborations = async () => {
        collaborations = identifyAllCountryCollaborations(collaborationData.value)
    };

    // given an array list of two letter country codes, what are the countries that have a potential collaboration with each of these countries?
    const findCollaboratingCountries = (collaboratingCountries) => {
        const result = [];

        for (const entry of collaborations) { // loop over all combinations of country collaborations (possibly with duplicates)
            let match = true;

            for (const country of collaboratingCountries) { // loop over countries in the current collaboration set; this set can be empty; in the latter case, all countries are candidate and can be selected
                if (!entry.includes(country)) {
                    match = false;
                    break;
                }
            }
            if (match) {
                for (const string of entry) {
                    if (!result.includes(string) && !collaboratingCountries.includes(string)) {
                        result.push(string);
                    }
                }
            }
        }
        return result;
    }


    function deriveCountryKey(countryArray) {
        return countryArray.sort().join('')
    }


    // input
    const prepareCollaborationData = (raw, countries) => {
        // format:
        // [{x: , techA: , techB: }]
        // all technologies should be present in every data object  - 0 if they have no value
        // 

        const data = []

        const countryKey = deriveCountryKey(countries)
        console.log(`countryKey= ${countryKey}`)

        // determine the country key for each record: alphabetical concatenation of country_1..country_4
        // filter on records with the right country key
        // determine all values for technology_name


        const consolidation = {}
        for (const rec of raw) {
            const collaboratingCountries = [rec.country_1, rec.country_2, rec.country_3, rec.country_4]
            const collabKey = deriveCountryKey(collaboratingCountries)

            // console.log(`collabkey ${collabKey}  emissions ${rec.collaboration_emissions} ${rec.technology_cost}`)    
            if (collabKey == countryKey) {
                const x = parseFloat(rec.collaboration_emissions)
                if (!consolidation[x]) consolidation[x] = {}
                consolidation[x][rec["technology_name"]] = parseFloat(rec.technology_cost)
            }
        }
        const uniquePropertyNames = new Set();

        // find the unique technologyNames
        for (let prop in consolidation) {
            if (consolidation.hasOwnProperty(prop)) {
                // Iterate over properties of each property of consolidation
                Object.keys(consolidation[prop]).forEach(innerProp => {
                    uniquePropertyNames.add(innerProp);
                });
            }
        }
        const technologyNames = [...uniquePropertyNames]
        // make sure that all entries in consolidation have entries for all technologies
        for (let prop in consolidation) { // loop over all X values
            // loop over technologyNames
            for (let technology of technologyNames)
                if (!consolidation[prop].hasOwnProperty(technology)) {
                    consolidation[prop][technology] = 0
                }
        }

        //data = []
        for (let prop in consolidation) { // loop over all X values
            consolidation[prop].x = parseFloat(prop)
            data.push(consolidation[prop])
        }
        prepareFakeData(data)
        return data
    }

// return an array of arrays of two letter country codes of potentially collaborating countries
// result can be [['ID','SG'], ['ID','PH'], ['ID','PH,'SG']]
function identifyAllCountryCollaborations(raw) {
    const collaborations = []

    for (const rec of raw) {
        const collaboratingCountries = [rec.country_1, rec.country_2, rec.country_3, rec.country_4]
        const collabKey = deriveCountryKey(collaboratingCountries)
        if (!collaborations.includes(collabKey)) collaborations.push(collabKey)
    }
    return collaborations.map(str => str.match(/.{1,2}/g) || []); // return an array of two letter string arrays
}

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
function deriveCountryKey(countryArray) {
    return countryArray.sort().join('')
}


    return {
        collaborationData,
        fakeDataSet,
        heatmapData,
        heatmapAutarkyData,
        dataSet,
        prepareData,
        prepareCountryCollaborations,
        findCollaboratingCountries,
        combinedCollaborationData,
        prepareCollaborationData,
        getCostOfAchievingMaximumMitigationPotentialInAutarkyvsCollaboration,
        getMitigationPotentialContributionsForCollaborationCandidate,
        getCombinedData,   
        deriveCountryKey,     
    
    };
});

