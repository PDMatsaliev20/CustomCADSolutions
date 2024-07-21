function InputField({ name, value, onInput, placeholder, type = 'search', maxLength = 1000}) {
    return (
        <input
            type={type}
            name={name}
            value={value}
            placeholder={placeholder}
            onInput={onInput}
            className="w-full h-full bg-inherit text-inherit text-center focus:outline-none"
            maxLength={maxLength}
        />
    );
}

export default InputField;