/* eslint-disable @typescript-eslint/no-explicit-any */
import type { Meta, StoryObj } from '@storybook/vue3';
import '@arla-lib/style/components/_tabs.component.scss';
import tabs from '@arla-lib/script/vue/components/arla-tabs.component.vue';

const meta: Meta<typeof tabs> = {
    /* 👇 The title prop is optional.
     * See https://storybook.js.org/docs/7.0/vue/configure/overview#configure-story-loading
     * to learn how to generate automatic titles
     */
    title: 'Componenten/Tabs',
    component: tabs,
    render: (args: any) => ({
        components: { tabs },
        setup(): { args: any } {
            return { args };
        },
        template: `
            <tabs :tabs="args.tabs">
                <template v-slot:content="tabName">
                    ${args.content}
                </template>
            </tabs>
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
type Story = StoryObj<typeof tabs>;

export const Tabs: Story = {
    args: {
        tabs: ['First Tab', 'Second Tab'],
        content: `<div v-if="tabName.tabName === 'First Tab'">First Tab content</div><div v-if="tabName.tabName === 'Second Tab'">Second Tab content</div>`,
    },
};
