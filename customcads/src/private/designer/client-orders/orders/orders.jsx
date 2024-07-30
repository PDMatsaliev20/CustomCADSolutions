import Order from './components/orders-item'
import { useLoaderData, useNavigate } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import axios from 'axios'

function AllOrders() {
    const { t } = useTranslation();
    const navigate = useNavigate();
    const { status, loadedOrders } = useLoaderData();
    let primaryBtn, secondaryBtn, onPrimaryBtnClick, onSecondaryBtnClick;

    switch (status.toLowerCase()) {
        case 'pending': 
            primaryBtn = t('Accept');
            onPrimaryBtnClick = async (id) => await axios
                .patch(`https://localhost:7127/API/Designer/Orders/Status/${id}?status=Begun`, {},
                    { withCredentials: true })
                .catch(e => console.error(e));

            secondaryBtn = t('Report');
            onSecondaryBtnClick = async (id) => await axios
                .patch(`https://localhost:7127/API/Designer/Orders/Status/${id}?status=Reported`, {},
                    { withCredentials: true })
                .catch(e => console.error(e));
            break;
        case 'begun': 
            primaryBtn = t('Complete');
            onPrimaryBtnClick = async (id) => navigate(`/designer/orders/complete/${id}`);

            secondaryBtn = t('Cancel');
            onSecondaryBtnClick = async (id) => await axios
                .patch(`https://localhost:7127/API/Designer/Orders/Status/${id}?status=Pending`, {},
                    { withCredentials: true })
                .catch(e => console.error(e));
            break;

        case 'finished':
            primaryBtn = t('Deliver');
            onPrimaryBtnClick = async (id) => await axios
                .patch(`https://localhost:7127/API/Designer/Orders/Status/${id}?status=Delivering`, {},
                    { withCredentials: true })
                .catch(e => console.error(e));

            secondaryBtn = t('Dismiss');
            onSecondaryBtnClick = () => { } // ?
            break;

        default: return <>nah</>; break;
    }
    
    return (
        <>
            <div className="flex flex-wrap justify-center gap-y-12 mb-8">
                <h1 className="basis-full text-center text-4xl text-indigo-950 font-bold">
                    {t(`body.designerOrders.${status} Title`)}
                </h1>
                <ul className="basis-full flex flex-wrap justify-center gap-y-8 gap-x-[5%]">
                    {loadedOrders.map(order =>
                        <li key={order.id} className="basis-[30%] max-w-[30%]">
                            <Order order={order}
                                primaryBtn={primaryBtn}
                                secondaryBtn={secondaryBtn}
                                onPrimaryBtnClick={onPrimaryBtnClick}
                                onSecondaryBtnClick={onSecondaryBtnClick}
                            />
                        </li>
                    )}
                </ul>
            </div>
        </>
    );
}

export default AllOrders;