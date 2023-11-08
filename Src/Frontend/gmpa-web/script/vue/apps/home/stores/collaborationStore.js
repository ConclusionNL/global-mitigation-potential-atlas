/* eslint-disable @typescript-eslint/explicit-function-return-type */
import { defineStore } from 'pinia';
import * as d3 from 'd3';

import collabRecords from './collaborationdata.csv';
import fakeRecords from './fakecollaborationdata.json';
import combinedAutarkyAndCollabRecords from './combined_data_autarky.csv';
import combinedAutarkyRecords from './autarky-and-collaboration-data/combined_data_autarky.csv';
import combinedCollaborationRecords from './autarky-and-collaboration-data/combined_data_collaboration.csv';


// these two files contain the finegrained data used for the stacked area chart (mitigation potential diagram) per technology
import totalAutarkyRecords from './autarky-and-collaboration-data/total_data_autarky.csv';
import totalCollaborationRecords from './autarky-and-collaboration-data/total_data_collaboration.csv';


import heatmapRecords from './heatmaps.csv';
import heatmapAutarkyRecords from './autarky-and-collaboration-data/heatmap_autarky.csv';
import heatmapCollaborationRecords from './autarky-and-collaboration-data/heatmap_collaboration.csv';
import { ref } from 'vue';

export const useCollaborationStore = defineStore('collaboration', () => {
    const collaborationData = ref(collabRecords); // details per technology and per country collaboration set
    const combinedCollaborationData = ref(combinedAutarkyAndCollabRecords); // aggregates per country collaboration set - both autarky and collaboration, both costs and emission reduction

    const fakeDataSet = ref(fakeRecords);
    const heatmapData = ref(heatmapRecords);
    // TODO preprocess heatmap records to create a map with countrieskey as key
    let processedHeatmapData = false
    const heatmapAutarkyData = {};
    const heatmapCollaborationData = {};
    let dataSet = ref([]);
    let collaborations; // an array that contains string arrays with country codes for collaborating countries. each entry looks like [countryA], [countryB], ... (with up to four countries)
    let processedCombinedData = false, processedTotalData = false
    let combinedData, totalData = {}

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

    // total data is an object with country keys as property names (keys) and objects with two properties: 
    // collaboration and autarky. These are both arrays of objects. The properties of these objects are all the same:
    // - x - the value for the emission reduction potential 
    // - one property for each technology that has the cost associated with the x value for that technology
    // { 'ID': {collaboration: [{x: 1200, Coal Power Plant:0.076,Electricity Distribution:0.215, Floating Solar PV:23 }, {x: 2400, Coal Power Plant:0.176,Electricity Distribution:2.15, Floating Solar PV:38 }]
    //        , autarky: [{x: 1200, Coal Power Plant:0.12,Electricity Distribution:0.815, Floating Solar PV:11 }, {x: 2400, Coal Power Plant:8.176,Electricity Distribution:1.15, Floating Solar PV:12 }]}, 'SG': {} }
    const getTotalData = () => {
        if (!processedTotalData) {
            prepareTotalDataFromFiles()
            processedTotalData = true
        }
        return totalData
    }



    const prepareTotalDataFromFiles = () => {
        function prepareTotalDataFromRecords(collaborationOrAutarky, records) {
            const data = {}
            for (const rec of records) {
                const countries = [rec.country_1, rec.country_2, rec.country_3, rec.country_4]
                const countriesKey = deriveCountryKey(countries)
                if (!data[countriesKey]) {
                    data[countriesKey] = {}
                }
                // create a map for each country with the x value (collaboration_emissions as key and all technologies as properties)
                const x = parseFloat(rec.collaboration_emissions)
                if (!data[countriesKey][x]) data[countriesKey][x] = {}
                data[countriesKey][x][rec["technology_name"]] = parseFloat(rec.technology_cost)
            }
            // loop over all country combinations (all keys/properties in data )
            // - find per combination the set of all technologies that occur, even if only once for one x value
            // - make sure that all maps (one for each x value) contain entries for all technologies
            for (let countryCombination in data) {

                const uniquePropertyNames = new Set();

                // find the unique technologyNames
                for (let prop in data[countryCombination]) {  // this prop refers to x values or emissions
                    if (data[countryCombination].hasOwnProperty(prop)) {
                        // Iterate over properties of each property of data[countryCombination]
                        Object.keys(data[countryCombination][prop]).forEach(technologyName => { //technologyName refers to technology name
                            uniquePropertyNames.add(technologyName);
                        });
                    }
                }
                const technologyNames = [...uniquePropertyNames]
                // make sure that all entries in consolidation have entries for all technologies
                for (let prop in data[countryCombination]) { // loop over all X values
                    // loop over technologyNames
                    let sum = 0
                    for (let technology of technologyNames) {
                        if (!data[countryCombination][prop].hasOwnProperty(technology)) {
                            data[countryCombination][prop][technology] = 0
                        } else sum += data[countryCombination][prop][technology]
                    }
                    data[countryCombination][prop]["sum"] = sum
                }

                const dataArray = []
                for (let prop in data[countryCombination]) { // loop over all X values               
                    data[countryCombination][prop].x = parseFloat(prop)
                    dataArray.push(data[countryCombination][prop])
                }
                dataArray.sort((a, b) => d3.ascending(a.x, b.x));


                if (!totalData[countryCombination]) {
                    totalData[countryCombination] = { "collaboration": [], "autarky": [] }
                }
                totalData[countryCombination][collaborationOrAutarky] = dataArray
            }

        }
        prepareTotalDataFromRecords("autarky", totalAutarkyRecords)
        prepareTotalDataFromRecords("collaboration", totalCollaborationRecords)
        console.log('done')
    }

    const processHeatmapData = () => {
        for (const rec of heatmapAutarkyRecords) {
            const countries = [rec.country_1, rec.country_2, rec.country_3, rec.country_4]
            const countriesKey = deriveCountryKey(countries)
            const countryRecord = {
                "Mitigation_Potential(MtCO2e)": rec["Mitigation_Potential(MtCO2e)"]
                , "Mitigation_Cost($/tCO2e)": rec["Mitigation_Cost($/tCO2e)"]
            }
            heatmapAutarkyData[countriesKey] = countryRecord
        }
        for (const rec of heatmapCollaborationRecords) {
            const countries = [rec.country_1, rec.country_2, rec.country_3, rec.country_4]
            const countriesKey = deriveCountryKey(countries)
            const countryRecord = {
                "Mitigation_Potential(MtCO2e)": rec["Mitigation_Potential(MtCO2e)"]
                , "Mitigation_Cost($/tCO2e)": rec["Mitigation_Cost($/tCO2e)"]
            }
            heatmapCollaborationData[countriesKey] = countryRecord
        }


    }

    const prepareCombinedDataFromFiles = () => {
        const data = {}

        // determine the country key for each record: alphabetical concatenation of country_1..country_4
        const collaborationProperties = ['emissions', 'cost']
        for (const rec of combinedCollaborationRecords) {
            const collaboratingCountries = [rec.country_1, rec.country_2, rec.country_3, rec.country_4]
            const collabKey = deriveCountryKey(collaboratingCountries)
            if (!data[collabKey]) { data[collabKey] = { "collaboration": [], "autarky": [] } }

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
            if (!data[combinedKey]) { data[combinedKey] = { "collaboration": [], "autarky": [] } }

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
            dataSet.value = prepareTotalCollaborationData(collaborationData.value, countries);
        }
        return dataSet.value;
    };

    const getCostOfAchievingMaximumMitigationPotentialInAutarkyvsCollaboration = (collaboratingCountries) => {
        // used for horizontal level gauge in benefits panel
        if (!processedHeatmapData) {
            processHeatmapData()
            processedHeatmapData = true
        }
        // given an array list of two two letter country codes (for example ['ID','SG']) return an object with four values: 
        // { mitigationPotentialAutarky: 210, mitigationPotentialCollaboration: 300, mitigationCostAutarky: 50, mitigationCostCollaboration: 30 }
        // get country key for selectedCountries 
        const countriesKey = deriveCountryKey(collaboratingCountries.map((country) => country.properties.iso_a2))
        // get Mitigation_Potential(MtCO2e),Mitigation_Cost($/tCO2e) values for countrykey from heatmap_collaboration and from heatmap_autarky
        // set object properties based on values  

        return {
            mitigationPotentialAutarky: parseFloat(heatmapAutarkyData[countriesKey]["Mitigation_Potential(MtCO2e)"])
            , mitigationPotentialCollaboration: parseFloat(heatmapCollaborationData[countriesKey]["Mitigation_Potential(MtCO2e)"])
            , mitigationCostAutarky: parseFloat(heatmapAutarkyData[countriesKey]["Mitigation_Cost($/tCO2e)"]).toPrecision(3)
            , mitigationCostCollaboration: parseFloat(heatmapCollaborationData[countriesKey]["Mitigation_Cost($/tCO2e)"]).toPrecision(3)
        }
    }

    // given the currently selected countries - give the collaboration candidate what it can contribute to the global mitigation (at 50, 100 and 200 $/MtCO2e)
    // this can be calculated by taking the currently selected countries plus the collaboration candidate ; find the mitigation potential values for the combination. then take the potential for the selected countries without the collaboration candidate. the delta is the contribution from the candidate
    // return an object with  values for mitigationPotentialAt50, mitigationPotentialAt100, mitigationPotentialAt200 
    const getMitigationPotentialContributionsForCollaborationCandidate = (selectedCountries, collaborationCandidate) => {
        // get country key for selectedCountries plus collaborationCandidate
        // get mitigation potential values for countrykey from heatmap_collaboration and from heatmap_autarky
        // subtract the latter from the former; 

        const data = { mitigationPotentialAt50: '', mitigationPotentialAt100: '', mitigationPotentialAt200: '' }


        data.mitigationPotentialAt50 = -12.3
        data.mitigationPotentialAt100 = (Math.random() * -21.8).toFixed(1);
        data.mitigationPotentialAt200 = (Math.random() * -34.8).toFixed(1);


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
    const prepareTotalCollaborationData = (raw, countries) => {
        getTotalData()
        // format:
        // [{x: , techA: , techB: }]
        // all technologies should be present in every data object  - 0 if they have no value
        // 

        const data = []

        const countryKey = deriveCountryKey(countries)

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
                Object.keys(consolidation[prop]).forEach(technologyName => {
                    uniquePropertyNames.add(technologyName);
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
        getCostOfAchievingMaximumMitigationPotentialInAutarkyvsCollaboration,
        getMitigationPotentialContributionsForCollaborationCandidate,
        getCombinedData,
        deriveCountryKey,
        getTotalData,

    };
});

