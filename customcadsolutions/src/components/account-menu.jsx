import { useTranslation } from 'react-i18next'

function AccountMenu({ handleLogout, username }) {
    const { t } = useTranslation();

    return (
        <ul className="text-lg flex items-center gap-x-3">
            <li className="float-left flex gap-x-2 items-center justify-center">
                <i className="text-3xl fa fa-user"></i>
                <span>{username}</span>
            </li>
            <li>
                <button onClick={handleLogout}>{t('Log out')}</button>
            </li>
        </ul>
    );
}

export default AccountMenu;