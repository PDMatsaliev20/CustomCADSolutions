import { useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

function Password({ id, label, isRequired, name, value, onInput, placeholder, rhfProps, hidden, error, className }) {
    const [isHidden, setIsHidden] = useState(hidden);

    return (
        <div className="w-full">
            <label htmlFor={id} className="block text-indigo-50">
                {label}{isRequired ? '*' : ''}
            </label>
            <div className="w-full flex bg-white rounded border-2 border-indigo-300 mt-1 p-2 px-4 focus-within:ring-indigo-500 focus-within:border-indigo-500">
                <input
                    id={id}
                    type={isHidden ? 'password' : 'text'}
                    name={name}
                    value={value}
                    placeholder={placeholder}
                    onInput={onInput}
                    className={className || "basis-full text-indigo-900 focus:outline-none"}
                    {...rhfProps}
                />
                <div
                    hidden={hidden}
                    type="button"
                    onClick={() => setIsHidden(isHidden => !isHidden)}
                    className="hover:cursor-pointer"
                >
                    <FontAwesomeIcon icon={isHidden ? 'eye' : 'eye-slash'} className="text-indigo-700" />
                </div>
            </div>
            {error &&
                <span className="inline-block text-sm bg-red-700 p-1 rounded text-indigo-100 font-bold">
                    {error.message}
                </span>}
        </div>
    );
}

export default Password;