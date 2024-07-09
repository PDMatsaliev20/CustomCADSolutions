import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

export default function HeaderBtn({ path, icon }) {
    return (
        <Link to={path}>
            <button className={
                `p-3 flex bg-indigo-100 rounded-[2rem] border-4 border-indigo-200 shadow-md shadow-indigo-700`
                + (icon.includes('cart-shopping') ? ' pb-2' : '')}
            >
                <FontAwesomeIcon icon={icon} className="text-indigo-700 text-2xl" />
            </button>
        </Link>
    );
}

export function HeaderBtnWithText({ path, text, icon }) {
    const { t } = useTranslation();

    return (
        <Link to={path}>
            <button className="px-2 py-2 flex items-center gap-x-2 bg-indigo-300 rounded-xl border-2 border-indigo-400">
                <FontAwesomeIcon icon={icon} className="text-2xl text-indigo-700" />
                <span className="text-indigo-950">{t(text)}</span>
            </button>
        </Link>
    );
}