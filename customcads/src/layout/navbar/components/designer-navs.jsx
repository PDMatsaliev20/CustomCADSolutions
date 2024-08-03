import { useTranslation } from 'react-i18next'
import { Link } from 'react-router-dom'
import { useState } from 'react'

function DesignerNavigationalMenu() {
    const { t } = useTranslation();

    const [menu, setMenu] = useState('orders');

    return (
        <ul className="px-2 flex gap-x-[0.75px]">
            <li className="basis-full text-center bg-indigo-300 rounded-b-3xl py-3 shadow-indigo-400 shadow-xl">
                <Link to="/cads">{t('navbar.Cads Link 1')}</Link>
            </li>
            <li className="basis-full text-center bg-indigo-300 rounded-b-3xl py-3 shadow-indigo-400 shadow-xl">
                <Link to="/cads/upload">{t('navbar.Cads Link 2')}</Link>
            </li>
            <li className="basis-full text-center bg-indigo-300 rounded-b-3xl py-3 shadow-indigo-400 shadow-xl">
                <Link to="/designer/cads">{t('navbar.Designer Link 1')}</Link>
            </li>
            <li className="basis-full text-center bg-indigo-300 rounded-b-3xl py-3 shadow-indigo-400 shadow-xl">
                <Link to="/designer/orders/pending">{t('navbar.Designer Link 2')}</Link>
            </li>
        </ul>
    );
}

export default DesignerNavigationalMenu;