import { useTranslation } from 'react-i18next'
import { useState } from 'react'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

function LanguageSelector() {
    const { i18n } = useTranslation();
    const [language, setLanguage] = useState(i18n.language);
    const languages = ['bg', 'en'];
    let languageIndex = languages.indexOf(language);

    const handleClick = (e) => {
        if (languageIndex + 1 > languages.length - 1) {
            languageIndex = 0;
        } else languageIndex++;

        setLanguage(languages[languageIndex]);
        localStorage.setItem('language', languages[languageIndex]);
        i18n.changeLanguage(languages[languageIndex]);
    };

    return (
        <button onClick={handleClick} className="text-indigo-600 hover:text-indigo-600 active:text-indigo-400">
            <FontAwesomeIcon icon={'fas', 'fa-globe'} className="text-3xl" />
        </button>
    );
}

export default LanguageSelector;