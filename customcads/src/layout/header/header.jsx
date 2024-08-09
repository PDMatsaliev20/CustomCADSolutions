import { useContext } from 'react';
import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import AuthContext from '@/components/auth-context';
import HeaderBtn from './components/header-btn';
import LoginBtn from './components/login-btn';
import AccountBtn from './components/account-btn';
import LanguageBtn from './components/language-btn';

function Header() {
    const { t } = useTranslation();
    const { isAuthenticated } = useContext(AuthContext);

    return (
        <header className="bg-indigo-200 border-b border-indigo-700 rounded-b-lg py-4">
            <ul className="flex justify-between items-center mx-5">
                <li className="basis-1/3 flex justify-start items-center gap-x-6">
                    <Link to="/home" className="hover:no-underline">
                        <HeaderBtn icon="home" text={t("header.Home")} orderReversed />
                    </Link>
                    <Link to="/gallery" className="hover:no-underline">
                        <HeaderBtn icon="basket-shopping" text={t("header.Gallery")} orderReversed />
                    </Link>
                </li>
                <li className="basis-1/3 flex justify-center">
                    <Link to="/about" className="w-80">
                        <img src="/src/assets/logo.png" className="w-full h-auto hover:opacity-60" />
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