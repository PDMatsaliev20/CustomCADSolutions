import PendingOrder from './components/pending-order'
import BegunOrder from './components/begun-order'
import FinishedOrder from './components/finished-order'
import { DeleteOrder } from '@/requests/private/orders'
import { useTranslation } from 'react-i18next'
import { Link, useLoaderData, useParams } from 'react-router-dom'
import { useState, useEffect } from 'react'

function UserOrders() {
    const { t } = useTranslation();
    const { loadedOrders } = useLoaderData();
    const { status } = useParams();
    const [orders, setOrders] = useState([]);

    const handleDelete = async (id) => {
        if (confirm(t('body.orders.Alert Delete'))) {
            try {
                await DeleteOrder(id);
                setOrders(orders => orders.filter(o => o.id !== id));
            } catch (e) {
                console.error(e);
            }
        }
    };

    const chooseOrder = (order) => {
        switch (status.toLowerCase()) {
            case 'pending': return <PendingOrder order={order} onDelete={() => handleDelete(order.id)} />;
            case 'begun': return <BegunOrder order={order} onDelete={() => handleDelete(order.id)} />;
            case 'finished': return <FinishedOrder order={order} />;
        }
    };
    
    useEffect(() => {
        setOrders(loadedOrders);
    }, [status]);

    return (
        <div className="flex flex-wrap justify-center gap-y-4 mb-8">
            <section className="basis-full flex flex-col gap-y-4">
                <h1 className="text-4xl text-center text-indigo-950 font-bold">
                    {t(`body.orders.${status}_title`)}
                </h1>
                <hr className="basis-full border-2 rounded border-indigo-600" />
            </section>
            <section className="flex flex-col justify-center items-center gap-y-8">
                <Link to="/orders/custom"
                    className="py-2 px-8 rounded bg-indigo-800 text-indigo-50 font-extrabold"
                >
                    {t('body.orders.New')}
                </Link>
                {!orders.length ?
                    <p className="text-lg text-indigo-900 text-center font-bold">{t('body.orders.No orders')}</p>
                    : <ul className="basis-full grid grid-cols-12 gap-x-16 gap-y-12">
                        {orders.map(order =>
                            <li key={order.id} className="col-span-6">{chooseOrder(order)}</li>)}
                    </ul>
                }
            </section>
        </div>
    );
}

export default UserOrders;