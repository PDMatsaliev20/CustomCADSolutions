import { useTranslation } from 'react-i18next'
import { Link } from 'react-router-dom'

function ContributorNavigationalMenu() {
    const { t } = useTranslation();

    return (
        <ul className="px-2 flex gap-x-[0.75px]">
            <li className="basis-full text-center bg-indigo-300 rounded-b-3xl py-3 shadow-indigo-400 shadow-xl">
                <Link to="/cads">{t('navbar.Cads Link 1')}</Link>
            </li>
            <li className="basis-full text-center bg-indigo-300 rounded-b-3xl py-3 shadow-indigo-400 shadow-xl">
                <Link to="/cads/upload">{t('navbar.Cads Link 2')}</Link>
            </li>
            <li className="basis-full text-center bg-indigo-300 rounded-b-3xl py-3 shadow-indigo-400 shadow-xl">
                <Link to="/cads/sell">{t('navbar.Cads Link 3')}</Link>
            </li>
        </ul>
    );
}

export default ContributorNavigationalMenu;