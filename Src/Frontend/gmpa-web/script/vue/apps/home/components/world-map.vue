<template>
    <div id="mapcontainer">
        <div class="search-container">
            <searchBar
                class="search-box"
                @country-searched="handleSearch"
                :countries="props.countries" />
        </div>
        <toggleBox class="toggle-box" @mitigation-value="handleMitigation" />
        <countryCard
            v-if="selectedCountry && !inCollabMode"
            :selected-country="selectedCountry"
            class="country-box"
            @country-closed="handleUnselectCard"
            @collab-mode="handleCollabMode" />
        <collabCard
            v-if="inCollabMode && selectedCountries && selectedCountries.length > 0"
            class="countries-box"
            :countries-list="selectedCountries"
            @country-closed="handleUnselectCollab" />
    </div>
</template>

<script setup>
import * as d3 from 'd3';
import * as topojson from 'topojson';
import { geoAlbers, geoEquirectangular, geoEqualEarth } from 'd3-geo';
import { scaleSequential } from 'd3-scale';
import { useCollaborationStore } from '../stores/collaborationStore';
import { ref, watch, onMounted, defineProps, defineEmits, computed } from 'vue';
import toggleBox from './toggle-box.vue';
import countryCard from './country-card.vue';
import collabCard from './collaboration-card.vue';
import searchBar from './searchbar.vue';

const emit = defineEmits(['country-clicked']);

let svg, g, countryDataSet, pathGenerator, zoooom;

const collaborationStore = useCollaborationStore();
const heatmapData = collaborationStore.heatmapData;

const mitigation = ref('None');
const selectedCountries = ref([]);
const selectedCountry = ref('');

const width = window.innerWidth - 200;
const height = window.innerHeight - 86;

const inCollabMode = ref(false);

const props = defineProps({
    countries: {},
});

const handleMitigation = (mitigationVal) => {
    mitigation.value = mitigationVal;
};

const handleSearch = (country) => {
    const d = countryDataSet.features.find((c) => c.properties.name === country.Name);
    const countrySelection = getCountryNodes().filter((c) => c.properties.name === country.Name);

    selectedCountry.value = d;
    toggleCountrySelection(d, countrySelection);
    // zoomToCountry(null, countrySelection);
};

const handleUnselectCollab = (country) => {
    unmarkCollabCountry(country);
    selectedCountries.value = selectedCountries.value.filter((c) => c !== country);
    if (selectedCountries.value.length < 1) {
        inCollabMode.value = false;
        selectedCountry.value = '';
        zoomToScale(1);
    }
};

const handleUnselectCard = () => {
    unmarkAllSelectedCountries();
    selectedCountries.value = [];
    selectedCountry.value = '';
    zoomToScale(1);
};

const handleCollabMode = () => {
    inCollabMode.value = true;
    markCollabCountry(selectedCountry.value);
};

