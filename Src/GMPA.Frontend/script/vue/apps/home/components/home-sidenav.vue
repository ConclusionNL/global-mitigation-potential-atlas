<template>
    asda
    <div class="container-fluid p-0">
        <div class="row p-0">
            <div class="col-2 shadow h-50 sticky-top t-100" style="background: white">
                <partial name="{{partialName}}" model="{{countriesModel}}" />
                {{textOrStuff}}
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    export default {
        props: {
            countries: Array,
        }
    };
</script>

<style lang="scss">
    .test {
        background: white;
    }
</style>