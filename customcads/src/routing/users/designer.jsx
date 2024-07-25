import AuthGuard from '../auth-guard'
import ClientOrders from '@/private/designer/client-orders/client-orders'
import CompleteOrder from '@/private/designer/client-orders/complete-order'
import ContributorCads from '@/private/designer/contributor-cads/contributor-cads'

export default {
    element: <AuthGuard isPrivate role={['Designer']} />,
    children: [
        {
            path: '/designer/orders',
            element: <ClientOrders />
        },
        {
            path: '/orders/comlete',
            element: <CompleteOrder />
        },
        {
            path: '/designer/cads',
            element: <ContributorCads />
        }
    ]
};