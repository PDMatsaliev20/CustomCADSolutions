import { useTranslation } from 'react-i18next'
import { Link } from 'react-router-dom'

function PublicNavigationalMenu() {
    const { t } = useTranslation();

    return (
        <ul>
            <li className="float-left ms-2 me-4">
                <Link to="/home">{t('Home')}</Link>
            </li>
            <li className="float-left me-4">
                <Link to="/gallery">{t('Gallery')}</Link>
            </li>
        </ul>
    );
}

export default PublicNavigationalMenu;