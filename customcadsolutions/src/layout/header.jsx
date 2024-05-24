import { Link, useNavigate } from 'react-router-dom'
import GuestMenu from '../components/guest-menu'
import AccountMenu from '../components/account-menu'
import axios from 'axios'


function Header({ isAuthenticated, setIsAuthenticated }) {
    const navigate = useNavigate();

    const logout = async () => {
        await axios.post('https://localhost:7127/API/Account/Logout');
    
        localStorage.removeItem('token');
        localStorage.removeItem('username');
        localStorage.removeItem('role');

        setIsAuthenticated(false);
    };

    return (
        <header className="bg-indigo-200 flex justify-between items-center border-b border-black py-1">
            <Link to="/" className="ms-3 w-60">
                <img src="../src/assets/logo.png" className="mw-100 h-auto hover:opacity-60" />  
            </Link>

            <form className="w-1/4 flex gap-3" onSubmit={() => navigate("/") } method="get">
                <input className="px-3 py-2 w-full rounded-md bg-indigo-50" type="search" placeholder="Search CustomCADSolutions" />
                <button type="submit"><i className="fa fa-search"></i></button>
            </form>
            
            <div className="flex me-3">
                {isAuthenticated ? <AccountMenu handleLogout={logout} /> : <GuestMenu />}
            </div>
        </header>
    );
}

export default Header;