onMounted(() => {
    watch(mitigation, (newValue) => {
        // WHAT SHOULD HAPPEN WHEN THE TOGGLEBOX IS UPDATED?
        // redefine colorScale2
        // redraw countries
        // redefine color legend

        colorScale2 = createColorScaleForHeatmapProperty(heatmapData, mitigation.value);
        yAxisScale = createYAxisScaleForHeatmapProperty(heatmapData, mitigation.value);

        drawAllCountries(countryDataSet);
        console.log(`New mitigation value: ${newValue}`);
        drawVerticalAxis();
    });

    const t0 = { k: width / 2 / Math.PI, x: width / 2, y: height / 2 };
    svg = d3
        .select('#mapcontainer')
        .append('svg')
        .attr('width', width)
        .attr('height', height)
        .attr('preserveAspectRatio', 'xMinYMin')
        .style('background', '#8ab5f9');

    //const projection = d3.geoNaturalEarth1().translate([t0.x, t0.y]).scale(t0.k);
    const projection = d3
        .geoEquirectangular()
        .rotate([-148, 0]) // rotate sets the spherical rotation angles. The default rotation is [0, 0], which centers the map on Greenwich (0° longitude). By adjusting the first value (longitude), you can center the map on a different region.
        .translate([t0.x, t0.y])
        .scale(t0.k);
    //const projection = d3.geoAlbers().translate([t0.x, t0.y]).scale(t0.k);

    pathGenerator = d3.geoPath().projection(projection);

    g = svg.append('g');

    console.log();

    const colorLegendG = svg.append('g').attr('transform', `translate(40,310)`);

    const heatmapLegendG = svg.append('g').attr('transform', `translate(-10,470)`);

    const colorScale = d3.scaleOrdinal();
    //      const colorValue = (d) => d.properties.economy;
    //      const colorValue = (d) => d.properties.continent ;
    //      const colorValue = (d) => d.properties.income_grp ;
    const colorValue = function (d) {
        const nameLength = d.properties.name.length;
        const nameLengthCategory = nameLength < 6 ? 'Short' : 'Long' + ' ' + nameLength;
        return d.properties.continent;
    };

    let colorScale2, yAxisScale;

    function findMinMax(collection, theProperty) {
        const values = collection.map((item) => item[theProperty]);
        return {
            min: Math.min(...values),
            max: Math.max(...values),
        };
    }
    function createColorScaleForHeatmapProperty(heatmapData, property) {
        const result = findMinMax(heatmapData, property);
        return scaleSequential(d3.interpolateBlues).domain([result.min, result.max]);
    }
    colorScale2 = createColorScaleForHeatmapProperty(heatmapData, mitigation.value);

    function createYAxisScaleForHeatmapProperty(heatmapData, property) {
        const result = findMinMax(heatmapData, property);
        return d3.scaleLinear().domain([result.min, result.max]).range([300, 0]); // Adjust the range to match the desired height of your axis
    }

    yAxisScale = createYAxisScaleForHeatmapProperty(heatmapData, mitigation.value);

    const loadAndProcessData = () =>
        Promise.all([
            d3.tsv('https://unpkg.com/world-atlas@1.1.4/world/50m.tsv'),
            d3.json('https://unpkg.com/world-atlas@1.1.4/world/50m.json'),
        ]).then(([tsvData, topoJSONdata]) => {
            const rowById = tsvData.reduce((accumulator, d) => {
                accumulator[d.iso_n3] = d;
                return accumulator;
            }, {});

            const countries = topojson.feature(topoJSONdata, topoJSONdata.objects.countries);

            countries.features.forEach((d) => {
                // add all country properties from the TSV file to the features of the countries
                Object.assign(d.properties, rowById[d.id]);
                const nameLength = parseInt(d.properties.name_len);
                d.properties['nameLengthCategory'] =
                    nameLength < 6 ? 'Short' : 'Long' + ' ' + nameLength;

                // using the ISo2 country code (iso_a2), check heatmapData array for an object with the right COUnTRY property value
                const countryCode = d.properties.iso_a2;
                heatmapData
                    .filter((h) => h['Country'] == countryCode)
                    .forEach((c) => {
                        // copy properties from c to d.properties
                        for (let key in c) {
                            if (key !== 'Country') {
                                d.properties[key] = c[key];
                            }
                        }
                    });

                // todo - these properties are added in a not very efficient way
                // "Mitigation_Potential(GtCO2e)":"234","Mitigation_Cost($/GtCO2e)":"5","Mitigation_Potential(GtCO2e)_at_50":"234","Mitigation_Potential(GtCO2e)_at_100":"250","Mitigation_Potential(GtCO2e)_at_200":"300"}
            });

            return countries;
        });

    const heatmapLegend = (selection, props) => {
        const { spacing, textOffset, backgroundRectWidth } = props;

        const backgroundRect = selection.selectAll('rect').data([null]);

        backgroundRect
            .enter()
            .append('rect')
            .merge(backgroundRect)
            .attr('x', 10 * 2)
            .attr('y', 5)
            .attr('rx', 10 * 2)
            .attr('width', backgroundRectWidth)
            .attr('fill', 'white')
            .attr('height', 350);
    };

    loadAndProcessData().then((countries) => {
        countryDataSet = countries;
        // "Mitigation_Potential(GtCO2e)":"234","Mitigation_Cost($/GtCO2e)":"5","Mitigation_Potential(GtCO2e)_at_50":"234","Mitigation_Potential(GtCO2e)_at_100":"250","Mitigation_Potential(GtCO2e)_at_200":"300"}
        // now we can determine - depending on the toggle that indicates which category of these data properties should be used for the heatmap
        // the color scale - get min and max for the desired property from the heatmap data

        colorScale
            .domain(countries.features.map(colorValue))
            .domain(colorScale.domain().sort().reverse())
            .range(d3.schemeSpectral[colorScale.domain().length]);
        heatmapLegendG.call(heatmapLegend, {
            spacing: 30,
            textOffset: 15,
            backgroundRectWidth: 100,
        });
        drawHeatmapLegend();
        drawAllCountries(countries);
    });

    let countryNodes = [];

    function drawHeatmapLegend() {
        // Define gradient
        const gradient = svg
            .append('defs')
            .append('linearGradient')
            .attr('id', 'gradient')
            .attr('x1', '0%')
            .attr('x2', '0%')
            .attr('y1', '100%')
            .attr('y2', '0%');

        // Define stops for the gradient based on the color scale
        for (let i = 0; i <= 1; i += 0.1) {
            gradient
                .append('stop')
                .attr('offset', `${i * 100}%`)
                .attr('stop-color', d3.interpolateBlues(i));
        }

        // Add rectangle with gradient fill
        svg.append('rect')
            .attr('x', 40)
            .attr('y', 0)
            .attr('width', 30)
            .attr('height', 300)
            .style('fill', 'url(#gradient)')
            .attr('transform', 'translate(0, 500)');

        // Draw the vertical axis using the scaleLinear
        drawVerticalAxis();
    }
    function removeElementIfExists(id) {
        const existing = svg.select(`#${id}`);
        if (!existing.empty()) {
            existing.remove();
        }
    }

    function drawVerticalAxis() {
        const yAxis = d3.axisRight(yAxisScale).ticks(5);

        removeElementIfExists('legend-axis');

        svg.append('g')
            .attr('id', 'legend-axis')
            .attr('transform', 'translate(75, 500) scale(1)') // Position the axis; adjust as needed
            .call(yAxis);
        removeElementIfExists('legend-axis-title');
        svg.append('text')
            .attr('id', 'legend-axis-title')
            .attr('transform', 'rotate(-90)') // Rotate the text for vertical axis
            .attr('y', 13) // Position it 40 pixels to the left of the axis
            .attr('x', -660) // Position it at the middle of the axis
            .attr('dy', '1em') // Adjustments for positioning
            .style('text-anchor', 'middle') // Center the text
            .text(mitigation.value);
    }

    function drawAllCountries(countries) {
        // draw all countries
        countryNodes = g.selectAll('path').data(countries.features);
        countryNodes
            // fill with gray (#dcdcdc) when the country's data is unknown
            .attr('fill', (d) =>
                d.properties.hasOwnProperty(mitigation.value)
                    ? colorScale2(d.properties[mitigation.value])
                    : '#dcdcdc'
            )
            .select('title') // Select the child title of each path
            .text(
                (d) =>
                    d.properties.name +
                    ' : ' +
                    (d.properties.hasOwnProperty(mitigation.value)
                        ? d.properties[mitigation.value] + ` ${mitigation.value}`
                        : '')
            );

        countryNodes
            .enter()
            .append('path')
            .attr('d', pathGenerator)
            // fill with gray (#dcdcdc) when the country's data is unknown
            .attr('fill', (d) =>
                d.properties.hasOwnProperty(mitigation.value)
                    ? colorScale2(d.properties[mitigation.value])
                    : '#dcdcdc'
            )
            .on('mouseover', handleMouseOver)
            .on('mouseleave', handleMouseLeave)
            .on('click', handleCountryClick)
            .append('title')
            .text(
                (d) =>
                    d.properties.name +
                    ' : ' +
                    (d.properties.hasOwnProperty(mitigation.value)
                        ? d.properties[mitigation.value] + ` ${mitigation.value}`
                        : '')
            )
            .attr('class', 'country');

        zoooom = d3
            .zoom()
            .scaleExtent([1, 5]) // Set the zoom extent
            .on('zoom', zoomed);

        // Attach the zoom behavior to the SVG
        svg.call(zoooom);

        // // Let the zoom take care of modifying the projection:
        // // thanks to https://stackoverflow.com/questions/62228556/reactjs-d3-how-to-zoom-in-d3-geo-world-map
        // zoom = d3.zoom()
        //   .on("zoom", function () {
        //     g.attr("transform", d3.zoomTransform(this))
        //     countryNodes.style("stroke-width", 1 / d3.zoomTransform(this).k); // update stroke width.
        //   })

        // svg.call(zoom);
    }

    function writeHTMLInLegend(htmlContent) {
        // Define the legend's dimensions and position
        const legendWidth = 370;
        const legendHeight = 180;
        const legendX = 1100; // X position
        const legendY = 600; // Y position

        // Select the SVG
        const svg = d3.select('svg');

        // Check if the legend rectangle already exists
        let legendRect = svg.select('.legend-rect');
        if (legendRect.empty()) {
            // If it doesn't exist, append it
            legendRect = svg
                .append('rect')
                .attr('x', legendX)
                .attr('y', legendY)
                .attr('width', legendWidth)
                .attr('height', legendHeight)
                .attr('fill', '#f5f5f5') // Light gray background
                .attr('stroke', '#000'); // Black border
        }

        // Remove any existing foreignObject in the legend
        svg.select('.legend-html').remove();

        // Append the foreignObject to the SVG
        const foreign = svg
            .append('foreignObject')
            .attr('class', 'legend-html')
            .attr('x', legendX + 5)
            .attr('y', legendY + 5)
            .attr('width', legendWidth)
            .attr('height', legendHeight);

        // Append the HTML content to the foreignObject
        foreign
            .append('xhtml:div')
            .style('font-family', 'Arial')
            .style('font-size', '14px')
            .html(htmlContent);
    }
});

