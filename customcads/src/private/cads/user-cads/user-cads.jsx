import SearchBar from '@/components/searchbar/searchbar'
import useObjectToURL from '@/hooks/useObjectToURL'
import UserCadItem from './components/user-cads-item'
import { GetCads } from '@/requests/private/cads'
import { useTranslation } from 'react-i18next'
import { useState, useEffect } from 'react'

function UserCads() {
    const { t } = useTranslation();
    const [cads, setCads] = useState([]);
    const [search, setSearch] = useState({ name: '', category: '', sorting: '' });

    useEffect(() => {
        fetchCads();
    }, [search]);

    return (
        <div className="flex flex-col gap-y-8 mb-8">
            <h1 className="text-4xl text-center text-indigo-950 font-bold">
                {t('body.cads.Title')}
            </h1>
            <section className="flex flex-wrap justify-center gap-y-8">
                <SearchBar setSearch={setSearch} />
                <ul className="basis-full grid grid-cols-3 gap-12">
                    {cads.map(cad => <UserCadItem key={cad.id} item={cad} />)}
                </ul>
            </section>
        </div>
    );

    async function fetchCads() {
        const requestSearchParams = useObjectToURL({ ...search });
        try {
            const { data: { cads } } = await GetCads(requestSearchParams);
            setCads(cads);
        } catch (e) {
            console.error(e);
        }
    }
}

export default UserCads;