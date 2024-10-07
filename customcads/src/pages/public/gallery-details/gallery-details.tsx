import { useNavigate, useParams } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useQuery } from '@tanstack/react-query';
import { GalleryCad } from '@/requests/public/home';
import ErrorPage from '@/components/error-page';
import useAuth from '@/hooks/useAuth';
import ThreeJS from '@/components/cads/three';
import getStatusCode from '@/utils/get-status-code';
import GalleryDetailsCad, { emptyGalleryDetailsCad } from './gallery-details.interface';

function GalleryDetailsPage() {
    const { t: tPages } = useTranslation('pages');
    const { t: tCommon } = useTranslation('common');
    const { userRole } = useAuth();
    const { id } = useParams();
    const navigate = useNavigate();

    const { data, isError, error } = useQuery({ 
        queryKey: ['gallery-details', id], 
        queryFn: () => GalleryCad(Number(id)).then(res => res.data) 
    });
    if (isError) {
        return <ErrorPage status={getStatusCode(error)} />
    }
    const cad: GalleryDetailsCad = data || emptyGalleryDetailsCad;

    const handleBuy = async () => {
        if (userRole !== 'Client') {
            alert(tPages('gallery.alert_order'));
            return;
        }
        navigate(`/client/purchase/${cad.id}`);
    }

    return (
        <div className="flex bg-indigo-300 rounded-md overflow-hidden border-2 border-indigo-600 shadow-lg shadow-indigo-400">
            <div className="flex justify-center items-center px-8">
                <div className="bg-indigo-200 w-96 h-96 rounded-xl">
                    <ThreeJS cad={cad} />
                </div>
            </div>
            <div className="grow bg-indigo-500 text-indigo-50 flex flex-col">
                <header className="relative px-4 py-4 text-3xl text-center font-bold">
                    <span className="absolute left-8 py-1 px-4 rounded bg-indigo-200 text-indigo-700 text-xl">
                        {tCommon(`categories.${cad.category.name}`)}
                    </span>
                    <div className="grow flex justify-center items-center gap-x-2 font-extrabold">
                        <span>{cad.name} -</span>
                        <span className="underline-offset-4 underline">{cad.price}$</span>
                    </div>
                </header>
                <hr className="border-t-2 border-indigo-700" />
                <section className="grow px-4 py-4">
                    <div className="flex flex-col justify-between gap-y-2 bg-indigo-400 min-h-full rounded-md px-4 py-4">
                        <textarea
                            value={cad.description}
                            readOnly
                            rows={9}
                            className="bg-inherit focus:outline-none resize-none"
                        />
                        <button onClick={handleBuy}
                            className="self-center bg-indigo-700 py-2 px-4 rounded hover:font-extrabold focus:outline-none active:opacity-80"
                        >
                            {tPages('gallery.buy')}
                        </button>
                    </div>
                </section>
                <hr className="border-t-4 border-indigo-700" />
                <footer className="px-4 py-2 flex justify-between">
                    <div className="flex gap-x-2 items-center">
                        <span className="italic">{tPages('gallery.uploaded_by')} </span>
                        <span className="font-bold text-lg">{cad.creatorName}</span>
                    </div>
                    <div className="flex gap-x-2 items-center">
                        <span className="italic">{tPages('gallery.uploaded_on')}</span>
                        <span className="font-bold text-lg">{cad.creationDate}</span>
                    </div>
                </footer>
            </div>
        </div>
    );
}

export default GalleryDetailsPage;