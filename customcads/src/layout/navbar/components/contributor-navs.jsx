import { useTranslation } from 'react-i18next'
import { Link } from 'react-router-dom'

function ContributorNavigationalMenu({ shouldBlur }) {
    const { t } = useTranslation();

    const handleClick = () => {
        if (shouldBlur) {
            alert(t('Only for Contributors!'))
        }
    };

    return (
        <ul className={`${shouldBlur ? "blur-sm" : ''} flex justify-around text-indigo-900 font-bold`} onClick={handleClick}>
            {
                shouldBlur ?
                    <>
                        <li className="float-left me-4">
                            <span>{t('Your 3D Models')}</span>
                        </li>
                        <li className="float-left me-4">
                            <span>{t('Upload 3D Model')}</span>
                        </li>
                        <li className="float-left me-2">
                            <span>{t('Sell us a 3D Model')}</span>
                        </li>
                    </> :
                    <>
                        <li className="float-left me-4">
                            <Link to="/cads">{t('Your 3D Models')}</Link>
                        </li>
                        <li className="float-left me-4">
                            <Link to="/cads/upload">{t('Upload 3D Model')}</Link>
                        </li>
                        <li className="float-left me-2">
                            <Link to="/cads/sell">{t('Sell us a 3D Model')}</Link>
                        </li>
                    </>
            }
        </ul>
    );
}

export default ContributorNavigationalMenu;