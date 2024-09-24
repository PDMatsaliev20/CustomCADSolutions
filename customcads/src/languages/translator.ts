import i18next from 'i18next';
import { initReactI18next } from 'react-i18next';
import en from './en/resources';
import bg from './bg/resources';

i18next
    .use(initReactI18next)
    .init({
        resources: { en, bg },
        ns: ['layout', 'pages', 'common'],
        defaultNS: 'pages',
        lng: localStorage.getItem('language') || 'bg',
        fallbackLng: 'en',
        interpolation: { escapeValue: false }
    });

export default i18next;