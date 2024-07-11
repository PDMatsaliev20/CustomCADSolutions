import i18n from "i18next";
import { initReactI18next } from "react-i18next";
import translationEN from '@/languages/en/translation.json'
import translationBG from '@/languages/bg/translation.json'

const resources = {
    en: {
        translation: translationEN
    },
    bg: {
        translation: translationBG
    }
};

i18n
    .use(initReactI18next)
    .init({
        resources,
        lng: "bg",
        fallbackLng: 'en',
        interpolation: { escapeValue: false }
    });

export default i18n;