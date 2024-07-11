import { useTranslation } from 'react-i18next'
import Cad from '@/components/cad'

function GalleryItem({ item }) {

    const { t } = useTranslation();

    return (
        <li className="flex flex-wrap gap-y-2 bg-indigo-200 rounded-xl shadow-2xl shadow-indigo-800 px-6 py-6 basis-3/12 ">
            <h3 className="basis-full text-center text-indigo-950 text-xl font-extrabold">{item.name}</h3>
            <div className="basis-full aspect-square bg-indigo-100 rounded-2xl border border-indigo-600 w-full">
                <Cad cad={item} />
            </div>
            <p className="basis-full text-center text-lg font-semibold">{t('body.gallery.by')} '{item.creatorName}'</p>
        </li>
    );
}

export default GalleryItem;