// Function to handle zooming
// zooming applies to all paths in a specific group - that includes countries but not legend etc
function zoomed(event) {
    g.selectAll('path').attr('transform', event.transform); // Apply the zoom transform to map elements
}
// Function to zoom to a specific country
function zoomToCountry(event, d) {
    // Calculate the bounding box of the selected feature
    const bounds = pathGenerator.bounds(d);
    const dx = bounds[1][0] - bounds[0][0];
    const dy = bounds[1][1] - bounds[0][1];
    const x = (bounds[0][0] + bounds[1][0]) / 2;
    const y = (bounds[0][1] + bounds[1][1]) / 2;
    const scale = Math.max(1, Math.min(3, 0.9 / Math.max(dx / width, dy / height)));

    // Transition to the selected feature's position and scale
    svg.transition()
        .duration(750)
        .call(
            zoooom.transform,
            d3.zoomIdentity
                .translate(width / 2, height / 2)
                .scale(scale)
                .translate(-x, -y)
        );
}

function zoomToScale(scale) {
    svg.transition()
        .duration(750)
        .call(zoooom.transform, d3.zoomIdentity.translate(0, 0).scale(scale));
}

function getCountryNodes() {
    return g.selectAll('path').data(countryDataSet.features);
}

function markCollabCountry(country) {
    getCountryNodes()
        .filter((c) => country.id === c.id)
        .classed('selected-country', false)
        .classed('selected-country-collab', true);
}

