import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'

function Order({ order, onDelete }) {
    const { t } = useTranslation();
    const machineReadableDateTime = order.orderDate && order.orderDate.replaceAll('.', '-');

    const handleOrderDelete = () => {
        confirm('Are you sure you want to delete this order?') && onDelete();
    };

    return (
        <div className="bg-indigo-200 px-4 py-4 rounded-lg flex flex-col gap-y-2 shadow-lg shadow-indigo-800">
            <h3 className="text-2xl text-indigo-950 text-center font-semibold">{order.name}</h3>
            <div className="border-b border-indigo-700"></div>
            <section className="py-3 px-2 flex flex-col gap-y-6">
                <p className="text-indigo-900 text-center text-lg truncate">{order.description}</p>
                <div className="flex justify-around text-indigo-50 font-bold">
                    <Link to={`${order.id}`}
                        className="basis-5/12 bg-indigo-700 border border-indigo-500 p-2 rounded text-center text-indigo-50 hover:bg-emerald-500 hover:border-transparent"
                    >
                        {t('body.orders.Details')}
                    </Link>
                    <button onClick={handleOrderDelete}
                        className="basis-5/12 bg-indigo-100 border border-indigo-600 p-2 rounded text-center text-indigo-950 hover:bg-rose-500 hover:border-transparent hover:text-indigo-50">
                        {t('body.orders.Cancel')}
                    </button>
                </div>
            </section>
            <div className="border-t-2 border-indigo-800"></div>
            <div className="text-indigo-800 text-center">
                <span className="font-semibold">{t('body.orders.Ordered on')}</span>
                <time dateTime={machineReadableDateTime} className="italic">
                    {order.orderDate}
                </time>
            </div>
        </div>
    );
}

export default Order;