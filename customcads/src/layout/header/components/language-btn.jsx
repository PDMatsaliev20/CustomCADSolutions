import { useTranslation } from 'react-i18next'
import { useState } from 'react'
import HeaderBtn from './header-btn'

function LanguageSelector() {
    const { i18n } = useTranslation();
    const [language, setLanguage] = useState(i18n.language);
    const languages = ['bg', 'en'];
    let languageIndex = languages.indexOf(language);

    const handleClick = () => {
        if (languageIndex + 1 > languages.length - 1) {
            languageIndex = 0;
        } else languageIndex++;

        setLanguage(languages[languageIndex]);
        localStorage.setItem('language', languages[languageIndex]);
        i18n.changeLanguage(languages[languageIndex]);
    };

    return <HeaderBtn icon="globe" padding="p-2" textSize="text-3xl" onClick={handleClick} />;
}

export default LanguageSelector;