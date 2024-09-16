import { useState, useEffect } from 'react';
import { Link, useLoaderData } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import usePagination from '@/hooks/usePagination';
import objectToUrl from '@/utils/object-to-url';
import { GetOrders, PatchOrder, DownloadOrderCad, DeleteOrder } from '@/requests/private/orders';
import SearchBar from '@/components/searchbar';
import Pagination from '@/components/pagination';
import ErrorPage from '@/components/error-page';
import Tab from '@/components/tab';
import Order from '@/components/order';

function UserOrders() {
    const { t: tPages } = useTranslation('pages');
    const [orders, setOrders] = useState([]);
    const { status } = useLoaderData();
    const [search, setSearch] = useState({ name: '', category: '', sorting: '' });
    const [total, setTotal] = useState(0);
    const { page, limit, handlePageChange } = usePagination(total, 3);
    
    const checkStatus = (check) => status === check;
    if (!(checkStatus('Pending') || checkStatus('Begun') || checkStatus('Finished'))) {
        return <ErrorPage status={404} />
    }

    useEffect(() => {
        fetchOrders();
        document.documentElement.scrollTo({ top: 0, left: 0, behavior: "instant" });
    }, [status, search, page]);

    const handleDelete = async (id) => {
        if (confirm(tPages('orders.alert_delete'))) {
            try {
                await DeleteOrder(id);
                fetchOrders();
            } catch (e) {
                console.error(e);
            }
        }
    };

    const chooseButtons = (order) => {
        switch (status.toLowerCase()) {
            case 'pending':
                return [
                    <Link to={`${order.id}`} className="bg-indigo-700 border-2 border-indigo-500 px-10 py-3 rounded text-center text-indigo-50 hover:opacity-70 hover:border-transparent">
                        {tPages('orders.details')}
                    </Link>,
                    <button onClick={handleDelete} className="bg-indigo-50 border-2 border-indigo-600 px-10 py-3 rounded text-center text-indigo-950 hover:bg-rose-500 hover:border-transparent hover:text-indigo-50">
                        {tPages('orders.delete')}
                    </button>
                ];

            case 'begun':
                return [
                    <Link to={`mailto:${order.designerEmail}`} className="basis-6/12 bg-indigo-700 border-2 border-indigo-500 px-10 py-3 rounded text-center text-indigo-50 hover:opacity-70 hover:border-transparent">
                        {tPages('orders.contact')}
                    </Link>,
                    <button onClick={handleDelete} className="basis-5/12 bg-indigo-50 border-2 border-indigo-600 px-10 py-3 rounded text-center text-indigo-950 hover:bg-rose-500 hover:border-transparent hover:text-indigo-50">
                        {tPages('orders.cancel_order')}
                    </button>
                ];

            case 'finished':
                const handleRequest = async () => {
                    try {
                        const flipShouldBeDelivered = {
                            op: 'replace',
                            path: '/shouldBeDelivered',
                            value: !order.shouldBeDelivered
                        };
                        await PatchOrder(order.id, [flipShouldBeDelivered]);

                        if (order.shouldBeDelivered) {
                            alert(tPages('orders.alert_cancel'));
                        } else {
                            alert(tPages('orders.alert_delivery'));
                        }
                        setOrders(orders => ([...orders, { ...order, shouldBeDelivered: !order.shouldBeDelivered }]));
                    } catch (e) {
                        console.error(e);
                    }
                };

                const handleDownload = async () => {
                    try {
                        const { data, headers: { 'content-type': contentType } } = await DownloadOrderCad(order.id);
                        const blob = new Blob([data], { type: contentType });

                        const url = window.URL.createObjectURL(blob);

                        const link = document.createElement('a');
                        link.href = url;

                        switch (contentType) {
                            case 'model/gltf-binary': link.download = `${order.name}.glb`; break;
                            case 'application/zip': link.download = `${order.name}.zip`; break;
                            default: console.error('Unsupported MIME type.'); return;
                        }

                        document.body.appendChild(link);
                        link.click();
                        document.body.removeChild(link);

                        window.URL.revokeObjectURL(url);
                    } catch (error) {
                        console.error('Failed to download the file', error);
                    }
                };

                return [
                    <button onClick={handleDownload} className="basis-6/12 bg-indigo-700 border-2 border-indigo-500 px-10 py-3 rounded text-center text-indigo-50 hover:opacity-70 hover:border-transparent">
                        {tPages('orders.download')}
                    </button>,
                    <button onClick={handleRequest} className="basis-5/12 bg-indigo-50 border-2 border-indigo-600 px-10 py-3 rounded text-center text-indigo-950 hover:bg-rose-500 hover:border-transparent hover:text-indigo-50">
                        {
                            order.shouldBeDelivered
                                ? tPages('orders.cancel_request')
                                : tPages('orders.request')
                        }
                    </button>
                ];
        }
        return [];
    };

    return (
        <div className="flex flex-wrap justify-center gap-y-12 mt-4 mb-8">
            <div className="basis-full flex flex-col">
                <h2 className="px-4 text-2xl text-indigo-950">
                    <div className="flex justify-between items-center rounded-t-xl border-4 border-b border-indigo-700 overflow-hidden bg-indigo-500 text-center font-bold">
                        <Tab to="/client/orders/pending" position='start' text={tPages('orders.pending_title')} isActive={checkStatus('Pending')} />
                        <Tab to="/client/orders/begun" position='middle' text={tPages('orders.begun_title')} isActive={checkStatus('Begun')} />
                        <Tab to="/client/orders/finished" position='end' text={tPages('orders.finished_title')} isActive={checkStatus('Finished')} />
                    </div>
                </h2>
                <SearchBar setSearch={setSearch} />
            </div>
            {!orders.length ?
                <p className="text-lg text-indigo-900 text-center font-bold">{tPages('orders.no_orders')}</p>
                : <ul className="basis-full flex flex-wrap gap-y-8">
                    {orders.map(order => <li key={order.id}>
                            <Order order={order} buttons={chooseButtons(order)}/>
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
        const requestSearchParams = objectToUrl({ ...search, page, limit });
        try {
            const { data: { orders, count } } = await GetOrders(status, requestSearchParams);
            setOrders(orders);
            setTotal(count);
        } catch (e) {
            console.error(e);
        }
    }
}

export default UserOrders;