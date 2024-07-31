import { useTranslation } from 'react-i18next'
import { useState, useEffect } from 'react'
import axios from 'axios'

function Order({ order }) {
    const { t } = useTranslation();
    const [cad, setCad] = useState({ cadPath: '' });
    const [shouldBeDelivered, setShouldBeDelivered] = useState(order.shouldBeDelivered);
    const machineReadableDateTime = order.orderDate && order.orderDate.replaceAll('.', '-');

    useEffect(() => {
        fetchCad();
    }, []);

    const handleRequest = async () => {
        await axios.patch(`https://localhost:7127/API/Orders/${order.id}`, [{
            op: 'replace', 
            path: '/shouldBeDelivered',
            value: !shouldBeDelivered
        }], {
            withCredentials: true
        }).catch(e => console.error(e));
        
        if (shouldBeDelivered) {
            alert(t('body.finishedOrders.Alert Cancel Message'));
        } else {
            alert(t('body.finishedOrders.Alert Delivery Message'));
        }
        setShouldBeDelivered(shouldBeDelivered => !shouldBeDelivered);
    };

    return (
        <div className="bg-indigo-200 px-4 py-4 rounded-lg flex flex-col gap-y-2 shadow-lg shadow-indigo-800">
            <h3 className="text-2xl text-indigo-950 text-center font-semibold">{order.name}</h3>
            <div className="border-b border-indigo-700"></div>
            <section className="py-3 px-2 flex flex-col gap-y-6">
                <p className="text-indigo-900 text-center text-lg truncate">{order.description}</p>
                <div className="flex flex-wrap justify-around text-indigo-50 font-bold">
                    <a href={cad.cadPath} download
                        className="basis-5/12 bg-indigo-700 border border-indigo-500 p-2 rounded text-center text-indigo-50 hover:opacity-70 hover:border-transparent"
                    >
                        {t('body.finishedOrders.Download')}
                    </a>
                    <button onClick={handleRequest}
                        className="basis-5/12 bg-indigo-100 border border-indigo-600 p-2 rounded text-center text-indigo-950 hover:bg-rose-500 hover:border-transparent hover:text-indigo-50">
                        {
                            shouldBeDelivered
                            ? t('body.finishedOrders.Cancel') 
                            : t('body.finishedOrders.Request')
                        }
                    </button>
                </div>
            </section>
            <div className="border-t-2 border-indigo-800"></div>
            <div className="text-indigo-800 text-center">
                <span className="font-semibold">{t('body.orders.Ordered on')}</span>
                <time dateTime={machineReadableDateTime} className="italic">
                    {order.orderDate}
                </time>
            </div>
        </div>
    );

    async function fetchCad () {
        const response = await axios.get(`https://localhost:7127/API/Orders/GetCad/${order.id}`, {
            withCredentials: true
        }).catch(e => console.error(e));
        setCad(response.data);
    };

}

export default Order;