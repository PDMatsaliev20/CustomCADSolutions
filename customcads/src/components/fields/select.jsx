function SelectField({ id, label, isRequired, name, value, onInput, rhfProps, defaultOption, items, onMap, className, error }) {
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