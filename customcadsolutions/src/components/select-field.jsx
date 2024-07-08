function SelectField({ value, label, onChange, defaultOption, items }) {
    return (
        <label className="basis-3/12 bg-indigo-200 border border-indigo-900 py-2 rounded-md text-center">
            <span className="text-indigo-800">{label}</span>
            <select value={value} onChange={onChange}
                className="bg-indigo-50 border border-indigo-700 text-indigo-900 py-2 px-1 pe-2 rounded-md focus:outline-none"
            >
                {defaultOption && <option key={0} value={''}>{defaultOption}</option> }
                {items.map(item =>
                    <option key={item.value && item.id} value={item.name}>
                        {item.name}
                    </option>
                )}
            </select>
        </label>
    );
}

export default SelectField;