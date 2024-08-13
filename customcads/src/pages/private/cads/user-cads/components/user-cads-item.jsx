import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

function UserCadItem({ item }) {
    const { t } = useTranslation();

    return (
        <li className="flex flex-wrap gap-y-4 bg-indigo-200 rounded-xl border border-indigo-700 shadow-2xl shadow-indigo-800 px-6 py-6 basis-3/12 ">
            <h3 className="basis-full text-center text-indigo-950 text-xl font-extrabold">{item.name}</h3>
            <Link to={`${item.id}`} className="basis-full aspect-square bg-indigo-100 rounded-2xl border border-indigo-600 overflow-hidden">
                <img src={item.imagePath} className="w-full h-full" />
            </Link>
            <p className="basis-full text-center text-lg text-indigo-900 font-semibold">
                {t('private.cads.uploaded_on')} '{item.creationDate}'
            </p>
        </li>   
    );
}

export default UserCadItem;