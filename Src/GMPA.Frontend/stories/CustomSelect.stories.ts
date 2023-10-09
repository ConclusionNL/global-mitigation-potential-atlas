/* eslint-disable @typescript-eslint/no-explicit-any */
import type { Meta, StoryObj } from '@storybook/vue3';
import { ref, type Ref } from 'vue';
import ArlaCustomSelect from '@arla-lib/script/vue/components/form-elements/ArlaCustomSelect.vue';

const meta: Meta<typeof ArlaCustomSelect> = {
    title: 'Componenten/Form elements/CustomSelect',
    component: ArlaCustomSelect,
    tags: ['autodocs'],
    parameters: {
        layout: 'centered',
    },
};

export default meta;

type Story = StoryObj<typeof ArlaCustomSelect>;

export const Default: Story = {
    render: (args: any) => ({
        components: { ArlaCustomSelect },
        setup() {
          //👇 The args will now be passed down to the template
          return { args };
        },
        template:`
        <ArlaCustomSelect v-model="selected" v-bind="args" >
            <template #option="{ item }" >
                <div class="flex space-between items-center gap-2">
                    <img :src="item.image" />
                    <span>{{ item.label }}</span>
                </div>
            </template>
        </ArlaCustomSelect>
        `,
      }),
      args: {
        id: 'product',
        optionLabel: 'label',
        label: 'Selecteer...',
        errorMessage: '',
        items: [
            { label: 'Optie 1', value: 'optie1', price: 1, image: 'https://picsum.photos/24/24' },
            { label: 'Optie 2', value: 'optie2', price: 2, image: 'https://picsum.photos/24/24'  },
            { label: 'Optie 3', value: 'optie3', price: 3, image: 'https://picsum.photos/24/24' },
        ],
      },
};

export const Error: Story = {
    render: (args: any) => ({
        components: { ArlaCustomSelect },
        setup() {
          //👇 The args will now be passed down to the template
          const selected = ref();
          return { args, selected };
        },
        template:`
        <ArlaCustomSelect v-model="selected" v-bind="args" >
            <template #option="{ item }" >
                <div class="flex space-between items-center gap-2">
                    <span>{{ item.label }}</span>
                </div>
            </template>
        </ArlaCustomSelect>
        `,
      }),
      args: {
        id: 'product',
        optionLabel: 'label',
        label: 'Selecteer...',
        errorMessage: 'Dit veld is verplicht.',
        items: [
            { label: 'Optie 1', value: 'optie1', price: 1 },
            { label: 'Optie 2', value: 'optie2', price: 2  },
            { label: 'Optie 3', value: 'optie3', price: 3 },
        ],
      },
};