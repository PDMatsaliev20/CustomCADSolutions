import { useTranslation } from 'react-i18next'
import { Link } from 'react-router-dom'

function ContributorNavigationalMenu({ shouldBlur }) {
    const { t } = useTranslation();

    const handleClick = () => {
        if (shouldBlur) {
            alert(t('navbar.Only for Contributors!'))
        }
    };

    return (
        <ul className={`${shouldBlur ? "blur-sm" : ''} flex justify-around`} onClick={handleClick}>
            {
                shouldBlur ?
                    <>
                        <li className="float-left me-4">
                            <span>{t('navbar.Your 3D Models')}</span>
                        </li>
                        <li className="float-left me-4">
                            <span>{t('navbar.Upload 3D Model')}</span>
                        </li>
                        <li className="float-left me-2">
                            <span>{t('navbar.Sell us a 3D Model')}</span>
                        </li>
                    </> :
                    <>
                        <li className="float-left me-4">
                            <Link to="/cads">{t('navbar.Your 3D Models')}</Link>
                        </li>
                        <li className="float-left me-4">
                            <Link to="/cads/upload">{t('navbar.Upload 3D Model')}</Link>
                        </li>
                        <li className="float-left me-2">
                            <Link to="/cads/sell">{t('navbar.Sell us a 3D Model')}</Link>
                        </li>
                    </>
            }
        </ul>
    );
}

export default ContributorNavigationalMenu;