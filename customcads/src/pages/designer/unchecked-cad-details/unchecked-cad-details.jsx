import { useEffect } from 'react';
import { useNavigate, useLoaderData } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { PatchCadStatus } from '@/requests/private/designer';
import ErrorPage from '@/components/error-page';
import ThreeJS from '@/components/cads/three';

function UncheckedCadDetails() {
    const navigate = useNavigate();

    const loaderData = useLoaderData();
    if (loaderData.error) {
        if (loaderData.unauthenticated) {
            return <ErrorPage status={401} />
        }

        if (loaderData.unauthorized) {
            return <ErrorPage status={403} />
        }

        return <ErrorPage status={loaderData.status} />
    }
    const { prevId, loadedCad, nextId } = loaderData;

    const handlePatch = async (status) => {
        try {
            await PatchCadStatus(loadedCad.id, status);
            navigate('/designer/cads/unchecked');
        } catch (e) {
            console.error(e);
        }
    };

    return (
        <>
            <div className="mt-2 h-[60vh] flex flex-col justify-between">
                <div className="basis-[80%] w-full self-center flex justify-around items-center">
                    <button onClick={() => navigate(`/designer/cads/unchecked/${prevId}`)} disabled={!prevId} className={prevId ? "" : "opacity-50"}>
                        <FontAwesomeIcon icon="circle-chevron-left" className="text-5xl text-indigo-800" />
                    </button>
                    <div className="h-full basis-1/2 rounded-3xl overflow-hidden">
                        <ThreeJS cad={loadedCad} />
                    </div>
                    <button onClick={() => navigate(`/designer/cads/unchecked/${nextId}`)} disabled={!nextId} className={nextId ? "" : "opacity-50"}>
                        <FontAwesomeIcon icon="circle-chevron-right" className="text-5xl text-indigo-800" />
                    </button>
                </div>
                <div className="basis-[5%] flex justify-evenly">
                    <button
                        onClick={() => handlePatch('Validated')}
                        className="bg-green-500 px-12 py-2 text-indigo-50 rounded-lg border-2 border-green-700 hover:opacity-80 active:bg-green-600"
                    >
                        <FontAwesomeIcon icon="check" className="text-2xl" />
                    </button>
                    <button
                        onClick={() => handlePatch('Reported')}
                        className="bg-red-500 px-12 py-2 text-indigo-50 rounded-lg border-2 border-red-700 hover:opacity-80 active:bg-red-600"
                    >
                        <FontAwesomeIcon icon="flag" className="text-2xl" />
                    </button>
                </div>
            </div>
        </>
    );
}

export default UncheckedCadDetails;