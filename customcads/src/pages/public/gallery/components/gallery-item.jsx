import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

function GalleryItem({ item }) {
    const { t } = useTranslation();

    return (
        <li className="flex flex-wrap gap-y-2 bg-indigo-200 rounded-xl border-2 border-indigo-600 shadow-2xl shadow-indigo-400 px-6 py-6 basis-3/12 hover:bg-indigo-300 active:bg-indigo-400">
            <h3 className="basis-full text-center text-indigo-950 text-xl font-extrabold">{item.name}</h3>
            <Link to={`${item.id}`}>
                <div className="basis-full aspect-square bg-indigo-100 rounded-2xl border border-indigo-600 w-full overflow-hidden">
                    <img src={item.imagePath} className="w-full h-full" />
                </div>
            </Link>
            <p className="basis-full text-center text-lg font-semibold">{t('public.gallery.by')} '{item.creatorName}'</p>
        </li>
    );
}

export default GalleryItem;