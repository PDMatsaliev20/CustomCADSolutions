import AuthContext from '@/components/auth-context'
import { useState, useContext } from 'react'
import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

function AccountBtn({ onLogout, username }) {
    const { t } = useTranslation();
    const { userRole } = useContext(AuthContext);
    const [isOpen, setIsOpen] = useState(false);

    const handleClick = () => setIsOpen(!isOpen);

    return (
        <div className="relative">
            <Link to="#">
                <button className="px-5 py-2 flex items-center gap-x-2 bg-indigo-300 relative z-50 rounded-t-xl border-2 border-indigo-400 shadow shadow-indigo-900" onClick={handleClick}>
                    <span className="text-indigo-950 text-lg">{username}</span>
                    <FontAwesomeIcon icon="fa-user-circle" className="text-3xl text-indigo-700" />
                </button>
            </Link>
            <ul
                className={
                    "absolute top-12 py-2 w-full bg-indigo-100 border border-indigo-500 rounded-b-md text-center text-indigo-700"
                    + (!isOpen ? ' hidden' : '')
                }
            >
                <li>
                    <span className="italic font-bold">
                        {t(userRole)}
                    </span>
                </li>
                <li>
                    <button className="underline underline-offset-4" onClick={onLogout}>
                        {t('Log out')}
                    </button>
                </li>
            </ul>
        </div>
    );
}

export default AccountBtn;