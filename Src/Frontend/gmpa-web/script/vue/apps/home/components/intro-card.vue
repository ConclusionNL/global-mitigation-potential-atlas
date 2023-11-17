<template>
    <div v-if="show" class="intro-card">
        <div class="content">
            <div class="title">Welcome to the Global Mitigation Potential Atlas</div>
            <div class="instructions">
                Welcome to the pilot of the Global Mitigation Potential Atlas! The Atlas is designed to inspire
greater climate change mitigation ambition and implementation efforts, by presenting where
significant and potentially transformative opportunities for reducing emissions are located,
and what the key levers are for realising these opportunities. It fosters domestic action, as
well as international collaboration and learning.
<br/> <br/>
We welcome your feedback on this pilot version for Southeast Asia – please get in touch
with info@mitigationatlas.org
            </div>
            <div class="instructions">~Aniq Ashan</div>
            <br/>
            <div class="buttons">
                <button @click="handleExploreMapClick" class="btn btn-alt-1">Explore the map</button>
                <button class="btn btn-alt-2">Discover opportunities</button>
            </div>
        </div>
        <div>
            <img src="/public/aniq.png" />
        </div>
    </div>
</template>

<script setup>
import { ref } from 'vue';

// Set this to false to remove constant introcard pop-up
const show = ref((!sessionStorage.getItem("AtlasWelcomeCardWasShown")));

const handleExploreMapClick = () => {
    sessionStorage.setItem("AtlasWelcomeCardWasShown", true)
    show.value = !show.value
} 
</script>

<style scoped>
.intro-card {
    display: flex;
    position: absolute;
    margin: auto;
    left: 0;
    right: 0;
    top: 0;
    bottom: 0;
    width: 867px;
    height: 335px;
    border-radius: 24px;
    padding: 40px;
    gap: 24px;
    z-index: 999;
    background-color: white;
    box-shadow: 0px 4px 8px 0px #214b6352;
}

.content {
    display: flex;
    flex-direction: column;
    justify-content: space-between;
}

.btn,
.btn:focus,
.btn:active {
    border: none;
    padding: 12px 18px;
    text-align: center;
    text-decoration: none;
    display: inline-block;
    font-size: 16px;
    border-radius: 4px;
    cursor: pointer;
    font-weight: 500;
}

.btn-alt-1,
.btn-alt-1:focus,
.btn-alt-1:active {
    color: white;
    border: 1px solid #214b63;
    background-color: #214b63;
}

.btn-alt-2,
.btn-alt-2:focus,
.btn-alt-2:active {
    color: white;
    background-color: #f07004;
    border: 1px solid #f07004;
}

.buttons {
    display: flex;
    gap: 10px;
}

.title {
    font-size: 20px;
    font-weight: 500;
}

.instructions {
    font-size: 18px;
    line-height: 24px;
}
</style>
