import { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next'
import { useLoaderData } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import ICad from '@/interfaces/cad';
import IOrder from '@/interfaces/order';
import { GetRecentOrders } from '@/requests/private/designer';
import ErrorPage from '@/components/error-page';
import RecentItem from '@/components/dashboard/recent-item';
import { getCookie, setCookie } from '@/utils/cookie-manager';

function DesignerHome() {
    const { t: tPages } = useTranslation('pages');
    const { t: tCommon } = useTranslation('common');
    const [orders, setOrders] = useState<IOrder[]>([]);
    const [status, setStatus] = useState<string>(getCookie('designer_dashboard_orders_status') ?? 'Pending');

    const { loadedCads: recentCads, error, status: statusCode } = useLoaderData() as {
        loadedCads: ICad[]
        loadedOrders: IOrder[]
        error: boolean
        status: number
    };
    if (error) {
        return <ErrorPage status={statusCode} />;
    }
    const fetchOrders = async () => {
        try {
            const { data } = await GetRecentOrders(status);
            setOrders(data.orders);
        } catch (e) {
            console.error(e);
        }
    };
    useEffect(() => {
        fetchOrders();
        setCookie('designer_dashboard_orders_status', status);
    }, [status]);

    const handlePrev = () => {
        switch (status) {
            case 'Pending': setStatus('Finished'); break;
            case 'Begun': setStatus('Pending'); break;
            case 'Finished': setStatus('Begun'); break; 
        }
    };
    const handleNext = () => {
        switch (status) {
            case 'Pending': setStatus('Begun'); break;
            case 'Begun': setStatus('Finished'); break;
            case 'Finished': setStatus('Pending'); break;
        }
    };

    return (
        <div className="flex flex-col gap-y-6 my-2">
            <div>
                <h1 className="text-3xl text-center text-indigo-950 font-bold">
                    {tPages('designer.home_title')}
                </h1>
                <hr className="mt-2 border-2 border-indigo-700" />
            </div>
            <div className="flex flex-wrap gap-x-8">
                <div className="basis-1/3 grow flex flex-col gap-y-4">
                    <h2 className="text-2xl text-indigo-950 text-center font-bold">
                        {tPages('designer.home_subtitle_1')}
                    </h2>
                    <ol className="grid grid-rows-5 gap-y-3 border-4 border-indigo-500 pt-3 pb-6 px-8 rounded-2xl bg-indigo-100">
                        <li key={0} className="px-2 pb-2 border-b-2 border-indigo-900 rounded">
                            <div className="flex items-center gap-x-4 font-bold">
                                <span className="basis-1/6">{tCommon('labels.name')}</span>
                                <div className="grow">
                                    <div className="flex justify-between items-center px-4 py-2">
                                        <p className="basis-1/3 text-start">{tCommon('labels.category')}</p>
                                        <p className="basis-1/3 text-center">{tCommon('labels.upload_date')}</p>
                                        <p className="basis-1/3 text-end">{tCommon('labels.status')}</p>
                                    </div>
                                </div>
                            </div>
                        </li>
                        {recentCads.map(cad => <li key={cad.id}>
                            <RecentItem to={`/designer/cads/${cad.id}`} item={cad} date={cad.creationDate} />
                        </li>)}
                    </ol>
                </div>
                <div className="min-h-full basis-1/3 grow flex flex-col gap-y-4">
                    <h2 className="text-2xl text-indigo-950 text-center font-bold flex gap-x-4 justify-between items-center">
                        <div onClick={handlePrev} className="hover:cursor-pointer flex">
                            <FontAwesomeIcon icon="circle-left" className="text-3xl text-indigo-600" />
                        </div>
                        <span>{tPages('designer.home_subtitle_2', { status: tCommon(`statuses.${status}_plural`) })}</span>
                        <div onClick={handleNext} className="hover:cursor-pointer flex">
                            <FontAwesomeIcon icon="circle-right" className="text-3xl text-indigo-600" />
                        </div>
                    </h2>
                    <ol className="grid grid-rows-5 gap-y-3 border-4 border-indigo-500 pt-3 pb-6 px-8 rounded-2xl bg-indigo-100">
                        <li key={0} className="px-2 pb-2 border-b-2 border-indigo-900 rounded">
                            <div className="flex items-center gap-x-4 font-bold">
                                <span className="basis-1/6">{tCommon('labels.name')}</span>
                                <div className="grow">
                                    <div className="flex justify-between items-center px-4 py-2">
                                        <p className="basis-1/3 text-start">{tCommon('labels.category')}</p>
                                        <p className="basis-1/3 text-center">{tCommon('labels.upload_date')}</p>
                                        <p className="basis-1/3 text-end">{tCommon('labels.status')}</p>
                                    </div>
                                </div>
                            </div>
                        </li>
                        {orders.map(order => <li key={order.id}>
                            <RecentItem to={`/designer/orders/pending/${order.id}`} item={order}  date={order.orderDate} />
                        </li>)}
                    </ol>
                </div>
            </div>
        </div>
    );
}

export default DesignerHome;