import { useNavigate } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { useState } from 'react'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

function SearchBtn() {
    const { t } = useTranslation();
    const navigate = useNavigate();
    const [search, setSearch] = useState('');

    const handleInput = (e) => setSearch(e.target.value);

    const handleSubmit = (e) => {
        e.preventDefault();
        if (search.trim()) {
            navigate(`/gallery?cad=${encodeURIComponent(search)}`);
        } else navigate('/gallery');
    };

    return (
        <form className="h-full w-full" onSubmit={handleSubmit}>
            <div className="h-full bg-indigo-50 rounded-md">
                <div className="flex gap-x-4 px-4 py-3 ">
                    <input type="search" placeholder={t('header.Searchbar')} onInput={handleInput}
                        className="w-full bg-indigo-50 text-lg text-indigo-900 focus:outline-none"
                    />
                    <button type="submit">
                        <FontAwesomeIcon icon={'search'} className="flex items-center text-indigo-500 text-2xl" />
                    </button>
                </div>
            </div>
        </form>
    )
}

export default SearchBtn;