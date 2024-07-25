import { useTranslation } from 'react-i18next'
import { Link } from 'react-router-dom'

function DesignerNavigationalMenu() {
    const { t } = useTranslation();

    return (
        <ul className="flex justify-around">
            <div className="basis-5/12 flex justify-evenly">
                <li className="float-left">
                    <Link to="/cads">{t('navbar.Your 3D Models')}</Link>
                </li>
                <li className="float-left">
                    <Link to="/cads/upload">{t('navbar.Upload 3D Model')}</Link>
                </li>
            </div>
            <div className="basis-5/12 flex justify-evenly">
                <li className="float-left">
                    <Link to="/designer/orders">{t('navbar.Client Orders')}</Link>
                </li>
                <li className="float-left">
                    <Link to="/designer/cads">{t('navbar.Contributor 3D Models')}</Link>
                </li>
            </div>
        </ul>
    );
}

export default DesignerNavigationalMenu;