import { useTranslation } from 'react-i18next'
import { Link } from 'react-router-dom'

function ContributorNavigationalMenu() {
    const { t } = useTranslation();

    return (
        <ul className="flex gap-x-4 justify-around">
            <li><Link to="/cads">{t('navbar.Cads Link 1')}</Link></li>
            <li><Link to="/cads/upload">{t('navbar.Cads Link 2')}</Link></li>
            <li><Link to="/cads/sell">{t('navbar.Cads Link 3')}</Link></li>
        </ul>
    );
}

export default ContributorNavigationalMenu;