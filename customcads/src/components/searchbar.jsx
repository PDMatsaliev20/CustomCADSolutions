import { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { GetCategories, GetSortings } from '@/requests/public/home';
import InputField from './input-field';
import SelectField from './select-field';

function SearchBar({ setSearch }) {
    const { t } = useTranslation();
    const [sortings, setSortings] = useState([]);
    const [categories, setCategories] = useState([]);
    const [copySearch, setCopySearch] = useState({ name: '', owner: '', category: '', sorting: 'Newest' });

    useEffect(() => {
        getCategories();
        getSortings();
    }, []);

    useEffect(() => {
        handleSearch();
    }, [copySearch]);

    const handleInput = (e) => setCopySearch(search => ({ ...search, [e.target.name]: e.target.value }));

    const handleSearch = (e) => {
        if (e) {
            e.preventDefault();
        }
        setSearch(oldQuery => ({ ...oldQuery, ...copySearch }));
    };

    return (
        <div className="basis-full bg-indigo-400 py-8 rounded-lg border-2 border-indigo-600 shadow shadow-indigo-600">
            <form onSubmit={handleSearch} method="get" autoComplete="off">
                <div className="flex flex-wrap justify-center items-center gap-y-5">
                    <div className="basis-full flex justify-evenly">
                        <SelectField
                            label={t('common.searchbar.Category')}
                            name="category"
                            value={copySearch.category}
                            onInput={handleInput}
                            items={categories}
                            defaultOption="All"
                            langPath="common.categories."
                            mapFunction={item => <option key={item.id} value={item.name}>{t(`common.categories.${item.name}`)}</option>}
                        />
                        <div className="basis-1/4 bg-indigo-100 rounded-md overflow-hidden flex gap-x-2 items-center px-3 py-2">
                            <InputField
                                name="name"
                                value={copySearch.name}
                                onInput={handleInput}
                                placeholder={t('common.searchbar.Search Name')}
                            />
                            <button>
                                <FontAwesomeIcon icon="search" className="text-lg mt-1 text-indigo-600" />
                            </button>
                        </div>
                        <SelectField
                            label={t('common.searchbar.Sort by')}
                            name="sorting"
                            value={copySearch.sorting}
                            onInput={handleInput}
                            items={sortings}
                            langPath="common.sortings."
                            mapFunction={item => <option key={item} value={item}>{t(`common.sortings.${item}`)}</option>}
                        />
                    </div>
                    <button className="hidden bg-indigo-200 py-1 px-4 rounded">{t('common.searchbar.Search')}</button>
                </div>
            </form>
        </div>
    );

    async function getCategories() {
        try {
            const { data } = await GetCategories();
            if (data) {
                setCategories(data);
            }
        } catch (e) {
            console.error(e);
        }
    }

    async function getSortings() {
        try {
            const { data } = await GetSortings();
            if (data) {
                setSortings(data);
            }
        } catch (e) {
            console.error(e);
        }
    }
}

export default SearchBar;