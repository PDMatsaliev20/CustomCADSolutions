import { useTranslation } from 'react-i18next'
import { useState, useEffect } from 'react'
import { PatchOrder, GetOrderCad } from '@/requests/private/orders'

function FinishedOrder({ order }) {
    const { t } = useTranslation();
    const [cad, setCad] = useState({ cadPath: '' });
    const [shouldBeDelivered, setShouldBeDelivered] = useState(order.shouldBeDelivered);
    const machineReadableDateTime = order.orderDate && order.orderDate.replaceAll('.', '-');

    useEffect(() => {
        fetchCad();
    }, []);

    const handleRequest = async () => {
        try {
            const flipShouldBeDelivered = {
                op: 'replace',
                path: '/shouldBeDelivered',
                value: !shouldBeDelivered
            };
            await PatchOrder(order.id, [flipShouldBeDelivered]);

            if (shouldBeDelivered) {
                alert(t('body.orders.Alert Cancel'));
            } else {
                alert(t('body.orders.Alert Delivery'));
            }
            setShouldBeDelivered(shouldBeDelivered => !shouldBeDelivered);
        } catch (e) {
            console.error(e);
        }
    };

    return (
        <div className="min-h-full bg-indigo-200 px-4 py-4 rounded-lg flex flex-col gap-y-2 shadow-lg shadow-indigo-800">
            <h3 className="text-2xl text-indigo-950 text-center font-semibold">{order.name}</h3>
            <hr className="border-b border-indigo-700" />
            <section className="grow py-3 px-2 flex flex-col gap-y-6">
                <p className="grow text-indigo-900 text-center text-lg line-clamp-3">{order.description}</p>
                <div className="flex flex-wrap justify-around text-indigo-50 font-bold">
                    <a href={cad.cadPath} download
                        className="basis-6/12 bg-indigo-700 border border-indigo-500 p-2 rounded text-center text-indigo-50 hover:opacity-70 hover:border-transparent"
                    >
                        {t('body.orders.Download')}
                    </a>
                    <button onClick={handleRequest}
                        className="basis-5/12 bg-indigo-100 border border-indigo-600 p-2 rounded text-center text-indigo-950 hover:bg-rose-500 hover:border-transparent hover:text-indigo-50">
                        {
                            shouldBeDelivered
                            ? t('body.orders.Cancel Request') 
                            : t('body.orders.Request')
                        }
                    </button>
                </div>
            </section>
            <hr className="border-t-2 border-indigo-800" />
            <div className="text-indigo-800 text-center">
                <span className="font-semibold">{t('body.orders.Ordered on')}</span>
                <time dateTime={machineReadableDateTime} className="italic">
                    {order.orderDate}
                </time>
            </div>
        </div>
    );

    async function fetchCad() {
        try {
            const { data } = await GetOrderCad(order.id);
            setCad(data);
        } catch (e) {
            console.error(e);
        }
    };

}

export default FinishedOrder;