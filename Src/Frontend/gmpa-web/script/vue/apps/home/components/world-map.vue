<template>
    <div id="mapcontainer">
        <div class="search-container">
            <searchBar class="search-box" :countries="props.countries" />
        </div>
        <toggleBox class="toggle-box" @mitigation-value="handleMitigation" />
        <countryCard v-if="selectedCountries[0] && !inCollabMode" class="country-box" @countryNavigation="navigateToCountry"
            @countryMitigationPotentialDiagram="stackedAreaModalVisible = true" />
        <collabCard v-if="inCollabMode && selectedCountries && selectedCountries.length > 0" class="countries-box"
            :collaboration-candidates-list="findCollaboratingCandidates(selectedCountries)"
            @show-benefits="stackedAreaModalVisible = true" @countryNavigation="navigateToCountry" />
        <div class="zoom-box">
            <div class="zoom-flex">
                <div @click="zoomIn()
                    " class="r-btn plus">
                    <plusIcon width="24" height="24" />
                </div>
                <div @click="zoomOut()
                    " class="r-btn minus">
                    <minusIcon width="24" height="4" />
                </div>
            </div>
        </div>
        <div class="modal-diagram" v-if="stackedAreaModalVisible">
            <div class="modal-diagram-content">
                <closeIcon width="24" height="24" class="close-icon" @click="closeModal" />
                <mitigationPotentialDiagram :countries-list="selectedCountries" @technologySelected="
                    console.log(
                        `technology bar selected in mitigation potential diagram (bar chart)`
                    )
                    " />
            </div>
        </div>
    </div>
</template>

<script setup>
import * as d3 from 'd3';
import * as topojson from 'topojson';
import { geoAlbers, geoEquirectangular, geoEqualEarth } from 'd3-geo';
import { scaleSequential } from 'd3-scale';
import { useCollaborationStore } from '../stores/collaborationStore';
import { useCountriesDataStore } from '../stores/countriesDataStore';
import { useSelectedCountries } from '../composables/useSelectedCountries';
import { ref, watch, onMounted, defineProps, defineEmits, computed } from 'vue';
import toggleBox from './toggle-box.vue';
import countryCard from './country-card.vue';
import collabCard from './collaboration-card.vue';
import searchBar from './searchbar.vue';
import plusIcon from '../assets/plus.svg';
import minusIcon from '../assets/minus.svg';
import closeIcon from '../assets/cross.svg';
import mitigationPotentialDiagram from './mitigation-potential-diagram.vue';

const useCountries = useSelectedCountries();
const countriesDataStore = useCountriesDataStore();
const selectedCountries = useCountries.selectedCountries;
const inCollabMode = useCountries.inCollabMode;

const stackedAreaModalVisible = ref(false);
const closeModal = () => (stackedAreaModalVisible.value = false);

// const emit = defineEmits(['country-clicked']);

let svg, countriesGroup, countryDataSet, pathGenerator, zoooom;

const collaborationStore = useCollaborationStore();
const heatmapData = collaborationStore.getHeatmapData();

const mitigation = ref('None');

const maxScaleFactor = 6

const width = window.innerWidth - 200;
const height = window.innerHeight - 86;

const props = defineProps({
    countries: {},
});

