import { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { GetCategories, GetSortings } from '@/requests/public/home';
import Input from './input';
import Select from './select';

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

    const categoryMap = (category) =>
        <option key={category.id} value={category.name}>
            {t(`common.categories.${category.name}`)}
        </option>;

    const sortingMap = (sorting) =>
        <option key={sorting} value={sorting}>
            {t(`common.sortings.${sorting}`)}
        </option>;

    return (
        <div className="basis-full bg-indigo-400 py-8 rounded-lg border-2 border-indigo-600 shadow-2xl shadow-indigo-400">
            <form onSubmit={handleSearch} method="get" autoComplete="off">
                <div className="flex flex-wrap justify-center items-center gap-y-5">
                    <div className="basis-full flex justify-evenly">
                        <div className="basis-1/4 bg-indigo-200 text-indigo-800 border border-indigo-900 py-2 rounded-md text-center">
                            <Select
                                id="category"
                                label={t('common.searchbar.category')}
                                name="category"
                                value={copySearch.category}
                                onInput={handleInput}
                                items={categories}
                                defaultOption={t('common.categories.All')}
                                onMap={categoryMap}
                                className="bg-indigo-50 border border-indigo-700 text-indigo-900 py-2 px-1 pe-2 rounded-md focus:outline-none"
                            />
                        </div>
                        <div className="basis-1/4 bg-indigo-100 rounded-md overflow-hidden flex gap-x-2 items-center px-3 py-2">
                            <Input
                                name="name"
                                type="search"
                                value={copySearch.name}
                                onInput={handleInput}
                                placeholder={t('common.searchbar.search_name')}
                                className="w-full h-full bg-inherit text-inherit text-center focus:outline-none"
                            />
                            <button>
                                <FontAwesomeIcon icon="search" className="text-lg mt-1 text-indigo-600" />
                            </button>
                        </div>
                        <div className="basis-1/4 bg-indigo-200 text-indigo-800 border border-indigo-900 py-2 rounded-md text-center">
                            <Select
                                id="sorting"
                                label={t('common.searchbar.sort_by')}
                                name="sorting"
                                value={copySearch.sorting}
                                onInput={handleInput}
                                items={sortings}
                                onMap={sortingMap}
                                className="bg-indigo-50 border border-indigo-700 text-indigo-900 py-2 px-1 pe-2 rounded-md focus:outline-none"
                            />
                        </div>
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