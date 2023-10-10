<template>
    
    <ul class="countries-list show">
        <li v-for="continent in continents" >
            <div class="country-divider">
                {{continent}}
            </div>
            <div v-for="country in props.countries">
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
    import { callWithAsyncErrorHandling, defineProps, reactive } from "vue";

    const props = defineProps({
        countries: {},
    })

    var continents = ["Asia", "Africa", "North America", "South America", "Europe", "Oceania"]

</script>

<style lang="scss">
    .test {
        background: white;
    }
</style>