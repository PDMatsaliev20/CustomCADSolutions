import { useTranslation } from 'react-i18next'

function SelectField({ label, name, value, onInput, defaultOption, langPath, items }) {
    const { t } = useTranslation();

    return (
        <label className="basis-4/12 bg-indigo-200 border border-indigo-900 py-2 rounded-md text-center">
            <span className="text-indigo-800">{label}</span>
            <select name={name} value={value} onInput={onInput}
                className="bg-indigo-50 border border-indigo-700 text-indigo-900 py-2 px-1 pe-2 rounded-md focus:outline-none"
            >
                {defaultOption && <option key={0} value={''}>{t(`${langPath || ''}${defaultOption}`)}</option> }
                {items.map(item =>
                    <option key={item.value || item.id} value={item.name}>
                        {t(`${langPath || ''}${item.name}`)}
                    </option>
                )}
            </select>
        </label>
    );
}

export default SelectField;