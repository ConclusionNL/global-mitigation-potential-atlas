<template>
    <div>
        <introCard></introCard>
        <div class="row">
            <div class="side-nav">
                <homeSidenav :countries="props.countries"></homeSidenav>
            </div>
            <div class="col-10 p-0">
                <worldMap :countries="props.countries"></worldMap>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import homeSidenav from './components/home-sidenav.vue';
import worldMap from './components/world-map.vue';
import introCard from './components/intro-card.vue';
import { useCollaborationStore } from './stores/collaborationStore';
import { ref, computed, defineProps } from 'vue';

// COMPOSABLE LOGIC FOR SELECTED_COUNTRIES
// import { useSelectedCountries } from './composables/useSelectedCountries';
// const useCountries = useSelectedCountries();
// const selectedCountries = useCountries.selectedCountries;

//import StackedAreaPlusBar from './components/StackedArea.vue'

const props = defineProps<{
    countries: {};
}>();

//const countries = ref([])
//const selectedTechnology = ref("")
const selectedCountryNav = ref('');
//const isChecked = ref(true);
//countries.value.push("SG")

//function handleButtonClick(collboaratingCountries) {
//    countries.value = collboaratingCountries
//}

//function handleBarClick(eventData) {
//    console.log('Bar Clicked:', eventData);
//    selectedTechnology.value = eventData.index.series
//}

function handleCountryNavClick(country: any) {
    selectedCountryNav.value = country;
}

const collaborationStore = useCollaborationStore();
//TODO read from collaboration store which combinations of collaborating countries are even available - in addition to the fake data set
</script>

<style lang="scss">
.side-nav {
    width: 200px;
    height: calc(100vh - 136px);
}
</style>
