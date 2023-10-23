/** @type {import("prettier").Options} */
const config = {
    plugins: ['prettier-plugin-tailwindcss'],
    printWidth: 100,
    tabWidth: 4,
    useTabs: false,
    semi: true,
    singleQuote: true,
    quoteProps: 'consistent',
    jsxSingleQuote: false,
    trailingComma: 'es5',
    bracketSpacing: true,
    bracketSameLine: true,
    arrowParens: 'always',
    htmlWhitespaceSensitivity: 'css',
    vueIndentScriptAndStyle: false,
    singleAttributePerLine: false,
    endOfLine: 'crlf',
};

export default config;
