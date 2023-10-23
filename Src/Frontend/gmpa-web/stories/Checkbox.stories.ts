/* eslint-disable @typescript-eslint/no-explicit-any */
import type { Meta, StoryObj } from '@storybook/vue3';
import { ref, type Ref } from 'vue';
import Checkbox from '@arla-lib/script/vue/components/form-elements/ArlaCheckbox.vue';
// import '@arla-lib/style/components/_modal.component.scss';

const meta: Meta<typeof Checkbox> = {
    title: 'Componenten/Form elements/Checkbox',
    component: Checkbox,
    render: (args: any) => ({
        components: { Checkbox },
        setup(): { args: any; value: Ref<string[]> } {
            const value = ref([]);
            return { args, value };
        },
        template: `
        <div>
            <Checkbox v-model="value" value="optie1" label="Optie 1" />
        </div>`,
    }),
    parameters: {
        // More on how to position stories at: https://storybook.js.org/docs/7.0/react/configure/story-layout
        layout: 'centered',
    },
    // This component will have an automatically generated docsPage entry: https://storybook.js.org/docs/7.0/vue/writing-docs/docs-page
    // tags: ['autodocs'],
};

export default meta;
type Story = StoryObj<typeof Checkbox>;

export const Default: Story = {};
