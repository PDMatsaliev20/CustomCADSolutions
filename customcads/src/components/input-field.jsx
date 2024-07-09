function InputField({ value, onChange, placeholder }) {
    return (
        <input className="basis-1/3 px-3 py-2 rounded-md w-full bg-indigo-100 focus:outline-none" type="search" placeholder={placeholder}
            value={value} onChange={onChange} />
    );
}

export default InputField;