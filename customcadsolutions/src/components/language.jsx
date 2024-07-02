import { useTranslation } from 'react-i18next'
import { useState } from 'react'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { library } from '@fortawesome/fontawesome-svg-core'
import { fas } from '@fortawesome/free-solid-svg-icons'
import { faTwitter, faFontAwesome } from '@fortawesome/free-brands-svg-icons'

library.add(fas, faTwitter, faFontAwesome);

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
        <div className="flex flex-col justify-center">
            <button onClick={handleClick}>
                <FontAwesomeIcon icon="fa-solid fa-globe" className="text-3xl text-indigo-500" />
            </button>
        </div>
    );
}

export default LanguageSelector;