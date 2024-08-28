import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { dateToMachineReadable } from '@/utils/date-manager';

function PendingOrder({ order, onDelete }) {
    const { t } = useTranslation();

    return (
        <div className="min-h-full bg-indigo-200 px-4 py-4 rounded-lg flex flex-col gap-y-2 shadow-lg shadow-indigo-800">
            <h3 className="text-2xl text-indigo-950 text-center font-semibold">{order.name}</h3>
            <hr className="border-b border-indigo-700" />
            <section className="grow py-3 px-4 flex flex-col gap-y-6">
                <p className="grow text-indigo-900 text-center text-lg line-clamp-3">{order.description}</p>
                <div className="flex flex-wrap justify-around text-indigo-50 font-bold">
                    <Link to={`${order.id}`}
                        className="basis-4/12 bg-indigo-700 border border-indigo-500 py-2 px-4 rounded text-center text-indigo-50 hover:opacity-70 hover:border-transparent"
                    >
                        {t('private.orders.details')}
                    </Link>
                    <button onClick={onDelete}
                        className="basis-5/12 bg-indigo-100 border border-indigo-600 py-2 px-4 rounded text-center text-indigo-950 hover:bg-rose-500 hover:border-transparent hover:text-indigo-50"
                    >
                        {t('private.orders.delete')}
                    </button>
                </div>
            </section>
            <hr className="border-t-2 border-indigo-800" />
            <div className="flex justify-between px-4 text-indigo-800 text-center">
                <div>
                    <span className="font-semibold">{t('private.orders.category')}</span>
                    <span className="italic">{t(`common.categories.${order.category}`)}</span>
                </div>
                <div>
                    <span className="font-semibold">{t('private.orders.ordered_on')}</span>
                    <time dateTime={dateToMachineReadable(order.orderDate)} className="italic">
                        {order.orderDate}
                    </time>
                </div>
            </div>
        </div>
    );
}

export default PendingOrder;