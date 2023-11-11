<template>
    {{ props.mitigationLevel }}
    <div id="svgContainer"></div>
</template>

<script setup>
import { ref, watch, onMounted, defineProps, defineEmits, computed } from 'vue';
import * as d3 from 'd3';
import { useCollaborationStore } from '../stores/collaborationStore';

const collaborationStore = useCollaborationStore();
const props = defineProps({
    countriesList: []
    , mitigationLevel: ''
});
const svgWidth = 420;
const svgHeight = 180;
const showMitigationCostsProxy = ref(props.showMitigationCosts);
const autarkyColor = "darkblue"
const collaborationColor = "#f07004"


onMounted(() => {
    setupGauge(props.countriesList, props.mitigationLevel);
});


watch(() => props.mitigationLevel, (newValue, oldValue) => {
    setupGauge(props.countriesList, newValue);
})


watch(() => props.countriesList, (newValue, oldValue) => {
    setupGauge(newValue, props.mitigationLevel);
})


const setupGauge = async (countriesList, mitigationLevel) => {


    const drawBalloon = () => {
        // TODO - position balloons and triangles within range and without overlap

        // triangles must point at area, not necessarily at exact boundary
        const balloonAreaHeight = 0;
        const balloonArea = svg
            .append('g')
            .attr('transform', `scale(${svgWidth / chartWidth}, ${svgHeight / chartHeight})`)
            .attr('width', chartWidth)
            .attr('height', chartHeight);

        const autarkyBalloon = balloonArea.append('g').attr('transform', `translate(50,0)`)
        const collaborationBalloon = balloonArea.append('g').attr('transform', `translate(50,0)`)

        // text balloon
        const lowValueBalloon = autarkyBalloon
            .append('rect')
            .attr('width', balloonWidth)
            .attr('height', balloonHeight - 50)
            .attr('x', rectWidth * lowPercentage - balloonWidth + 30)
            .attr('y', 57)
            .attr('fill', emptyFillColor)
            .attr('stroke', lowValueFillColor)
            .attr('stroke-width', borderThickness)

            ;

        autarkyBalloon
            .append('text')
            .text(lowValuePrompt)
            .attr('text-anchor', 'middle') // Align the text to the end (rightmost) of the rectangle
            .attr('font-size', '24px')
            .attr('x', rectWidth * lowPercentage - 0.5 * balloonWidth + 30)
            .attr('y', 47);


        const balloonUnitArea = autarkyBalloon
            .append('rect')
            .attr('width', balloonWidth)
            .attr('height', 40)
            .attr('x', rectWidth * lowPercentage - balloonWidth + 30)
            .attr('y', 10 + balloonHeight - 40)
            .attr('fill', lowValueFillColor);
        autarkyBalloon
            .append('text')
            .text(balloonPrompt)
            .attr('text-anchor', 'middle') // Align the text to the end (rightmost) of the rectangle
            .attr('font-size', '28px')
            .attr('x', rectWidth * lowPercentage - 0.5 * balloonWidth + 30)
            .attr('y', 10 + balloonHeight - 10)
            .attr('fill', 'white');
        autarkyBalloon
            .append('text')
            .text(lowValue)
            .attr('text-anchor', 'middle') // Align the text to the end (rightmost) of the rectangle
            .attr('font-size', '38px')
            .attr('x', rectWidth * lowPercentage - 0.5 * balloonWidth + 30)
            .attr('y', 92);

        // Create a symbol generator for triangles
        const triangleSymbol = d3.symbol().type(d3.symbolTriangle);

        // Draw a triangle symbol
        autarkyBalloon
            .append('path')
            .attr('d', triangleSymbol.size(300)()) // Adjust the size as needed
            .attr(
                'transform',
                `translate(${rectWidth * lowPercentage - 0}, ${balloonHeight + 10 + borderThickness + 3
                }) rotate(180)`
            ) // Position the triangle
            .attr('fill', lowValueFillColor); // Fill color


        // text balloon
        const highValueBalloon = collaborationBalloon
            .append('rect')
            .attr('width', balloonWidth)
            .attr('height', balloonHeight - 50)
            .attr('x', rectWidth * highPercentage - 30)
            .attr('y', 57)
            .attr('fill', emptyFillColor)
            .attr('stroke', highValueFillColor) // Add a dark blue outline
            .attr('stroke-width', borderThickness); // Adjust the outline width

        collaborationBalloon
            .append('text')
            .text(highValuePrompt)
            .attr('text-anchor', 'middle') // Align the text to the end (rightmost) of the rectangle
            .attr('font-size', '24px')
            .attr('x', rectWidth * highPercentage - 30 + 0.5 * balloonWidth)
            .attr('y', 47);

        // highvalue balloon
        collaborationBalloon
            .append('rect')
            .attr('width', balloonWidth)
            .attr('height', 40)
            .attr('x', rectWidth * highPercentage - 30)
            .attr('y', 10 + balloonHeight - 40)
            .attr('fill', highValueFillColor);
        collaborationBalloon
            .append('text')
            .text(balloonPrompt)
            .attr('text-anchor', 'middle') // Align the text to the end (rightmost) of the rectangle
            .attr('font-size', '28px')
            .attr('x', rectWidth * highPercentage - 30 + 0.5 * balloonWidth)
            .attr('y', 10 + balloonHeight - 10)
            .attr('fill', 'white');
        collaborationBalloon
            .append('text')
            .text(highValue)
            .attr('text-anchor', 'middle') // Align the text to the end (rightmost) of the rectangle
            .attr('font-size', '38px')
            .attr('x', rectWidth * highPercentage - 30 + 0.5 * balloonWidth)
            .attr('y', 92);

        // Draw a triangle symbol
        collaborationBalloon
            .append('path')
            .attr('d', triangleSymbol.size(300)()) // Adjust the size as needed
            .attr(
                'transform',
                `translate(${rectWidth * highPercentage - 0}, ${balloonHeight + 10 + borderThickness + 3
                }) rotate(180)`
            ) // Position the triangle
            .attr('fill', highValueFillColor); // Fill color

    }

    d3
        .select('.chartContainer').remove()

    const svg = d3
        .select('#svgContainer')
        .append('svg')
        .attr('class', 'chartContainer')
        .attr('width', svgWidth)
        .attr('height', svgHeight)
        ;
    const data = collaborationStore.getCostOfAchievingMaximumMitigationPotentialInAutarkyvsCollaboration(countriesList)

    const unit = 'MtCO2e'

    // TODO remove Math.min and Math.max
    const highValue = Math.max( data[`mitigationPotentialAutarky${mitigationLevel}`],data[`mitigationPotentialCollaboration${mitigationLevel}`]);
    const lowValue = Math.min( data[`mitigationPotentialAutarky${mitigationLevel}`],data[`mitigationPotentialCollaboration${mitigationLevel}`]);
    const maxValue =  Math.max( data['mitigationPotentialCollaborationMax'], 1.1 * highValue);
    
    // TODO check on values highValue > lowValue  and highValue < maxValue


    const lowValueFillColor = autarkyColor;
    const highValueFillColor = collaborationColor;

    const lowValuePrompt = "Autarky"
    const highValuePrompt = "Collaboration"

    const lowPercentage = lowValue / maxValue;
    const highPercentage = highValue / maxValue;



    // Create the D3.js bar chart within the SVG container
    const chartWidth = 600;
    const chartHeight = 360;
    const chart = svg
        .append('g')
        .attr(
            'transform',
            `scale(${svgWidth / chartWidth}, ${svgHeight / chartHeight}) translate(0, 160)`
        )
        .attr('width', chartWidth)
        .attr('height', chartHeight);


    const rectWidth = 600;
    const rectHeight = 125;
    const cornerRadius = 3 + 0.5 * rectHeight; // Adjust this value to control the curvature

    const emptyFillColor = 'white';

    const outlineColor = highValueFillColor;
    const lowValueOutlineColor = lowValueFillColor;
    const borderThickness = 4;

    const balloonHeight = 130;
    const balloonWidth = 130;
    const balloonPrompt = unit;

    chart
        .append('text')
        .text(lowValue)
        .attr('text-anchor', 'start') // Align the text to the end (rightmost) of the rectangle
        .attr('font-size', '28px')
        .attr('x', 1) // Adjust the x-coordinate as needed
        .attr('y', -35 + rectHeight + borderThickness + 70);

    chart
        .append('text')
        .text(highValue)
        .attr('text-anchor', 'end') // Align the text to the end (rightmost) of the rectangle
        .attr('font-size', '28px')
        .attr('x', rectWidth) // Adjust the x-coordinate as needed
        .attr('y', -25 + rectHeight + borderThickness + 70);

    drawBalloon()
    let leftCenterX = 0.5 * rectHeight;
    let centerY = 0.5 * rectHeight; // Y-coordinate of the center
    let radius = 0.5 * rectHeight - 0.5 * borderThickness; // Radius of the semicircle

    const leftEnd = chart.append('g')
    leftEnd
        .append('rect')
        .attr('width', 0.5 * rectHeight)
        .attr('height', rectHeight)
        .attr('x', 0)
        .attr('fill', emptyFillColor)
        .attr('stroke', outlineColor)
        .attr('stroke-width', borderThickness);
    leftEnd
        .append('rect')
        .attr('width', rectWidth * highPercentage)
        .attr('height', rectHeight)
        .attr('x', 0)
        .attr('fill', highValueFillColor)
        .attr('stroke', highValueFillColor)
        .attr('stroke-width', borderThickness);
    leftEnd
        .append('rect')
        .attr('width', rectWidth * lowPercentage)
        .attr('height', rectHeight)
        .attr('x', 0)
        .attr('fill', lowValueFillColor)
        .attr('stroke', lowValueFillColor)
        .attr('stroke-width', borderThickness);

    // Create a clipPath for the left end curvature
    svg.append("clipPath")
        .attr("id", `clipLeftCircle`)
        .append("circle")
        .attr("cx", 0.5 * rectHeight + borderThickness)
        .attr("cy", centerY)
        .attr("r", 0.5 * rectHeight + borderThickness)
    // apply a circular peephole to the left side of the gauge    
    leftEnd.attr("clip-path", `url(#clipLeftCircle)`);

    // reapply rectangles - starting at 0.5 * rectHeight
    const rightEnd = chart.append('g')


    rightEnd
        .append('rect')
        .attr('width', rectWidth - 0.5 * rectHeight)
        .attr('height', rectHeight)
        .attr('x', 0.5 * rectHeight)
        .attr('fill', emptyFillColor)
        .attr('stroke', outlineColor)
        .attr('stroke-width', borderThickness);
    rightEnd
        .append("circle")
        .attr("cx", rectWidth - 0.5 * rectHeight)
        .attr("cy", centerY)
        .attr("r", 0.5 * rectHeight)
        .attr('fill', emptyFillColor)
        .attr('stroke', outlineColor)
        .attr('stroke-width', borderThickness);

    if (highPercentage > 0.5 * rectHeight / rectWidth) {
        rightEnd
            .append('rect')
            .attr('width', rectWidth * highPercentage - 0.5 * rectHeight)
            .attr('height', rectHeight)
            .attr('x', 0.5 * rectHeight)
            .attr('fill', highValueFillColor)
            .attr('class', 'maximum-potential')
            .attr('stroke', highValueFillColor)
            .attr('stroke-width', borderThickness);
    }

    if (lowPercentage > 0.5 * rectHeight / rectWidth) {
        rightEnd
            .append('rect')
            .attr('width', rectWidth * lowPercentage - 0.5 * rectHeight)
            .attr('height', rectHeight)
            .attr('x', 0.5 * rectHeight)
            .attr('fill', lowValueFillColor)
            .attr('stroke', lowValueFillColor)
            .attr('stroke-width', borderThickness);
    }
    // Create a clipPath for the right end curvature
    svg.append("clipPath")
        .attr("id", `clipRightCircle`)
        .append("circle")
        .attr("cx", rectWidth - 0.5 * rectHeight - borderThickness)
        .attr("cy", centerY)
        .attr("r", 0.5 * rectHeight + borderThickness)
        .attr('stroke', outlineColor)
        .attr('stroke-width', borderThickness);

    // apply a circular peephole to the right side of the gauge    
    rightEnd.attr("clip-path", `url(#clipRightCircle)`);

    chart
        .append('rect')
        .attr('width', rectWidth - rectHeight)
        .attr('height', rectHeight)
        .attr('x', 0.5 * rectHeight)
        .attr('fill', emptyFillColor)
        .attr('stroke', outlineColor)
        .attr('stroke-width', borderThickness);

    chart // empty borderless rectangle on top of (to hide) right border of "empty" rectangle 
        .append('rect')
        .attr('width', borderThickness)
        .attr('height', rectHeight - 2 * borderThickness)
        .attr('x', rectWidth - 0.5 * rectHeight - borderThickness)
        .attr('y', borderThickness)
        .attr('fill', emptyFillColor)
        .attr('stroke', emptyFillColor)
        .attr('stroke-width', borderThickness);

    if (highPercentage > 0.5 * rectHeight / rectWidth) {
        chart
            .append('rect')
            .attr('width', Math.min(rectWidth * highPercentage, rectWidth - 0.5 * rectHeight) - 0.5 * rectHeight)
            .attr('height', rectHeight)
            .attr('x', 0.5 * rectHeight)
            .attr('fill', highValueFillColor)
            .attr('class', 'maximum-potential')
            .attr('stroke', highValueFillColor)
            .attr('stroke-width', borderThickness);
    }

    if (lowPercentage > 0.5 * rectHeight / rectWidth) {
        chart
            .append('rect')
            .attr('width', rectWidth * lowPercentage - 0.5 * rectHeight)
            .attr('height', rectHeight)
            .attr('x', 0.5 * rectHeight)
            .attr('fill', lowValueFillColor)
            .attr('stroke', lowValueFillColor)
            .attr('stroke-width', borderThickness);
    }



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
