<template>
    <h4 class="country-divider">Countries</h4>
    <ul class="countries-list">
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
        <div v-for="(continent, i) in continents" :index="continent">
        <li>
            <div class="country-divider" @click="continentsCollapsable[i] = !continentsCollapsable[i]">
                {{continent}}
            </div>
            <div v-for="country in props.countries" class="expander" :class="{ 'expanded': continentsCollapsable[i] }">
                <div class="expander-content">
                    <li v-if="country.Continent == continent">
                        <div class="countries-list-item-active">
                            <div v-if="country.Active == 'True'">
                                <a class="countries-link-active" :href=country.Url>{{country.Name}}</a>
                            </div>
                            <div v-else class="countries-list-item-disabled">
                                {{country.Name}}
                            </div>
                        </div>
                    </li>
                </div>
            </div>
        </li>
        </div>
    </ul>
</template>

<script lang="ts" setup>
    import { defineProps, ref } from "vue";

    const props = defineProps({
        countries: {},
    })

    const continents = ["Africa", "Asia", "North America", "South America", "Europe", "Oceania"]
    const continentsCollapsable = ref([]);

    continents.forEach(continent => {
        continentsCollapsable.value.push(true);
    });
</script>

<style lang="scss">
    .expander {
        display: grid;
        grid-template-rows: 0fr;
        overflow: hidden;
        transition: grid-template-rows 0.2s;
    }

    .expander-content {
        min-height: 0;
        transition: visibility 0.2s;
        visibility: hidden;
    }

    .expander.expanded {
        grid-template-rows: 1fr;
    }

    .expander.expanded .expander-content {
        visibility: visible;
    }
</style>