import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

function GuestNavigationalMenu() {
    const { t } = useTranslation();

    return (
        <ul className="px-2 flex gap-x-[0.75px]">
            <li className="basis-full text-center bg-indigo-300 rounded-b-3xl py-3 shadow-indigo-400 shadow-xl">
                <Link to="/info/client">{t('navbar.Guest Link 1')}</Link>
            </li>
            <li className="basis-full text-center bg-indigo-300 rounded-b-3xl py-3 shadow-indigo-400 shadow-xl">
                <Link to="/info/contributor">{t('navbar.Guest Link 2')}</Link>
            </li>
            <li className="basis-full text-center bg-indigo-300 rounded-b-3xl py-3 shadow-indigo-400 shadow-xl">
                <Link to="/info/designer">{t('navbar.Guest Link 3')}</Link>
            </li>
        </ul>
    );
}

export default GuestNavigationalMenu;