function InputField({ name, value, onInput, placeholder }) {
    return (
        <input
            type="search"
            name={name}
            value={value}
            placeholder={placeholder}
            onInput={onInput}
            className="w-full h-full bg-inherit focus:outline-none"
        />
    );
}

export default InputField;