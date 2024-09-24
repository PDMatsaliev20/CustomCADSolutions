import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { MouseEventHandler } from 'react';

interface ArrowBtnProps {
    text: string
    type: string
    onClick: MouseEventHandler<HTMLButtonElement>
}

function ArrowBtn({ text, type, onClick }: ArrowBtnProps) {

    let icon: IconProp | undefined;
    let rounded: string | undefined;
    switch (type) {
        case 'beginning': 
            icon = 'angles-left' 
            rounded = 'rounded-s-md';
            break;

        case 'previous': 
            icon = 'angle-left';
            break;

        case 'next': 
            icon = 'angle-right' 
            break;

        case 'end': 
            icon = 'angles-right';
            rounded = 'rounded-e-md';
            break;

    }

    return (
        <button onClick={onClick}
            className={`relative inline-flex items-center ${rounded} px-4 py-2 ring-1 ring-inset ring-indigo-600 hover:bg-indigo-200 active:bg-indigo-300`}
        >
            <span className="sr-only">{text}</span>
            <FontAwesomeIcon icon={icon ?? 'angles-left'} className="text-indigo-800" />
        </button>
    );
}

export default ArrowBtn;