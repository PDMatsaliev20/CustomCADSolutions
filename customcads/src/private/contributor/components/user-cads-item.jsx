import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'

function UserCadItem({ item, onDelete }) {
    const { t } = useTranslation();

    const handleClick = () => {
        if (confirm('sure?')) {
            onDelete();
        }
    };

    const handleLinkClick = (e) => {
        e.stopPropagation();
    }

    return (
        <li className="flex flex-wrap gap-y-4 bg-indigo-200 rounded-xl border border-indigo-700 shadow-2xl shadow-indigo-800 px-6 py-6 basis-3/12 ">
            <h3 className="basis-full text-center text-indigo-950 text-xl font-extrabold">{item.name}</h3>
            <Link to={`${item.id}`} className="basis-full aspect-square bg-indigo-100 rounded-2xl border border-indigo-600 overflow-hidden">
                <img src={item.imagePath} className="w-full h-full" />
            </Link>
            <div className="w-full flex justify-around font-bold">
                <Link to={`edit/${item.id}`}
                    className="basis-5/12 bg-indigo-700 text-center text-indigo-50 py-2 rounded-md hover:opacity-70 border"
                >
                    {t('body.cads.Edit')}
                </Link>
                <button onClick={handleClick}
                    className="basis-5/12 bg-indigo-50 py-2 rounded-md hover:text-indigo-50 hover:bg-red-500 border border-indigo-700"
                >
                    {t('body.cads.Delete')}
                </button>
            </div>
            <p className="basis-full text-center text-lg text-indigo-900 font-semibold">
                {t('body.cads.Uploaded on')} '{item.creationDate}'
            </p>
        </li>   
    );
}

export default UserCadItem;