/* eslint-disable @typescript-eslint/no-explicit-any */
import type { Meta, StoryObj } from '@storybook/vue3';
import { ref, type Ref } from 'vue';
import ArlaSelect from '@arla-lib/script/vue/components/form-elements/ArlaSelect.vue';

interface Option {
    label: string;
    value: string;
    price: number;
}

const meta: Meta<typeof ArlaSelect> = {
    title: 'Componenten/Form elements/Select',
    component: ArlaSelect,
    tags: ['autodocs'],
    render: (args: any) => ({
        components: { ArlaSelect },
        setup(): { args: any; selected: Ref<Option>; options: Ref<Option[]> } {
            const options = ref<Option[]>([
                { label: 'Optie 1', value: 'optie1', price: 1 },
                { label: 'Optie 2', value: 'optie2', price: 2 },
                { label: 'Optie 3', value: 'optie3', price: 3 },
            ]);
            const selected = ref<Option>(options.value[0]);
            return { args, selected, options };
        },
        template: `<ArlaSelect v-model="selected" v-bind="args" />`,
    }),
    parameters: {
        layout: 'centered',
    },
};

export default meta;

type Story = StoryObj<typeof ArlaSelect>;

export const Default: Story = {
    args: {
        ['v-model']: 'selected',
        label: 'Selecteer...',
        options: [
            { label: 'Optie 1', value: 'optie1', price: 1 },
            { label: 'Optie 2', value: 'optie2', price: 2 },
            { label: 'Optie 3', value: 'optie3', price: 3 },
        ],
        optionLabel: 'label',
        optionValue: 'value',
    },
};

export const Placeholder: Story = {
    args: {
        ['v-model']: 'selected',
        label: 'Selecteer...',
        options: [
            { label: 'Optie 1', value: 'optie1', price: 1 },
            { label: 'Optie 2', value: 'optie2', price: 2 },
            { label: 'Optie 3', value: 'optie3', price: 3 },
        ],
        optionLabel: 'label',
        optionValue: 'value',
        placeholderOption: 'Selecteer...',
    },
};

export const Error: Story = {
    args: {
        ['v-model']: 'selected',
        label: 'Selecteer...',
        options: [
            { label: 'Optie 1', value: 'optie1', price: 1 },
            { label: 'Optie 2', value: 'optie2', price: 2 },
            { label: 'Optie 3', value: 'optie3', price: 3 },
        ],
        optionLabel: 'label',
        optionValue: 'value',
        errorMessage: 'Dit veld is verplicht.',
        placeholderOption: 'Selecteer...',
    },
};
