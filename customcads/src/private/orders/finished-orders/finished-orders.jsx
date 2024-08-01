import Order from './components/finished-orders-item'
import { useTranslation } from 'react-i18next'
import { useLoaderData } from 'react-router-dom'
import { useState, useEffect } from 'react'

function FinishedOrders() {
    const { t } = useTranslation();
    const { loadedOrders } = useLoaderData();
    const [orders, setOrders] = useState([]);
    
    useEffect(() => {
        setOrders(loadedOrders);
    }, []);

    return (
        <div className="flex flex-wrap justify-center gap-y-12 mb-8">
            <h1 className="basis-full text-center text-4xl text-indigo-950 font-bold">{t('body.finishedOrders.Title')}</h1>
            <ul className="basis-full flex flex-wrap justify-center gap-y-8 gap-x-[5%]">
                {orders.map(order =>
                    <li key={order.id} className="basis-[40%] max-w-[40%]">
                        <Order order={order} />
                    </li>
                )}
            </ul>
        </div>
    );
}

export default FinishedOrders;