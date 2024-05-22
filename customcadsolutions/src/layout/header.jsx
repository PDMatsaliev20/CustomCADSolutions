import { Link } from 'react-router-dom'

function Header() {
    return (
        <header className="bg-indigo-200 flex justify-between items-center border-b border-black py-1">
            <Link to="/" className="ms-3 w-60">
                <img src="../src/assets/logo.png" className="mw-100 h-auto hover:opacity-60" />  
            </Link>
            <h1 className="text-2xl font-bold">CustomCADSolutions</h1>
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