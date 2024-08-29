import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

function GuestNavigationalMenu() {
    const { t: tLayout } = useTranslation('layout');

    return (
        <ul className="px-2 flex gap-x-[0.75px]">
            <li className="basis-full text-center bg-indigo-300 rounded-b-3xl py-3 shadow-indigo-400 shadow-xl">
                <Link to="/info/client">{tLayout('navbar.guest_link_1')}</Link>
            </li>
            <li className="basis-full text-center bg-indigo-300 rounded-b-3xl py-3 shadow-indigo-400 shadow-xl">
                <Link to="/info/contributor">{tLayout('navbar.guest_link_2')}</Link>
            </li>
            <li className="basis-full text-center bg-indigo-300 rounded-b-3xl py-3 shadow-indigo-400 shadow-xl">
                <Link to="/info/designer">{tLayout('navbar.guest_link_3')}</Link>
            </li>
        </ul>
    );
}

export default GuestNavigationalMenu;