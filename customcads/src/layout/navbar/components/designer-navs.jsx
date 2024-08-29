import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

function DesignerNavigationalMenu() {
    const { t: tLayout } = useTranslation('layout');

    return (
        <ul className="px-2 flex gap-x-[0.75px]">
            <li className="basis-full text-center bg-indigo-300 rounded-b-3xl py-3 shadow-indigo-400 shadow-xl">
                <Link to="/designer/cads">{tLayout('navbar.cads_link_1')}</Link>
            </li>
            <li className="basis-full text-center bg-indigo-300 rounded-b-3xl py-3 shadow-indigo-400 shadow-xl">
                <Link to="/designer/cads/upload">{tLayout('navbar.cads_link_2')}</Link>
            </li>
            <li className="basis-full text-center bg-indigo-300 rounded-b-3xl py-3 shadow-indigo-400 shadow-xl">
                <Link to="/designer/cads/unchecked">{tLayout('navbar.designer_link_1')}</Link>
            </li>
            <li className="basis-full text-center bg-indigo-300 rounded-b-3xl py-3 shadow-indigo-400 shadow-xl">
                <Link to="/designer/orders/pending">{tLayout('navbar.designer_link_2')}</Link>
            </li>
        </ul>
    );
}

export default DesignerNavigationalMenu;