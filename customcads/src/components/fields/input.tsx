import { FormEventHandler, HTMLAttributes } from "react"

interface InputProps {
    id?: string
    label?: string
    isRequired?: boolean
    type?: string
    name?: string
    value?: any
    onInput?: FormEventHandler<HTMLInputElement>
    placeholder?: string
    className?: string
    rhfProps?: HTMLAttributes<HTMLInputElement>
    error?: { message?: string }
}

function Input({ id, label, isRequired, type, name, value, onInput, placeholder, className, rhfProps, error }: InputProps) {

    return (
        <div className="w-full">
            <label htmlFor={id} className="block text-indigo-50">
                {label}{isRequired ? '*' : ''}
            </label>
            <input
                id={id}
                type={type || 'text'}
                name={name}
                value={value}
                placeholder={placeholder}
                onInput={onInput}
                className={className || "text-indigo-900 w-full mt-1 p-2 px-4 border border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"}
                {...rhfProps}
            />
            {error &&
                <span className="inline-block text-sm bg-red-700 p-1 rounded text-indigo-100 font-bold">
                    {error.message}
                </span>}
        </div>
    );
}

export default Input;