import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

function LoginBtn() {
    const { t } = useTranslation();

    return (
        <Link to="/login">
            <button className="p-2 flex items-center gap-x-2 bg-indigo-300 rounded-xl border-2 border-indigo-400 shadow shadow-indigo-900">
                <span className="text-indigo-950 ">{t('Log in')}</span>
                <FontAwesomeIcon icon={'person-circle-question'} className="text-2xl text-indigo-700" />
            </button>
        </Link>
    );
}

export default LoginBtn;