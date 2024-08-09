import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

function ClientNavigationalMenu() {
    const { t } = useTranslation();

    return (
        <ul className="px-2 flex gap-x-[0.75px]">
            <li className="basis-full text-center bg-indigo-300 rounded-b-3xl py-3 shadow-indigo-400 shadow-xl">
                <Link to="/orders/pending">{t('navbar.Orders Link 1')}</Link>
            </li>
            <li className="basis-full text-center bg-indigo-300 rounded-b-3xl py-3 shadow-indigo-400 shadow-xl">
                <Link to="/orders/custom">{t('navbar.Orders Link 2')}</Link>
            </li>
        </ul>
    );
}

export default ClientNavigationalMenu;