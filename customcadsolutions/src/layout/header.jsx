import AuthContext from '../auth-context'
import GuestMenu from '../components/guest-menu'
import AccountMenu from '../components/account-menu'
import LanguageSelector from '../components/language'
import { Link, useNavigate } from 'react-router-dom'
import { useContext } from 'react'
import { useTranslation } from 'react-i18next'
import axios from 'axios'

function Header() {
    const { t } = useTranslation();
    const { isAuthenticated, setIsAuthenticated } = useContext(AuthContext);

    const navigate = useNavigate();

    const logout = async () => {
        await axios.post('https://localhost:7127/API/Identity/Logout', {}, {
            withCredentials: true
        });

        setIsAuthenticated(false);
        navigate("/");
    };

    return (
        <header className="bg-indigo-200 border-b border-black py-1">
            <ul className="flex mx-5 justify-between items-center">
                <li className="my-4 w-60">
                    <Link to="/">
                        <img src="../src/assets/logga.png" className="mw-100 h-auto hover:opacity-60" />
                    </Link>
                </li>
                <li className="w-1/3 flex gap-x-4"> 
                    <LanguageSelector />
                    <form className="flex basis-full align-center gap-3" onSubmit={() => navigate("/")} method="get">
                        <input className="px-3 py-2 w-full rounded-md bg-indigo-50" type="search" placeholder={t('Searchbar')} />
                        <button type="submit">
                            <i className="fa fa-search"></i>
                        </button>
                    </form>
                </li>
                <li>
                    <div className="flex">
                        {isAuthenticated ? <AccountMenu handleLogout={logout} username={localStorage.getItem('username')} /> : <GuestMenu />}
                    </div>
                </li>
            </ul>           
        </header>
    );
}

export default Header;