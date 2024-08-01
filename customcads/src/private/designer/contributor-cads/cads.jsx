import ContributorCadItem from './components/cads-item'
import useQueryConverter from '@/hooks/useQueryConverter'
import QueryBar from '@/components/query-bar/query-bar'
import { useTranslation } from 'react-i18next'
import { useState, useEffect } from 'react'
import { GetCadsByStatus, PatchCadStatus } from '@/requests/private/designer'

function UnvalidatedCads() {
    const { t } = useTranslation();
    const [cads, setCads] = useState([]);
    const [query, setQuery] = useState({ searchName: '', category: '', creator: '', sorting: 1 });
    
    useEffect(() => {
        fetchCads();
    }, [query]);

    const handlePatch = async (id, status) => {
        try {
            await PatchCadStatus(id, status);
            fetchCads();
        } catch (e) {
            console.error(e);
        }
    };
    
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
                                onValidate={() => handlePatch(cad.id, 'Validated')}
                                onReport={() => handlePatch(cad.id, 'Reported')} />)    
                        }
                    </ul>
                </section>
            </div>
        </>
    );

    async function fetchCads() {
        const queryString = useQueryConverter(query);
        try {
            const { data: { cads } } = await GetCadsByStatus(queryString);
            setCads(cads);
        } catch (e) {
            console.error(e);
        }
    }
}

export default UnvalidatedCads;