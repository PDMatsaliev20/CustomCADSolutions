import { useEffect } from 'react';
import { useTranslation } from 'react-i18next';

function useLanguages() {
    const { i18n } = useTranslation();

    useEffect(() => {
        const language = localStorage.getItem('language');
        if (language && i18n.language !== language) {
            i18n.changeLanguage(language);
            localStorage.setItem('language', language);
        }
    }, []);

    return i18n.language;
}

export default useLanguages;