import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useAuth } from '@/contexts/auth-context';
import HeaderBtn from './components/header-btn';
import LoginBtn from './components/login-btn';
import AccountBtn from './components/account-btn';
import LanguageBtn from './components/language-btn';

function Header() {
    const { t: tLayout } = useTranslation('layout');
    const { isAuthenticated, userRole } = useAuth();
    
    return (
        <header className="bg-indigo-200 border-b border-indigo-700 rounded-b-lg py-4">
            <ul className="flex justify-between items-center mx-5">
                <li className="basis-1/3 flex justify-start items-center gap-x-6">
                    <Link to={!isAuthenticated ? '/' : `/${userRole.toLowerCase()}`} className="hover:no-underline">
                        <HeaderBtn icon="home" text={tLayout("header.home")} orderReversed />
                    </Link>
                    <Link to="/gallery" className="hover:no-underline">
                        <HeaderBtn icon="basket-shopping" text={tLayout("header.gallery")} orderReversed />
                    </Link>
                </li>
                <li className="basis-1/3 flex justify-center">
                    <Link to="/about" className="w-80">
                        <img src="/logo.png" className="w-full h-auto hover:opacity-60 active:opacity-80" />
                    </Link>
                </li>
                <li className="basis-1/3 flex justify-end items-center gap-x-4">
                    {isAuthenticated ? <AccountBtn /> : <LoginBtn />}
                    <LanguageBtn />
                </li>
            </ul>
        </header>
    );
}

export default Header;