import axios from 'axios'
import { useTranslation } from 'react-i18next'
import { useLocation } from 'react-router-dom'
import { useState, useEffect } from 'react'
import Cad from '../cad'

function GalleryPage() {
    const { t } = useTranslation();
    const [cads, setCads] = useState([]);
    const [_, setTotalCount] = useState(0);

    const query = new URLSearchParams(useLocation().search).get('search');

    useEffect(() => {
        if (!cads.length) {
            getCads();
        }
    }, [cads]);

    return (
        <>
            <h1 className="mt-3 text-3xl text-center font-bold">{t('Our Gallery')}</h1>
            <section className="my-10">
                <ul className="flex flex-wrap justify-between gap-y-12 gap-x-3">
                    {cads.map(cad =>
                        <li key={cad.id} className="basis-3/12 shrink">
                            <div className="aspect-square bg-indigo-100 rounded-2xl border border-indigo-600 w-full">
                                <Cad cad={cad} />
                            </div>
                        </li>
                    )}
                </ul>
            </section>
        </>
    )
        ;

    async function getCads() {
        const { cads, count } = await axios.get(`https://localhost:7127/API/Home/Gallery?CadsPerPage=9&SearchName=${query || ''}`)
            .then(response => response.data);

        if (count > 0) {
            setCads([...cads]);
            setTotalCount(count);
        }
    }
}

export default GalleryPage;