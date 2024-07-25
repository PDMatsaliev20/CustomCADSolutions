import { useTranslation } from 'react-i18next'
import { Link } from 'react-router-dom'

function ClientNavigationalMenu() {
    const { t } = useTranslation();

    return (
        <ul className={`flex justify-around`}>
            <li className="float-left me-4">
                <Link to="/orders">{t('navbar.Your Orders')}</Link>
            </li>
            <li className="float-left me-4">
                <Link to="/orders/custom">{t('navbar.Order Custom 3D Model')}</Link>
            </li>
            <li className="float-left me-4">
                <Link to="/orders/gallery">{t('navbar.Order from Gallery')}</Link>
            </li>
        </ul>
    );
}

export default ClientNavigationalMenu;