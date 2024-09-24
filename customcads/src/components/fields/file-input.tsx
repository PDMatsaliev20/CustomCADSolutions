import { IconProp } from '@fortawesome/fontawesome-svg-core';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { FormEventHandler } from 'react';

interface FileInputProps {
    id?: string
    label?: string
    isRequired?: boolean
    icon: IconProp
    file?: File | null
    type?: string
    accept?: string
    name?: string
    onInput?: FormEventHandler<HTMLInputElement>
}

function FileInput({ id, label, isRequired, icon, file, type="file", accept, name, onInput }: FileInputProps) {
    return (
        <div className="basis-1/4">
            <label htmlFor={id}>
                <p className="text-indigo-50 text-center">{label}{isRequired ? '*' : ''}</p>
                <div className="flex justify-center gap-x-4 bg-indigo-200 rounded-xl py-2 px-4 border-2 border-indigo-500 shadow-lg shadow-indigo-900">
                    <FontAwesomeIcon icon={icon} className="text-2xl text-indigo-800" />
                    <div className={`${file ? 'max-w-full text-indigo-800 font-bold flex items-center' : 'hidden'}`}>
                        <span className="truncate max-w-full">{file && file.name}</span>
                    </div>
                </div>
            </label>
            <input
                id={id}
                type={type}
                accept={accept}
                name={name}
                onInput={onInput}
                className="w-full rounded bg-indigo-50 text-indigo-900 focus:outline-none p-2"
                hidden
            />
        </div>
    );
}

export default FileInput;