import { useTranslation } from 'react-i18next'
import { Link } from 'react-router-dom'

function DesignerNavigationalMenu() {
    const { t } = useTranslation();

    return (
        <ul className={`flex justify-around`}>
            <li className="float-left me-4">
                <Link to="/cads">{t('navbar.Your 3D Models')}</Link>
            </li>
            <li className="float-left me-4">
                <Link to="/designer/orders">{t('navbar.Client Orders')}</Link>
            </li>
            <li className="float-left me-4">
                <Link to="/designer/cads">{t('navbar.Contributor 3D Models')}</Link>
            </li>
        </ul>
    );
}

export default DesignerNavigationalMenu;