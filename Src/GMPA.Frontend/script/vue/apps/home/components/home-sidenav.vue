<template>
    <ul class="countries-list show">
        <!-- #region Pilot countries-->
        <li>
            <div class="country-divider">
                Pilot
            </div>
        <li v-for="country in props.countries">
            <div v-if="country.Active == 'True'">
                <a class="countries-link-active" :href=country.Url>{{country.Name}}</a>
            </div>
        </li>
        </li>
        <!-- #endregion -->

        <li v-for="continent in continents" @click="toggleCollapse" :class="{'collapsed': isCollapsed}">
            <div class="country-divider">
                {{continent}}
            </div>
            <div v-for="country in props.countries" v-show="isCollapsed" class="collapsible-content">
        <li v-if="country.Continent == continent">
            <div class="countries-list-item-active show">
                <div v-if="country.Active == 'True'">
                    <a class="countries-link-active" :href=country.Url>{{country.Name}}</a>
                </div>
                <div v-else class="countries-list-item-disabled">
                    {{country.Name}}
                </div>
            </div>
        </li>
        </div>
        </li>
    </ul>
</template>

<script lang="ts" setup>
    import { defineProps, ref } from "vue";

    const props = defineProps({
        countries: {},
    })

    const continents = ["Asia", "Africa", "North America", "South America", "Europe", "Oceania"]

    const isCollapsed = ref(true);

    const toggleCollapse = () => {
        console.log(isCollapsed.value);
        isCollapsed.value = !isCollapsed.value;
    };

</script>

<style lang="scss">
    .collapsible-content {
        max-height: 0;
        overflow: hidden;
        transition: max-height 0.5s ease-in-out;
    }

    .collapsed .collapsible-content {
        max-height: 500px; /* Set an appropriate max-height value */
    }
</style>