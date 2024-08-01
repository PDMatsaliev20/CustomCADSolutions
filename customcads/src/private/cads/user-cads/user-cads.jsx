import AuthContext from '@/components/auth-context'
import QueryBar from '@/components/query-bar/query-bar'
import useQueryConverter from '@/hooks/useQueryConverter'
import UserCadItem from './components/user-cads-item'
import { GetCads, DeleteCad } from '@/requests/private/cads'
import { useTranslation } from 'react-i18next'
import { useState, useEffect, useContext } from 'react'

function UserCads() {
    const { username } = useContext(AuthContext);
    const { t } = useTranslation();
    const [cads, setCads] = useState([]);
    const [query, setQuery] = useState({
        searchName: '',
        category: '',
        sorting: 1,
        currentPage: 1,
        cadsPerPage: 9,
        creator: username,
        validated: true,
        unvalidated: true,
    });

    useEffect(() => {
        setQuery((query) => ({ ...query, creator: username }));
    }, [username]);

    useEffect(() => {
        if (query.creator) {
            fetchCads();
        }
    }, [query]);

    const handleDelete = async (id) => {
        try {
            await DeleteCad(id);
            fetchCads();
        } catch (e) {
            console.error(e);
        }
    };

    return (
        <div className="flex flex-col gap-y-8 mb-8">
            <h1 className="text-4xl text-center text-indigo-950 font-bold">
                {t('body.cads.Title')}
            </h1>
            <section className="flex flex-wrap justify-center gap-y-8">
                <QueryBar setQuery={setQuery} />
                <ul className="basis-full grid grid-cols-3 gap-12">
                    {cads.map(cad => <UserCadItem key={cad.id} item={cad} onDelete={() => handleDelete(cad.id)} />)}
                </ul>
            </section>
        </div>
    );

    async function fetchCads() {
        const queryString = useQueryConverter(query);
        try {
            const { data: { cads } } = await GetCads(queryString);
            setCads(cads);
        } catch (e) {
            console.error(e);
        }
    }
}

export default UserCads;