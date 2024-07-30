import ContributorCadItem from './components/cads-item'
import useQueryConverter from '@/hooks/useQueryConverter'
import QueryBar from '@/components/query-bar/query-bar'
import { useTranslation } from 'react-i18next'
import { useNavigate } from 'react-router-dom'
import { useState, useEffect } from 'react'
import axios from 'axios'

function UnvalidatedCads() {
    const { t } = useTranslation();
    const navigate = useNavigate();
    const [cads, setCads] = useState([]);
    const [query, setQuery] = useState({ searchName: '', category: '', creator: '', sorting: 1 });
    
    useEffect(() => {
        fetchCads();
    }, [query]);

    const handleValidate = async (id) => {
        await axios.patch(`https://localhost:7127/API/Designer/Cads/Status/${id}?status=1`, {}, {
            withCredentials: true
        }).catch(e => console.error(e));

        fetchCads();
    }
    
    const handleReport = async (id) => {
        await axios.patch(`https://localhost:7127/API/Designer/Cads/Status/${id}?status=2`, {}, {
            withCredentials: true
        }).catch(e => console.error(e));

        fetchCads();
    }

    return (
        <>
            <div className="flex flex-col gap-y-8 mb-8">
                <h1 className="text-4xl text-center text-indigo-950 font-bold">
                    {t('body.designerCads.Title')}
                </h1>
                <section className="flex flex-wrap justify-center gap-y-8">
                    <QueryBar setQuery={setQuery} />
                    <ul className="basis-full grid grid-cols-3 gap-12">
                        {cads.map(cad =>
                            <ContributorCadItem key={cad.id}
                                item={cad}
                                onValidate={() => handleValidate(cad.id)}
                                onReport={() => handleReport(cad.id)} />)    
                        }
                    </ul>
                </section>
            </div>
        </>
    );

    async function fetchCads() {
        const queryString = useQueryConverter(query);
        const response = await axios.get(`https://localhost:7127/API/Designer/Cads?${queryString}`, {
            withCredentials: true
        }).catch(e => console.error(e));

        const { cads } = response.data;
        setCads(cads);
    }
}

export default UnvalidatedCads;