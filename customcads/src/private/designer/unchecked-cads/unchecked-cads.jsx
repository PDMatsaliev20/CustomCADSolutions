import { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import objectToUrl from '@/utils/object-to-url';
import { GetCadsByStatus, PatchCadStatus } from '@/requests/private/designer';
import SearchBar from '@/components/searchbar';
import ContributorCadItem from './components/unchecked-cad';

function UncheckedCads() {
    const { t } = useTranslation();
    const [cads, setCads] = useState([]);
    const [search, setSearch] = useState({ name: '', category: '', creator: '', sorting: '' });

    useEffect(() => {
        fetchCads();
    }, [search]);

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
                    {t('private.designer.unchecked-cads_title')}
                </h1>
                <section className="flex flex-wrap justify-center gap-y-8">
                    <SearchBar setSearch={setSearch} />
                    {!cads.length
                        ? <p className="text-lg text-indigo-900 text-center font-bold">{t('private.designer.no_cads')}</p>
                        : <ul className="basis-full grid grid-cols-3 gap-12">
                            {cads.map(cad =>
                                <ContributorCadItem key={cad.id}
                                    item={cad}
                                    onValidate={() => handlePatch(cad.id, 'Validated')}
                                    onReport={() => handlePatch(cad.id, 'Reported')} />)
                            }
                        </ul>}
                </section>
            </div>
        </>
    );

    async function fetchCads() {
        const requestSearchParams = objectToUrl({ ...search });
        try {
            const { data: { cads } } = await GetCadsByStatus(requestSearchParams);
            setCads(cads);
        } catch (e) {
            console.error(e);
        }
    }
}

export default UncheckedCads;