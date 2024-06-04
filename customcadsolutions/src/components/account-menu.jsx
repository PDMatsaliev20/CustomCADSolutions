function AccountMenu({handleLogout, username}) {
    return (
        <ul className="flex items-center gap-1">
            <li className="float-left flex flex-col items-center justify-center">
                <i className="text-2xl fa fa-user-circle"></i>
                <span className="text-lg">{username}</span>
            </li>
            <li className="float-left ms-5">
                <button onClick={handleLogout} className="text-lg">Log out</button>
            </li>
        </ul>
    );
}

export default AccountMenu;