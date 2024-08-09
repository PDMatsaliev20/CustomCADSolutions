import { useState, useEffect } from 'react';
import { Link, useLoaderData } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import useObjectToURL from '@/hooks/useObjectToURL';
import { GetOrdersByStatus } from '@/requests/private/designer';
import SearchBar from '@/components/searchbar/searchbar';
import PendingOrder from './components/pending-order';
import BegunOrder from './components/begun-order';
import FinishedOrder from './components/finished-order';

function OngoingOrders() {
    const { t } = useTranslation();
    const { status } = useLoaderData();
    const [orders, setOrders] = useState([]);
    const [search, setSearch] = useState({ name: '', category: '', owner: '', sorting: '' });

    useEffect(() => {
        fetchOrders();
    }, [status, search]);

    const updateOrders = (id) => {
        setOrders(orders => orders.filter(order => order.id !== id));
    };

    const chooseOrder = (order) => {
        switch (status) {
            case 'Pending': return <PendingOrder order={order} updateParent={updateOrders} />;
            case 'Begun': return <BegunOrder order={order} updateParent={updateOrders} />;
            case 'Finished': return <FinishedOrder order={order} updateParent={updateOrders} />;
        }
    };

    return (
        <div className="flex flex-wrap justify-center gap-y-12 mt-4 mb-8">
            <div className="basis-full flex flex-col gap-y-[1px]">
                <h2 className="px-4 basis-full text-3xl text-indigo-950">
                    <div className="flex justify-between items-center rounded-t-xl border-4 border-indigo-600 overflow-hidden bg-indigo-500 text-center font-bold">
                        <Link
                            to="/designer/orders/pending"
                            className={`basis-1/3 bg-indigo-300 py-4 hover:no-underline ${status === 'Pending' ? 'font-extrabold bg-indigo-500 text-indigo-50' : ''}`}
                        >
                            {t('body.designerOrders.Pending Title')}
                        </Link>
                        <Link
                            to="/designer/orders/begun"
                            className={`basis-1/3 bg-indigo-300 py-4 border-x-2 border-indigo-600 hover:no-underline ${status === 'Begun' ? 'font-extrabold bg-indigo-500 text-indigo-50' : ''}`}
                        >
                            {t('body.designerOrders.Begun Title')}
                        </Link>
                        <Link
                            to="/designer/orders/finished"
                            className={`basis-1/3 bg-indigo-300 py-4 hover:no-underline ${status === 'Finished' ? 'font-extrabold bg-indigo-500 text-indigo-50' : ''}`}
                        >
                            {t('body.designerOrders.Finished Title')}
                        </Link>
                    </div>
                </h2>
                <SearchBar setSearch={setSearch} />
            </div>
            {!orders.length ?
                <p className="basis-full text-lg text-indigo-900 text-center font-bold">{t('body.designerOrders.No orders')}</p>
                : <ul className="basis-full grid grid-cols-3 gap-y-8 gap-x-[5%]">
                    {orders.map(order =>
                        <li key={order.id}>{chooseOrder(order)}</li>
                    )}
                </ul>}
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

export default OngoingOrders;