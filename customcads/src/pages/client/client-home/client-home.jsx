import { useLoaderData } from 'react-router-dom';
import { useTranslation } from 'react-i18next'
import RecentOrder from './components/recent-order';
import OrdersCount from './components/orders-count';

function ClientHome() {
    const { t } = useTranslation();
    const { loadedOrders: recentOrders, loadedCounts: counts } = useLoaderData();

    return (
        <div className="flex flex-col gap-y-6 my-2">
            <div>
                <h1 className="text-3xl text-center text-indigo-950 font-bold">
                    {t('private.orders.home_title')}
                </h1>
                <hr className="mt-2 border-2 border-indigo-700" />
            </div>
            <div className="flex flex-wrap gap-x-8">
                <div className="basis-1/3 grow flex flex-col gap-y-4">
                    <h2 className="text-2xl text-indigo-950 text-center font-bold">
                        {t('private.orders.home_subtitle_1')}
                    </h2>
                    <ol className="grid grid-rows-5 gap-y-3 border-4 border-indigo-500 pt-3 pb-6 px-8 rounded-2xl bg-indigo-100">
                        <li key={0} className="px-2 pb-2 border-b-2 border-indigo-900 rounded">
                            <div className="flex items-center gap-x-4 font-bold">
                                <span className="basis-1/6">Name</span>
                                <div className="grow">
                                    <div className="flex justify-between items-center px-4 py-2">
                                        <p className="basis-1/3 text-start">Category</p>
                                        <p className="basis-1/3 text-center">Order Date</p>
                                        <p className="basis-1/3 text-end">Status</p>
                                    </div>
                                </div>
                            </div>
                        </li>
                        {recentOrders.map(order => <li key={order.id}><RecentOrder order={order} /></li>)}
                    </ol>
                </div>
                <div className="min-h-full basis-1/4 flex flex-col gap-y-4">
                    <h2 className="text-2xl text-indigo-950 text-center font-bold">
                        {t('private.orders.home_subtitle_2')}
                    </h2>
                    <ul className="basis-full flex flex-col justify-items-center items-between border-4 border-indigo-500 rounded-2xl overflow-hidden bg-indigo-100 text-xl italic">
                        <OrdersCount text={`${t('common.statuses.Pending_plural')} - ${counts.pending}`} />
                        <OrdersCount text={`${t('common.statuses.Begun_plural')} - ${counts.begun}`} />
                        <OrdersCount text={`${t('common.statuses.Finished_plural')} - ${counts.finished}`} />                        
                        <OrdersCount text={`${t('common.statuses.Reported_plural')} - ${counts.reported}`} />                        
                        <OrdersCount text={`${t('common.statuses.Removed_plural')} - ${counts.removed}`} />                        
                    </ul>
                </div>
            </div>
        </div>
    );
}

export default ClientHome;