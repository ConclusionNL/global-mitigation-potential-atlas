<template>
    <div class="mitigation-card">
        <div v-for="(mitigation, i) in mitigationList" :key="i" class="radio-text">
            <input :id="mitigation" :checked="i == 0" type="radio" :value="mitigation.value" name="mitigation"
                @change="selectedMitigation = mitigation" />
            <label :for="mitigation">{{ mitigation.label }}</label>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue';
const mitigationList = ref([]);
const selectedMitigation = ref(mitigationList.value[0]);

const emit = defineEmits(['mitigation-value']);

watch(selectedMitigation, (newMitigation) => {
    emit('mitigation-value', newMitigation);
});

onMounted(() => {
    mitigationList.value = [
    {label: 'None', value:"None"},
    {label: 'Mitigation Potential (MtCO2e)', value:"Mitigation_Potential(GtCO2e)"},
    {label: 'Mitigation Potential (MtCO2e) at 0', value:"Mitigation_Potential_at_0"},
    {label: 'Mitigation Potential (MtCO2e) at 50', value:"Mitigation_Potential_at_50"},
    {label: 'Mitigation Potential (MtCO2e) at 100', value:"Mitigation_Potential_at_100"},
    {label: 'Mitigation Potential (MtCO2e) at 200', value:"Mitigation_Potential_at_200"},
    ];

    emit('mitigation-value', mitigationList.value[0]);
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
</style>
