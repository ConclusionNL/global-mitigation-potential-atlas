<template>
    <div id="chart"></div>
    <!-- Create a container for the bar chart -->
    <div id="bar-chart"></div>
</template>
<script setup>
import { ref, onMounted, watch, defineProps, computed } from 'vue';
import { useCollaborationStore } from '../stores/collaborationStore';
import * as d3 from 'd3';

const collaborationStore = useCollaborationStore();

const xAxisTitle ="Annual System Emissions (MtCO2e)"
const yAxisTitle = "Annualized System Costs (Billion $)"



const emit = defineEmits(['technology-selected']);

const props = defineProps({
    collaborationCountriesList: [], dataSetType : String
});
watch(
    () => props.collaborationCountriesList,
    (newValue, oldValue) => {
        setupDiagram(newValue, props.dataSetType);
    }
);

watch(
    () => props.dataSetType,
    (newValue, oldValue) => {
        setupDiagram(props.collaborationCountriesList, newValue);
    }
);

onMounted(() => {
    setupDiagram(props.collaborationCountriesList, props.dataSetType);
})
let color
const setupDiagram = async (collaborationCountriesList, dataSetType) => {
    const isDetailedNationalModelling = collaborationCountriesList.length==1 && collaborationCountriesList[0]['DNM']
    console.log(`isDetailedNationalModelling ${isDetailedNationalModelling}`)
    if (isDetailedNationalModelling) {
        console.log(`get data from total_data.csv`)
    }
    const countriesKey = collaborationCountriesList.map((country) => country.properties.iso_a2).sort().join('')
    const data = collaborationStore.getTotalData()[countriesKey][dataSetType]

    if (data.length==0){
        // remove existing charts
        d3.select('#chart').selectAll('*').remove();
        d3.select('#bar-chart').selectAll('*').remove();    
        return
    }
    // Create a color scale for the series
    //  const x = Object.keys(data[0]).filter((key) => key !== 'x' && key !== 'sum')
    color = d3
        .scaleOrdinal()
        .domain(Object.keys(data[0]).filter((key) => key !== 'x' && key !== 'sum'))
        .range(d3.schemeCategory10);
    createAreaChart(data, color);
    // create BAR CHART
    const barData = [];
    Object.keys(data[0])
        .filter((key) => key !== 'x' && key !== 'sum')
        .forEach((key) => barData.push({ series: key, value: data[0][key] }));

    drawBarChart(barData, color);
};

