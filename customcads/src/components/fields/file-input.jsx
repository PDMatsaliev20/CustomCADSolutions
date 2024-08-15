import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

function FileInput({ id, label, isRequired, icon, file, type="file", accept, name, onInput }) {
    return (
        <>
            <label htmlFor={id}>
                <p className="text-indigo-50 text-center">{label}{isRequired ? '*' : ''}</p>
                <div className="max-w-1/2 flex justify-center gap-x-4 bg-indigo-200 rounded-xl py-2 px-4 border-2 border-indigo-500 shadow-lg shadow-indigo-900">
                    <FontAwesomeIcon icon={icon} className="text-2xl text-indigo-800" />
                    <div className={`${file ? 'text-indigo-800 font-bold flex items-center' : 'hidden'}`}>
                        <span className="truncate max-w-32">{file && file.name}</span>
                    </div>
                </div>
            </label>
            <input
                type={type}
                accept={accept}
                id={id}
                name={name}
                onInput={onInput}
                className="w-full rounded bg-indigo-50 text-indigo-900 focus:outline-none p-2"
                hidden
            />
        </>
    );
}

export default FileInput;