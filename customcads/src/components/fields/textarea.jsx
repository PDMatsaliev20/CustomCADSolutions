function TextArea({ id, label, isRequired, name, value, onInput, rhfProps, className, placeholder, rows = 3, error }) {
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
                {...rhfProps}
                className={className || "w-full rounded bg-indigo-50 text-indigo-900 p-2 border-2 focus:outline-none focus:border-indigo-300 resize-none"}
                placeholder={placeholder}
                rows={rows}
            />
            {error && <span className="inline-block text-sm bg-red-700 p-1 rounded text-indigo-100 font-bold mt-1">
                {error.message} 
            </span>}
        </>
    );
}

export default TextArea;