import i18n from "i18next";
import { initReactI18next } from "react-i18next";

const resources = {
    en: {
        translation: {
            "Welcome to React": "Welcome to React and react-i18next"
        }
    },
    bg: {
        translation: {
            "Welcome to React": "Добре дошли при Реакт и реакт-и18н"
        }
    }
};

i18n
    .use(initReactI18next)
    .init({
        resources,
        lng: "bg",
        interpolation: {
            escapeValue: false 
        }
    });

export default i18n;