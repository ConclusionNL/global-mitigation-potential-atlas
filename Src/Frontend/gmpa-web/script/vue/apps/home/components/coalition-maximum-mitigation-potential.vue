﻿<template>
    <div id="svgLineChartContainer"></div>
</template>

<script setup>
import { ref, onMounted, watch, defineProps, computed } from 'vue';
import { useCollaborationStore } from '../stores/collaborationStore';
import * as d3 from 'd3';

const collaborationStore = useCollaborationStore();

const props = defineProps({
    countriesList: []
})
const svgWidth = 340;
const svgHeight = 180;

onMounted(() => {
    setupLineAreaChart(props.countriesList)
})

const setupLineAreaChart = async (countriesList) => {
    const data = await collaborationStore.prepareCombinedCollaborationData(countriesList.map(country => country.properties.iso_a2))

    console.log(`combined data ${JSON.stringify(data)}`)

    const svg = d3.select("#svgLineChartContainer")
        .append("svg")
        .attr("width", svgWidth)
        .attr("height", svgHeight);

    // Create the D3.js bar chart within the SVG container
    const chartWidth = 700;
    const chartHeight = 400;
    const chart = svg.append("g")
        .attr("transform", `scale(${0.8 * svgWidth / chartWidth}, ${0.65 * svgHeight / chartHeight}) translate(100,120)`)
        .attr("width", chartWidth)
        .attr("height", chartHeight);

    const lineChartArea = chart.append("g")
        .attr("transform", `translate(0,0)`)
        .attr("width", chartWidth - 20)
        .attr("height", chartHeight - 20);
    //         const collaborationProperties = ['collaboration_emissions', 'collaboration_cost', 'autarky_emissions', 'autarky_cost']
    // draw a line chart with two lines - one for collaboration - emissions along x axis and cost along y axis 
    // and one for autarky

    const maxCollabCost = d3.max(data, d => d['collaboration_cost'])
    const maxAutarkyCost = d3.max(data, d => d['autarky_cost'])

    const maxCollabEmissions = d3.max(data, d => d['collaboration_emissions'])
    const maxAutarkyEmissions = d3.max(data, d => d['autarky_emissions'])

    // Define scales for x and y axes
    const xScale = d3.scaleLinear()
        .domain([0, Math.max(maxCollabEmissions, maxAutarkyEmissions)])
        .range([0, chartWidth]);


    const yScale = d3.scaleLinear()
        .domain([0, Math.max(maxCollabCost, maxAutarkyCost)])
        .range([chartHeight, 0]);


    // Create a line generator
    const lineAutarky = d3.line()
        .x(d => xScale(d['autarky_emissions']))
        .y(d => yScale(d['autarky_cost']))
        .curve(d3.curveNatural); // Use a natural curve for the line

    // Append the line to the chart
    lineChartArea.append("path")
        .datum(data)
        .attr("fill", "none")
        .attr("stroke", "darkblue")
        .attr("stroke-width", 1)
        .attr("d", lineAutarky);
    const autarkyArea = d3.area()
        .x(d => xScale(d['autarky_emissions']))
        .y0(chartHeight) // The bottom of the area is at the height of the chart
        .y1(d => yScale(d['autarky_cost']))
        .curve(d3.curveNatural); // Use a natural curve for the area

    // Append the area path to the chart
    lineChartArea.append("path")
        .datum(data)
        .attr("fill", "darkblue") // Fill color for the area
        .attr("d", autarkyArea);


    // Create a line generator
    const lineCollaboration = d3.line()
        .x(d => xScale(d['collaboration_emissions']))
        .y(d => yScale(d['collaboration_cost']))
        .curve(d3.curveNatural); // Use a natural curve for the line

    // Append the line to the chart
    lineChartArea.append("path")
        .datum(data)
        .attr("fill", "none")
        .attr("stroke", "orange")
        .attr("stroke-width", 1)
        .attr("d", lineCollaboration);

    const area = d3.area()
        .x(d => xScale(d['collaboration_emissions']))
        .y0(chartHeight) // The bottom of the area is at the height of the chart
        .y1(d => yScale(d['collaboration_cost']))
        .curve(d3.curveNatural); // Use a natural curve for the area

    // Append the area path to the chart
    lineChartArea.append("path")
        .datum(data)
        .attr("fill", "orange") // Fill color for the area
        .attr("d", area);

    // Add x-axis
    chart.append("g")
        .attr("transform", `translate(0,${chartHeight})`)
        .call(d3.axisBottom(xScale)
            .ticks(6)
            .tickSize(5) // Set the size of the ticks (adjust as needed)
            .tickPadding(5) // Set the distance between ticks and labels (adjust as needed)
            .tickFormat(d3.format(".0f")))
        .selectAll("text")
        .style("font-size", "28px")


    // Add y-axis
    chart.append("g")
        .call(d3.axisLeft(yScale)
            .ticks(5)
            .tickSize(5) // Set the size of the ticks (adjust as needed)
            .tickPadding(5) // Set the distance between ticks and labels (adjust as needed)
            .tickFormat(d3.format(".0f")))
            .selectAll("text")
  .style("font-size", "28px")

    chart
        .append('text')
        .attr('class', 'x-axis-title')
        .attr('x', 0)
        .attr('y', chartHeight + 75) // Adjusted y position
        .style('text-anchor', 'start')
        .text('Total system Emissions (Mt CO2e)')
        .attr("font-size", "42px")
        ;

    chart
        .append('g')
        .attr('transform', 'translate(-65 , 400)')
        .append('text')
        .attr('text-anchor', 'start')
        .attr('transform', 'rotate(-90)')
        .text('Total System Cost (Billion $)')
        .attr("font-size", "42px")
        ;






}
</script>

<style></style>