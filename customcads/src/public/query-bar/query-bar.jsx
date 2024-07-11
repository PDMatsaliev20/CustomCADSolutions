import InputField from './components/input-field'
import SelectField from './components/select-field'
import { useState, useEffect } from 'react'
import { useTranslation } from 'react-i18next'
import axios from 'axios'

function QueryBar({ setQueryString }) {
    const { t } = useTranslation();

    const [sortings, setSortings] = useState([]);
    const [categories, setCategories] = useState([]);
    const [query, setQuery] = useState({ category: '', searchName: '', searchCreator: '', sorting: 1 });

    useEffect(() => {
        getCategories();
        getSortings();
    }, []);

    const handleCategory = (e) => {
        setQuery(prevQuery => {
            return { ...prevQuery, category: e.target.value };
        });
    };

    const handleSorting = (e) => {
        setQuery(prevQuery => {
            return { ...prevQuery, sorting: e.target.value };
        });
    };

    const handleCadNameInput = (e) => {
        const cadName = e.target.value;
        setQuery(prevQuery => {
            return { ...prevQuery, searchName: cadName };
        });
    }

    const handleCreatorNameInput = (e) => {
        const creatorName = e.target.value;
        setQuery(prevQuery => {
            return { ...prevQuery, searchCreator: creatorName };
        });
    }

    const handleSearch = (e) => {
        e.preventDefault();
        const searchName = query.searchName && `searchName=${query.searchName}&`;
        const searchCreator = query.searchCreator && `searchCreator=${query.searchCreator}&`;
        const category = query.category && `category=${query.category}&`;
        const sorting = `sorting=${query.sorting}`;
        const queryString = searchName + searchCreator + category + sorting;
        setQueryString(queryString);
    };

    return (
        <div className="basis-10/12 bg-indigo-400 py-8 rounded-lg">
            <form onSubmit={handleSearch} method="get">
                <div className="flex flex-wrap justify-center items-center gap-y-5">
                    <div className="basis-full flex justify-evenly">
                        <InputField value={query.searchName} onChange={handleCadNameInput} placeholder={t('body.gallery.Search 3D Models')} />
                        <InputField value={query.searchCreator} onChange={handleCreatorNameInput} placeholder={t('body.gallery.Search 3D Designers')} />
                    </div>
                    <div className="basis-full flex flex-wrap justify-evenly">
                        <SelectField label={t('body.gallery.Category')} value={query.category} onChange={handleCategory} items={categories} defaultOption={'All'} langPath={'common.categories.'} />
                        {/* TODO: Add front-end, back-end and database functionality for date range */}
                        <SelectField label={t('body.gallery.Sort by')} value={query.sorting} onChange={handleSorting} items={sortings} langPath={'common.sortings.'} />
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