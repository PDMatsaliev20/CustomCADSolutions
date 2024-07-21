import Cad from '@/components/cad'
import Coords from './components/coords'
import { useLoaderData, useNavigate } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { useState, useEffect } from 'react'
import axios from 'axios'

function CadDetailsPage() {
    const { t } = useTranslation();
    const navigate = useNavigate();
    let { loadedCad } = useLoaderData();
    const [cad, setCad] = useState(loadedCad);
    const [isChanged, setIsChanged] = useState(false);

    useEffect(() => {
        loadedCad.price = loadedCad.price.toString();
        setCad(loadedCad);
    }, [loadedCad]);
    
    const compareValues = (val1, val2) => JSON.stringify(val1) === JSON.stringify(val2);

    const updateIsChanged = () => {
        if (compareValues(cad.coords, loadedCad.coords)
            && compareValues(cad.panCoords, loadedCad.panCoords)) {
            setIsChanged(false);
        } else setIsChanged(true);
    };

    useEffect(() => {
        updateIsChanged();
    }, [cad.coords, cad.panCoords]);

    const handleSaveChanges = async () => {
        await axios.patch(`https://localhost:7127/API/Cads/${cad.id}`, [
            {
                path: "/coords",
                op: "replace",
                value: cad.coords
            },
            {
                path: "/panCoords",
                op: "replace",
                value: cad.panCoords
            },
        ], {
            withCredentials: true
        }).catch(e => console.error(e));

        navigate('', { replace: true });
    };

    return (
        <div className="flex flex-wrap justify-evenly gap-y-4 gap-x-8">
            <div className="basis-full flex min-h-12 justify-center gap-x-8">
                <h1 className="text-4xl text-indigo-950 text-center font-bold">
                    {cad.name}
                </h1>
                <div className={`${isChanged ? 'flex justify-center' : 'hidden'}`}>
                    <button onClick={handleSaveChanges} className="bg-indigo-500 text-indigo-50 p-2 text-lg rounded-md">
                        {t('body.cadDetails.Save changes')}
                    </button>
                </div>
            </div>
            <div className="basis-3/12 flex flex-wrap items-center gap-y-4">
                <div className="basis-full">
                    <Coords
                        type="camera"
                        title={t('body.cadDetails.Camera Coords')}
                        coords={cad.coords}
                        relatedCoords={cad.panCoords}
                        setCoords={(coords) => setCad(cad => ({ ...cad, coords }))}
                        setRelatedCoords={(panCoords) => setCad(cad => ({ ...cad, panCoords }))}
                    />
                </div>
            </div>
            <div className="h-80 w-80 flex justify-center">
                <div className="w-full h-full bg-indigo-100 rounded-xl border-2 border-indigo-300 shadow-lg shadow-indigo-400">
                    <Cad cad={cad} />
                </div>
            </div>
            <div className="basis-3/12 flex flex-wrap items-center gap-y-4">
                <div className="basis-full">
                    <Coords
                        type="pan"
                        title={t('body.cadDetails.CAD Coords')}
                        coords={cad.panCoords}
                        relatedCoords={cad.coords}
                        setCoords={(panCoords) => setCad(cad => ({ ...cad, panCoords }))}
                        setRelatedCoords={(coords) => setCad(cad => ({ ...cad, coords }))}
                    />
                </div>
            </div>
        </div>
    );
}

export default CadDetailsPage;