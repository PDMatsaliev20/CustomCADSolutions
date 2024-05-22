function Header() {
    return (
        <header className="bg-gray-200 flex justify-between items-center border-b border-black py-1">
            <a className="ms-3 w-16">
                <img src="../public/logo.png" className="mw-100 h-auto hover:opacity-60" />  
            </a>
            <h1 className="text-2xl font-bold">CustomCADSolutions</h1>
            <div className="flex me-3">
                <ul>
                    <li className="float-left ms-5">
                        <a className="text-lg">Log in</a>
                    </li>
                    <li className="float-left ms-5">
                        <a className="text-lg">Register</a>
                    </li>
                </ul>
            </div>
        </header>
    );
}

export default Header;