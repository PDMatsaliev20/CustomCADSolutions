import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'

function GuestMenu() {
    const { t } = useTranslation();

    return (
        <ul>
            <li className="float-left ms-5">
                <Link to="/login" className="text-lg">{t('Log in')}</Link>
            </li>
            <li className="float-left ms-5">
                <Link to="/register" className="text-lg">{t('Register')}</Link>
            </li>
        </ul>
    );
}

export default GuestMenu;