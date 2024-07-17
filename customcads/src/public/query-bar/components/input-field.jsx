function InputField({ name, value, onInput, placeholder }) {
    return (
        <input type="search" name={name} value={value} placeholder={placeholder} onInput={onInput}
            className="basis-1/3 px-3 py-2 rounded-md w-full bg-indigo-100 focus:outline-none"
        />
    );
}

export default InputField;