import { useTranslation } from 'react-i18next'
import { Link } from 'react-router-dom'

function ClientNavigationalMenu() {
    const { t } = useTranslation();

    return (
        <ul className="flex gap-x-4 justify-around">
            <li><Link to="/orders">{t('navbar.Your Orders')}</Link></li>
            <li><Link to="/orders/custom">{t('navbar.Order Custom 3D Model')}</Link></li>
            <li><Link to="/orders/gallery">{t('navbar.Order from Gallery')}</Link></li>
        </ul>
    );
}

export default ClientNavigationalMenu;