import { FormEventHandler, HTMLAttributes } from "react"

interface SelectFieldProps {
    id?: string
    label?: string
    isRequired?: boolean
    name?: string
    value?: any
    onInput?: FormEventHandler<HTMLSelectElement>
    rhfProps?: HTMLAttributes<HTMLSelectElement>
    defaultOption?: string
    items: any[]
    onMap: (value: any, index: number, array: any[]) => any, thisArg?: any
    className?: string
    error?: { message?: string }
}

function SelectField({ id, label, isRequired, name, value, onInput, rhfProps, defaultOption, items, onMap, className, error }: SelectFieldProps) {
    return (
        <label htmlFor={id} className="w-full">
            <span>{label}{isRequired ? '*' : ''}</span>
            <select
                id={id}
                name={name}
                value={value}
                onInput={onInput}
                {...rhfProps}
                className={className}
            >
                {defaultOption && <option key={0} value="">{defaultOption}</option> }
                {items.map(onMap)}
            </select>
            {error && <span className={`inline-block text-sm bg-red-700 p-1 rounded text-indigo-100 font-bold`}>
                {error.message}
            </span>}
        </label>
    );
}

export default SelectField;