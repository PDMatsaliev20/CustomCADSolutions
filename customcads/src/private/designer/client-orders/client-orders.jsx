import Order from './components/client-orders-item'
import { useLoaderData } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import axios from 'axios'

function AllOrders() {
    const { t } = useTranslation();
    const { status, loadedOrders } = useLoaderData();
    let statusValue;

    switch (status.toLowerCase()) {
        case 'pending': statusValue = 1; break;
        case 'begun': statusValue = 2; break;
        case 'finished': statusValue = 0; break;
        default: return <>nah</>; break;
    }

    const handleCompleteOrder = async (id) => {
        await axios.patch(`https://localhost:7127/API/Designer/Orders/Status/${id}?status=${statusValue}`, {
            withCredentials: true
        }).catch(e => console.error(es));
    };

    return (
        <>
            <div className="flex flex-wrap justify-center gap-y-12 mb-8">
                <h1 className="basis-full text-center text-4xl text-indigo-950 font-bold">
                    {t(`body.designerOrders.${status} Title`)}
                </h1>
                <ul className="basis-full flex flex-wrap justify-center gap-y-8 gap-x-[5%]">
                    {loadedOrders.map(order =>
                        <li key={order.id} className="basis-[30%] max-w-[30%]">
                            <Order order={order} onComplete={() => handleCompleteOrder(order.id)} />
                        </li>
                    )}
                </ul>
            </div>
        </>
    );
}

export default AllOrders;