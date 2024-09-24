import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { MouseEventHandler } from 'react';

interface HeaderBtnProps {
    icon: IconProp
    text: string | null
    orderReversed?: boolean
    onClick?: MouseEventHandler<HTMLButtonElement>
}

function HeaderBtn({ icon, text, orderReversed, onClick }: HeaderBtnProps) {
    return (
        <button onClick={onClick} className="bg-indigo-100 rounded-full border-4 border-indigo-300 shadow-md shadow-indigo-700 hover:bg-indigo-50 active:bg-indigo-200 active:border-indigo-400">
            <div className={`flex ${orderReversed && 'flex-row-reverse'} items-end gap-x-2 px-4 py-3 text-indigo-600 active:text-indigo-700`}>
                <span className="flex font-bold text-lg mx-1">{text}</span>
                <FontAwesomeIcon icon={icon} className="text-3xl" />
            </div>
        </button>
    );
}

export default HeaderBtn;