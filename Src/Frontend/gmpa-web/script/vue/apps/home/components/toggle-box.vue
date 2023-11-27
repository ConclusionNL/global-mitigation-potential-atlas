<template>
    <div class="mitigation-card">
        Annual Mitigation Potential (MtCO2e) at Additional Average Abatement Cost<br />  (Compared to Current Energy Mix) of:
        <div v-for="(mitigation, i) in mitigationList" :key="i" class="radio-text">
            <input :id="mitigation" :checked="i == 4" type="radio" :value="mitigation.value" name="mitigation"
                @change="selectedMitigation = mitigation" />
            <label :for="mitigation"><span v-html="mitigation.label"></span></label>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue';
const mitigationList = ref([]);
const selectedMitigation = ref(mitigationList.value[4]);

const emit = defineEmits(['mitigation-value']);

watch(selectedMitigation, (newMitigation) => {
    console.log(`togglebox selected mitigation changed to ${newMitigation.label} `)
    emit('mitigation-value', newMitigation);
});

onMounted(() => {
    
    mitigationList.value = [
        { label: 'Net Zero Additional Cost', value: "Mitigation_Potential_at_0" },
        { label: '10 $/tCO2e', value: "Mitigation_Potential_at_10" },
        { label: '20 $/tCO2e', value: "Mitigation_Potential_at_20" },
        { label: '50 $/tCO2e', value: "Mitigation_Potential_at_50" },
        { label: 'No cost limit', value: "Mitigation_Potential" },
        { label: 'Additional Average Abatement Cost (Compared to Current Energy Mix)<br />  to Reach Net Zero Emissions ($ /tCO2e)', value: "Mitigation_Cost_NetZero" },
    ];

    emit('mitigation-value', mitigationList.value[4]);
});
</script>

<style scoped>
.mitigation-card {
    padding: 10px;
    width: fit-content;
    height: fit-content;
    background: rgba(255, 255, 255, 0.5);
    border-radius: 8px;
    font-size: 13px;
}

input[type='radio'] {
    margin-right: 8px;
}

/* Media query for screens less than 1200 pixels wide */
@media screen and (max-height: 750px) {
    .mitigation-card {
        padding: 3px;
        border-radius: 4px;
        font-size: 12px;
    }
}
</style>