let xScale, yScale;
const createAreaChart = (data, color) => {
    // Create an SVG container
    const margin = { top: 20, right: 30, bottom: 40, left: 40 };
    const width = 900 - margin.left - margin.right;
    const height = 600 - margin.top - margin.bottom;
    d3.select('#chart').selectAll('*').remove();

    const svg = d3
        .select('#chart')
        .append('svg')
        .attr('width', 50+width + margin.left + margin.right)
        .attr('height', height + margin.top + margin.bottom + 50)
        .append('g')
        .attr('transform', `translate(${margin.left + 50},${margin.top})`);

    const tooltip = d3
        .select('body')
        .append('div')
        .attr('class', 'popup-container')
        .style('position', 'absolute')
        .style('z-index', '2000')
        .style('visibility', 'hidden')
        .text('');

    let minimumX = d3.min(data, (d) => d.x);
    // Create x and y scales
    xScale = d3
        .scaleLinear()
        // always have x scale start at zero, even if minimum value is larger than 0
        .domain([0, d3.max(data, (d) => d.x)])
        .range([0, width]);

    function xValueFromMouse(mouseX) {
        return xScale.invert(mouseX);
    }

    yScale = d3
        .scaleLinear()
        .domain([0, d3.max(data, (d) => d.sum)])
        .range([height, 0]);

    function yValueFromMouse(mouseY) {
        return yScale.invert(mouseY);
    }
    // Create a stack generator
    const stack = d3
        .stack()
        .keys(Object.keys(data[0]).filter((key) => key !== 'x' && key !== 'sum'))
        .order(d3.stackOrderNone)
        .offset(d3.stackOffsetNone);

    // Compute the stacked data
    const seriesData = stack(data);
    // Create an area generator with curveBasis interpolation
    const area = d3
        .area()
        .x((d) => xScale(d.data.x))
        .y0((d) => yScale(d[0]))
        .y1((d) => yScale(d[1]))
        .curve(d3.curveMonotoneX); // Use curved shape  d3.curveBasis , curveLinear

    // Draw the stacked areas
    svg.selectAll('.area')
        .data(seriesData)
        .enter()
        .append('path')
        .attr('class', 'area')
        .attr('fill', (d) => color(d.key))
        .attr('d', area)
        .on('mouseover', function (event, d) {
            let tooltipText = `Technology:  ${d.key}`;
            const coordinates = d3.pointer(event);

            const x = coordinates[0];
            const y = coordinates[1];
            const xValue = xValueFromMouse(x);
            const yValue = yValueFromMouse(y);
            tooltipText = `${tooltipText}: Reductions: ${parseFloat(xValue.toPrecision(2))} Cost: ${parseFloat(yValue.toPrecision(2))}`;
            tooltip.text(tooltipText);

            return tooltip.style('visibility', 'visible');
        })
        .on('mousemove', function (event, d) {
            let tooltipText = `Technology:  ${d.key}`;
            const coordinates = d3.pointer(event);

            const x = coordinates[0];
            const y = coordinates[1];

            const xValue = xValueFromMouse(x)
            parseFloat(xValue.toPrecision(3))
            // find the Y value for the selected series
            const yValue = findYforXinSerie(d.key, xValue, data);
            tooltipText = `${tooltipText}: Reductions: ${parseFloat(xValue.toPrecision(2))} Cost: ${parseFloat(yValue.toPrecision(2))}`;
            tooltip.text(tooltipText);
            return tooltip.style("left", (event.pageX + 10) + "px")
                .style("top", (event.pageY - 50) + "px")
        })
        .on("mouseout", function () { return tooltip.style("visibility", "hidden"); })
        .on('click', function (event, d) {
            const coordinates = d3.pointer(event);
            const x = coordinates[0];
            const y = coordinates[1];
            // Determine which series was clicked
            const series = d.key;

            const xCoordToFind = xValueFromMouse(x); // Replace with the desired x-coordinate - translate mouse x to value on x-axis
            const yValue = yValueFromMouse(y);
            const valuesAtX = findValuesAtX(xCoordToFind, data);
            const updatedBarData = [];
            for (const areaValue of valuesAtX) {
                updatedBarData.push({
                    series: areaValue.series,
                    value: areaValue.value,
                });
            }
            repaintBar(updatedBarData, color);
        });

    // Add x and y axes
    const xAxis = d3.axisBottom(xScale);
    const yAxis = d3.axisLeft(yScale);

    svg.append('g').attr('class', 'x-axis').attr('transform', `translate(0,${height})`).call(xAxis);

    svg.append('g').attr('class', 'y-axis').call(yAxis);

    // Add x-axis and y-axis titles to the area chart
    svg.append('text')
        .attr('class', 'x-axis-title')
        .attr('x', margin.left + 100)
        .attr('y', height + 35) // Adjusted y position
        .style('text-anchor', 'middle')
        .text(xAxisTitle);

    svg
        .append('g')
        .attr('transform', 'translate(' + -70 + ', ' + 530 + ')')
        .append('text')
        .attr('text-anchor', 'start')
        .attr('transform', 'rotate(-90)')
        .text(yAxisTitle);

    drawCrosshairLines(svg, data, minimumX, height, width, xValueFromMouse, yValueFromMouse);
};

