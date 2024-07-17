import InputField from './components/input-field'
import SelectField from './components/select-field'
import { useState, useEffect } from 'react'
import { useTranslation } from 'react-i18next'
import axios from 'axios'

function QueryBar({ setQuery }) {
    const { t } = useTranslation();

    const [sortings, setSortings] = useState([]);
    const [categories, setCategories] = useState([]);
    const [copyQuery, setCopyQuery] = useState({ searchName: '', searchCreator: '', category: '', sorting: 1 });

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
        setQuery(oldQuery => ({...oldQuery, ...copyQuery}));
    };

    return (
        <div className="basis-10/12 bg-indigo-400 py-8 rounded-lg">
            <form onSubmit={handleSearch} method="get" autoComplete="off">
                <div className="flex flex-wrap justify-center items-center gap-y-5">
                    <div className="basis-full flex justify-evenly">
                        <InputField name="searchName" value={copyQuery.searchName} onInput={handleInput}
                            placeholder={t('body.gallery.Search 3D Models')}
                        />
                        <InputField name="searchCreator" value={copyQuery.searchCreator} onInput={handleInput}
                            placeholder={t('body.gallery.Search 3D Designers')}
                        />
                    </div>
                    <div className="basis-full flex flex-wrap justify-evenly">
                        <SelectField
                            label={t('body.gallery.Category')}
                            name="category"
                            value={copyQuery.category}
                            onInput={handleInput}
                            items={categories}
                            defaultOption={'All'}
                            langPath={'common.categories.'}
                        />
                        {/* TODO: Add front-end, back-end and database functionality for date range */}
                        <SelectField
                            label={t('body.gallery.Sort by')}
                            name="sorting"
                            value={copyQuery.sorting}
                            onInput={handleInput}
                            items={sortings}
                            langPath={'common.sortings.'}
                        />
                    </div>
                    <button className="hidden bg-indigo-200 py-1 px-4 rounded">{t('body.gallery.Search')}</button>
                </div>
            </form>
        </div>
    );

    async function getCategories() {
        const response = await axios.get('https://localhost:7127/API/Categories')
            .catch(e => console.error(e));

        if (response.data) {
            setCategories(response.data);
        }
    }

    async function getSortings() {
        const response = await axios.get('https://localhost:7127/API/Sortings')
            .catch(e => console.error(e));

        if (response.data) {
            setSortings(response.data);
        }
    }
}

export default QueryBar;