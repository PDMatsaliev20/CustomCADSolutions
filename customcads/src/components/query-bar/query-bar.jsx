import InputField from './input-field'
import SelectField from './select-field'
import { useState, useEffect } from 'react'
import { GetCategories, GetCadSortings } from '@/requests/public/home'
import { useTranslation } from 'react-i18next'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

function QueryBar({ setQuery }) {
    const { t } = useTranslation();

    const [sortings, setSortings] = useState([]);
    const [categories, setCategories] = useState([]);
    const [copyQuery, setCopyQuery] = useState({ searchName: '', category: '', sorting: 'Newest' });

    useEffect(() => {
        getCategories();
        getSortings();
    }, []);

    useEffect(() => {
        handleSearch();
    }, [copyQuery]);

    const handleInput = (e) => setCopyQuery(query => ({ ...query, [e.target.name]: e.target.value }));

    const handleSearch = (e) => {
        if (e) {
            e.preventDefault();
        }
        setQuery(oldQuery => ({ ...oldQuery, ...copyQuery }));
    };

    return (
        <div className="basis-full bg-indigo-400 py-8 rounded-lg border-2 border-indigo-600 shadow shadow-indigo-600">
            <form onSubmit={handleSearch} method="get" autoComplete="off">
                <div className="flex flex-wrap justify-center items-center gap-y-5">
                    <div className="basis-full flex justify-evenly">
                        <SelectField
                            label={t('body.cads.Category')}
                            name="category"
                            value={copyQuery.category}
                            onInput={handleInput}
                            items={categories}
                            defaultOption="All"
                            langPath="common.categories."
                            mapFunction={item => <option key={item.id} value={item.name}>{t(`common.categories.${item.name}`)}</option>}
                        />
                        <div className="basis-1/4 bg-indigo-100 rounded-md overflow-hidden flex gap-x-2 items-center px-3 py-2">
                            <InputField name="searchName" value={copyQuery.searchName} onInput={handleInput}
                                placeholder={t('body.cads.Search 3D Models')}
                            />
                            <button>
                                <FontAwesomeIcon icon="search" className="text-lg mt-1 text-indigo-600" />
                            </button>
                        </div>
                        <SelectField
                            label={t('body.cads.Sort by')}
                            name="sorting"
                            value={copyQuery.sorting}
                            onInput={handleInput}
                            items={sortings}
                            langPath="common.sortings."
                            mapFunction={item => <option key={item} value={item}>{t(`common.sortings.${item}`)}</option>}
                        />
                    </div>
                    <button className="hidden bg-indigo-200 py-1 px-4 rounded">{t('body.cads.Search')}</button>
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
            const { data } = await GetCadSortings();
            if (data) {
                setSortings(data);
            }
        } catch (e) {
            console.error(e);
        }
    }
}

export default QueryBar;