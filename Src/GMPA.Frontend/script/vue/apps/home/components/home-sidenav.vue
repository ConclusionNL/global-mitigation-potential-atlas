<template>
    <h1 v-html="textOrStuff"></h1>
    <!--@model List<GMPA.Core.Models.CountryModel>-->
    <h4 class="country-divider" data-bs-toggle="collapse" data-bs-target="#collapseContinents" aria-expanded="true" aria-controls="collapseContinents">Countries</h4>
    <ul class="countries-list show" id="collapseContinents">

        <!-- #region Currently Active countries -->
        <li class="country-divider" data-bs-toggle="collapse" data-bs-target="#collapsePilot" aria-expanded="true" aria-controls="collapsePilot">
            <b>Pilot</b>
        </li>
        @foreach (var country in Model.Where(country => country.Active?.First() == "True"))
        {
        <li class="countries-list-item-active show" id="collapsePilot">
            <a href="@country.Url" class="countries-link-active">
                <span class="side-nav-list-item-text">
                    @country.Name
                </span>
            </a>
        </li>
        }

        <!-- #endregion -->
        <!-- #region Africa-->
        <li class="country-divider" data-bs-toggle="collapse" data-bs-target="#collapseAfrica" aria-expanded="true" aria-controls="collapseAfrica">
            <b>Africa</b>
        </li>

        @foreach (var country in Model.Where(country => country.Continent.First() == "Africa"))
        {
        if (country.Active.First() == "True")
        {
        <li class="countries-list-item-active show" id="collapseAfrica">
            <a href="@country.Url" class="countries-link-active">
                <span class="side-nav-list-item-text">
                    @country.Name
                </span>
            </a>
        </li>
        }
        else
        {
        <li class="countries-list-item-disabled show" id="collapseAfrica">
            <span class="side-nav-list-item-text">
                @country.Name
            </span>
        </li>
        }
        }
        <!-- #endregion -->
        <!-- #region Asia-->
        <li class="country-divider" data-bs-toggle="collapse" data-bs-target="#collapseAsia" aria-expanded="true" aria-controls="collapseAsia">
            <b>Asia</b>
        </li>

        @foreach (var country in Model.Where(country => country.Continent.First() == "Asia"))
        {
        if (country.Active.First() == "True")
        {
        <li class="countries-list-item-active show" id="collapseAsia">
            <a href="@country.Url" class="countries-link-active">
                <span class="side-nav-list-item-text">
                    @country.Name
                </span>
            </a>
        </li>
        }
        else
        {
        <li class="countries-list-item-disabled show" id="collapseAsia">
            <span class="side-nav-list-item-text">
                @country.Name
            </span>
        </li>
        }
        }
        <!-- #endregion -->
        <!-- #region Europe-->
        <li class="country-divider" data-bs-toggle="collapse" data-bs-target="#collapseEurope" aria-expanded="true" aria-controls="collapseEurope">
            <b>Europe</b>
        </li>

        @foreach (var country in Model.Where(country => country.Continent.First() == "Europe"))
        {
        if (country.Active.First() == "True")
        {
        <li class="countries-list-item-active show" id="collapseEurope">
            <a href="@country.Url" class="countries-link-active">
                <span class="side-nav-list-item-text">
                    @country.Name
                </span>
            </a>
        </li>
        }
        else
        {
        <li class="countries-list-item-disabled show" id="collapseEurope">
            <span class="side-nav-list-item-text">
                @country.Name
            </span>
        </li>
        }
        }
        <!-- #endregion -->
        <!-- #region North America-->
        <li class="country-divider" data-bs-toggle="collapse" data-bs-target="#collapseNorthAmerica" aria-expanded="true" aria-controls="collapseNorthAmerica">
            <b>North America</b>
        </li>

        @foreach (var country in Model.Where(country => country.Continent.First() == "North America"))
        {
        if (country.Active.First() == "True")
        {
        <li class="countries-list-item-active show" id="collapseNorthAmerica">
            <a href="@country.Url" class="countries-link-active">
                <span class="side-nav-list-item-text">
                    @country.Name
                </span>
            </a>
        </li>
        }
        else
        {
        <li class="countries-list-item-disabled show" id="collapseNorthAmerica">
            <span class="side-nav-list-item-text">
                @country.Name
            </span>
        </li>
        }
        }
        <!-- #endregion -->
        <!-- #region Oceania-->
        <li class="country-divider" data-bs-toggle="collapse" data-bs-target="#collapseOceania" aria-expanded="true" aria-controls="collapseOceania">
            <b>Oceania</b>
        </li>

        @foreach (var country in Model.Where(country => country.Continent.First() == "Oceania"))
        {
        if (country.Active.First() == "True")
        {
        <li class="countries-list-item-active show" id="collapseOceania">
            <a href="@country.Url" class="countries-link-active">
                <span class="side-nav-list-item-text">
                    @country.Name
                </span>
            </a>
        </li>
        }
        else
        {
        <li class="countries-list-item-disabled show" id="collapseOceania">
            <span class="side-nav-list-item-text">
                @country.Name
            </span>
        </li>
        }
        }
        <!-- #endregion -->
        <!-- #region South America-->
        <li class="country-divider" data-bs-toggle="collapse" data-bs-target="#collapseSouthAmerica" aria-expanded="true" aria-controls="collapseSouthAmerica">
            <b>South America</b>
        </li>

        @foreach (var country in Model.Where(country => country.Continent.First() == "South America"))
        {
        if (country.Active.First() == "True")
        {
        <li class="countries-list-item-active show" id="collapseSouthAmerica">
            <a href="@country.Url" class="countries-link-active">
                <span class="side-nav-list-item-text">
                    @country.Name
                </span>
            </a>
        </li>
        }
        else
        {
        <li class="countries-list-item-disabled show" id="collapseSouthAmerica">
            <span class="side-nav-list-item-text">
                @country.Name
            </span>
        </li>
        }
        }
        <!-- #endregion -->
    </ul>
</template>

<script lang="ts">
    export default {
        props: {
            countries: Array,
            textOrStuff: String,
        }
    };
</script>

<style lang="scss">
    .test {
        background: white;
    }
</style>