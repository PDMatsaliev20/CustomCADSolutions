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
        <div className="flex flex-wrap justify-center gap-y-8 mb-8">
            <h1 className="basis-full text-4xl text-center text-indigo-950 font-bold">
                {t(`body.orders.${status}_title`)}
            </h1>
            <Link to="/orders/custom"
                className="py-2 px-8 rounded bg-indigo-800 text-indigo-50 font-extrabold"
            >
                {t('body.orders.New')}
            </Link>
            <SearchBar setSearch={setSearch} />
            {!orders.length ?
                <p className="text-lg text-indigo-900 text-center font-bold">{t('body.orders.No orders')}</p>
                : <ul className="basis-full grid grid-cols-12 gap-x-16 gap-y-12">
                    {orders.map(order =>
                        <li key={order.id} className="col-span-6">{chooseOrder(order)}</li>)}
                </ul>
            }
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