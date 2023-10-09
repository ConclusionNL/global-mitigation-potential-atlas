import { createI18n } from 'vue-i18n';

//import en from '../../Arlanet.Frontend.Library.Test/wwwroot/lang/en.json';
//import nl from '../../Arlanet.Frontend.Library.Test/wwwroot/lang/nl-NL.json';

export default createI18n({
    locale: import.meta.env.VITE_DEFAULT_LOCALE,
    fallbackLocale: import.meta.env.VITE_FALLBACK_LOCALE,
    legacy: false,
    globalInjection: true,
    /*messages: { en, nl },*/
});
