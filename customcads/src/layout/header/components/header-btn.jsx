import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

function LongBtn({ icon, text, orderReversed, onClick }) {
    return (
        <button onClick={onClick} className="bg-indigo-100 rounded-full border-4 border-indigo-300 shadow-md shadow-indigo-700">
            <div className={`flex flex-row-${orderReversed && 'reverse'} items-end gap-x-2 px-3 py-3 text-indigo-600`}>
                <span className="flex font-bold text-lg mx-1">{text}</span>
                <FontAwesomeIcon icon={icon} className="text-3xl" />
            </div>
        </button>
    );
}

export default LongBtn;