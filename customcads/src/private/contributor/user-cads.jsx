import AuthContext from '@/components/auth-context'
import QueryBar from '@/components/query-bar/query-bar'
import useQueryConverter from '@/hooks/useQueryConverter'
import { useLoaderData, useNavigate } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { useState, useEffect, useContext } from 'react'
import axios from 'axios'
import UserCadItem from './components/user-cads-item'

function UserCads() {
    const { username } = useContext(AuthContext);
    const { t } = useTranslation();
    const navigate = useNavigate();
    const { loadedCads } = useLoaderData();
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
        if (!cads.length) {
            setCads(loadedCads);
        }
    }, []);

    useEffect(() => {
        setQuery((query) => ({ ...query, creator: username }));
    }, [username]);

    useEffect(() => {
        if (username) {
            fetchCads();
        }
    }, [query]);

    const handleDelete = async (id) => {
        await axios.delete(`https://localhost:7127/API/Cads/${id}`, {
            withCredentials: true
        }).catch(e => console.error(e));

        fetchCads();
        navigate("");
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

        const { cads } = await axios.get(`https://localhost:7127/API/Cads?${queryString}`, {
            withCredentials: true
        }).then(response => response.data).catch(e => console.error(e));
        setCads(cads);
    }
}

export default UserCads;