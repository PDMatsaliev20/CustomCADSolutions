import { useTranslation } from 'react-i18next'
import { Link } from 'react-router-dom'

function ContributorNavigationalMenu() {
    const { t } = useTranslation();

    return (
        <ul className="flex gap-x-4 justify-around">
            <li><Link to="/cads">{t('navbar.Your 3D Models')}</Link></li>
            <li><Link to="/cads/upload">{t('navbar.Upload 3D Model')}</Link></li>
            <li><Link to="/cads/sell">{t('navbar.Sell us a 3D Model')}</Link></li>
        </ul>
    );
}

export default ContributorNavigationalMenu;