import PendingOrder from './components/pending-order'
import BegunOrder from './components/begun-order'
import FinishedOrder from './components/finished-order'
import { useLoaderData } from 'react-router-dom'
import { useTranslation } from 'react-i18next'

function AllOrders() {
    const { t } = useTranslation();
    const { status, loadedOrders } = useLoaderData();

    const chooseOrder = (order) => {
        switch (status.toLowerCase()) {
            case 'pending': return <PendingOrder order={order} />;
            case 'begun': return <BegunOrder order={order} />;
            case 'finished': return <FinishedOrder order={order} />;
        }
    };

    return (
        <div className="flex flex-wrap justify-center gap-y-12 mb-8">
            <h1 className="basis-full text-center text-4xl text-indigo-950 font-bold">
                {t(`body.designerOrders.${status} Title`)}
            </h1>
            {!loadedOrders.length ?
                <p className="basis-full text-lg text-indigo-900 text-center font-bold">{t('body.designerOrders.No orders')}</p>
                : <ul className="basis-full grid grid-cols-3 gap-y-8 gap-x-[5%]">
                    {loadedOrders.map(order =>
                        <li key={order.id}>{chooseOrder(order)}</li>
                    )}
                </ul>
            }
        </div>
    );
}

export default AllOrders;