const drawCrosshairLines = (
    svg,
    data,
    minimumX,
    height,
    width,
    xValueFromMouse,
    yValueFromMouse
) => {
    // Create the vertical line
    // determine the X coord for the minimumX value
    let minimumXCoord = xScale(minimumX);
    const verticalLine = svg
        .append('line')
        .attr('class', 'vertical-line')
        .attr('x1', minimumXCoord)
        .attr('x2', minimumXCoord)
        .attr('y1', 0)
        .attr('y2', height + 40);

    function draggingVerticalLineMarker(event, d) {
        // note: 10 = half of width marker
        if (event.x < minimumXCoord - 10 || event.x > width + 20) {
            return;
        }
        const x = Math.min(width, Math.max(event.x - 10, minimumXCoord));

        d3.select(this).attr('x', x - 10); // 10 is half of the rectangle's width

        verticalLine.attr('x1', x).attr('x2', x); // Move line

        // find current x coordinate and synchronize bar chart
        const xCoordToFind = xValueFromMouse(x); // Replace with the desired x-coordinate - translate mouse x to value on x-axis
        const valuesAtX = findValuesAtX(xCoordToFind, data);

        const updatedBarData = [];
        let sum = 0;
        for (const areaValue of valuesAtX) {
            updatedBarData.push({
                series: areaValue.series,
                value: areaValue.value,
            });
            sum = sum + areaValue.value;
        }
        const newY = yScale(sum);
        // update horizontal line and marker
        horizontalLine.attr('y1', newY).attr('y2', newY); // Move line
        horizontalLineMarker.attr('y', newY - 10); // Move marker
        repaintBar(updatedBarData, color);
    }

    // vertical line Marker
    const verticalLineMarker = svg
        .append('rect')
        .attr('class', 'marker')
        .attr('x', minimumXCoord - 10)
        .attr('y', height + 20)
        .attr('width', 20)
        .attr('height', 20)
        .call(
            d3
                .drag() // Call the drag behavior
                .on('drag', draggingVerticalLineMarker)
        );
    // calculate the Y coordinate from the the sum of all series values at the first, most left data point in order to position the horizontal line correctly
    const newY = yScale(data[0].sum);
    // Create the horizontal line
    const horizontalLine = svg
        .append('line')
        .attr('class', 'vertical-line')
        .attr('x1', -20)
        .attr('x2', width)
        .attr('y1', newY)
        .attr('y2', newY);

    // vertical line Marker
    const horizontalLineMarker = svg
        .append('rect')
        .attr('class', 'marker')
        .attr('x', -30)
        .attr('y', newY - 10)
        .attr('width', 20)
        .attr('height', 20)
        .call(
            d3
                .drag() // Call the drag behavior
                .on('drag', draggingHorizontalLineMarker)
        );
    function draggingHorizontalLineMarker(event, d) {
        if (event.y < 0 || event.y > height) {
            return;
        }

        d3.select(this).attr('y', event.y - 10); // 10 is half of the rectangle's height
        const y = event.y;

        horizontalLine.attr('y1', y).attr('y2', y); // Move line

        // find current x coordinate and synchronize bar chart
        const yCoordToFind = yValueFromMouse(y);
        // find the X that goes with this total sum of Capacity - the X that produces this stack
        // iterate over data, take sum for each X
        // find the first X where the previous sum is larger and the next is smaller than yCoordToFind or the prev is smaller and the next is larger

        // this code looks for the first data that is lower, assuming declining sum
        let prevX = data[0].x,
            nextX;
        let prevSum = data[0].sum,
            nextSum;

        for (const obj of data) {
            nextSum = obj.sum;
            nextX = obj.x;
            if (nextSum < yCoordToFind) {
                // && prevSum > yCoordToFind) {
                break;
            }
            // we can assume that sum is only decreasing
            // if (obj.sum > yCoordToFind && prevSum < yCoordToFind) {
            //     nextX = obj.x
            //     break
            // }
            else {
                prevX = nextX;
                prevSum = nextSum;
            }
        }
        // if the previous x has a value closer to the set Y value than the nextX then use prevX

        if (Math.abs(prevSum - yCoordToFind) < Math.abs(nextSum - yCoordToFind)) {
            nextX = prevX;
        }
        const valuesAtX = findValuesAtX(nextX, data);


        const updatedBarData = [];
        let sum = 0;
        for (const areaValue of valuesAtX) {
            updatedBarData.push({
                series: areaValue.series,
                value: areaValue.value,
            });
            sum = sum + areaValue.value;
        }
        // calculate interpolated X??
        const newX = xScale(nextX);

        //update vertical line and marker
        verticalLine.attr('x1', newX).attr('x2', newX); // Move line
        verticalLineMarker.attr('x', newX - 10); // Move marker

        repaintBar(updatedBarData, color);

    };

}

