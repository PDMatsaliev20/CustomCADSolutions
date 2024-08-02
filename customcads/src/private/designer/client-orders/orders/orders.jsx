import SearchBar from '@/components/searchbar/searchbar'
import useObjectToURL from '@/hooks/useObjectToURL'
import PendingOrder from './components/pending-order'
import BegunOrder from './components/begun-order'
import FinishedOrder from './components/finished-order'
import { useLoaderData } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { useState, useEffect } from 'react'
import { GetOrdersByStatus } from '@/requests/private/designer'

function AllOrders() {
    const { t } = useTranslation();
    const { status } = useLoaderData();
    const [orders, setOrders] = useState([]);
    const [search, setSearch] = useState({ name: '', category: '', owner: '', sorting: '' });

    useEffect(() => {
        fetchOrders();
    }, [status, search]);


    const chooseOrder = (order) => {
        switch (status) {
            case 'Pending': return <PendingOrder order={order} />;
            case 'Begun': return <BegunOrder order={order} />;
            case 'Finished': return <FinishedOrder order={order} />;
        }
    };

    return (
        <div className="flex flex-wrap justify-center gap-y-12 mb-8">
            <h1 className="basis-full text-center text-4xl text-indigo-950 font-bold">
                {t(`body.designerOrders.${status} Title`)}
            </h1>
            <SearchBar setSearch={setSearch} />
            {!orders.length ?
                <p className="basis-full text-lg text-indigo-900 text-center font-bold">{t('body.designerOrders.No orders')}</p>
                : <ul className="basis-full grid grid-cols-3 gap-y-8 gap-x-[5%]">
                    {orders.map(order =>
                        <li key={order.id}>{chooseOrder(order)}</li>
                    )}
                </ul>
            }
        </div>
    );
    async function fetchOrders() {
        const requestSearchParams = useObjectToURL({ ...search });
        try {
            const { data: { orders } } = await GetOrdersByStatus(status, requestSearchParams);
            setOrders(orders);
        } catch (e) {
            console.error(e);
        }
    }
}

export default AllOrders;