const navigateToCountry = (countryNavigationEvent) => {
    if (countryNavigationEvent && countryNavigationEvent.properties) {
        const countryNameWithDashes = countryNavigationEvent.properties.name
            .replace(/'/g, '')       // Remove single quotes
            .replace(/%20/g, '-')     // Replace '%20' with '-'
            .replace(/\s+/g, '-');    // Replace spaces with '-'


        window.location.href = `/Countries/${countryNameWithDashes}`;
    } else {
        console.log('Invalid countryNavigationEvent object or missing properties.');
    }
};

function highlightCollaborationCandidates() {
    const selectedCountryCodes = selectedCountries.value.map(
        (country) => country.properties.iso_a2
    );
    const collaborationCandidateCountryCodes =
        collaborationStore.findCollaboratingCountries(selectedCountryCodes);
    // unhighlight all currently highlighted collaboration candidates
    getCountryNodes().classed('suggested-country-collab', false);
    // given the the currently selected countries in selectedCountries
    // find the collaboration candidates and highlight each of them;
    if (collaborationCandidateCountryCodes.length > 0 && inCollabMode.value) {
        getCountryNodes()
            .filter((d) => collaborationCandidateCountryCodes.includes(d.properties.iso_a2))
            .classed('suggested-country-collab', true);
    }
}
const handleChangeInCountriesSelection = () => {
    console.log('handleChangeInCountriesSelection')
    highlightCollaborationCandidates();
    zoomInOnSelectedCountries();
};

watch(
    selectedCountries,
    (newVal, oldVal) => {
        if (!newVal[0]) {
            unmarkAllSelectedCountries();
            zoomToScale(1);
        } else {
            if (inCollabMode.value) {
                markCollabCountry(selectedCountries.value[selectedCountries.value.length - 1]);
            } else {
                unmarkAllSelectedCountries();
                markSelectCountry(selectedCountries.value[selectedCountries.value.length - 1]);
            }
        }

        if (oldVal.length > newVal.length) {
            const country = oldVal.filter((c) => !newVal.includes(c));
            unmarkCollabCountry(useCountries.getCountryByName(country[0].properties.name));
        }

        if (selectedCountries.value.length < 1) {
            useCountries.setCollabMode(false);
        }
        handleChangeInCountriesSelection();
    },
    { deep: true }
);

watch(inCollabMode, (newVal) => {
    if (newVal) {
        markCollabCountry(selectedCountries.value[0]);
        highlightCollaborationCandidates();
    } else {
        unmarkAllSelectedCountries();
    }
});

const handleMitigation = (mitigationVal) => {
    mitigation.value = mitigationVal;
};

const findCollaboratingCandidates = (selectedCountries) => {
    let collaborationCandidateCountries = [];
    if (selectedCountries.length > 0) {
        const selectedCountryCodes = selectedCountries.map((country) => country.properties.iso_a2);
        const collaborationCandidateCountryCodes =
            collaborationStore.findCollaboratingCountries(selectedCountryCodes);
        // create an array of country objects for the countries whose code is in collaborationCandidateCountryCodes
        collaborationCandidateCountries = countryDataSet.features.filter((c) =>
            collaborationCandidateCountryCodes.includes(c.properties.iso_a2)
        );
    }
    return collaborationCandidateCountries;
};
let zoomBehavior

onMounted(() => {


    watch(mitigation, (newValue) => {
        colorScale2 = createColorScaleForHeatmapProperty(heatmapData, mitigation.value.value);
        yAxisScale = createYAxisScaleForHeatmapProperty(heatmapData, mitigation.value.value);

        drawAllCountries(countryDataSet);
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

    const projection = d3
        .geoEquirectangular()
        .rotate([-148, 0]) // rotate sets the spherical rotation angles. The default rotation is [0, 0], which centers the map on Greenwich (0° longitude). By adjusting the first value (longitude), you can center the map on a different region.
        .translate([t0.x, t0.y])
        .scale(t0.k);

    pathGenerator = d3.geoPath().projection(projection);

    countriesGroup = svg.append('g');


    // Append a <rect> element with fill and opacity 0 (invisible) to allow every pixel in the area to be zoomable and draggable 
    countriesGroup.append("rect")
        .attr("x", 0) // X-coordinate
        .attr("y", 0) // Y-coordinate
        .attr("width", width) // Width
        .attr("height", height) // Height
        .attr("fill", "blue") // Fill color - however: invisible
        .attr("opacity", 0); // Opacity set to 0 (invisible)


    zoomBehavior = d3.zoom()
        .scaleExtent([1, maxScaleFactor])
        .translateExtent([[0, 0], [width, height]])
        .on("zoom", handleZoom)

    // Attach the zoom behavior to the SVG
    countriesGroup.call(zoomBehavior)

    // Function to handle zooming
    // zooming applies to all paths in a specific group - that includes countries but not legend etc
    function handleZoom(event) {
        countriesGroup.selectAll('path').attr('transform', event.transform); // Apply the zoom transform to map elements
    }


    const colorLegendG = svg.append('g').attr('transform', `translate(40,310)`);

    const heatmapLegendG = svg.append('g').attr('transform', `translate(-10,470)`);

    let colorScale2, yAxisScale;

    function findMinMax(someObject, theProperty) {
        
        const values = Object.keys(someObject).map((key) => someObject[key][theProperty]);
        return {
            min: Math.min(...values),
            max: Math.max(...values),
        };
    }
    function createColorScaleForHeatmapProperty(heatmapData, property) {
        const result = findMinMax(heatmapData, property);
        return scaleSequential(d3.interpolateBlues).domain([result.min, result.max]);
    }
    colorScale2 = createColorScaleForHeatmapProperty(heatmapData, mitigation.value.value);

    function createYAxisScaleForHeatmapProperty(heatmapData, property) {
        const result = findMinMax(heatmapData, property);
        return d3.scaleLinear().domain([result.min, result.max]).range([300, 0]); // Adjust the range to match the desired height of your axis
    }

    yAxisScale = createYAxisScaleForHeatmapProperty(heatmapData, mitigation.value.value);

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

    countriesDataStore.fetchData()
    .then((countries) => {
        countryDataSet = countries;
        collaborationStore.prepareCountryCollaborations();
        useCountries.dataSet.value = countries.features;
        // "Mitigation_Potential(GtCO2e)":"234","Mitigation_Cost($/GtCO2e)":"5","Mitigation_Potential(GtCO2e)_at_50":"234","Mitigation_Potential(GtCO2e)_at_100":"250","Mitigation_Potential(GtCO2e)_at_200":"300"}
        // now we can determine - depending on the toggle that indicates which category of these data properties should be used for the heatmap
        // the color scale - get min and max for the desired property from the heatmap data
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
            .text( mitigation.value.label);
    }

    function drawAllCountries(countries) {
        // draw all countries
        countryNodes = countriesGroup.selectAll('path').data(countries.features);
        countryNodes
            // fill with gray (#dcdcdc) when the country's data is unknown
            .attr('fill', (d) =>
                d.properties['in_heatmap']
                    ? d.properties.hasOwnProperty(mitigation.value.value)
                        ? colorScale2(d.properties[mitigation.value.value])
                        : '#ffffff'
                    : '#dcdcdc'
            )
            .select('title') // Select the child title of each path
            .text(
                (d) =>
                    d.properties.name_long +
                    ' : ' +
                    (d.properties.hasOwnProperty(mitigation.value.value)
                        ? d.properties[mitigation.value.value] + ` ${mitigation.value.label}`
                        : '')
            );

        countryNodes
            .enter()
            .append('path')
            .attr('d', pathGenerator)
            // fill with gray (#dcdcdc) when the country's data is unknown
            .attr('fill', (d) =>
                d.properties['in_heatmap']
                    ? d.properties.hasOwnProperty(mitigation.value.value)
                        ? colorScale2(d.properties[mitigation.value.value])
                        : '#ffffff'
                    : '#dcdcdc'
            )
            .on('mouseover', handleMouseOver)
            .on('mouseleave', handleMouseLeave)
            .on('click', handleCountryClick)
            .append('title')
            .text(
                (d) => d.properties.name +
                    
                    (d.properties.hasOwnProperty(mitigation.value.value)
                        ? `: ${d.properties[mitigation.value.value]} ${mitigation.value.label}`
                        : '')
            )
            .attr('class', 'country');



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


function zoomToScale(scale) {
    countriesGroup
        .transition()
        .call(zoomBehavior.scaleTo, scale); // if scale > maxScaleFactor then nothing happens
}

function zoomOut() {
    countriesGroup
        .transition()
        .call(zoomBehavior.scaleBy, 0.667);
}

function zoomIn() {
    countriesGroup
        .transition()
        .call(zoomBehavior.scaleBy, 1.5);
}
function getCountryNodes() {
    return countriesGroup.selectAll('path').data(countryDataSet.features);
}

function markCollabCountry(country) {
    getCountryNodes()
        .filter((c) => country.id === c.id)
        .classed('selected-country', false)
        .classed('selected-country-collab', true);
}

function markSelectCountry(country) {
    getCountryNodes()
        .filter((c) => country.id === c.id)
        .classed('selected-country', true)
        .classed('selected-country-collab', false);
}

function unmarkAllSelectedCountries() {
    getCountryNodes()
        .filter((d) => selectedCountries.value.id !== d.id)
        .classed('selected-country', false)
        .classed('selected-country-collab', false);
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
    // ignore click for unsupported countries
    if (!d.properties['in_heatmap']) return;
    // if we are in collabmode, then only process the click if the clicked country is in the set of collaboration candidates
    if (!inCollabMode.value) {
        unmarkAllSelectedCountries();

        if (!useCountries.isCountryInList(d)) {
            useCountries.setCountry(d);
        } else {
            useCountries.resetCountries();
        }
        //  zoomInOnSelectedCountries()
    } else {
        // if the user clicked on a countrythat is not a collaboration candidate, then do not process the click

        if (useCountries.isCountryInList(d)) {
            useCountries.removeCountry(useCountries.getCountryById(d.id));
            return;
        }

        const selectedCountryCodes = selectedCountries.value.map(
            (country) => country.properties.iso_a2
        );
        const collaborationCandidateCountryCodes =
            collaborationStore.findCollaboratingCountries(selectedCountryCodes);
        if (
            collaborationCandidateCountryCodes.length == 0 ||
            !collaborationCandidateCountryCodes.includes(d.properties.iso_a2)
        )
            return;

        useCountries.addCountry(d);
    }
}

function zoomInOnSelectedCountries() {
    if (selectedCountries.value.length > 0) {
        var minX = Number.POSITIVE_INFINITY;
        var minY = Number.POSITIVE_INFINITY;
        var maxX = Number.NEGATIVE_INFINITY;
        var maxY = Number.NEGATIVE_INFINITY;
        selectedCountries.value.forEach((c) => {

            var selectedCountryGeoJSON = countryDataSet.features.filter((d) => d.id == c.id);
            // Calculate zoom parameters
            //                var bounds = d3.geoBounds(selectedCountryGeoJSON[0]);


            selectedCountryGeoJSON.forEach((geojson) => { // for example australia consists of multiple polygons
            
            const bounds = pathGenerator.bounds(geojson);
            minX = Math.min(minX, bounds[0][0]);
            minY = Math.min(minY, bounds[0][1]);
            maxX = Math.max(maxX, bounds[1][0]);
            maxY = Math.max(maxY, bounds[1][1]);
            
            })
        });
        const dx = maxX - minX;
        const dy = maxY - minY;
        const x = (minX + maxX) / 2;
        const y = (minY + maxY) / 2;
        const newScale = Math.max(1, Math.min(3, 0.9 / Math.max(dx / width, dy / height)));
        // Transition to the selected feature's position and scale
        const xcorrectionfactor = inCollabMode.value ? 1 : 1.1; // *  to position box more to the left of the center and out of overlap with country details popup window
        const ycorrectionfactor = inCollabMode.value ? 1.2 : 1; // *  to position box a little bit higher to prevent too much overlap with collaboration panel

        // the transition consists of two steps - scale and translate. if we reverse the steps, the selected country is not in the right place
        // possible solution: calculate one transform that does both scale and translate
        const transform = d3.zoomTransform(countriesGroup.node());
        // Access the current scale factor
        let currentScaleFactor = transform.k;
        let currentTranslateX = 0, currentTranslateY = 0
            try {
                const matrix = countriesGroup.node().transform.baseVal.consolidate().matrix
                currentTranslateX = matrix.e
                currentTranslateY = matrix.f

            } catch (e) { }

        // if the current scale factor is close to the desired one, we can use a smooth transition for translate; scale will hardly be noticeable 
        if (currentScaleFactor / newScale > 0.8 && currentScaleFactor / newScale < 1.2) {
            countriesGroup
                .transition()
                .duration(10)
                .call(zoomBehavior.scaleTo, newScale)
                .transition()
                .duration(500)
                .call(zoomBehavior.translateTo, x + 75, y + 20)
        } else {
            const transform = d3.zoomIdentity.scale(newScale).translate((-x-0.5 * dx)/newScale  - 180,(-y-0.5 * dy)/newScale  - 120)
            countriesGroup.transition().duration(750).call(zoomBehavior.transform, transform);
        }


    }

}
</script>

<style>
.hover-country {
    stroke: lightblue;
    stroke-width: 1.75px;
}

.selected-country {
    /*fill: #95ad28;*/
    stroke: #f07004;
    /*white;*/
    stroke-width: 2px;
}

.selected-country-collab {
    /*fill: #f07004;*/
    stroke: #f07004;
    /*white;*/
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
    top: 350px;
    right: 80px;
}

.countries-box {
    position: absolute;
    bottom: 20px;
    margin-left: 120px;
}

.zoom-box {
    position: absolute;
    bottom: 40px;
    right: 40px;
}

.zoom-flex {
    display: flex;
    flex-direction: column;
    gap: 15px;
}

.r-btn {
    display: flex;
    justify-content: center;
    align-items: center;
    width: 56px;
    height: 56px;
    border-radius: 50px;
    background-color: white;
    color: #214b63;
    cursor: pointer;
    box-shadow: 0px 4px 8px 0px #214b6352;
}

.modal-diagram {
    display: flex;
    flex-direction: column;
    position: absolute;
    top: 10%;
    left: 50%;
    transform: translate(-50%, 0);
    width: calc(52vw + 48px);
    height: calc(100vh - 142px);
    background: rgba(255, 255, 255, 0.95);
    z-index: 1500;
    overflow-y: scroll;
    border: 1px solid #ccc;
    border-radius: 4px;
    box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
}

/* Modal Content */
.modal-diagram-content {
    background-color: #fff;
    padding: 4px 16px;
    border: 0px;
}

.close-icon {
    position: absolute;
    right: 20px;
    top: 20px;
    cursor: pointer;
}
</style>
