<template>
    <div id="svgContainer"></div>
</template>

<script setup>
import { ref, watch, onMounted, defineProps, defineEmits, computed } from 'vue';
import * as d3 from 'd3';
import { useCollaborationStore } from '../stores/collaborationStore';

const collaborationStore = useCollaborationStore();
const props = defineProps({
      countriesList: []
    , showAbsolutePotential: Boolean
});
const svgWidth = 420;
const svgHeight = 180;
const showAbsolutePotentialProxy = ref(props.showAbsolutePotential);
const autarkyColor = "darkblue"
const collaborationColor ="#f07004"


onMounted(() => {
    setupGauge(props.countriesList, showAbsolutePotentialProxy);
});


watch(() => props.showAbsolutePotential, (newValue, oldValue) => {
        setupGauge(props.countriesList, newValue);
    })

const setupGauge = async (countriesList, showAbsolutePotential) => {
    d3
        .select('.chartContainer').remove()

    const svg = d3
        .select('#svgContainer')
        .append('svg')
        .attr('class','chartContainer')
        .attr('width', svgWidth)
        .attr('height', svgHeight);
    const data = collaborationStore.getCostOfAchievingMaximumMitigationPotentialInAutarkyvsCollaboration(countriesList)

    const costNotPotential = !showAbsolutePotential
    const unit = costNotPotential ? '$/tCO2e' : 'MtCO2e'
    const highValue = costNotPotential ? data.mitigationCostAutarky : data.mitigationPotentialCollaboration;
    const lowValue = costNotPotential ? data.mitigationCostCollaboration : data.mitigationPotentialAutarky;;
    const lowValueFillColor = costNotPotential ? collaborationColor:autarkyColor;
    const highValueFillColor = costNotPotential ? autarkyColor : collaborationColor;

    const lowValuePrompt = costNotPotential ?  "Collaboration": "Autarky" 
    const highValuePrompt = costNotPotential ?  "Autarky": "Collaboration" 
    const maxValue = Math.round(highValue * 0.14) * 10;

    const lowPercentage = lowValue / maxValue;
    const highPercentage = highValue / maxValue;



    // Create the D3.js bar chart within the SVG container
    const chartWidth = 600;
    const chartHeight = 360;
    const balloonAreaHeight = 0;
    const chart = svg
        .append('g')
        .attr(
            'transform',
            `scale(${svgWidth / chartWidth}, ${svgHeight / chartHeight}) translate(0, 160)`
        )
        .attr('width', chartWidth)
        .attr('height', chartHeight);

    const balloonArea = svg
        .append('g')
        .attr('transform', `scale(${svgWidth / chartWidth}, ${svgHeight / chartHeight})`)
        .attr('width', chartWidth)
        .attr('height', chartHeight);

    const rectWidth = 600;
    const rectHeight = 125;
    const cornerRadius = 3 + 0.5 * rectHeight; // Adjust this value to control the curvature

    const emptyFillColor = 'white';

    const outlineColor = highValueFillColor;
    const borderThickness = 4;

    const balloonHeight = 130;
    const balloonWidth = 130;
    const balloonPrompt = unit;

    // text balloon
    const lowValueBalloon = balloonArea
        .append('rect')
        .attr('width', balloonWidth)
        .attr('height', balloonHeight-50)
        .attr('x', rectWidth * lowPercentage - balloonWidth + 30)
        .attr('y', 57)
        .attr('fill', emptyFillColor)
        .attr('stroke', lowValueFillColor)
        .attr('stroke-width', borderThickness);

        balloonArea
        .append('text')
        .text(lowValuePrompt)
        .attr('text-anchor', 'middle') // Align the text to the end (rightmost) of the rectangle
        .attr('font-size', '24px')
        .attr('x', rectWidth * lowPercentage - 0.5 * balloonWidth + 30)
        .attr('y', 47);

        
        const balloonUnitArea = balloonArea
        .append('rect')
        .attr('width', balloonWidth)
        .attr('height', 40)
        .attr('x', rectWidth * lowPercentage - balloonWidth + 30)
        .attr('y', 10 + balloonHeight - 40)
        .attr('fill', lowValueFillColor);
    balloonArea
        .append('text')
        .text(balloonPrompt)
        .attr('text-anchor', 'middle') // Align the text to the end (rightmost) of the rectangle
        .attr('font-size', '28px')
        .attr('x', rectWidth * lowPercentage - 0.5 * balloonWidth + 30)
        .attr('y', 10 + balloonHeight - 10)
        .attr('fill', 'white');
    balloonArea
        .append('text')
        .text(lowValue)
        .attr('text-anchor', 'middle') // Align the text to the end (rightmost) of the rectangle
        .attr('font-size', '38px')
        .attr('x', rectWidth * lowPercentage - 0.5 * balloonWidth + 30)
        .attr('y', 92);

    // Create a symbol generator for triangles
    const triangleSymbol = d3.symbol().type(d3.symbolTriangle);

    // Draw a triangle symbol
    balloonArea
        .append('path')
        .attr('d', triangleSymbol.size(300)()) // Adjust the size as needed
        .attr(
            'transform',
            `translate(${rectWidth * lowPercentage - 0}, ${balloonHeight + 10 + borderThickness + 3
            }) rotate(180)`
        ) // Position the triangle
        .attr('fill', lowValueFillColor); // Fill color

    // text balloon
    const highValueBalloon = balloonArea
        .append('rect')
        .attr('width', balloonWidth)
        .attr('height', balloonHeight-50)
        .attr('x', rectWidth * highPercentage - 30)
        .attr('y', 57)
        .attr('fill', emptyFillColor)
        .attr('stroke', highValueFillColor) // Add a dark blue outline
        .attr('stroke-width', borderThickness); // Adjust the outline width

        balloonArea
        .append('text')
        .text(highValuePrompt)
        .attr('text-anchor', 'middle') // Align the text to the end (rightmost) of the rectangle
        .attr('font-size', '24px')
        .attr('x', rectWidth * highPercentage - 30 + 0.5 * balloonWidth)
        .attr('y', 47);

        // highvalue balloon
    balloonArea
        .append('rect')
        .attr('width', balloonWidth)
        .attr('height', 40)
        .attr('x', rectWidth * highPercentage - 30)
        .attr('y', 10 + balloonHeight - 40)
        .attr('fill', highValueFillColor);
    balloonArea
        .append('text')
        .text(balloonPrompt)
        .attr('text-anchor', 'middle') // Align the text to the end (rightmost) of the rectangle
        .attr('font-size', '28px')
        .attr('x', rectWidth * highPercentage - 30 + 0.5 * balloonWidth)
        .attr('y', 10 + balloonHeight - 10)
        .attr('fill', 'white');
    balloonArea
        .append('text')
        .text(highValue)
        .attr('text-anchor', 'middle') // Align the text to the end (rightmost) of the rectangle
        .attr('font-size', '38px')
        .attr('x', rectWidth * highPercentage - 30 + 0.5 * balloonWidth)
        .attr('y', 92);

    // Draw a triangle symbol
    balloonArea
        .append('path')
        .attr('d', triangleSymbol.size(300)()) // Adjust the size as needed
        .attr(
            'transform',
            `translate(${rectWidth * highPercentage - 0}, ${balloonHeight + 10 + borderThickness + 3
            }) rotate(180)`
        ) // Position the triangle
        .attr('fill', highValueFillColor); // Fill color

    // Create a clip path for the left curved corner
    svg.append('clipPath')
        .attr('id', 'left-clip')
        .append('rect')
        .attr('width', cornerRadius * 2)
        .attr('height', rectHeight);

    // Create a clip path for the right curved corner
    svg.append('clipPath')
        .attr('id', 'right-clip')
        .append('rect')
        .attr('width', cornerRadius * 2)
        .attr('height', rectHeight)
        .attr('x', rectWidth - cornerRadius);

    // Create the left orange part with a curved corner on the left
    chart
        .append('rect')
        .attr('width', rectWidth * lowPercentage)
        .attr('height', rectHeight)
        .attr('rx', cornerRadius)
        .attr('ry', cornerRadius)
        .attr('fill', lowValueFillColor)
        .attr('clip-path', 'url(#left-clip)')
        .attr('class', 'maximum-potential');

    chart
        .append('rect')
        .attr('width', rectWidth * lowPercentage - cornerRadius * 2 + 1)
        .attr('height', rectHeight)
        .attr('x', cornerRadius * 2 - 1)
        .attr('fill', lowValueFillColor)
        .attr('class', 'maximum-potential');

    // Create the central part for the high value  with no curved corners
    chart
        .append('rect')
        .attr('width', rectWidth * (highPercentage - lowPercentage))
        .attr('height', rectHeight)
        .attr('x', rectWidth * lowPercentage) // Position it after the orange part
        .attr('fill', highValueFillColor)
        .attr('class', 'maximum-potential');

    // // Create the right empty part with a curved corner on the right
    // chart.append("rect")
    //     .attr("width", rectWidth * (1 - highPercentage))
    //     .attr("height", rectHeight)
    //     .attr("rx", cornerRadius)
    //     .attr("ry", cornerRadius)
    //     .attr("x", rectWidth * highPercentage)
    //     .attr("fill", emptyFillColor)
    //     .attr("clip-path", "url(#right-clip)")
    //     .attr("stroke", outlineColor)
    //     .attr("stroke-width", borderThickness);    // Adjust the outline width
    // ;

    const centerX = rectWidth - 0.5 * rectHeight; // Y-coordinate of the center
    const centerY = 0.5 * rectHeight; // Y-coordinate of the center
    const radius = 0.5 * rectHeight - 0.5 * borderThickness; // Radius of the semicircle

    // Create a path for the semicircle
    const pathData = `M ${centerX} ${centerY - radius} A ${radius} ${radius} 0 1 1 ${centerX} ${centerY + radius
        }`;

    chart
        .append('path')
        .attr('d', pathData)
        .attr('fill', emptyFillColor)
        .attr('stroke', outlineColor)
        .attr('stroke-width', borderThickness);

    chart
        .append('rect')
        .attr('width', rectWidth * (1 - highPercentage) - 0.9 * cornerRadius)
        .attr('height', rectHeight)
        .attr('x', rectWidth * highPercentage)
        .attr('fill', emptyFillColor)
        .attr('fill-opacity', 0);

    chart
        .append('rect')
        .attr('width', rectWidth * (1 - highPercentage) - 0.9 * cornerRadius)
        .attr('height', borderThickness)
        .attr('x', rectWidth * highPercentage)
        .attr('y', rectHeight - borderThickness)
        .attr('fill', highValueFillColor);

    chart
        .append('rect')
        .attr('width', rectWidth * (1 - highPercentage) - 0.9 * cornerRadius)
        .attr('height', borderThickness)
        .attr('x', rectWidth * highPercentage)
        .attr('fill', highValueFillColor);

    chart
        .append('text')
        .text('0')
        .attr('text-anchor', 'start') // Align the text to the end (rightmost) of the rectangle
        .attr('font-size', '48px')
        .attr('x', 1) // Adjust the x-coordinate as needed
        .attr('y', rectHeight + borderThickness + 70);

    chart
        .append('text')
        .text(maxValue)
        .attr('text-anchor', 'end') // Align the text to the end (rightmost) of the rectangle
        .attr('font-size', '48px')
        .attr('x', rectWidth) // Adjust the x-coordinate as needed
        .attr('y', rectHeight + borderThickness + 70);
};
</script>

<style scoped>
.maximum-potential {
    opacity: 1;
}
</style>
