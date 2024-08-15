import { useTranslation } from 'react-i18next';
import ThreeJS from '@/components/cads/three';

function UncheckedCad({ item, onValidate, onReport }) {
    const { t } = useTranslation();

    const handleValidate = () => {
        if (confirm(t('private.designer.confirm_validate'))) {
            onValidate();
        }
    };

    const handleReport = () => {
        if (confirm(t('private.designer.confirm_report'))) {
            onReport();
        }
    };

    return (
        <li className="flex flex-wrap gap-y-4 bg-indigo-200 rounded-xl border border-indigo-700 shadow-2xl shadow-indigo-800 px-6 py-6 basis-3/12 ">
            <h3 className="basis-full text-center text-indigo-950 text-xl font-extrabold">{item.name}</h3>
            <div className="h-80 w-80 flex justify-center">
                <div className="w-full h-full bg-indigo-100 rounded-xl border-2 border-indigo-300 shadow-lg shadow-indigo-400">
                    <ThreeJS cad={item} />
                </div>
            </div>
            <div className="w-full flex justify-around font-bold">
                <button onClick={handleValidate}
                    className="basis-5/12 bg-indigo-700 text-center text-indigo-50 py-2 rounded-md hover:opacity-70 border-2 border-indigo-300"
                >
                    {t('private.designer.validate')}
                </button>
                <button onClick={handleReport}
                    className="basis-5/12 bg-indigo-50 py-2 rounded-md hover:text-indigo-50 hover:bg-red-500 border border-indigo-700"
                >
                    {t('private.designer.report')}
                </button>
            </div>
        </li>
    );
}

export default UncheckedCad;