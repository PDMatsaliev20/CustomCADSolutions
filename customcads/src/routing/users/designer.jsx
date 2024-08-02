import AuthGuard from '../auth-guard'
import ClientOrders from '@/private/designer/client-orders/orders/orders'
import CompleteOrder from '@/private/designer/client-orders/complete/complete-order'
import ContributorCads from '@/private/designer/contributor-cads/cads'
import { GetOrdersByStatus } from '@/requests/private/designer'

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

                try {
                    const { data: { orders } } = await GetOrdersByStatus(status);
                    return { loadedOrders: orders, status: capitalizedStatus };
                } catch (e) {
                    console.error(e);
                }
            }
        },
        {
            path: '/designer/orders/complete/:id',
            element: <CompleteOrder />
        },
    ]
};