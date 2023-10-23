module.exports = {
  "stories": ["../stories/**/*.mdx", "../stories/**/*.stories.@(js|jsx|ts|tsx)"],
  "addons": [
  "@storybook/addon-links", 
  "@storybook/addon-essentials", 
  "@storybook/addon-interactions", 
  "@storybook/addon-mdx-gfm", 
  '@storybook/addon-styling',
  ],
  "framework": {
    "name": "@storybook/vue3-vite",
    "options": {}
  },
  "docs": {
    "autodocs": "tag"
  }
};