import { useNavigate } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { useState } from 'react'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

function SearchBtn() {
    const { t } = useTranslation();
    const navigate = useNavigate();
    const [search, setSearch] = useState('');

    const handleSearch = (e) => setSearch(e.target.value);

    const handleClick = () => {
        if (search) {
            navigate(`/gallery?cad=${search}`);
        } else navigate('/gallery');
    };

    return (
        <form className="h-4/6 w-full">
            <div className="h-full flex gap-x-4 bg-indigo-50 px-4 py-2 rounded-md">
                <input type="search" placeholder={t('Searchbar')} onChange={handleSearch}
                    className="w-full bg-indigo-50 text-indigo-900 focus:outline-none"
                />
                <button type="submit" onClick={handleClick}>
                    <FontAwesomeIcon icon={'search'} className="flex items-center text-indigo-500 text-2xl" />
                </button>
            </div>
        </form>
    )
}

export default SearchBtn;