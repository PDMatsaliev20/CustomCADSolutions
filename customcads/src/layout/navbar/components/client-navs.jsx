import { useTranslation } from 'react-i18next'
import { Link } from 'react-router-dom'

function ClientNavigationalMenu() {
    const { t } = useTranslation();

    return (
        <ul className="flex gap-x-4 justify-around">
            <li><Link to="/orders">{t('navbar.Orders Link 1')}</Link></li>
            <li><Link to="/orders/custom">{t('navbar.Orders Link 2')}</Link></li>
            <li><Link to="/orders/finished">{t('navbar.Orders Link 3')}</Link></li>
        </ul>
    );
}

export default ClientNavigationalMenu;