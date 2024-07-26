import AuthGuard from '../auth-guard'
import ClientOrders from '@/private/designer/client-orders/client-orders'
import CompleteOrder from '@/private/designer/client-orders/complete-order'
import ContributorCads from '@/private/designer/contributor-cads/contributor-cads'
import axios from 'axios'

export default {
    element: <AuthGuard isPrivate role={['Designer']} />,
    children: [
        {
            path: '/designer/cads',
            element: <ContributorCads />
        },
        {
            path: '/designer/orders/:status',
            element: <ClientOrders />,
            loader: async ({ params }) => {
                const { status } = params;
                const capitalizedStatus = status[0].toUpperCase() + status.slice(1);

                const orders = await axios.get(`https://localhost:7127/API/Designer/Orders?status=${status}`, {
                    withCredentials: true
                }).catch(e => console.error(e));

                return { loadedOrders: orders.data, status: capitalizedStatus };
            }
        },
        {
            path: '/designer/orders/:status/:id',
            element: <CompleteOrder />
        },
    ]
};