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
        <form className="h-full w-full">
            <div className="h-full bg-indigo-50 rounded-md">
                <div className="flex gap-x-4 px-4 py-3 ">
                    <input type="search" placeholder={t('header.Searchbar')} onChange={handleSearch}
                        className="w-full bg-indigo-50 text-lg text-indigo-900 focus:outline-none"
                    />
                    <button type="submit" onClick={handleClick}>
                        <FontAwesomeIcon icon={'search'} className="flex items-center text-indigo-500 text-2xl" />
                    </button>
                </div>
            </div>
        </form>
    )
}

export default SearchBtn;