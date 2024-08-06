import SearchBar from '@/components/searchbar/searchbar'
import useObjectToURL from '@/hooks/useObjectToURL'
import PendingOrder from './components/pending-order'
import BegunOrder from './components/begun-order'
import FinishedOrder from './components/finished-order'
import { DeleteOrder } from '@/requests/private/orders'
import { useTranslation } from 'react-i18next'
import { Link, useParams } from 'react-router-dom'
import { useState, useEffect } from 'react'
import { GetOrders } from '@/requests/private/orders'

function UserOrders() {
    const { t } = useTranslation();
    const [orders, setOrders] = useState([]);
    const { status } = useParams();
    const [search, setSearch] = useState({ name: '', category: '', sorting: '' });

    useEffect(() => {
        fetchOrders();
    }, [search, status]);

    const handleDelete = async (id) => {
        if (confirm(t('body.orders.Alert Delete'))) {
            try {
                await DeleteOrder(id);
                fetchOrders();
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

    return (
        <div className="flex flex-wrap justify-center gap-y-8 mt-4 mb-8">
            <div className="basis-full flex flex-col gap-y-[0.4px]"> 
                <h2 className="px-4 text-2xl text-indigo-950">
                    <div className="flex justify-between items-center rounded-t-xl border-4 border-indigo-700 overflow-hidden bg-indigo-500 text-center font-bold">
                        <Link
                            to="/orders/pending"
                            className={`basis-1/3 bg-indigo-300 py-4 hover:no-underline ${status === 'pending' ? 'font-extrabold bg-indigo-500 text-indigo-50' : ''}`}
                        >
                            {t('body.orders.pending_title')}
                        </Link>
                        <Link
                            to="/orders/begun"
                            className={`basis-1/3 bg-indigo-300 py-4 border-x-2 border-indigo-700 hover:no-underline ${status === 'begun' ? 'font-extrabold bg-indigo-500 text-indigo-50' : ''}`}
                        >
                            {t('body.orders.begun_title')}
                        </Link>
                        <Link
                            to="/orders/finished"
                            className={`basis-1/3 bg-indigo-300 py-4 hover:no-underline ${status === 'finished' ? 'font-extrabold bg-indigo-500 text-indigo-50' : ''}`}
                        >
                            {t('body.orders.finished_title')}
                        </Link>
                    </div>
                </h2>
                <SearchBar setSearch={setSearch} />
            </div>
            {!orders.length ?
                <p className="text-lg text-indigo-900 text-center font-bold">{t('body.orders.No orders')}</p>
                : <ul className="basis-full grid grid-cols-12 gap-x-16 gap-y-12">
                    {orders.map(order =>
                        <li key={order.id} className="col-span-6">{chooseOrder(order)}</li>)}
                </ul>}
        </div>
    );

    async function fetchOrders() {
        const requestSearchParams = useObjectToURL({ ...search });
        try {
            const { data: { orders } } = await GetOrders(status, requestSearchParams);
            setOrders(orders);
        } catch (e) {
            console.error(e);
        }
    }
}

export default UserOrders;