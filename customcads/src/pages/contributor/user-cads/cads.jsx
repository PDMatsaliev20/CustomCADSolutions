import { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import usePagination from '@/hooks/usePagination';
import objectToUrl from '@/utils/object-to-url';
import { GetCads } from '@/requests/private/cads';
import SearchBar from '@/components/searchbar';
import Pagination from '@/components/pagination';
import UserCadItem from '@/components/cads/item';

function UserCads() {
    const { t } = useTranslation();
    const [cads, setCads] = useState([]);
    const [search, setSearch] = useState({ name: '', category: '', sorting: '' });
    const [total, setTotal] = useState(0);
    const { page, limit, handlePageChange } = usePagination(total, 12);

    useEffect(() => {
        fetchCads();
        document.documentElement.scrollTo({ top: 0, left: 0, behavior: "instant" });
    }, [search, page]);

    return (
        <div className="flex flex-col gap-y-8 mb-8">
            <h1 className="text-4xl text-center text-indigo-950 font-bold">
                {t('private.cads.title')}
            </h1>
            <section className="flex flex-wrap justify-center gap-y-8">
                <SearchBar setSearch={setSearch} />
                {!cads.length
                    ? <p className="text-lg text-indigo-900 text-center font-bold">{t('private.cads.no_cads')}</p>
                    : <ul className="basis-full grid grid-cols-3 gap-12">
                        {cads.map(cad => <UserCadItem key={cad.id} item={cad} on />)}
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
    );

    async function fetchCads() {
        const requestSearchParams = objectToUrl({ ...search });
        try {
            const { data: { cads, count } } = await GetCads(requestSearchParams);
            setCads(cads);
            setTotal(count);
        } catch (e) {
            console.error(e);
        }
    }
}

export default UserCads;