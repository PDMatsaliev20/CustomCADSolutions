import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import useEnglish from '@/languages/hooks/useEnglish';
import useBulgarian from '@/languages/hooks/useBulgarian';

const resources = {
    en: useEnglish(),
    bg: useBulgarian()
};

i18n.use(initReactI18next)
    .init({
        resources,
        lng: localStorage.getItem('language') || 'bg',
        fallbackLng: 'en',
        interpolation: { escapeValue: false }
    });

export default i18n;