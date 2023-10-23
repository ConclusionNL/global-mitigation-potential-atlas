/* eslint-disable @typescript-eslint/no-explicit-any */
import type { Meta, StoryObj } from '@storybook/vue3';
import { ref, type Ref } from 'vue';
import ArlaInput from '@arla-lib/script/vue/components/form-elements/ArlaInput.vue';

const meta: Meta<typeof ArlaInput> = {
    title: 'Componenten/Form elements/Input',
    component: ArlaInput,
    tags: ['autodocs'],
    render: (args: any) => ({
        components: { ArlaInput },
        setup(): { args: any; value: Ref<string> } {
            const value = ref('');
            return { args, value };
        },
        template: `<ArlaInput type="text" v-model="value" v-bind="args" input-class="border" />`,
    }),
    parameters: {
        layout: 'centered',
    },
};

export default meta;

type Story = StoryObj<typeof ArlaInput>;

export const Default: Story = {
    args: {
        label: 'E-mailadres',
        error: '',
    },
};

export const Error: Story = {
    args: {
        label: 'E-mailadres',
        error: 'Dit veld is verplicht.',
    },
};
