function TextArea({ id, label, isRequired, name, value, onInput, onBlur, className, placeholder, rows = 3, touched, error }) {
    return (
        <>
            <label htmlFor={id} className="basis-full text-indigo-50">
                {label}{isRequired ? '*' : ''}
            </label>
            <textarea
                id={id}
                name={name}
                value={value}
                onInput={onInput}
                onBlur={onBlur}
                className={className}
                placeholder={placeholder}
                rows={rows}
            />
            <span className={`${touched && error ? 'inline-block' : 'hidden'} text-sm bg-red-700 p-1 rounded text-indigo-100 font-bold mt-1`}>
                {error}
            </span>
        </>
    );
}

export default TextArea;