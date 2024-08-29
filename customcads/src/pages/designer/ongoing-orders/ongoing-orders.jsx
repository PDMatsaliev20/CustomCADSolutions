import { useState, useEffect } from 'react';
import { Link, useLoaderData } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import usePagination from '@/hooks/usePagination';
import objectToUrl from '@/utils/object-to-url';
import { GetOrdersByStatus } from '@/requests/private/designer';
import SearchBar from '@/components/searchbar';
import Pagination from '@/components/pagination';
import Tab from '@/components/tab';
import PendingOrder from './components/pending-order';
import BegunOrder from './components/begun-order';
import FinishedOrder from './components/finished-order';

function OngoingOrders() {
    const { t: tPages } = useTranslation('pages');
    const { status } = useLoaderData();
    const [orders, setOrders] = useState([]);
    const [search, setSearch] = useState({ name: '', category: '', owner: '', sorting: '' });
    const [total, setTotal] = useState(0);
    const { page, limit, handlePageChange } = usePagination(total, 12);

    useEffect(() => {
        fetchOrders();
        document.documentElement.scrollTo({ top: 0, left: 0, behavior: "instant" });
    }, [status, search, page]);

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
            <div className="basis-full flex flex-col">
                <h2 className="px-4 basis-full text-3xl text-indigo-950">
                    <div className="flex justify-between items-center rounded-t-xl border-t-4 border-x-4 border-b border-indigo-600 overflow-hidden bg-indigo-500 text-center font-bold">
                        <Tab position="start" to="/designer/orders/pending" text={tPages('designer.pending_title')} isActive={status === 'Pending'} />
                        <Tab position="middle" to="/designer/orders/begun" text={tPages('designer.begun_title')} isActive={status === 'Begun'} />
                        <Tab position="end" to="/designer/orders/finished" text={tPages('designer.finished_title')} isActive={status === 'Finished'} />
                    </div>
                </h2>
                <SearchBar setSearch={setSearch} />
            </div>
            {!orders.length
                ? <p className="basis-full text-lg text-indigo-900 text-center font-bold">
                    {tPages('designer.no_orders')}
                </p>
                : <ul className="basis-full grid grid-cols-3 gap-y-8 gap-x-[5%]">
                    {orders.map(order =>
                        <li key={order.id}>{chooseOrder(order)}</li>
                    )}
                </ul>}
            <div className="basis-full" hidden={!orders.length}>
                <Pagination
                    page={page}
                    limit={limit}
                    onPageChange={handlePageChange}
                    total={total}
                />
            </div>
        </div>
    );
    async function fetchOrders() {
        const requestSearchParams = objectToUrl({ ...search });
        try {
            const { data: { orders, count } } = await GetOrdersByStatus(status, requestSearchParams);
            setOrders(orders);
            setTotal(count);
        } catch (e) {
            console.error(e);
        }
    }
}

export default OngoingOrders;