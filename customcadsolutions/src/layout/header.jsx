import { Link, useNavigate } from 'react-router-dom'

function Header() {
    const navigate = useNavigate();
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
                <ul>
                    <li className="float-left ms-5">
                        <Link to="/login" className="text-lg">Log in</Link>
                    </li>
                    <li className="float-left ms-5">
                        <Link to="/register" className="text-lg">Register</Link>
                    </li>
                </ul>
            </div>
        </header>
    );
}

export default Header;