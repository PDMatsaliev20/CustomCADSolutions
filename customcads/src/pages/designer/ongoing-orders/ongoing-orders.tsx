import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import IOrder from '@/interfaces/order';
import usePagination from '@/hooks/usePagination';
import objectToUrl from '@/utils/object-to-url';
import { BeginOrder, ReportOrder, CancelOrder, GetOrdersByStatus } from '@/requests/private/designer';
import capitalize from '@/utils/capitalize';
import SearchBar from '@/components/searchbar';
import Pagination from '@/components/pagination';
import Tab from '@/components/tab';
import Order from '@/components/order';

function OngoingOrders() {
    const { t: tPages } = useTranslation('pages');
    const status = capitalize(useParams().status ?? '');
    const navigate = useNavigate();
    const [orders, setOrders] = useState<IOrder[]>([]);
    const [search, setSearch] = useState({ name: '', category: '', sorting: '' });
    const [total, setTotal] = useState(0);
    const { page, limit, handlePageChange } = usePagination(total, 12);

    useEffect(() => {
        fetchOrders();
        document.documentElement.scrollTo({ top: 0, left: 0, behavior: "instant" });
    }, [status, search, page]);

    const chooseButtons = (order: IOrder) => {
        const mainBtn = "bg-indigo-700 border-2 border-indigo-500 py-3 rounded text-center text-indigo-50 hover:opacity-70 hover:border-transparent";
        const sideBtn = "bg-indigo-50 border-2 border-indigo-600 py-3 rounded text-center text-indigo-950 hover:bg-rose-500 hover:border-transparent hover:text-indigo-50";

        switch (status.toLowerCase()) {
            case 'pending':
                const handleBegin = async () => {
                    try {
                        await BeginOrder(order.id);
                        setOrders(orders => orders.filter(o => o.id !== order.id));
                    } catch (e) {
                        console.error(e);
                    }
                };

                const handleReport = async () => {
                    if (confirm(tPages('designer.confirm_order_report'))) {
                        try {
                            await ReportOrder(order.id);
                            setOrders(orders => orders.filter(o => o.id !== order.id));
                        } catch (e) {
                            console.error(e);
                        }
                    }
                };

                return [
                    <button onClick={handleBegin} className={mainBtn}>
                        {tPages('designer.accept')}
                    </button>,
                    <button onClick={handleReport} className={sideBtn}>
                        {tPages('designer.report')}
                    </button>
                ];

            case 'begun':
                const handleCancel = async () => {
                    try {
                        await CancelOrder(order.id);
                        setOrders(orders => orders.filter(o => o.id !== order.id));
                    } catch (e) {
                        console.error(e);
                    }
                };

                return [
                    <button onClick={() => navigate(`/designer/cads/upload/${order.id}`)} className={mainBtn}>
                        {tPages('designer.complete')}
                    </button>,
                    <button onClick={handleCancel} className={sideBtn}>
                        {tPages('designer.cancel')}
                    </button>
                ];

            case 'finished':
                return [
                    <button onClick={() => { }} className={mainBtn}>
                        {tPages('designer.deliver')}
                    </button>,
                    <button onClick={() => { }} className={sideBtn}>
                        {tPages('designer.dismiss')}
                    </button>
                ];

            default: return [];
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
                : <ul className="flex flex-col gap-y-8">
                    {orders.map(order => <li key={order.id}>
                        <Order order={order} buttons={chooseButtons(order)} />
                    </li>)}
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