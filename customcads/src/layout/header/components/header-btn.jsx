import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

export default function HeaderBtn({ onClick, icon, padding, textSize }) {
    return (
        <button onClick={onClick}
            className={`flex bg-indigo-100 rounded-[100%] border-4 border-indigo-200 shadow-md shadow-indigo-700 ${padding}`}
        >
            <FontAwesomeIcon icon={icon} className={`text-indigo-700 ${textSize}`} />
        </button>
    );
}