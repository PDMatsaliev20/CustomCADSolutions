import { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import useObjectToURL from '@/hooks/useObjectToURL';
import { Gallery } from '@/requests/public/home';
import SearchBar from '@/components/searchbar';
import GalleryItem from './components/gallery-item';

function GalleryPage() {
    const { t } = useTranslation();
    const [cads, setCads] = useState([]);
    const [search, setSearch] = useState({ name: '', category: '', owner: '', sorting: '' });

    useEffect(() => {
        loadCads();
    }, [search]);

    return (
        <div className="mb-8 flex flex-wrap justify-center gap-y-8">
            <h1 className="basis-full text-center text-4xl text-indigo-950 font-bold">{t('body.gallery.Our Gallery')}</h1>
            <SearchBar setSearch={setSearch} />
            <section className="basis-full">
                {!cads.length
                    ? <p className="text-lg text-indigo-900 text-center font-bold">{t('body.gallery.No cads')}</p>
                    : <ul className="grid grid-cols-3 gap-y-12 gap-x-10">
                        {cads.map(cad => <GalleryItem key={cad.id} item={cad} />)}
                    </ul>}
            </section>
        </div>
    );

    async function loadCads() {
        const requestSearchParams = useObjectToURL({ ...search });
        const { data: { cads } } = await Gallery(requestSearchParams);
        setCads(cads);
    }
}

export default GalleryPage;