import axios from 'axios'
import { useTranslation } from 'react-i18next'
import { useState, useEffect } from 'react'
import QueryBar from '../query-bar/query-bar'
import GalleryItem from './components/gallery-item'

function GalleryPage() {
    const { t } = useTranslation();
    const [cads, setCads] = useState([]);
    const [_, setTotalCount] = useState(0);

    const [queryString, setQueryString] = useState('sorting=1');

    useEffect(() => {
        if (!cads.length) {
            getCads();
        }
    }, [cads]);

    useEffect(() => {
        filterCads();
    }, [queryString]);

    return (
        <div className="mt-4 mb-8 flex flex-wrap justify-center gap-y-8">
            <h1 className="basis-full text-center text-4xl text-indigo-950 font-bold">{t('Our Gallery')}</h1>
            <QueryBar setQueryString={setQueryString} />
            <section className="basis-full">
                <ul className="flex flex-wrap justify-between gap-y-12 gap-x-3">
                    {cads.map(cad => <GalleryItem key={cad.id} item={cad} />)}
                </ul>
            </section>
        </div>
    );

    async function getCads() {
        const { cads, count } = await axios.get(`https://localhost:7127/API/Home/Gallery?cadsPerPage=6&${queryString}`)
            .then(response => response.data);

        if (count > 0) {
            setCads(cads);
            setTotalCount(count);
        }
    }

    async function filterCads() {
        const { cads, count } = await axios.get(`https://localhost:7127/API/Home/Gallery?cadsPerPage=6&${queryString}`)
            .then(response => response.data);

        setCads(cads);
        setTotalCount(count);
    }
}

export default GalleryPage;