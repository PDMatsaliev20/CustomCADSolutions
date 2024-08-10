function SelectField({ id, label, isRequired, name, value, onInput, onBlur, defaultOption, items, onMap, className, touched, error }) {
    return (
        <label htmlFor={id} className="w-full">
            <span>{label}{isRequired ? '*' : ''}</span>
            <select
                id={id}
                name={name}
                value={value}
                onInput={onInput}
                onBlur={onBlur}
                className={className}
            >
                {defaultOption && <option key={0} value="">{defaultOption}</option> }
                {items.map(onMap)}
            </select>
            <span className={`${touched && error ? 'inline-block' : 'hidden'} text-sm bg-red-700 p-1 rounded text-indigo-100 font-bold`}>
                {error}
            </span>
        </label>
    );
}

export default SelectField;