import AuthContext from '../auth-context'
import GuestMenu from '../components/guest-menu'
import AccountMenu from '../components/account-menu'
import LanguageSelector from '../components/language'
import SearchBar from '../components/search-bar'
import { Link, useNavigate } from 'react-router-dom'
import { useContext } from 'react'
import axios from 'axios'

function Header() {
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
        <header className="bg-indigo-200 h-20 border-b border-black py-1">
            <ul className="h-full flex mx-5 justify-between items-center">
                <li className="basis-1/4 flex gap-x-4 items-center">
                    <Link to="/" className="w-5/6 h-auto">
                        <img src="../src/assets/logo.png" className="w-full h-auto hover:opacity-60" />
                    </Link>
                    <SearchBar />
                </li>
                <li className="w-1/3 flex gap-x-4"> 
                </li>
                <li className="h-full flex items-center">
                    <LanguageSelector />
                    <div className="flex gap-x-5">
                        {isAuthenticated ? <AccountMenu handleLogout={logout} username={localStorage.getItem('username')} /> : <GuestMenu />}
                    </div>
                </li>
            </ul>           
        </header>
    );
}

export default Header;