function unmarkAllSelectedCountries() {
    getCountryNodes()
        .filter((d) => selectedCountries.value.id !== d.id)
        .classed('selected-country', false);
}

function unmarkCollabCountry(country) {
    getCountryNodes()
        .filter((c) => country.id === c.id)
        .classed('selected-country-collab', false);
}

// Add the hover effect on mouse over
function handleMouseOver() {
    d3.select(this).classed('hover-country', true);
}

// Function to handle mouse leave
function handleMouseLeave() {
    d3.select(this).classed('hover-country', false);
}

function handleCountryClick(event, d) {
    if (!inCollabMode.value) zoomToCountry(event, d);
    toggleCountrySelection(d, d3.select(this));

    emit('country-clicked', d);
}

// Function to toggle country selection
function toggleCountrySelection(d, countryPath) {
    if (selectedCountry.value === d || selectedCountries.value.includes(d)) {
        return;
    }

    if (inCollabMode.value) {
        countryPath.classed('selected-country', false);
        countryPath.classed('selected-country-collab', true);
    } else {
        unmarkAllSelectedCountries();
        selectedCountries.value = [];
        selectedCountry.value = d;
        countryPath.classed('selected-country-collab', false);
        countryPath.classed('selected-country', true);
    }

    selectedCountries.value.push(d);
}
</script>

<style>
.country {
    stroke: black;
    stroke-width: 0.05px;
}

.hover-country {
    stroke: lightblue;
    stroke-width: 1.75px;
}

/* Style for selected country */
.selected-country {
    fill: #95ad28;
    stroke: white;
    stroke-width: 1px;
}

.selected-country-collab {
    fill: #f07004;
    stroke: white;
    stroke-width: 1px;
}

.suggested-country-collab {
    fill: #f07004;
    fill-opacity: 0.6;
    stroke: #f07004;
    stroke-width: 1px;
}

.tick text {
    font-size: 1em;
    font-family: sans-serif;
    fill: black;
}

rect {
    opacity: 0.7;
}

p {
    padding-left: 10px;
}

/* CSS styles for hiding the country details box by default */
.hidden {
    display: none;
    /* Add any other styling you need */
}

/* CSS styles for positioning the box at the lower-right corner */
#country-details {
    position: fixed;
    bottom: 10px;
    right: 10px;
    background-color: white;
    border: 1px solid #ccc;
    padding: 10px;
    /* Add any other styling you need */
}

.search-box {
    position: absolute;
    top: 136px;
}

.search-container {
    display: flex;
    justify-content: center;
}

.toggle-box {
    position: absolute;
    top: 250px;
    margin-left: 10px;
}

.country-box {
    position: absolute;
    top: 400px;
    right: 80px;
}

.countries-box {
    position: absolute;
    bottom: 20px;
    margin-left: 120px;
}
</style>
