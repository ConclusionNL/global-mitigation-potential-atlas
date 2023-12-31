module.exports = {
    root: true,
    extends: [
        'eslint:recommended',
        'plugin:@typescript-eslint/recommended',
        'plugin:vue/vue3-essential',
        'plugin:vue/vue3-recommended',
        'plugin:vue/vue3-strongly-recommended',
        '@vue/eslint-config-typescript',
        '@vue/eslint-config-prettier',
        'plugin:storybook/recommended',
        'plugin:storybook/recommended',
    ],
    parserOptions: {
        ecmaVersion: 'latest',
        project: ['./tsconfig.json'],
        tsconfigRootDir: __dirname,
    },
    rules: {
        'no-magic-numbers': 'off',
        '@typescript-eslint/no-magic-numbers': [
            'warn',
            {
                ignore: [0, 1],
                ignoreEnums: true,
                ignoreTypeIndexes: true,
                ignoreReadonlyClassProperties: true,
            },
        ],
        'no-nested-ternary': 'warn',
        '@typescript-eslint/explicit-function-return-type': 'warn',
        // forse the use of public private except on 'constructor' and 'get' & 'set'
        '@typescript-eslint/explicit-member-accessibility': [
            'warn',
            {
                accessibility: 'explicit',
                overrides: {
                    constructors: 'no-public',
                    accessors: 'no-public',
                },
            },
        ],
        '@typescript-eslint/explicit-module-boundary-types': 'warn',
        'no-unused-vars': ['off'],
        '@typescript-eslint/no-unused-vars': [
            'warn',
            {
                argsIgnorePattern: '^_',
            },
        ],
    },
};
