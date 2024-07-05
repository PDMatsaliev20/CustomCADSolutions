import { useTranslation } from 'react-i18next'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

function AccountMenu({ handleLogout, username }) {
    const { t } = useTranslation();

    return (
        <ul className="text-lg flex items-center">
            <li className="flex flex-wrap items-center justify-center">
                <FontAwesomeIcon icon={'fa', 'fa-user-circle'} className="text-3xl text-indigo-800 basis-full" />
                <span>{username}</span>
            </li>
            <li>
                <button onClick={handleLogout}>{t('Log out')}</button>
            </li>
        </ul>
    );
}

export default AccountMenu;