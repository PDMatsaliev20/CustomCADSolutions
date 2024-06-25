function AccountMenu({handleLogout, username}) {
    return (
        <ul className="text-lg flex items-center gap-x-3">
            <li className="float-left flex gap-x-2 items-center justify-center">
                <i className="text-3xl fa fa-user"></i>
                <span>{username}</span>
            </li>
            <li>
                <button onClick={handleLogout}>Log out</button>
            </li>
        </ul>
    );
}

export default AccountMenu;