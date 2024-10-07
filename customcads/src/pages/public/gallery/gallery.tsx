import { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { useQuery } from '@tanstack/react-query';
import usePagination from '@/hooks/usePagination';
import objectToUrl from '@/utils/object-to-url';
import { Gallery } from '@/requests/public/home';
import ErrorPage from '@/components/error-page';
import SearchBar from '@/components/searchbar';
import Pagination from '@/components/pagination';
import CadItem from '@/components/cads/item';
import getStatusCode from '@/utils/get-status-code';
import GalleryPageCad from './gallery.interface';

function GalleryPage() {
    const { t: tPages } = useTranslation('pages');
    const [search, setSearch] = useState({ name: '', category: '', sorting: '' });
    
    let total = 0;
    const { page, limit, handlePageChange } = usePagination(total, 12);
    let cads: GalleryPageCad[] = [];

    const { data, isError, error } = useQuery({
        queryKey: ['gallery', search, page],
        queryFn: async () => {
            const requestSearchParams = objectToUrl({ ...search, page, limit });
            const { data } = await Gallery(requestSearchParams);
            return data;
        },
    });
    if (isError) {
        return <ErrorPage status={getStatusCode(error)} />
    }
    if (data) {
        cads = data.cads;
        total = data.count;
    }

    useEffect(() => {
        document.documentElement.scrollTo({ top: 0, left: 0, behavior: "instant" });
    }, [search, page]);

    return (
        <div className="mt-4 mb-6 flex flex-col gap-y-6">
            <h1 className="basis-full text-center text-4xl text-indigo-950 font-bold">
                {tPages('gallery.title')}
            </h1>
            <div className="flex flex-wrap justify-center gap-y-10">
                <SearchBar setSearch={setSearch} />
                <section className="basis-full">
                    {!cads.length
                        ? <p className="text-lg text-indigo-900 text-center font-bold">
                            {tPages('gallery.no_cads')}
                        </p>
                        : <ul className="grid grid-cols-3 gap-y-12 gap-x-10">
                            {cads.map(cad => <CadItem key={cad.id} cad={cad} by />)}
                        </ul>}
                </section>
                <div className="basis-full" hidden={!cads.length}>
                    <Pagination
                        page={page}
                        limit={limit}
                        onPageChange={handlePageChange}
                        total={total}
                    />
                </div>
            </div>
        </div>
    );
}

export default GalleryPage;