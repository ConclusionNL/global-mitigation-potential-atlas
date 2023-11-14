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

        let autarkyTranslate = Math.min( rectWidth - 2* balloonWidth - 10, Math.max(rectWidth * lowPercentage - balloonWidth+10,3))
        let collaborationTranslate = Math.max( Math.min( rectWidth * highPercentage - 10, rectWidth- balloonWidth-3),  balloonWidth+10)

        // // cater for low and high close together
        // // high close to max
        // // high close 0  (compared to max)
        // if ((collaborationTranslate - autarkyTranslate) < 100) {
        //     // too close together
        //     if (autarkyTranslate > 300) {
        //         autarkyTranslate -= 30
        //         collaborationTranslate += 17
        //     } else { collaborationTranslate += 30 }
        // }

        const autarkyBalloon = balloonArea.append('g').attr('transform', `translate(${autarkyTranslate},0)`)
        const collaborationBalloon = balloonArea.append('g').attr('transform', `translate(${collaborationTranslate},0)`)

        const createBalloon = (balloonGroup,value, prompt, valueFillColor) => {
            // text balloon
            balloonGroup
                .append('rect')
                .attr('width', balloonWidth)
                .attr('height', balloonHeight - 50)
                .attr('x', 0)
                .attr('y', 57)
                .attr('fill', emptyFillColor)
                .attr('stroke', valueFillColor)
                .attr('stroke-width', borderThickness)

                ;

            balloonGroup
                .append('text')
                .text(prompt)
                .attr('text-anchor', 'middle') // Align the text to the end (rightmost) of the rectangle
                .attr('font-size', '24px')
                .attr('x', 0.5 * balloonWidth)
                .attr('y', 47);


            balloonGroup
                .append('rect')
                .attr('width', balloonWidth)
                .attr('height', 40)
                .attr('x', 0)
                .attr('y', 10 + balloonHeight - 40)
                .attr('fill', valueFillColor);
            balloonGroup
                .append('text')
                .text(balloonUnitPrompt)
                .attr('text-anchor', 'middle') // Align the text to the end (rightmost) of the rectangle
                .attr('font-size', '28px')
                .attr('x', 0.5 * balloonWidth)
                .attr('y', 10 + balloonHeight - 10)
                .attr('fill', 'white');
            balloonGroup
                .append('text')
                .text(value)
                .attr('text-anchor', 'middle') // Align the text to the end (rightmost) of the rectangle
                .attr('font-size', '38px')
                .attr('x', 0.5 * balloonWidth)
                .attr('y', 92);
        }
        createBalloon(autarkyBalloon,lowValue.toFixed(1),lowValuePrompt, lowValueFillColor)
        createBalloon(collaborationBalloon,highValue.toFixed(1),highValuePrompt, highValueFillColor)
        // Create a symbol generator for triangles
        const triangleSymbol = d3.symbol().type(d3.symbolTriangle);

        // Draw a triangle symbol
        balloonArea
            .append('path')
            .attr('d', triangleSymbol.size(300)()) // Adjust the size as needed
            .attr(
                'transform',
                `translate(${Math.max(10, rectWidth * lowPercentage)}, ${balloonHeight + 10 + borderThickness + 3
                }) rotate(180)`
            ) // Position the triangle
            .attr('fill', lowValueFillColor); // Fill color


        // Draw a triangle symbol
        balloonArea
            .append('path')
            .attr('d', triangleSymbol.size(300)()) // Adjust the size as needed
            .attr(
                'transform',
                `translate(${Math.max(20, Math.min(rectWidth - 10, rectWidth * highPercentage - 0))}, ${balloonHeight + 10 + borderThickness + 3
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

    const lowValue = data[`mitigationPotentialAutarky${mitigationLevel}`];
    const highValue = data[`mitigationPotentialCollaboration${mitigationLevel}`];
    const maxValue = data['mitigationPotentialCollaborationMax'];
    // const lowValue = 0.2
    // const highValue = 0.4
    // const maxValue = 10

    console.log(`low : ${lowValue} high : ${highValue} max: ${maxValue}`)

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
    const balloonUnitPrompt = unit;


    drawBalloon()
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

    if (highPercentage < (1 - 0.5 * rectHeight / rectWidth)) {
        chart // empty borderless rectangle on top of (to hide) right border of "empty" rectangle 
            .append('rect')
            .attr('width', borderThickness + 2)
            .attr('height', rectHeight - 2 * borderThickness)
            .attr('x', rectWidth - 0.5 * rectHeight - borderThickness)
            .attr('y', borderThickness)
            .attr('fill', emptyFillColor)
            .attr('stroke', emptyFillColor)
            .attr('stroke-width', borderThickness);
    }
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
    if (highPercentage < 0.5 * rectHeight / rectWidth) {
        chart // empty borderless rectangle on top of (to hide) left border of "empty" rectangle 
            .append('rect')
            .attr('width', borderThickness + 2)
            .attr('height', rectHeight - 2 * borderThickness)
            .attr('x', 0.5 * rectHeight - borderThickness)
            .attr('y', borderThickness)
            .attr('fill', emptyFillColor)
            .attr('stroke', emptyFillColor)
            .attr('stroke-width', borderThickness);
    }
    if (lowPercentage > 0.5 * rectHeight / rectWidth) {
        chart
            .append('rect')
            .attr('width', Math.min(rectWidth * lowPercentage - 0.5 * rectHeight, rectWidth - rectHeight))
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
        .text(parseFloat(maxValue).toFixed(1))
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
