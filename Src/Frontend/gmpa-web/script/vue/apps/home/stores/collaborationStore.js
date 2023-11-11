/* eslint-disable @typescript-eslint/explicit-function-return-type */
import { defineStore } from 'pinia';
import * as d3 from 'd3';


import fakeRecords from './fakecollaborationdata.json';
import combinedAutarkyAndCollabRecords from './combined_data_autarky.csv';
import combinedAutarkyRecords from './autarky-and-collaboration-data/combined_data_autarky.csv';
import combinedCollaborationRecords from './autarky-and-collaboration-data/combined_data_collaboration.csv';


// these two files contain the finegrained data used for the stacked area chart (mitigation potential diagram) per technology
import totalAutarkyRecords from './autarky-and-collaboration-data/total_data_autarky.csv';
import totalCollaborationRecords from './autarky-and-collaboration-data/total_data_collaboration.csv';

import heatmapCollaborationRecords from './autarky-and-collaboration-data/heatmap_collaboration.csv';
import { ref } from 'vue';

export const useCollaborationStore = defineStore('collaboration', () => {

    const combinedCollaborationData = ref(combinedAutarkyAndCollabRecords); // aggregates per country collaboration set - both autarky and collaboration, both costs and emission reduction

    const fakeDataSet = ref(fakeRecords);

    let processedHeatmapData = false

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

    const getHeatmapData = () => {
        if (!processedHeatmapData) {
            processHeatmapData()
            processedHeatmapData = true
        }
        return heatmapCollaborationData
    }
    const processHeatmapData = () => {
        for (const rec of heatmapCollaborationRecords) {
            const countries = [rec.country_1, rec.country_2, rec.country_3, rec.country_4]
            const countriesKey = deriveCountryKey(countries)
            const countryRecord = {
                "Mitigation_Potential(MtCO2e)": parseFloat(rec["Mitigation_Potential(MtCO2e)"])
                , "Mitigation_Potential(GtCO2e)": parseFloat(rec["Mitigation_Potential(MtCO2e)"])
                , "Mitigation_Potential(GtCO2e)_at_50": parseFloat(rec["Mitigation_Potential_at_Average_50($/tCO2e)"])
                , "Mitigation_Potential(GtCO2e)_at_100": parseFloat(rec["Mitigation_Potential_at_Average_100($/tCO2e)"])
                , "Mitigation_Potential(GtCO2e)_at_200": parseFloat(rec["Mitigation_Potential_at_Average_200($/tCO2e)"])
                , "Mitigation_Cost($/tCO2e)": parseFloat(rec["Mitigation_Cost($/tCO2e)"])
                , "Mitigation_Cost($/GtCO2e)": parseFloat(rec["Mitigation_Cost($/tCO2e)"])
                , "Mitigation_Potential_at_Average_50($/tCO2e)": parseFloat(rec["Mitigation_Potential_at_Average_50($/tCO2e)"])
                , "BAU_Emissions(MtCO2e)": parseFloat(rec["BAU_Emissions(MtCO2e)"])
                // here are some better names for the heatmap properties

                , "Mitigation_Potential": parseFloat(rec["Mitigation_Potential(MtCO2e)"])
                , "Mitigation_Potential_at_0": parseFloat(rec["Mitigation_Potential_at_Average_0($/tCO2e)"])
                , "Mitigation_Potential_at_50": parseFloat(rec["Mitigation_Potential_at_Average_50($/tCO2e)"])
                , "Mitigation_Potential_at_100": parseFloat(rec["Mitigation_Potential_at_Average_100($/tCO2e)"])
                , "Mitigation_Potential_at_200": parseFloat(rec["Mitigation_Potential_at_Average_200($/tCO2e)"])
                , "Mitigation_Cost": parseFloat(rec["Mitigation_Cost($/tCO2e)"])
                , "Mitigation_Potential_at_Average_50": parseFloat(rec["Mitigation_Potential_at_Average_50($/tCO2e)"])
                , "BAU_Emissions": parseFloat(rec["BAU_Emissions(MtCO2e)"])
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



    const getCostOfAchievingMaximumMitigationPotentialInAutarkyvsCollaboration = (collaboratingCountries) => {
        const data = {}
        // used for horizontal level gauge in benefits panel
        const heatmapData = getHeatmapData()
        // given an array list of two two letter country codes (for example ['ID','SG']) return an object with these values: 
        // { mitigationPotentialAutarkyAt0 (50, 100, 200): 210, mitigationPotentialCollaborationAt0 (50,100,200): 300
        // , mitigationPotentialCollaborationMax:300 }

        // get country key for selectedCountries 
        const countriesKey = deriveCountryKey(collaboratingCountries.map((country) => country.properties.iso_a2))

        const levels = ['0', '50', '100', '200']
        for (const level of levels) {
            // for every individual country, get Mitigation_Potential_at_Average_<current value> ($/tCO2e) values for country from heatmap_collaboration 
            // add the individual country values together to get the autarky value
            let Mitigation_Potential_sum = 0
            for (const country of collaboratingCountries) {
                Mitigation_Potential_sum += heatmapData[country.properties.iso_a2][`Mitigation_Potential_at_${level}`]
            }
            data[`mitigationPotentialAutarkyAt${level}`] = Mitigation_Potential_sum
            data[`mitigationPotentialCollaborationAt${level}`] = parseFloat(heatmapData[countriesKey][`Mitigation_Potential_at_${level}`])            
        }
        // BAU_Emissions(MtCO2e) values for countrykey from heatmap_collaboration 
        // this latter value (BAU) provides the maximum value displayed at the far right of the horizontal gauge
        // TODO BAU should always be there!!
        data['mitigationPotentialCollaborationMax'] = heatmapData[countriesKey]["BAU_Emissions"]?parseFloat(heatmapData[countriesKey]["BAU_Emissions"]):1.5*data[`mitigationPotentialCollaborationAt200`]
        return data
    }

    // given the currently selected countries - give the collaboration candidate what it can contribute to the global mitigation (at 50, 100 and 200)
    // Benefit (contribution for collaboration candidate) = (New Collaboration)– { (Old Collaboration)+ (new country)}
    // or: {selectedCountries + candidate} - {selectedCountries} - {candidate in autarky}

    // this can be calculated by taking the currently selected countries plus the collaboration candidate 
    // - A find the mitigation potential values for the combination. 
    // - B then take the potential for the selected countries without the collaboration candidate. 
    // - C and find the potential for the candidate by itself
    // the delta (A - B - C) is the contribution from the candidate
    // return an object with  values for mitigationPotentialAt50, mitigationPotentialAt100, mitigationPotentialAt200 
    const getMitigationPotentialContributionsForCollaborationCandidate = (selectedCountries, collaborationCandidate) => {

        // get country key for selectedCountries 
        // get mitigation potential values for countrykey from heatmap_collaboration 
        const selectedCountriesIsoa2Array = selectedCountries.map((country) => country.properties.iso_a2)
        const selectedCountriesKey = deriveCountryKey(selectedCountriesIsoa2Array)

        const properties = ["50", "100", "200"]
        const data = {}

        for (const property of properties) {

            const mpSelected = getHeatmapData()[selectedCountriesKey][`Mitigation_Potential_at_${property}`]

            // get country key for selectedCountries plus collaborationCandidate
            // get mitigation potential values for countrykey from heatmap_collaboration 
            const collaboratingCountriesIsoa2Array = [...selectedCountriesIsoa2Array]
            collaboratingCountriesIsoa2Array.push(collaborationCandidate.properties.iso_a2)
            const collaboratingCountriesKey = deriveCountryKey(collaboratingCountriesIsoa2Array)
            const mpCollaborating = getHeatmapData()[collaboratingCountriesKey][`Mitigation_Potential_at_${property}`]

            // get mitigation potential values for just the candidate country from heatmap_collaboration 
            const mpCountry = getHeatmapData()[collaborationCandidate.properties.iso_a2][`Mitigation_Potential_at_${property}`]
            // subtract the latter two from the first; 
            data[`mitigationPotentialAt${property}`] = mpCollaborating - mpSelected - mpCountry
        }

        return data
    }



    // create an array that contains entries for each individual supported collaboration country set (each combination of countries for which we have an entry in heatmap_collaboration.csv)
    const prepareCountryCollaborations = async () => {
        collaborations = identifyAllCountryCollaborations(heatmapCollaborationRecords)
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

    function deriveCountryKey(countryArray) {
        return countryArray.sort().join('')
    }


    return {
        fakeDataSet,
        dataSet,
        prepareCountryCollaborations,
        findCollaboratingCountries,
        combinedCollaborationData,
        getCostOfAchievingMaximumMitigationPotentialInAutarkyvsCollaboration,
        getMitigationPotentialContributionsForCollaborationCandidate,
        getCombinedData,
        deriveCountryKey,
        getTotalData,
        getHeatmapData,

    };
});

