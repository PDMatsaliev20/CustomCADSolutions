function Field({ id, label, isRequired, type = "text", name, value, onInput, onBlur, placeholder, touched, error, className }) {
    return (
        <div className="w-full" >
            <label htmlFor={id} className="block text-indigo-50">
                {label}{isRequired ? '*' : ''}
            </label>
            <input
                id={id}
                type={type}
                name={name}
                value={value}
                placeholder={placeholder}
                onInput={onInput}
                onBlur={onBlur}
                className={className || "text-indigo-900 w-full mt-1 p-2 px-4 border border-indigo-300 rounded focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"}
            />
            <span className={`${touched && error ? 'inline-block' : 'hidden'} text-sm bg-red-700 p-1 rounded text-indigo-100 font-bold`}>
                {error}
            </span>
        </div>
    );
}

export default Field;