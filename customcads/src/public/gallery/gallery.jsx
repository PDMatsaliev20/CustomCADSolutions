import axios from 'axios'
import { useSearchParams } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { useState, useEffect } from 'react'
import QueryBar from '../query-bar/query-bar'
import GalleryItem from './components/gallery-item'

function GalleryPage() {
    const { t } = useTranslation();
    const [searchParams] = useSearchParams();
    const [cads, setCads] = useState([]);
    const [_, setTotalCount] = useState(0);
    const [query, setQuery] = useState({
        searchName: '',
        searchCreator: '',
        category: '',
        sorting: 1,
        validated: true,
        unvalidated: false,
        currentPage: 1,
        cadsPerPage: 6,
    });

    const turnQueryToString = (query) => {

        // [ [key: searchName, value: ''], [key: searchCreator, value: ''], [key: category, value: '']... ]
        const queryKeyValueArrays = Object.entries(query);

        // ['', '', '', 'sorting=1', 'validated=true'...]
        const queryStringArray = queryKeyValueArrays.map(
            ([key, value]) =>
                (key.includes('validated') || value)
                    ? `${encodeURIComponent(key)}=${encodeURIComponent(value)}`
                    : ''
        );

        // ['sorting=1', 'validated=true', 'unvalidated=false'...]
        const queryStringArrayWithoutEmptyEntries = queryStringArray.filter(q => q || (typeof q == Boolean));

        // 'sorting=1&validated=true&unvalidated=false...'
        const queryString = queryStringArrayWithoutEmptyEntries.join('&');

        return queryString;
    };

    const cadSearchParam = searchParams.get('cad');
    useEffect(() => {
        if (cadSearchParam) {
            setQuery(query => ({ ...query, searchName: cadSearchParam }));
        }
    }, [searchParams]);

    useEffect(() => {
        if (query.searchName || query.searchCreator || query.category || query.sorting) {
            filterCads();
        } else {
            if (!cads.length) {
                loadCads();
            }
        }
    }, [query]);

    return (
        <div className="mb-8 flex flex-wrap justify-center gap-y-8">
            <h1 className="basis-full text-center text-4xl text-indigo-950 font-bold">{t('body.gallery.Our Gallery')}</h1>
            <QueryBar setQuery={setQuery} />
            <section className="basis-full">
                <ul className="flex flex-wrap justify-between gap-y-12 gap-x-3">
                    {cads.map(cad => <GalleryItem key={cad.id} item={cad} />)}
                </ul>
            </section>
        </div>
    );

    async function loadCads() {
        const queryString = turnQueryToString(query);
        const { cads, count } = await axios.get(`https://localhost:7127/API/Home/Gallery?${queryString}`)
            .then(response => response.data);

        if (count) {
            setCads(cads);
            setTotalCount(count);
        }
    }

    async function filterCads() {
        const queryString = turnQueryToString(query);
        const { cads, count } = await axios.get(`https://localhost:7127/API/Home/Gallery?${queryString}`)
            .then(response => response.data);

        setCads(cads);
        setTotalCount(count);
    }
}

export default GalleryPage;