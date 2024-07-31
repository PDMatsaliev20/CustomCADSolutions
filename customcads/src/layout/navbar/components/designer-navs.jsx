import { useTranslation } from 'react-i18next'
import { Link } from 'react-router-dom'
import { useState } from 'react'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

function DesignerNavigationalMenu() {
    const { t } = useTranslation();

    const [menu, setMenu] = useState('orders');

    const handleRightArrow = () => setMenu('orders');
    const handleLeftArrow = () => setMenu('cads');

    return (
        <div className="px-8">
            <ul className={`${menu === 'cads' ? 'flex justify-around' : 'hidden'}`}>
                <li className="flex items-center opacity-40">
                    <button disabled>
                        <FontAwesomeIcon icon={'arrow-circle-left'} className="text-3xl text-indigo-600" />
                    </button>
                </li>
                <li className="grow flex items-center justify-evenly ">
                    <Link to="/cads">{t('navbar.Cads Link 1')}</Link>
                    <Link to="/cads/upload">{t('navbar.Cads Link 2')}</Link>
                    <Link to="/designer/cads">{t('navbar.Cads Link 4')}</Link>
                </li>
                <li className="">
                    <button onClick={handleRightArrow}>
                        <FontAwesomeIcon icon={'arrow-circle-right'} className="text-3xl text-indigo-600" />
                    </button>
                </li>
            </ul>
            <ul className={`${menu === 'orders' ? 'flex justify-around' : 'hidden'}`}>
                <li className="flex items-center">
                    <button onClick={handleLeftArrow}>
                        <FontAwesomeIcon icon={'arrow-circle-left'} className="text-3xl text-indigo-600" />
                    </button>
                </li>
                <li className="grow flex items-center justify-evenly">
                    <Link to="/designer/orders/pending">{t('navbar.Orders Link 4')}</Link>
                    <Link to="/designer/orders/begun">{t('navbar.Orders Link 5')}</Link>
                    <Link to="/designer/orders/finished">{t('navbar.Orders Link 6')}</Link>
                </li>
                <li className="flex items-center opacity-40">
                    <button disabled>
                        <FontAwesomeIcon icon={'arrow-circle-right'} className="text-3xl text-indigo-600" />
                    </button>
                </li>
            </ul>
        </div>
    );
}

export default DesignerNavigationalMenu;