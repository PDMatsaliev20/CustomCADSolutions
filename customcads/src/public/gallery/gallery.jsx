import QueryBar from '@/components/query-bar/query-bar'
import useQueryConverter from '@/hooks/useQueryConverter'
import GalleryItem from './components/gallery-item'
import { Gallery } from '@/requests/public/home'
import { useSearchParams } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { useState, useEffect } from 'react'

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

    const cadSearchParam = searchParams.get('cad');
    useEffect(() => {
        if (cadSearchParam) {
            setQuery(query => ({ ...query, searchName: cadSearchParam }));
        }
    }, [searchParams]);

    useEffect(() => {
        loadCads();
    }, [query]);
    
    return (
        <div className="mb-8 flex flex-wrap justify-center gap-y-8">
            <h1 className="basis-full text-center text-4xl text-indigo-950 font-bold">{t('body.gallery.Our Gallery')}</h1>
            <QueryBar query={{ ...query, searchName: cadSearchParam }} setQuery={setQuery} />
            <section className="basis-full">
                <ul className="grid grid-cols-3 gap-y-12 gap-x-10">
                    {cads.map(cad => <GalleryItem key={cad.id} item={cad} />)}
                </ul>
            </section>
        </div>
    );

    async function loadCads() {
        const queryString = useQueryConverter(query);
        const { data: { cads, count } } = await Gallery(queryString)
        
        setCads(cads);
        setTotalCount(count);
    }
}

export default GalleryPage;