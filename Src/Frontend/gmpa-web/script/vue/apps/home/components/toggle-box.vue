<template>
    <div class="mitigation-card">
        <div v-for="(mitigation, i) in mitgiationList" :key="i" class="radio-text">
            <input :id="mitigation" :checked="i == 0" type="radio" :value="mitigation" name="mitigation"
                @change="selectedMitigation = mitigation" />
            <label :for="mitigation">{{ mitigation.replace(/_/g, " ") }}</label>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue';
const mitgiationList = ref([]);
const selectedMitigation = ref(mitgiationList.value[0]);

const emit = defineEmits(['mitigation-value']);

watch(selectedMitigation, (newMitigation) => {
    emit('mitigation-value', newMitigation);
});

onMounted(() => {
    mitgiationList.value = [
        'None',
        'Mitigation_Potential(GtCO2e)',
        'Mitigation_Cost($/GtCO2e)',
        'Mitigation_Potential(GtCO2e)_at_50',
        'Mitigation_Potential(GtCO2e)_at_100',
        'Mitigation_Potential(GtCO2e)_at_200',
    ];

    emit('mitigation-value', mitgiationList.value[0]);
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