let barSvg;
let barXScale, barYScale;

let barXAxis, barYAxis;
const barMargin = { top: 20, right: 30, bottom: 40, left: 40 };
const barWidth = 900 - barMargin.left - barMargin.right;
const barHeight = 250 - barMargin.top - barMargin.bottom ;

const bartooltip = d3.select("body")
    .append("div")
    .attr('class', 'bar-tooltip-popup-container')
    .style("position", "absolute")
    .style("z-index", "2000")
    .style("visibility", "hidden")
    .text("");

const drawBarChart = (barData, color) => {
    d3.select('#bar-chart').selectAll('*').remove();
    // Create an SVG container for the bar chart
    barSvg = d3
        .select('#bar-chart')
        .append('svg')
        .attr('width', barWidth + barMargin.left + barMargin.right)
        .attr('height', barHeight + barMargin.top + barMargin.bottom+50)
        .append('g')
        .attr('transform', `translate(${barMargin.left + 50},${barMargin.top})`);



    // Create x and y scales for the bar chart
    barXScale = d3
        .scaleBand()
        .domain(barData.map((d) => d.series))
        .range([0, barWidth])
        .padding(0.1);

    barYScale = d3
        .scaleLinear()
        .domain([0, d3.max(barData, (d) => d.value)])
        .nice()
        .range([barHeight, 0]);

    // Add x and y axes for the bar chart
    barXAxis = d3.axisBottom(barXScale);
    barYAxis = d3.axisLeft(barYScale);

    barSvg
        .append('g')
        .attr('class', 'x-axis')
        .attr('transform', `translate(0,${barHeight})`)
        .call(barXAxis);

    barSvg.append('g').attr('class', 'y-axis').call(barYAxis);
    repaintBar(barData, color)
}

const repaintBar = (updatedBarData, color) => {
    // Replace the bar chart data with the updated dataset
    // TODO only if the values have changed should we proceed (to prevent unnecessary repaints)

    barSvg.selectAll(".bar").remove();

    const cutOffPercentage = 1 // % that the value of a series entry must at least represent of the total sum in order to be displayed
    const topX = Math.min(updatedBarData.length,10) // no more than 10 bars, no more than we actually have

    // find out the value associated with topX
    const cutoffTopXValue = updatedBarData.slice().sort((a, b) => b.value - a.value)[topX-1].value;

    // find out value associated with cutoffPercentage of the total sum
    const cutoffPercentageValue = updatedBarData.reduce((sum, obj) => sum + obj.value, 0) * cutOffPercentage/100;
    // use the highest  of the two cutoff values to filter the series

    const cutoffValue = Math.max(cutoffTopXValue, cutoffPercentageValue)
    const topNBarData = updatedBarData.filter(d => d.value >= cutoffValue) 

    const updatedBars = barSvg
        .selectAll('.bar')
        .data(topNBarData)

    // Create x and y scales for the bar chart
    barXScale = d3
        .scaleBand()
        .domain(topNBarData.map((d) => d.series))
        .range([0, barWidth])
        .padding(0.1);

    barYScale = d3
        .scaleLinear()
        .domain([0, d3.max(topNBarData, (d) => d.value)])
        .nice()
        .range([barHeight, 0]);



    updatedBars.enter()
        .append('rect')
        .attr('class', 'bar')
        .attr('x', (d) => barXScale(d.series))
        .attr('y', (d) => barYScale(d.value))
        .attr('width', barXScale.bandwidth())
        .attr('height', (d) => barHeight - barYScale(d.value))
        .attr('fill', (d, i) => color(d.series))
        .on('click', (event, data) => {
            // Emit a custom event when a bar is clicked
            emit('technology-selected', { data: data });
        })
        .on("mouseover", function (event, d) {
            let tooltipText = `System Cost: ${d.value.toFixed(2)} ,  Technology:  ${d.series}`;
            bartooltip.text(tooltipText);
            return bartooltip.style("visibility", "visible");
        })
        .on("mousemove", function (event, d) {
            let tooltipText = `System Cost: ${d.value.toFixed(2)} ,  Technology:  ${d.series}`;
            bartooltip.text(tooltipText);
            return bartooltip.style("left", (event.pageX + 10) + "px")
                .style("top", (event.pageY - 50) + "px")
        })
        .on("mouseout", function () { return bartooltip.style("visibility", "hidden"); })

    barXAxis = d3.axisBottom(barXScale);
    barYAxis = d3.axisLeft(barYScale);

    // Update the x and y axes
    barSvg.select('.x-axis').transition().duration(300).call(barXAxis);
    barSvg.select('.y-axis').transition().duration(1000).call(barYAxis);

    barSvg.selectAll(".x-axis text")
  .style("text-anchor", "end") // Set the anchor to the end of the text
  .attr("dx", "-0.5em") // Offset the text leftwards to center it correctly
  .attr("dy", "0.5em") // Offset the text downwards to center it correctly
  .attr("transform", "rotate(-15)"); // Rotate the text by -45 degrees
}

