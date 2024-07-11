import { useTranslation } from 'react-i18next'
import { Link } from 'react-router-dom'

function ClientNavigationalMenu({ shouldBlur }) {
    const { t } = useTranslation();

    const handleClick = () => {
        if (shouldBlur) {
            alert(t('navbar.Only for Clients!'));
        }
    }

    return (
        <ul className={`${shouldBlur ? "blur-sm" : ''} flex justify-around text-indigo-900 font-bold`} onClick={handleClick}>
            {
                shouldBlur ?
                    <>
                        <li className="float-left me-4">
                            <span>{t('navbar.Your Orders')}</span>
                        </li>
                        <li className="float-left me-4">
                            <span>{t('navbar.Order Custom 3D Model')}</span>
                        </li>
                        <li className="float-left me-4">
                            <span>{t('navbar.Order from Gallery')}</span>
                        </li>
                    </>
                    :
                    <>
                        <li className="float-left me-4">
                            <Link to="/orders">{t('navbar.Your Orders')}</Link>
                        </li>
                        <li className="float-left me-4">
                            <Link to="/orders/custom">{t('navbar.Order Custom 3D Model')}</Link>
                        </li>
                        <li className="float-left me-4">
                            <Link to="/orders/gallery">{t('navbar.Order from Gallery')}</Link>
                        </li>
                    </>
            }
        </ul>
    );
}

export default ClientNavigationalMenu;