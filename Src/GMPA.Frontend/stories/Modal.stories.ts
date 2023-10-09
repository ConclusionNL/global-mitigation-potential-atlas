/* eslint-disable @typescript-eslint/no-explicit-any */
import type { Meta, StoryObj } from '@storybook/vue3';
import { Ref, ref } from 'vue';
import modal from '@arla-lib/script/vue/components/arla-modal.component.vue';
import '@arla-lib/style/components/_modal.component.scss';

const meta: Meta<typeof modal> = {
    title: 'Componenten/Modal',
    component: modal,
    render: (args: any) => ({
        components: { modal },
        setup(): { args: any; toggleModal: () => void; open: Ref<boolean> } {
            const open = ref(false);
            const toggleModal = (): void => {
                open.value = !open.value;
            };

            return { args, toggleModal, open };
        },
        template: `
            <button class= "button button--border" @click="toggleModal" >Open modal</button>
            <modal :toggle="toggleModal" :show="open">
                <template #header>${args.header}</template>
                <template #body>${args.body}</template>
            </modal>
        `,
    }),
    parameters: {
        // More on how to position stories at: https://storybook.js.org/docs/7.0/react/configure/story-layout
        layout: 'centered',
    },
    // This component will have an automatically generated docsPage entry: https://storybook.js.org/docs/7.0/vue/writing-docs/docs-page
    // tags: ['autodocs'],
};

export default meta;
type Story = StoryObj<typeof modal>;

export const Modal: Story = {
    args: {
        header: '<h3>Modal title</h3>',
        body: 'Body content',
    },
};
