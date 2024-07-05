import { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { useTranslation } from 'react-i18next'

function SearchBar() {
    const navigate = useNavigate();
    const { t } = useTranslation();

    const [text, setText] = useState('');

    const handleInput = (e) => {
        setText(e.target.value);
        localStorage.setItem('search', e.target.value);
    }
    
    const handleSearch = (e) => {
        e.preventDefault();
        navigate(`/gallery?search=${text}`);
    };

    useEffect(() => {
        const searchQuery = new URLSearchParams(location.search).get('search');
        if (searchQuery) {
            setText(searchQuery);
            navigate(`gallery?search=${searchQuery}`);
        } else {
            setText('');
        }
    }, [location.search]);

    useEffect(() => {
        if (!location.search.includes('search')) {
            setText('');
        }
    }, [location.pathname]);

    return (
        <>
            <button type="submit">
                <FontAwesomeIcon icon={'fa-solid', 'fa-search'} className="align-center text-indigo-500 text-2xl" />
            </button>
            <form className="hidden flex basis-full align-center gap-3" onSubmit={handleSearch} method="get">
                <input className="px-3 py-2 w-full rounded-md bg-indigo-50" type="search" placeholder={t('Searchbar')}
                    value={text} onChange={handleInput} />
            </form>
        </>
    )
}

export default SearchBar;