// Function to find values for every area at a given x-coordinate
function findValuesAtX(xCoord, data) {
    // Filter the data to find the data point with the closest x-coordinate
    const closestDataPoint = data.reduce((closest, current) => {
        const xDiff = Math.abs(current.x - xCoord);
        if (xDiff < Math.abs(closest.x - xCoord)) {
            return current;
        }
        return closest;
    });

    // Extract values for each series at the specified x-coordinate
    const values = Object.keys(closestDataPoint)
        .filter((key) => key !== 'x' && key !== 'sum')
        .map((series) => ({
            series,
            value: closestDataPoint[series],
        }));

    return values;
}
function findYforXinSerie(serie, xValue, data) {
    // x0: find largest X in serie that is smaller than or equal to xValue
    // Initialize variables to keep track of the largest x and the corresponding object
    let largestX = -Infinity; // Initialize to negative infinity so that any x value is greater
    let largestXObject = null;

    // Iterate through the array of objects
    for (const obj of data) {
        // Check if the current object's x property is smaller than P and larger than the largestX found so far
        if (obj.x < xValue && obj.x > largestX) {
            largestX = obj.x; // Update the largestX
            largestXObject = obj; // Update the corresponding object
        }
    }

    // x1: find smallest X in serie that is larger than xValue

    let smallestX = Infinity; // Initialize to negative infinity so that any x value is greater
    let smallestXObject = null;

    // Iterate through the array of objects
    for (const obj of data) {
        // Check if the current object's x property is smaller than P and larger than the largestX found so far
        if (obj.x > xValue && obj.x < smallestX) {
            smallestX = obj.x;
            smallestXObject = obj; // Update the corresponding object
        }
    }

    // get the cost values for for these two: y0 and y1
    const y0 = largestXObject[serie];
    const y1 = smallestXObject[serie];

    // interpolate: return y0 + (x-x0/x1-x0) * (y1-y0)
    const interpolatedValue = y0 + (y1 * (xValue - largestX)) / (smallestX - largestX);
    return interpolatedValue;
}
</script>

<style>
/* Style the line */
.vertical-line {
    stroke: steelblue;
    stroke-width: 2;
    stroke-dasharray: 5, 5;
    /* Add dashed line style */
}

/* Style the lines */
.line {
    stroke-width: 2;
    stroke-dasharray: 5, 5;
    /* Add dashed line style */
}

#chart {
    position: relative;
}

.marker {
    cursor: pointer;
    fill: red;
}

.popup-container {
    background-color: #fff;
    border: 1px solid #ccc;
    padding: 10px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    /* Add other styling properties as needed */
}

.bar-tooltip-popup-container {
    background-color: #fff;
    border: 1px solid #ccc;
    padding: 10px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    /* Add other styling properties as needed */
}
</style>
