/* eslint-disable @typescript-eslint/no-explicit-any */
import type { Meta, StoryObj } from '@storybook/vue3';
import { ref, type Ref } from 'vue';
import ArlaRadio from '@arla-lib/script/vue/components/form-elements/ArlaRadio.vue';

const meta: Meta<typeof ArlaRadio> = {
    title: 'Componenten/Form elements/Radio',
    component: ArlaRadio,
    render: (args: any) => ({
        components: { ArlaRadio },
        setup(): { args: any; value: Ref<string> } {
            const value = ref('optie1');
            return { args, value };
        },
        template: `<ArlaRadio v-model="value" value="optie1" label="Optie 3" />`,
    }),
    parameters: {
        // More on how to position stories at: https://storybook.js.org/docs/7.0/react/configure/story-layout
        layout: 'centered',
    },
    // This component will have an automatically generated docsPage entry: https://storybook.js.org/docs/7.0/vue/writing-docs/docs-page
    // tags: ['autodocs'],
};

export default meta;
type Story = StoryObj<typeof ArlaRadio>;

export const Default: Story = {};
