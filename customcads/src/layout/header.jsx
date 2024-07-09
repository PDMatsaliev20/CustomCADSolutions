import AuthContext from '../auth-context'
import LoginBtn from '../components/login-btn'
import HeaderBtn from '../components/header-btn'
import AccountBtn from '../components/account-btn'
import LanguageSelector from '../components/language'
import SearchBar from '../components/search-bar'
import { Link, useNavigate } from 'react-router-dom'
import { useContext } from 'react'
import axios from 'axios'

function Header() {
    const { isAuthenticated, setIsAuthenticated } = useContext(AuthContext);

    const navigate = useNavigate();

    const handleLogout = async () => {
        await axios.post('https://localhost:7127/API/Identity/Logout', {}, {
            withCredentials: true
        });

        setIsAuthenticated(false);
        navigate("/");
    };

    return (
        <>
            <header className="bg-indigo-200 h-20 border-b border-indigo-700 py-1">
                <ul className="h-full flex mx-5 justify-between items-center">
                    <li className="h-full flex gap-x-6 items-center">
                        <HeaderBtn path="/home" icon="home" />
                        <Link to="/about" className="w-56">
                            <img src="../src/assets/loggafr.png" className="w-full h-auto hover:opacity-60" />
                        </Link>
                    </li>
                    <li className="h-full basis-4/12 flex gap-x-4 items-center">
                        <SearchBar />
                    </li>
                    <li className="h-full gap-x-4 flex items-center justify-end ">
                        <HeaderBtn path="/gallery" icon="cart-shopping" />
                        <div className="flex gap-x-5">
                            {isAuthenticated ? <AccountBtn handleLogout={handleLogout} username={localStorage.getItem('username')} /> : <LoginBtn />}
                        </div>
                        <LanguageSelector />
                    </li>
                </ul>
            </header>
        </>
    );
}

export default Header;